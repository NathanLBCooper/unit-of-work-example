﻿namespace Uow.Sqlite.Database.UnitOfWork
{
    public interface ICreateUnitOfWork
    {
        IUnitOfWork Create();
    }
}