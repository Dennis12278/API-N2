using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamApi.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaPerfilUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Biografia",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Links",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Biografia",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Links",
                table: "Usuarios");
        }
    }
}
