using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.Domain.Interfaces.Repositories.Common
{
    public interface IRepositoryBase { }
    public interface IRepositoryBase<TEntity, TFiltro, TFiltroGrid> : IRepositoryBase
    {
        TEntity Add(TEntity obj);
        List<TEntity> List(TFiltroGrid filter);
        List<TEntity> GetAll(TFiltro filter);
        TEntity GetById(int id);
        bool Update(TEntity obj);
        bool Remove(int id);

    }

}
