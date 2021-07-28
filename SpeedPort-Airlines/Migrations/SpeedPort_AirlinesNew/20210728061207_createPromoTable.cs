using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SpeedPort_Airlines.Migrations.SpeedPort_AirlinesNew
{
    public partial class createPromoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Promo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DestinationCountry = table.Column<string>(nullable: true),
                    PromoValidity = table.Column<DateTime>(nullable: false),
                    TravelAgency = table.Column<string>(nullable: true),
                    PromoPrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promo", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Promo");
        }
    }
}
