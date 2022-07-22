using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Filters;
using Control.Facilites.Domain.Interfaces.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.Domain.Interfaces
{
    public interface IEmpresaFornecedorDapperRepository : IRepositoryBase<EmpresaFornecedor, EmpresaFornecedorFilter, Filters.DataTableFilter>
    {
        List<EmpresaFornecedor> List(int empresa, Filters.DataTableFilter filter);
        List<EmpresaFornecedor> GetEmpresaFornecedorByEmpresa(int empresaId);
        bool Remove(int empresa, int fornecedor);
        EmpresaFornecedor Add(int empresa, int fornecedor);
    }
}
