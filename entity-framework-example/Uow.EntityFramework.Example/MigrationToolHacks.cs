using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Uow.EntityFramework.Example.Storage;

namespace Uow.EntityFramework.Example;

/*
 *  This class is just here to allow `dotnet ef migrations add [MigrationName]` to run on this class libary
 */
internal class MigrationToolDbContextFactory : IDesignTimeDbContextFactory<ExampleDbContext>
{
    public ExampleDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ExampleDbContext>()
            .UseSqlServer("thisiswheretheconnectionstringgoes")
            .ReplaceService<ISqlGenerationHelper, NoGoSqlGenerationHelper>();

        return new ExampleDbContext(optionsBuilder.Options);
    }
}

/*
 * Remove the GO statements.
 *  This has the advantage the generated SQL is actually SQL now,
 *      but might cause trouble if the batch seperated are needed (but I think that's unlikely).
 */

#pragma warning disable EF1001 // Internal EF Core API usage.
internal class NoGoSqlGenerationHelper : SqlServerSqlGenerationHelper
{
    public NoGoSqlGenerationHelper(RelationalSqlGenerationHelperDependencies dependencies)
         : base(dependencies)
    {
    }

    // Avoids generating GO in scripts
    public override string BatchTerminator => Environment.NewLine;
}
#pragma warning restore EF1001 // Internal EF Core API usage.
