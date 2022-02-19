﻿using System;
using System.Data.SQLite;

namespace Uow.Sqlite.Database.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void RollBack();
        void Commit();
        SQLiteConnection Connection { get; }
        bool IsDisposed { get; }
    }
}