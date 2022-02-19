using SimpleMigrations;

namespace Uow.Postgresql.Migrator.Migrations
{
    [Migration(1, "CreateEntityTable")]
    public class Migration001_CreateEntityTable : Migration
    {
        protected override void Up()
        {
            Execute(@"
                CREATE TABLE public.Entity(
                    Id serial primary key,
                    Value integer not null
                );");
        }

        protected override void Down()
        {
            Execute(@"DROP TABLE public.Entity;");
        }
    }
}
