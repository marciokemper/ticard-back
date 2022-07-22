using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Filters;
using Control.Facilites.Domain.Interfaces.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.Domain.Interfaces
{
    public interface IFornecedorDapperRepository : IRepositoryBase<Fornecedor, FornecedorFilter, Filters.DataTableFilter>
    {
        List<Fornecedor> List(string filter);
        List<Fornecedor> getAllByAtivo();
        List<Fornecedor> ListByEmpresa(DataTableFilter filter, int? loggedEmpresaId);

        List<Fornecedor> ListSelecionadas(int empresaId);
        List<Fornecedor> ListNaoSelecionadas(int empresaId);
    }
}
