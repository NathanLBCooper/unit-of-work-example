﻿using System;
using System.Data.SQLite;

namespace UnitOfWorkExample.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void RollBack();
        void Commit();
    }

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SQLiteTransaction _transaction;
        public SQLiteConnection Connection { get; }

        public bool IsDisposed { get; private set; } = false;

        public UnitOfWork(SQLiteConnection connection)
        {
            Connection = connection;
            _transaction = Connection.BeginTransaction();
        }

        public void RollBack()
        {
            _transaction.Rollback();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            _transaction?.Dispose();

            IsDisposed = true;
        }
    }
}
