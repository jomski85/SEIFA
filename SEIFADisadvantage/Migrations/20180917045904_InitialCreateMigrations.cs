using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SEIFADisadvantage.Migrations
{
    public partial class InitialCreateMigrations : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Data2011",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    State = table.Column<string>(nullable: true),
                    Place = table.Column<string>(nullable: true),
                    Disadvantage = table.Column<int>(nullable: false),
                    AdvantageDisadvantage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Data2011", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Data2016",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LgaCode = table.Column<int>(nullable: false),
                    Place = table.Column<string>(nullable: true),
                    DisadvantageScore = table.Column<int>(nullable: false),
                    DisadvantageDecile = table.Column<int>(nullable: false),
                    AdvantageAndDisadvantageScore = table.Column<int>(nullable: false),
                    AdvantageAndDisadvantageDecile = table.Column<int>(nullable: false),
                    EconomicResourcesScore = table.Column<int>(nullable: false),
                    EconomicResourcesDecile = table.Column<int>(nullable: false),
                    EducationAndOccupationScore = table.Column<int>(nullable: false),
                    EducationAndOccupationDecile = table.Column<int>(nullable: false),
                    Population = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Data2016", x => x.Id);
                });

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Data2011");

            migrationBuilder.DropTable(
                name: "Data2016");
        }
    }
}
