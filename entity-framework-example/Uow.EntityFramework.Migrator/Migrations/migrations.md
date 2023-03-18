How to create these from EF Core Migrations:

1)  In *Uow.EntityFramework.Example*

        \entity-framework-example\Uow.EntityFramework.Example\

    Call `dotnet ef` to generate SQL from the EF Migrations.

        dotnet ef migrations script PreviousMigration NewMigration     # Generate Up
        dotnet ef migrations script NewMigration PreviousMigration     # Generate Down

2) Create a new migration in this project.
    Copy in the SQL from the previous step.
    But discard any changes to `[__EFMigrationsHistory]`. This project doesn't use EF Migrations in the database, only to generate SQL for *Simple.Migrations*.
