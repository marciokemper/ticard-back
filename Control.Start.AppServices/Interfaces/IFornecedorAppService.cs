using Control.Facilites.AppServices.Dtos;
using Control.Facilites.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.AppServices.Interfaces
{
    public interface IFornecedorAppService
    {
        Fornecedor Add(Fornecedor fornecedor);
        List<Fornecedor> List(DataTableListaDto filter);
        List<Fornecedor> ListByEmpresa(DataTableListaDto filter, int? loggedEmpresaId);
        Fornecedor GetById(int id);
        bool Update(Fornecedor fornecedor);
        bool Remove(int id);
        List<Fornecedor> List(string filter);
        List<Fornecedor> getAllByAtivo();
        List<Fornecedor> ListSelecionadas(int empresaId);
        List<Fornecedor> ListNaoSelecionadas(int empresaId);
    }
}
