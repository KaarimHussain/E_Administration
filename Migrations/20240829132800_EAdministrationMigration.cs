using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Administration.Migrations
{
    /// <inheritdoc />
    public partial class EAdministrationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    department_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    department_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__departme__C22324229EDE6AF9", x => x.department_id);
                });

            migrationBuilder.CreateTable(
                name: "floors",
                columns: table => new
                {
                    floor_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__floors__76040CCC689CF7BC", x => x.floor_id);
                });

            migrationBuilder.CreateTable(
                name: "hardwares",
                columns: table => new
                {
                    hard_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    software_name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    processor = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ram = table.Column<int>(type: "int", nullable: false),
                    storage_capacity = table.Column<int>(type: "int", nullable: true),
                    operating_system = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__hardware__2646D038CDD871DC", x => x.hard_id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__roles__760965CC60F63F18", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "softwares",
                columns: table => new
                {
                    soft_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    software_name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__software__FDAD1D125144F152", x => x.soft_id);
                });

            migrationBuilder.CreateTable(
                name: "labs",
                columns: table => new
                {
                    lab_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    floor_id = table.Column<int>(type: "int", nullable: false),
                    lab_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__labs__66DE64DB10E261FE", x => x.lab_id);
                    table.ForeignKey(
                        name: "FK__labs__floor_id__5535A963",
                        column: x => x.floor_id,
                        principalTable: "floors",
                        principalColumn: "floor_id");
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users__3213E83F951B45F0", x => x.id);
                    table.ForeignKey(
                        name: "FK__users__role_id__3C69FB99",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "role_id");
                });

            migrationBuilder.CreateTable(
                name: "pcs",
                columns: table => new
                {
                    pc_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    soft_id = table.Column<int>(type: "int", nullable: false),
                    hard_id = table.Column<int>(type: "int", nullable: false),
                    lab_id = table.Column<int>(type: "int", nullable: false),
                    pc_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    purchased_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__pcs__1D3A69C0273E265A", x => x.pc_id);
                    table.ForeignKey(
                        name: "FK__pcs__hard_id__693CA210",
                        column: x => x.hard_id,
                        principalTable: "hardwares",
                        principalColumn: "hard_id");
                    table.ForeignKey(
                        name: "FK__pcs__lab_id__6A30C649",
                        column: x => x.lab_id,
                        principalTable: "labs",
                        principalColumn: "lab_id");
                    table.ForeignKey(
                        name: "FK__pcs__soft_id__68487DD7",
                        column: x => x.soft_id,
                        principalTable: "softwares",
                        principalColumn: "soft_id");
                });

            migrationBuilder.CreateTable(
                name: "additional_info",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    address = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true, defaultValueSql: "(NULL)"),
                    phone_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    profile_picture = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true, defaultValueSql: "(NULL)"),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    gender = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__addition__3213E83FDAA90A25", x => x.id);
                    table.ForeignKey(
                        name: "FK__additiona__user___4222D4EF",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "complaints",
                columns: table => new
                {
                    complaints_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    complaints_response = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__complain__5C8661EEB51A97C3", x => x.complaints_id);
                    table.ForeignKey(
                        name: "FK__complaint__user___6D0D32F4",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_additional_info_user_id",
                table: "additional_info",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_complaints_user_id",
                table: "complaints",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_labs_floor_id",
                table: "labs",
                column: "floor_id");

            migrationBuilder.CreateIndex(
                name: "UQ__labs__6761F66560A4A4B7",
                table: "labs",
                column: "lab_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pcs_hard_id",
                table: "pcs",
                column: "hard_id");

            migrationBuilder.CreateIndex(
                name: "IX_pcs_lab_id",
                table: "pcs",
                column: "lab_id");

            migrationBuilder.CreateIndex(
                name: "IX_pcs_soft_id",
                table: "pcs",
                column: "soft_id");

            migrationBuilder.CreateIndex(
                name: "UQ__roles__783254B181C5427B",
                table: "roles",
                column: "role_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "UQ__users__AB6E6164FF826E34",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "additional_info");

            migrationBuilder.DropTable(
                name: "complaints");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "pcs");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "hardwares");

            migrationBuilder.DropTable(
                name: "labs");

            migrationBuilder.DropTable(
                name: "softwares");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "floors");
        }
    }
}
