using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Filters;
using Control.Facilites.Domain.Interfaces.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.Domain.Interfaces
{
    public interface IEmpresaDapperRepository : IRepositoryBase<Empresa, EmpresaFilter, Filters.DataTableFilter>
    {
        List<Empresa> List(DataTableFilter filter);

        List<Empresa> List(int id, DataTableFilter filter);
        List<Empresa> List(string filter);

    }
}
