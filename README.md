# unit-of-work-example

This is my preferred **Unit of Work** pattern.

> Skip to [My Pattern](#my-pattern) to skip the essay and see the pattern

> Skip to [Examples](#examples)

<br/>

# Background

A unit of work is something that tracks the changes made during a business transaction, coordinates the writing of those changes and resolves concurrency problems. It allows the changes to be written or, in the event of failure not written, together as a single unit.

Usually, I write my 'database code' in repositories and I use these repositories in my 'business code'. It's sometimes the case that I want to make multiple calls to multiple repositories and have those calls succeed or fail together. When that is the case, I use a unit of work pattern.

This repositories contains examples of the pattern I use. All are written in C# and use relational databases, and most use [Dapper](https://github.com/DapperLib/Dapper). But the pattern is applicable to different technologies, languages and storage mechanisms.

# Other patterns

Before using this patterns, you should consider these other options:

## The best unit of work pattern is no pattern at all

If you don't need to tie multiple repository methods together into transactions, then don't. Treating each method as an atomic operation is simplest and therefore best option.

ORMs make it easy to save complex objects atomically and eliminate a lot of the need for transactions. It also could be the case, that when you find the need to tie disparate objects together with transactions, it might be better to re-consider your design rather than reaching for a unit of work pattern.

## Just use a DBContext

In Entity Framework, this is very simple. Make changes on the `DBContext`, save them and you're done.

    public class MyEntityFrameworkDbContext : DbContext
    {
        ... // There is a transaction object in here somewhere
        // Your tables as collection objects:
        public DbSet<Foo> Foos { get; set; }
        public DbSet<Bar> Bars { get; set; }
        public void SaveChanges() => { ... } // commit transaction
    }

    // Just makes your changes and save
    _myEfDbContext.Foos.Add(foo);
    _myEfDbContext.Bars.Add(bar);
    _myEfDbContext.SaveChanges();

A similar thing is possible in Dapper. Just replace the DBSets with your repositories and write your own `SaveChanges` to  manage the transaction. 

This simple, and if it works for you, it's a great option. My problem with this pattern is that it requires the caller to always have access to a container of all possible repositories. I think it's a bit of a God object. I prefer to be able to individually inject only the repositories needed into my business code, rather than having everything always just a "`.`"  away.

<br/>

# My pattern

For the external code calling the repository, the pattern looks like this:

    using (var uow = _createUnitOfWork.Create())
    {
        // Do things inside the unit of work
        await _fooRepository.Create(foo);
        await _barRepository.Create(bar);

        await uow.Commit();  // Or maybe uow.Rollback() instead
    }
    
    // The unit of work is now finished

That external code just needs to take the repositories it needs and also a reference to `ICreateUnitOfWork`. For example, here is the constructor for a class that uses `FooRepository` and `BarRepository`:

    public MyClass(ICreateUnitOfWork createUnitOfWork, FooRepository fooRepository, BarRepository barRepository) {...}

There is a magic link between the repositories and the `ICreateUnitOfWork` that makes all this work. And the examples will show how that is done.

<br/>

# Examples

## 01: Basic Example using SQLite and Dapper

Folder: *basic-example/*

This is the most basic example. It uses Sqlite, an in-memory database, and Dapper. This should be your entry point for understanding this pattern.

This code contains an Entity, a Repository, a Controller and the unit of work pattern. There are unit tests on both the Controller and the Repository. These tests can be run without any special setup.

### Notes:

There is one strange quirk about how this example is written. In Sqlite the database only exists for as long as the connection exists. This makes `SQLiteConnection` a very long lived object. The other non-Sqlite examples create and destroy the connection alongside the transaction inside the `UnitOfWork`.

The code is split up into `Application` and `Storage`, where `Storage` depends on `Application`. The first folder contains the parts of this pattern that need to be visible to all the code that uses Unit Of Work. The second contains the parts that are only needed internally by the repositories. The Controller class does not need to reference Storage to use `ICreateUnitOfWork`, so this pattern supports architectures where database code is decoupled.

With this unit of work pattern everything has to be done inside a unit of work. But you could modify that by making chnages to what the UnitOfWorkContext does when no Unit of Work is open, if you disagree with that choice.

This (and every other) example is just my current favourite way of structuring this pattern. If you were to dig through the commit history, you'll see that my preferences have changed over time. You can also change things to your preferences.

<br/>

## 02: Example with events in the transaction

Folder: *example-with-events/*

Again, this uses Sqlite and Dapper. This time however, it also publishes events alongside the database changes. This an example of how non-database actions can included in the unit of work. This shows how Unit of work is a bigger concept than just a database transactions.

I have used this pattern in real applications, where I have used a transactional event publisher to send messages to RabbitMq and enqueue work on Hangfire.

<br/>

## 03: PostgreSQL and Dapper Example

Folder *postgresql-example/*

An example using PostgreSQL and Dapper.

Requires Docker. Run `docker-compose` to start postgresql container needed for tests. Then just press play on the tests as usual.

<br/>

## 04: SQL Server and Dapper Example

Folder: *mssql-example/*

An example using Sql Server and Dapper. A quirk of using Sql Server is that the transaction must be passed into Dapper separately from the connection. To make this possible the interface `IGetUnitOfWork ` is expanded to return the transaction in addition to the connection.

Requires Docker. Run `docker-compose` to start sql server container needed for tests. Then just press play on the tests as usual.

<br/>

## 05: Entity Framework Example

Folder: *entity-framework-example/*

An example using Entity Framework and SQL server. It could probably be easily adapted to other EF Data Providers, as long as they support transactions.

Note, there is less of a usecase for having to use your own transactions with Ef, or any proper ORM. If you just use `SaveChanges`, EF will save large aggregates atomically by magic. Maybe that's all you need? Consider whether you really need this pattern before using it.

Requires Docker. Run `docker-compose` to start sql server container needed for tests. Then just press play on the tests as usual.

<br/>

# How to use this in a dependency injection container

Here is a a Dependency configuration lifted from one of my projects where I use this pattern. The IOC container I'm using is SimpleInjector. The important part is that `ICreateUnitOfWork` and `IGetConnection` are the same registration. The magic of this pattern is that the Unit of Work created via `ICreateUnitOfWork` in the 'business code' can be accessed by the repositories via `IGetConnection`, so it's critical that these interfaces are registered as the same underlying object.

    using System;
    using System.Linq;
    using System.Reflection;
    using MyProject.Domain.UnitOfWork;
    using MyProject.Storage;
    using MyProject.Storage.UnitOfWork;
    using Microsoft.Extensions.Configuration;
    using SimpleInjector;

    namespace MyProject.Api;

    internal static class Dependencies
    {
        internal static void Register(Container container, IConfiguration configuration)
        {
            var sqlSettings = configuration.GetSection(nameof(SqlSettings)).Get<SqlSettings>();
            if (sqlSettings == null)
            {
                throw new ArgumentNullException(nameof(SqlSettings));
            }

            container.RegisterInstance<SqlSettings>(sqlSettings);
            var uowRegistration = Lifestyle.Scoped.CreateRegistration<UnitOfWorkContext>(
                () => new UnitOfWorkContext(container.GetInstance<SqlSettings>()), container);
            container.AddRegistration(typeof(ICreateUnitOfWork), uowRegistration);
            container.AddRegistration(typeof(IGetConnection), uowRegistration);
            RegisterByConvention(typeof(MyProject.Storage.Users.UserRepository).Assembly, container, t => t.Name.EndsWith("Repository"));
        }

        private static void RegisterByConvention(Assembly assembly, Container container, Func<Type, bool> condition)
        {
            var registrations =
                from type in assembly.GetExportedTypes()
                where condition(type)
                from service in type.GetInterfaces()
                select new { service, implementation = type };

            foreach (var reg in registrations)
            {
                container.Register(reg.service, reg.implementation, Lifestyle.Scoped);
            }
        }
    }


