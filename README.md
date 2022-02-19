# unit-of-work-example

This is my preferred **Unit of Work** pattern.

> Skip to [My Pattern](#my-pattern) to skip the essay and see the pattern

> Skip to [SQLite Example](#sqlite-example)

<br/>

## Background

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

## My pattern

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

## SQLite Example

Folder: *sqlite-example/*

Basic Sqlite in-memory database example. It just works, just press play on the tests.

<br/>

## SQLite Example with a non-database action in the transaction

Folder: *sqlite-example-with-events/*

Another simple in-memory Sqlite example. This time, shows  how to include non-database actions (event publishing) in the unit of work. Unit of work isn't, after all, always just an abstraction of a database transaction.

A transactional event publisher is included in the unit of work, so that events can commit and be rolled back along with the database changes. The event publisher is very basic in this example, but on commit it could do something like send messages to rabbitmq or enqueue jobs in hangfire.

<br/>

## SQL Server Example

Folder: *mssql-example/*

An example using Sql Server. A quirk of using Sql Server is that the transaction must be passed into Dapper separately from the connection. To make this possible the interface `IGetUnitOfWork ` is expanded to return the transaction in addition to the connection.

Requires Docker. Run `docker-compose` to start sql server container needed for tests. Then just press play on the tests as usual.

-----

## Potential future examples:

Postgres, show in the context of an asp.net app, non-dapper example (EF Core?)

Share examples (in readme?) of how to put in DI

TODO have a simple SQLServer example