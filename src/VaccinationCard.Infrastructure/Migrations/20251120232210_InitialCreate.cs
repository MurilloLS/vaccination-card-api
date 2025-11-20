using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaccinationCard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PERSON",
                columns: table => new
                {
                    id_person = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nm_person = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    nr_age_person = table.Column<int>(type: "INTEGER", nullable: false),
                    sg_gender_person = table.Column<string>(type: "CHAR(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERSON", x => x.id_person);
                });

            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    id_user = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nm_user = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    pwd_hash_user = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    sg_role = table.Column<string>(type: "CHAR(5)", nullable: false, defaultValue: "USER")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER", x => x.id_user);
                });

            migrationBuilder.CreateTable(
                name: "VACCINE_CATEGORY",
                columns: table => new
                {
                    id_vaccine_category = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nm_vaccine_category = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VACCINE_CATEGORY", x => x.id_vaccine_category);
                });

            migrationBuilder.CreateTable(
                name: "VACCINE",
                columns: table => new
                {
                    id_vaccine = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nm_vaccine = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    id_vaccine_category = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VACCINE", x => x.id_vaccine);
                    table.ForeignKey(
                        name: "FK_VACCINE_VACCINE_CATEGORY_id_vaccine_category",
                        column: x => x.id_vaccine_category,
                        principalTable: "VACCINE_CATEGORY",
                        principalColumn: "id_vaccine_category",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VACCINATION",
                columns: table => new
                {
                    id_vaccination = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    id_person = table.Column<int>(type: "INTEGER", nullable: false),
                    id_vaccine = table.Column<int>(type: "INTEGER", nullable: false),
                    cd_dose_vaccination = table.Column<string>(type: "CHAR(5)", nullable: false),
                    dt_application_vaccination = table.Column<DateTime>(type: "DATE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VACCINATION", x => x.id_vaccination);
                    table.ForeignKey(
                        name: "FK_VACCINATION_PERSON_id_person",
                        column: x => x.id_person,
                        principalTable: "PERSON",
                        principalColumn: "id_person",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VACCINATION_VACCINE_id_vaccine",
                        column: x => x.id_vaccine,
                        principalTable: "VACCINE",
                        principalColumn: "id_vaccine",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_USER_nm_user",
                table: "USER",
                column: "nm_user",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VACCINATION_id_person",
                table: "VACCINATION",
                column: "id_person");

            migrationBuilder.CreateIndex(
                name: "IX_VACCINATION_id_vaccine",
                table: "VACCINATION",
                column: "id_vaccine");

            migrationBuilder.CreateIndex(
                name: "IX_VACCINE_id_vaccine_category",
                table: "VACCINE",
                column: "id_vaccine_category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "USER");

            migrationBuilder.DropTable(
                name: "VACCINATION");

            migrationBuilder.DropTable(
                name: "PERSON");

            migrationBuilder.DropTable(
                name: "VACCINE");

            migrationBuilder.DropTable(
                name: "VACCINE_CATEGORY");
        }
    }
}
