using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NibulonUrkPosta.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OBL",
                columns: table => new
                {
                    OBL = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOBL = table.Column<string>(type: "nvarchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBL", x => x.OBL);
                });

            migrationBuilder.CreateTable(
                name: "RAJ",
                columns: table => new
                {
                    KRAJ = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RAJ = table.Column<string>(type: "nvarchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAJ", x => x.KRAJ);
                });

            migrationBuilder.CreateTable(
                name: "CITY",
                columns: table => new
                {
                    CITY_KOD = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CITY = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    OBL = table.Column<short>(type: "smallint", nullable: false),
                    KRAJ = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CITY", x => x.CITY_KOD);
                    table.ForeignKey(
                        name: "FK_CITY_OBL_OBL",
                        column: x => x.OBL,
                        principalTable: "OBL",
                        principalColumn: "OBL",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CITY_RAJ_KRAJ",
                        column: x => x.KRAJ,
                        principalTable: "RAJ",
                        principalColumn: "KRAJ",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AUP",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    INDEX_A = table.Column<string>(type: "nvarchar(6)", nullable: false),
                    CITY = table.Column<int>(type: "int", nullable: false),
                    NCITY = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    OBL = table.Column<short>(type: "smallint", nullable: false),
                    NOBL = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    RAJ = table.Column<int>(type: "int", nullable: false),
                    NRAJ = table.Column<string>(type: "nvarchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUP", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AUP_CITY_CITY",
                        column: x => x.CITY,
                        principalTable: "CITY",
                        principalColumn: "CITY_KOD",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AUP_OBL_OBL",
                        column: x => x.OBL,
                        principalTable: "OBL",
                        principalColumn: "OBL",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AUP_RAJ_RAJ",
                        column: x => x.RAJ,
                        principalTable: "RAJ",
                        principalColumn: "KRAJ",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AUP_CITY",
                table: "AUP",
                column: "CITY");

            migrationBuilder.CreateIndex(
                name: "IX_AUP_OBL",
                table: "AUP",
                column: "OBL");

            migrationBuilder.CreateIndex(
                name: "IX_AUP_RAJ",
                table: "AUP",
                column: "RAJ");

            migrationBuilder.CreateIndex(
                name: "IX_CITY_KRAJ",
                table: "CITY",
                column: "KRAJ");

            migrationBuilder.CreateIndex(
                name: "IX_CITY_OBL",
                table: "CITY",
                column: "OBL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUP");

            migrationBuilder.DropTable(
                name: "CITY");

            migrationBuilder.DropTable(
                name: "OBL");

            migrationBuilder.DropTable(
                name: "RAJ");
        }
    }
}
