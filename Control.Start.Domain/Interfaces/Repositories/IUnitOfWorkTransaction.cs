using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.Domain.Interfaces.Repositories
{
    public interface IUnitOfWorkTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
