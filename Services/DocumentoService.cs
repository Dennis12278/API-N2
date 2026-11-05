using DreamApi.Data;
using DreamApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamApi.Services
{
    public class DocumentoService
    {
        private readonly DreamDbContext context;
        private readonly IWebHostEnvironment env;

        private static readonly string[] ExtensoesPermitidas =
            { ".jpg", ".jpeg", ".png", ".webp" };

        private const long TamanhoMaximoCapa = 2 * 1024 * 1024; // 2 MB

        public DocumentoService(DreamDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }

        public List<DocumentoListaDto> Listar(bool? publicado = null, int? idUsuario = null)
        {
            IQueryable<Documento> consulta = context.Documentos.AsNoTracking();

            if (publicado.HasValue)
            {
                consulta = consulta.Where(d => d.Publicado == publicado.Value);
            }

            if (idUsuario.HasValue)
            {
                consulta = consulta.Where(d => d.IdUsuario == idUsuario.Value);
            }

            return consulta
                .OrderByDescending(d => d.DataPublicacao)
                .Select(d => new DocumentoListaDto
                {
                    Id = d.Id,
                    IdUsuario = d.IdUsuario,
                    Titulo = d.Titulo,
                    Conteudo = d.Conteudo,
                    Revisado = d.Revisado,
                    Publicado = d.Publicado,
                    Visibilidade = d.Visibilidade,
                    DataPublicacao = d.DataPublicacao,
                    Capa = d.Capa
                })
                .ToList();
        }

        public Documento? Obter(int id)
        {
            return context.Documentos
                .AsNoTracking()
                .FirstOrDefault(d => d.Id == id);
        }

        public string? ValidarCapa(IFormFile? capa)
        {
            if (capa == null || capa.Length == 0)
            {
                return null;
            }

            string extensao = Path.GetExtension(capa.FileName).ToLowerInvariant();

            if (!ExtensoesPermitidas.Contains(extensao))
            {
                return "Formato de imagem inválido. Use JPG, PNG ou WEBP.";
            }

            if (capa.Length > TamanhoMaximoCapa)
            {
                return "A imagem deve ter no máximo 2 MB.";
            }

            return null;
        }

        private async Task<string?> SalvarCapa(IFormFile? capa)
        {
            if (capa == null || capa.Length == 0)
            {
                return null;
            }

            string extensao = Path.GetExtension(capa.FileName).ToLowerInvariant();

            string pasta = Path.Combine(env.WebRootPath, "capas");
            Directory.CreateDirectory(pasta);

           
            string nomeArquivo = Guid.NewGuid() + extensao;

            await using FileStream stream = File.Create(Path.Combine(pasta, nomeArquivo));
            await capa.CopyToAsync(stream);

            return "/capas/" + nomeArquivo;
        }

        public async Task<Documento> Adicionar(DocumentoCriarDto dto)
        {
            string? caminhoCapa = await SalvarCapa(dto.Capa);

            Documento documento = new Documento
            {
                IdUsuario = dto.IdUsuario,
                IdTipoDocumento = dto.IdTipoDocumento,
                Titulo = dto.Titulo.Trim(),
                Conteudo = dto.Conteudo ?? "",
                Visibilidade = dto.Visibilidade ?? "publica",
                Revisado = false,
                Publicado = true,
                DataPublicacao = DateTime.UtcNow,
                Capa = caminhoCapa
            };

            try
            {
                context.Documentos.Add(documento);
                context.SaveChanges();
            }
            catch
            {      
                ApagarArquivoCapa(caminhoCapa);
                throw;
            }

            return documento;
        }

        public async Task<Documento?> Atualizar(int id, DocumentoEditarDto dto)
        {
            Documento? documento = context.Documentos.Find(id);

            if (documento == null)
            {
                return null;
            }

            string? capaAntiga = documento.Capa;
            string? capaNova = null;

            if (dto.Capa is { Length: > 0 })
            {
                capaNova = await SalvarCapa(dto.Capa);
                documento.Capa = capaNova;
            }
            else if (dto.RemoverCapa)
            {
                documento.Capa = null;
            }

            documento.Titulo = dto.Titulo.Trim();
            documento.Conteudo = dto.Conteudo ?? "";
            documento.Visibilidade = dto.Visibilidade ?? documento.Visibilidade;

            try
            {
                context.SaveChanges();
            }
            catch
            {
                ApagarArquivoCapa(capaNova);
                throw;
            }

            if (capaAntiga != documento.Capa)
            {
                ApagarArquivoCapa(capaAntiga);
            }

            return documento;
        }

        public void Editar(Documento documento)
        {
            Documento? documentoExistente = context.Documentos.Find(documento.Id);

            if (documentoExistente != null)
            {
                documentoExistente.IdUsuario = documento.IdUsuario;
                documentoExistente.IdTipoDocumento = documento.IdTipoDocumento;
                documentoExistente.Titulo = documento.Titulo;
                documentoExistente.Conteudo = documento.Conteudo;
                documentoExistente.Revisado = documento.Revisado;
                documentoExistente.Publicado = documento.Publicado;
                documentoExistente.Visibilidade = documento.Visibilidade;
                documentoExistente.DataPublicacao = documento.DataPublicacao;

                context.SaveChanges();
            }
        }

        public void Revisar(int id)
        {
            Documento? documento = context.Documentos.Find(id);

            if (documento != null)
            {
                documento.Revisado = true;
                context.SaveChanges();
            }
        }

        public void Publicar(int id)
        {
            Documento? documento = context.Documentos.Find(id);

            if (documento != null)
            {
                documento.Publicado = true;
                documento.DataPublicacao = DateTime.UtcNow;

                context.SaveChanges();
            }
        }

        public void Excluir(int id)
        {
            Documento? documento = context.Documentos.Find(id);

            if (documento != null)
            {
                string? capa = documento.Capa;

                context.Documentos.Remove(documento);
                context.SaveChanges();

                // só apaga o arquivo depois que o registro saiu do banco
                ApagarArquivoCapa(capa);
            }
        }

        private void ApagarArquivoCapa(string? capa)
        {
            // publicações antigas guardam base64 no lugar do caminho: não há arquivo
            if (string.IsNullOrEmpty(capa) || !capa.StartsWith("/capas/"))
            {
                return;
            }

            // GetFileName impede caminhos como "../../algo"
            string nomeArquivo = Path.GetFileName(capa);
            string caminho = Path.Combine(env.WebRootPath, "capas", nomeArquivo);

            try
            {
                if (File.Exists(caminho))
                {
                    File.Delete(caminho);
                }
            }
            catch (IOException)
            {
                // arquivo em uso: a publicação já foi excluída, então não falha por isso
            }
        }
    }
}