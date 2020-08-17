using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TravianTools.DAL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NeighborsVillageInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PointX = table.Column<string>(nullable: true),
                    PointY = table.Column<string>(nullable: true),
                    UntilProtectionTime = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NeighborsVillageInfos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "NeighborsVillageInfos");
        }
    }
}
