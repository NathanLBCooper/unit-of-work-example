How to create these Migrations:

1) Make your changes to the code (to the DBContext)

2) Call `dotnet ef` to generate SQL from the EF Migrations.

        dotnet ef migrations add NameOfTheNewMigration

3) No go and follow the instructions in the *Uow.EntityFramework.Migrator* project.
