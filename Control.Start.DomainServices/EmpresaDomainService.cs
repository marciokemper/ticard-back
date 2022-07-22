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
    internal class EmpresaDomainService : IEmpresaDomainService
    {
        private readonly IEmpresaDapperRepository repository;
        private readonly IUnitOfWork uow;

        public EmpresaDomainService(IEmpresaDapperRepository repository, IUnitOfWork uow)
        {
            this.repository = repository;
            this.uow = uow;
        }
        public Empresa Add(Empresa empresa)
        {
            Empresa result;
            using (var uowTransaction = this.uow.Begin(repository))
            {
                try
                {
                    result = repository.Add(empresa);
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

        public Empresa GetById(int id)
        {
            return repository.GetById(id);
        }

        public List<Empresa> GetAll(EmpresaFilter filter)
        {
            return repository.GetAll(filter);
        }

        public bool Update(Empresa empresa)
        {
            return repository.Update(empresa);
        }

        public List<Empresa> List(string filter)
        {
            return repository.List(filter);
        }

        public List<Empresa> List(DataTableFilter filter)
        {
            return repository.List(filter);
        }

        public List<Empresa> List(int id, DataTableFilter filter)
        {
            return repository.List(id,filter);
        }
    }
}
