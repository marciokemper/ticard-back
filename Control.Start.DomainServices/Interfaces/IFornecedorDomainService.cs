using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.DomainServices.Interfaces
{
    public interface IFornecedorDomainService
    {
        Fornecedor Add(Fornecedor fornecedor);
        List<Fornecedor> List(string filter);
        List<Fornecedor> ListByEmpresa(DataTableFilter filter, int? loggedEmpresaId);
        List<Fornecedor> GetAll(FornecedorFilter filter);
        Fornecedor GetById(int id);
        bool Update(Fornecedor fornecedor);
        bool Remove(int id);
        List<Fornecedor> getAllByAtivo();
        List<Fornecedor> ListSelecionadas(int empresaId);
        List<Fornecedor> ListNaoSelecionadas(int empresaId);

    }
}
