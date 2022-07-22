using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.DomainServices.Interfaces
{
    public interface IEmpresaFornecedorDomainService
    {
        EmpresaFornecedor Add(EmpresaFornecedor empresaFornecedor);
        List<EmpresaFornecedor> List(DataTableFilter filter);
        List<EmpresaFornecedor> List(string filter);
        List<EmpresaFornecedor> GetAll(EmpresaFornecedorFilter filter);
        EmpresaFornecedor GetById(int id);
        bool Update(EmpresaFornecedor empresaFornecedor);
        bool Remove(int id);

        //

        List<EmpresaFornecedor> List(int empresa, DataTableFilter filter);
        List<EmpresaFornecedor> GetEmpresaFornecedorByEmpresa(int empresaId);
        bool Remove(int empresa, int fornecedor);
        EmpresaFornecedor Add(int empresa, int fornecedor);


    }
}
