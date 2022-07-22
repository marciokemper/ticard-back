using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Filters;
using Control.Facilites.Domain.Interfaces.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.Domain.Interfaces
{
    public interface IUsuarioDapperRepository : IRepositoryBase<Usuario, UsuarioFilter, Filters.DataTableFilter>  {

        Usuario Login(string login, string password);
        List<Usuario> ListByFornecedor(DataTableFilter filter, int? loggedfornecedorId, int? loggedEmpresaId);
        Usuario LoginReset(int id, string login, string password);
    }
}
