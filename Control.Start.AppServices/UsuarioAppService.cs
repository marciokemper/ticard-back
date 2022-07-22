using Control.Facilites.AppServices.Dtos;
using Control.Facilites.AppServices.Extensions;
using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Filters;
using Control.Facilites.DomainServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Control.Facilites.AppServices
{
    internal class UsuarioAppService : Interfaces.IUsuarioAppService
    {
        private readonly IUsuarioDomainService service;

        public UsuarioAppService(IUsuarioDomainService service)
        {
            this.service = service;
        }

        public Usuario Add(Usuario usuario)
        {
            var result = service.Add(usuario.MapTo<Usuario>());
            return result.MapTo<Usuario>();
        }

        public bool Remove(int id)
        {
            return service.Remove(id);
        }

        public Usuario GetById(int id)
        {
            return service.GetById(id).MapTo<Usuario>();
        }

        public List<Usuario> GetAll(UsuarioFilterDto filter)
        {
            return service.GetAll(filter.MapTo<UsuarioFilter>()).ToList();
        }

        public bool Update(Usuario usuario)
        {
            return service.Update(usuario.MapTo<Usuario>());
        }

        public Usuario Login(string login, string password)
        {
            return service.Login(login, password).MapTo<Usuario>();
        }

        public List<Usuario> List(DataTableListaDto filter)
        {
            return service.List(filter.MapTo<DataTableFilter>()).ToList();
        }

        public List<Usuario> ListByFornecedor(DataTableListaDto filter, int? loggedfornecedorId, int? loggedEmpresaId)
        {
            return service.ListByFornecedor(filter.MapTo<DataTableFilter>(), loggedfornecedorId, loggedEmpresaId).ToList();
        }

        public Usuario LoginReset(int id, string login, string password)
        {
            return service.LoginReset(id,login, password).MapTo<Usuario>();
        }
    }
}
