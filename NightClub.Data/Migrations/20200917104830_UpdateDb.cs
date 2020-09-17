using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NightClub.Data.Migrations
{
    public partial class UpdateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Membres",
                columns: new[] { "Id", "DebutDateBlacklister", "Email", "FinDateBlacklister", "IsBlacklister", "Telephone" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "jawadchemlal@hotmail.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null });

            migrationBuilder.InsertData(
                table: "IDCartes",
                columns: new[] { "Id", "DateExpiration", "DateNaissance", "DateValidation", "MembreId", "Nom", "NumeroCarte", "Prenom", "RegistreNational" },
                values: new object[] { 1, new DateTime(2030, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1995, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2017, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Jawad", 1000001, "Jawadoux", "137174" });

            migrationBuilder.InsertData(
                table: "MembreCartes",
                columns: new[] { "Id", "Code", "IsActive", "MembreId" },
                values: new object[] { 1, "2535", true, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IDCartes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MembreCartes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Membres",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
