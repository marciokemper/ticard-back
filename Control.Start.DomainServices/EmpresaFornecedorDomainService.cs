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
    internal class EmpresaFornecedorDomainService : IEmpresaFornecedorDomainService
    {
        private readonly IEmpresaFornecedorDapperRepository repository;
        private readonly IUnitOfWork uow;

        public EmpresaFornecedorDomainService(IEmpresaFornecedorDapperRepository repository, IUnitOfWork uow)
        {
            this.repository = repository;
            this.uow = uow;
        }
        public EmpresaFornecedor Add(EmpresaFornecedor empresaFornecedor)
        {
            EmpresaFornecedor result;
            using (var uowTransaction = this.uow.Begin(repository))
            {
                try
                {
                    result = repository.Add(empresaFornecedor);
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

        public EmpresaFornecedor GetById(int id)
        {
            return repository.GetById(id);
        }

        public List<EmpresaFornecedor> GetAll(EmpresaFornecedorFilter filter)
        {
            return repository.GetAll(filter);
        }

        public bool Update(EmpresaFornecedor empresaFornecedor)
        {
            return repository.Update(empresaFornecedor);
        }

        public List<EmpresaFornecedor> List(DataTableFilter filter)
        {
            return repository.List(filter);
        }


        public List<EmpresaFornecedor> List(int empresa, DataTableFilter filter)
        {
            return repository.List(empresa,filter);
        }

        public List<EmpresaFornecedor> GetEmpresaFornecedorByEmpresa(int empresaId)
        {
            throw new NotImplementedException();
        }

        public List<EmpresaFornecedor> List(string filter)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int empresa, int fornecedor)
        {
            return repository.Remove(empresa, fornecedor);
        }

        public EmpresaFornecedor Add(int empresa, int fornecedor)
        {
            EmpresaFornecedor result;
            using (var uowTransaction = this.uow.Begin(repository))
            {
                try
                {
                    result = repository.Add(empresa, fornecedor);
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
    }
}
