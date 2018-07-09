using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyDotnetProject.Migrations
{
    public partial class AddFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                });

            migrationBuilder.Sql("INSERT INTO Features (Name) VALUES ('Auto Drive')");
            migrationBuilder.Sql("INSERT INTO Features (Name) VALUES ('All-Wheel Drive')");
            migrationBuilder.Sql("INSERT INTO Features (Name) VALUES ('Side Airbag')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Features");
            migrationBuilder.Sql("DELETE FROM Features WHERE Name IN ('Auto Drive', 'All-Wheel Drive', 'Side Airbag')");
        }
    }
}
