using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FORMULARIOPRUEBA.Data.Migrations
{
    /// <inheritdoc />
    public partial class SaveMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "archivo",
                table: "t_prueba",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archivo_name",
                table: "t_prueba",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "confederado",
                table: "t_prueba",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "archivo",
                table: "t_prueba");

            migrationBuilder.DropColumn(
                name: "archivo_name",
                table: "t_prueba");

            migrationBuilder.DropColumn(
                name: "confederado",
                table: "t_prueba");
        }
    }
}
