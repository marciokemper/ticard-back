using Control.Facilites.AppServices.Dtos;
using Control.Facilites.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.AppServices.Interfaces
{
    public interface IUsuarioAppService
    {
        Usuario Add(Usuario usuario);
        List<Usuario> List(DataTableListaDto filter);
        List<Usuario> GetAll(UsuarioFilterDto filter);
        List<Usuario> ListByFornecedor(DataTableListaDto filter, int? loggedfornecedorId, int? loggedEmpresaId);
        Usuario GetById(int id);
        bool Update(Usuario usuario);
        bool Remove(int id);
        Usuario Login(string login, string password);
        Usuario LoginReset(int id, string login, string password);


    }
}
