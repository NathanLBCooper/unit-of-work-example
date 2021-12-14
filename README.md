# unit-of-work-example

This is my preferred **Unit of Work** pattern.

I have used this with Dapper and will show it with Dapper examples, but the pattern can be used with other data access technologies, or indeed with different languages. Pretty much anywhere when you want to tie together multiple operations together so that they succeed or fail as a unit... of work.

Skip to [My approach](#my-approach) to skips the reasoning and see the pattern.

Skip to [Example: Basic SQLite example](#example-basic-sqlite-example) to see a basic example of the pattern.

## Goal

When I use dapper, I write my sql code in repository methods and I tie together related changes that need to happen together in my business code. I'm not a fan of using a unit of work inside a repository. As far as I'm concerned, repository methods are simple building blocks for business code. So I need some kind of abstraction outside the repository that's easy to use.

The first thing I did before I wrote my own "unit-of-work-example" was to see if I could just copy someone else's pattern.

### The EF approach:

Entity Framework takes a simple approach with its `DBContext`. Internally to the DBContext there is a transaction: You just save when you're done.

    public class MyDBContext : DbContext
    {
        ...
        public DbSet<Foo> Foos { get; set; }
        public DbSet<Bar> Bars { get; set; }
    }

    _myDbContext.Foos.Add(foo);
    _myDbContext.Bars.Add(bar);
    _myDbContext.SaveChanges();

You can do the exact same thing with your own SQL and Dapper. With a Context managing the state of what transaction you're in and returning Repositories that will make queries within that transaction.

    public class MyDBContext
    {
        ...
        FooRepository FooRepo { get; } => new FooRepo(uowObject)
        BarRepository BarRepo { get; } => new BarRepo(uowObject)

        // Where uowObject is some kind of object containing a connection with a transaction. Or some kind of lazy equivalent of that, since we don't really want a {get} opening a connection.

        void SaveChanges(); // Save changes and reset the context so it can be used again
    }

And done. There is the Entity-framework unit of work pattern... sort of... I haven't filled in the gaps.

And that's because I don't love that I have to pass around a object containing all the repositories the caller could ever possibly want. I think it's a god object. I prefer to inject individual repositories to make it clear what a class in my code needs to do, rather than all the repositories being one "`.`" away at all times.

So no. I don't like this pattern.

What's my approach?


## My approach

I choose to have something that expressed a transaction happening like this:

    public async Task DoBusiness(Foo foo, Bar bar)
    {
        using (var uow = _createUnitOfWork.Create())
        {
            await _fooRepository.Create(foo);
            await _barRepository.Create(bar);
            await uow.Commit();
        }
    }

which still enables one to pass around repositories independently, as long as there is reference to the context as well. 

    public MyBusinessCode(ICreateUnitOfWork createUnitOfWork, FooRepository fooRepository, BarRepository barRepository)
    {
        _createUnitOfWork = createUnitOfWork;
        _fooRepository = fooRepository;
        _barRepository = barRepository;
    }

Obviously the repositories and the object are connected in some manner. This pattern will show how it's done through examples.

<br/>

## Example: Basic SQLite example

Folder: *sqlite-example/*

A simple, easy to run, basic example using a Sqlite in-memory database. Start by looking at the `Example` class for usage.

<br/>

## Example: Sql Server example with a non-database action tied to the transaction

Folder: *mssql-example/*

An example using Sql Server and an example of how to include non-database actions (event publishing) in the unit of work. Unit of work isn't, after all, always just an abstraction of a database transaction.

A complication of using SQL is that the transaction must be passed into Dapper separately from the connection. To make this possible the interface `IGetUnitOfWork ` is expanded from `DBConnection GetConnection()` to `(IDbConnection connection, IDbTransaction transaction) GetConnection()`.

A transactional event publisher is also made available to the user of the unit of work, so that they can send events that will commit and rollback alongside the database requests.

-----

## Potential future examples:

Postgres, show in the context of an asp.net app, non-dapper example (EF Core?)

Share examples (in readme?) of how to put in DI
