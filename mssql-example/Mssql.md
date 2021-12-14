# Sql Server

This is an example of the unit pattern using Sql Server and an Event Publisher.

Unit of work isn't always just a transaction in a database. Sometimes there might be some other action (like sending an event, enqueuing a job etc) that ought to be done if and only if the transaction succeeds.



**TODO set up docker and get the tests to run**