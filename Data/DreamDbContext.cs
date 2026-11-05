using DreamApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DreamApi.Data
{
    public class DreamDbContext : DbContext
    {
        public DreamDbContext(DbContextOptions<DreamDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Anotacao> Anotacoes { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<TipoDocumento> TiposDocumento { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<GrupoUsuario> GruposUsuarios { get; set; }
        public DbSet<Compartilhamento> Compartilhamentos { get; set; }
        public DbSet<Notificacao> Notificacoes { get; set; }
        public DbSet<Calendario> Calendarios { get; set; }
        public DbSet<Colecao> Colecoes { get; set; }
        public DbSet<HistoricoLeitura> HistoricosLeitura { get; set; }
        public DbSet<ColecaoDocumento> ColecoesDocumentos { get; set; }
        public DbSet<ColecaoCurtida> ColecoesCurtidas { get; set; }
        public DbSet<MarcadorPublicacao> MarcadoresPublicacao { get; set; }


        // Toda data lida do banco passa a ser marcada como UTC
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<DateTime>()
                .HaveConversion<DataUtcConverter>();

            configurationBuilder
                .Properties<DateTime?>()
                .HaveConversion<DataUtcNulavelConverter>();
        }
    }

    public class DataUtcConverter : ValueConverter<DateTime, DateTime>
    {
        public DataUtcConverter()
            : base(
                v => v.Kind == DateTimeKind.Local ? v.ToUniversalTime() : v,
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
        {
        }
    }

    public class DataUtcNulavelConverter : ValueConverter<DateTime?, DateTime?>
    {
        public DataUtcNulavelConverter()
            : base(
                v => v.HasValue && v.Value.Kind == DateTimeKind.Local ? v.Value.ToUniversalTime() : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v)
        {
        }
    }
}