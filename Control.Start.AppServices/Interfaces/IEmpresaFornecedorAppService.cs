using Control.Facilites.AppServices.Dtos;
using Control.Facilites.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.AppServices.Interfaces
{
    public interface IEmpresaFornecedorAppService
    {
        EmpresaFornecedor Add(EmpresaFornecedor empresaFornecedor);
        List<EmpresaFornecedor> List(DataTableListaDto filter);
        List<EmpresaFornecedor> List(string filter);
        List<EmpresaFornecedor> GetAll(EmpresaFornecedorFilterDto filter);
        EmpresaFornecedor GetById(int id);
        bool Update(EmpresaFornecedor empresaFornecedor);
        bool Remove(int id);

        //

        List<EmpresaFornecedor> List(int empresa, DataTableListaDto filter);
        List<EmpresaFornecedor> GetEmpresaFornecedorByEmpresa(int empresaId);
        bool Remove(int empresa, int fornecedor);
        EmpresaFornecedor Add(int empresa, int fornecedor);

    }
}
