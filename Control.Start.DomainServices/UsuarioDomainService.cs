using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Filters;
using Control.Facilites.Domain.Interfaces;
using Control.Facilites.Domain.Interfaces.Repositories;
using Control.Facilites.DomainServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.DomainServices
{
    internal class UsuarioDomainService : IUsuarioDomainService
    {
        private readonly IUsuarioDapperRepository repository;
        private readonly IUnitOfWork uow;

        public UsuarioDomainService(IUsuarioDapperRepository repository, IUnitOfWork uow)
        {
            this.repository = repository;
            this.uow = uow;
        }
        public Usuario Add(Usuario usuario)
        {
            Usuario result;
            using (var uowTransaction = this.uow.Begin(repository))
            {
                try
                {
                    result = repository.Add(usuario);
                    uowTransaction.Commit();
                }
                catch 
                {
                    uowTransaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        public bool Remove(int id)
        {
            return repository.Remove(id);
        }

        public Usuario GetById(int id)
        {
            return repository.GetById(id);
        }

        public List<Usuario> GetAll(UsuarioFilter filter)
        {
            return repository.GetAll(filter);
        }

        public bool Update(Usuario usuario)
        {
            return repository.Update(usuario);
        }

        public Usuario Login(string login, string password)
        {
            return repository.Login(login, password);
        }

        public List<Usuario> List(DataTableFilter filter)
        {
            return repository.List(filter);
        }

        public List<Usuario> ListByFornecedor(DataTableFilter filter, int? loggedfornecedorId, int? loggedEmpresaId)
        {
            return repository.ListByFornecedor(filter, loggedfornecedorId, loggedEmpresaId);
        }

        public Usuario LoginReset(int id, string login, string password)
        {
            return repository.LoginReset(id,login, password);
        }
    }
}
