using SimpleMigrations;

namespace Uow.EntityFramework.Migrator.Migrations;

[Migration(1, "CreateEntityTable")]
public class Migration001_CreateEntityTable : Migration
{
    protected override void Up()
    {
        Execute(@"
CREATE TABLE [Entities] (
    [Id] int NOT NULL IDENTITY,
    [Value] int NOT NULL,
    CONSTRAINT [PK_Entities] PRIMARY KEY ([Id])
);
");
    }

    protected override void Down()
    {
        Execute(@"
DROP TABLE [Entities];
");
    }
}
