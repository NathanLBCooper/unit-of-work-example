using SimpleMigrations;

namespace Uow.Mssql.Migrator.Migrations;

[Migration(1, "CreateEntityTable")]
public class Migration001_CreateEntityTable : Migration
{
    protected override void Up()
    {
        Execute(@"
                CREATE TABLE [dbo].[Entity](
                    [Id] [int] IDENTITY PRIMARY KEY,
                    [Value] [int] NOT NULL,
                );");
    }

    protected override void Down()
    {
        Execute(@"DROP TABLE [dbo].[Entity];");
    }
}
