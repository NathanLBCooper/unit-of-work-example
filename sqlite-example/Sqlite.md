# SQLite

This is an example of the unit pattern using SQLite.

In the `Uow.Sqlite.Database` project there is the unit of work pattern, an example entity and an example repository.
In the `Uow.Sqlite.Tests` project there is an example test, which you can run, that performs basic operations using the unit of work. The test runs completely in memory.

An unusual thing about this unit of work implementation is that sqlite needs the connection to stay open for the database to continue to exist. With another type of database, the lifetime of the connection would be shorter and managed by the UnitOfWork class.
