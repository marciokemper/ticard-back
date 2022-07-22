using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.DomainServices.Interfaces
{
    public interface IUsuarioDomainService
    {
        Usuario Add(Usuario usuario);
        List<Usuario> List(DataTableFilter filter);
        List<Usuario> GetAll(UsuarioFilter filter);
        Usuario GetById(int id);
        bool Update(Usuario usuario);
        bool Remove(int id);
        List<Usuario> ListByFornecedor(DataTableFilter filter, int? loggedfornecedorId, int? loggedEmpresaId);
        Usuario Login(string login, string password);
        Usuario LoginReset(int id, string login, string password);

    }
}
