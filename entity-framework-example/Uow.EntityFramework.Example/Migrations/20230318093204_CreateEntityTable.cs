﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Uow.EntityFramework.Example.Migrations;

public partial class CreateEntityTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.CreateTable(
            name: "Entities",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Value = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_Entities", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropTable(
            name: "Entities");
    }
}
