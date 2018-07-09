using Microsoft.EntityFrameworkCore.Migrations;

namespace MyDotnetProject.Migrations
{
    public partial class SeedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Makes (Name) VALUES ('Audi') ");
            migrationBuilder.Sql("INSERT INTO Makes (Name) VALUES ('Honda') ");
            migrationBuilder.Sql("INSERT INTO Makes (Name) VALUES ('Toyota') ");

            migrationBuilder.Sql("INSERT INTO Models (Name, MakeID) VALUES ('A6', (SELECT ID FROM Makes WHERE Name = 'Audi'))");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeID) VALUES ('A8', (SELECT ID FROM Makes WHERE Name = 'Audi'))");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeID) VALUES ('Q5', (SELECT ID FROM Makes WHERE Name = 'Audi'))");

            migrationBuilder.Sql("INSERT INTO Models (Name, MakeID) VALUES ('Accord', (SELECT ID FROM Makes WHERE Name = 'Honda'))");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeID) VALUES ('CR-V', (SELECT ID FROM Makes WHERE Name = 'Honda'))");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeID) VALUES ('Civic', (SELECT ID FROM Makes WHERE Name = 'Honda'))");

            migrationBuilder.Sql("INSERT INTO Models (Name, MakeID) VALUES ('Camry Hybird', (SELECT ID FROM Makes WHERE Name = 'Toyota'))");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeID) VALUES ('Corolla', (SELECT ID FROM Makes WHERE Name = 'Toyota'))");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeID) VALUES ('C-HR', (SELECT ID FROM Makes WHERE Name = 'Toyota'))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete From Makes where Name in ('Audi', 'Honda', 'Toyota')");
        }
    }
}
