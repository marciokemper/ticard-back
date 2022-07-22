using Control.Facilites.Domain.Interfaces.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IUnitOfWorkTransaction Begin(params IRepositoryBase[] repositories);
    }
}
