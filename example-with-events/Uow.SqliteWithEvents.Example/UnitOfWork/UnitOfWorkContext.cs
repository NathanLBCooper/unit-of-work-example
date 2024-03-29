﻿using System;
using System.Data;
using System.Data.SQLite;

namespace Uow.SqliteWithEvents.Example.UnitOfWork;

public class UnitOfWorkContext : ICreateUnitOfWork, IGetConnection
{
    private readonly ITransactionalEventPublisherFactory _transactionalEventPublisherFactory;
    private readonly SQLiteConnection _connection;
    private UnitOfWork? _unitOfWork;

    private bool IsUnitOfWorkOpen => !(_unitOfWork == null || _unitOfWork.IsDisposed);

    public UnitOfWorkContext(SQLiteConnection connection, ITransactionalEventPublisherFactory transactionalEventPublisherFactory)
    {
        _transactionalEventPublisherFactory = transactionalEventPublisherFactory;
        _connection = connection;
    }

    public IDbConnection GetConnection()
    {
        if (!IsUnitOfWorkOpen)
        {
            throw new InvalidOperationException(
                "There is not current unit of work from which to get a connection. Call Create first");
        }

        return _unitOfWork!.Connection;
    }

    public IEventPublisher GetEventPublisher()
    {
        if (!IsUnitOfWorkOpen)
        {
            throw new InvalidOperationException(
                "There is not current unit of work from which to get a event publisher. Call Create first");
        }

        return _unitOfWork!.EventPublisher;
    }

    public IUnitOfWork Create()
    {
        if (IsUnitOfWorkOpen)
        {
            throw new InvalidOperationException(
                "Cannot begin a transaction before the unit of work from the last one is disposed");
        }

        _unitOfWork = new UnitOfWork(_connection, _transactionalEventPublisherFactory);
        return _unitOfWork;
    }
}
