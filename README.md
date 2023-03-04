# unit-of-work-example

This is my preferred **Unit of Work** pattern.

> Skip to [My Pattern](#my-pattern) to skip the essay and see the pattern

> Skip to [SQLite Example](#sqlite-example)

<br/>

# Background

A unit of work is something that tracks the changes made during a business transaction, coordinates the writing of those changes and resolves concurrency problems. It allows the changes to be written or, in the event of failure not written, together as a single unit.

This repository mostly uses databases as the place changes are written. It uses [Dapper](https://github.com/DapperLib/Dapper), an object-relational mapping for .NET. But the pattern is applicable to different technologies, languages and storage mechanisms.

When using Dapper, I wish to write my 'database code' in repositories, but I need to combine multiple calls to multiple repositories together in a single unit of work in my 'business code'. I don't see a single repository method as being the entire transaction as I want to keep writing simple repository methods, even when I have business transactions that have multiple consequences. So I need some kind of abstraction to control that unit of work for the code that uses my repositories.

Before coming up with my pattern, I looked at other ones. I didn't like most of  them, but I feel the approach that Entity Framework takes is simple and makes a lot of sense. Make changes on the `DBContext`, save them and you're done.

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

Exactly the same approach is possible in Dapper. Just the DBSets with your repositories and have your DBContext manage the transaction.

It's certainly simple. If that works for you, great, you don't need this pattern. I don't love that it requires the caller to always have access to a container of all possible repositories. I think it's a bit of a God object. I prefer to just inject the individual repositories into my business code, rather than having everything always just a "`.`"  away.

So instead, I chose to do something else.

<br/>

# My pattern

Using a unit of work in the business code looks like this:

    using (var uow = _createUnitOfWork.Create())
    {
        await _fooRepository.Create(foo);
        await _barRepository.Create(bar);
        await uow.Commit();
    }

The repositories and the `ICreateUnitOfWork` are are passed into the business objects separately.

    public MyClass(ICreateUnitOfWork createUnitOfWork, FooRepository fooRepository, BarRepository barRepository) {...}

That's just the usuage. There is a relationship between repositories and the `ICreateUnitOfWork`, but that is defined higher up, typically in the IOC container. The examples will show how this is done.

<br/>

# Examples

## 01: Basic Example using SQLite and Dapper

Folder: *01-basic-example/*

This is the most basic example. It uses Sqlite, an in-memory database, and Dapper. This should be your entry point for understanding this pattern.

This code contains an Entity, a Repository, a Controller and the unit of work pattern. There are unit tests on both the Controller and the Repository. These tests can be run without any special setup.

### Notes:

There is one strange quirk about how this example is written. In Sqlite the database only exists for as long as the connection exists. This makes `SQLiteConnection` a very long lived object. The other non-Sqlite examples create and destroy the connection alongside the transaction inside the `UnitOfWork`.

The code is split up into `Application` and `Storage`, where `Storage` depends on `Application`. The first folder contains the parts of this pattern that need to be visible to all the code that uses Unit Of Work. The second contains the parts that are only needed internally by the repositories. The Controller class does not need to reference Storage to use `ICreateUnitOfWork`, so this pattern supports architectures where database code is decoupled.

<br/>

## 02: Example with events in the transaction

Folder: *02-example-with-events/*

Again, this uses Sqlite and Dapper. This time however, it also publishes events alongside the database changes. This an example of how non-database actions can included in the unit of work. This shows how Unit of work is a bigger concept than just a database transactions.

I have used this pattern in real applications, where I have used a transactional event publisher to send messages to RabbitMq and enqueue work on Hangfire.

<br/>

## PostgreSQL

Folder *postgresql-example/*

An example using PostgreSQL.

Requires Docker. Run `docker-compose` to start postgresql container needed for tests. Then just press play on the tests as usual.

<br/>

## SQL Server Example

Folder: *mssql-example/*

An example using Sql Server. A quirk of using Sql Server is that the transaction must be passed into Dapper separately from the connection. To make this possible the interface `IGetUnitOfWork ` is expanded to return the transaction in addition to the connection.

Requires Docker. Run `docker-compose` to start sql server container needed for tests. Then just press play on the tests as usual.


# How to use this in a dependancy injection container

Here is a brief snippet of how I've used this with SimpleInjector in a hosted service. I'm not sure this is the best code and you might be using a different DI library, or the same one in a different context. But it demonstrates what's important, which is that the same object must be registered as both `ICreateUnitOfWork` and `IGetUnitOfWork` so the magic connection between the unit of work and the repositories can happen.

```
private static void ConfigureDependencies(Container container, IConfiguration configuration)
        {
            ...

            container.RegisterInstance(configuration.GetSection(nameof(SqlSettings)).Get<SqlSettings>());
            var uowRegistration = Lifestyle.Scoped.CreateRegistration<UnitOfWorkContext>(
                () => new UnitOfWorkContext(container.GetInstance<SqlSettings>()), container);
            container.AddRegistration(typeof(ICreateUnitOfWork), uowRegistration);
            container.AddRegistration(typeof(IGetUnitOfWork), uowRegistration);
            container.Register<IEntityRepository, EntityRepository>(Lifestyle.Scoped);
            container.Register(typeof(IProcessedEventRepository<>), typeof(ProcessedEventRepository<>), Lifestyle.Scoped);
			...
        }
```

