using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cor.Apt.Migrations
{
    public partial class NewEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Patients",
                table: "Patients");

            migrationBuilder.RenameTable(
                name: "Patients",
                newName: "Patient");

            migrationBuilder.AddColumn<bool>(
                name: "IsAndulation",
                table: "Appointments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Patient",
                table: "Patient",
                column: "PatientId");

            migrationBuilder.CreateTable(
                name: "Analyses",
                columns: table => new
                {
                    AnalysisId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnalysisDate = table.Column<DateTime>(nullable: false),
                    AnalysisFolderName = table.Column<string>(nullable: true),
                    PatientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analyses", x => x.AnalysisId);
                    table.ForeignKey(
                        name: "FK_Analyses_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Anamnesises",
                columns: table => new
                {
                    AnamnesisId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Age = table.Column<int>(nullable: false),
                    Profession = table.Column<string>(nullable: true),
                    Height = table.Column<double>(nullable: false),
                    Weight = table.Column<double>(nullable: false),
                    Diet = table.Column<string>(nullable: true),
                    Disease = table.Column<string>(nullable: true),
                    Smoking = table.Column<string>(nullable: true),
                    Alcohol = table.Column<string>(nullable: true),
                    Allergy = table.Column<string>(nullable: true),
                    ConstipationAndDiarrhea = table.Column<string>(nullable: true),
                    Drugs = table.Column<string>(nullable: true),
                    PatientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anamnesises", x => x.AnamnesisId);
                    table.ForeignKey(
                        name: "FK_Anamnesises_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Andulations",
                columns: table => new
                {
                    AndulationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionDate = table.Column<DateTime>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Session = table.Column<int>(nullable: false),
                    Schedule = table.Column<int>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    Vibration = table.Column<int>(nullable: false),
                    Chest = table.Column<double>(nullable: false),
                    Waist = table.Column<double>(nullable: false),
                    Abdomen = table.Column<double>(nullable: false),
                    Hip = table.Column<double>(nullable: false),
                    RightLeg = table.Column<double>(nullable: false),
                    RightArm = table.Column<double>(nullable: false),
                    PatientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Andulations", x => x.AndulationId);
                    table.ForeignKey(
                        name: "FK_Andulations_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeightRecords",
                columns: table => new
                {
                    WeightRecordId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordDate = table.Column<DateTime>(nullable: false),
                    RecordName = table.Column<string>(nullable: true),
                    PatientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightRecords", x => x.WeightRecordId);
                    table.ForeignKey(
                        name: "FK_WeightRecords_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_PatientId",
                table: "Analyses",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Anamnesises_PatientId",
                table: "Anamnesises",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Andulations_PatientId",
                table: "Andulations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightRecords_PatientId",
                table: "WeightRecords",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patient_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patient_PatientId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "Analyses");

            migrationBuilder.DropTable(
                name: "Anamnesises");

            migrationBuilder.DropTable(
                name: "Andulations");

            migrationBuilder.DropTable(
                name: "WeightRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Patient",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "IsAndulation",
                table: "Appointments");

            migrationBuilder.RenameTable(
                name: "Patient",
                newName: "Patients");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Patients",
                table: "Patients",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
