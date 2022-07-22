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
    internal class FornecedorDomainService : IFornecedorDomainService
    {
        private readonly IFornecedorDapperRepository repository;
        private readonly IUnitOfWork uow;

        public FornecedorDomainService(IFornecedorDapperRepository repository, IUnitOfWork uow)
        {
            this.repository = repository;
            this.uow = uow;
        }
        public Fornecedor Add(Fornecedor fornecedor)
        {
            Fornecedor result;
            using (var uowTransaction = this.uow.Begin(repository))
            {
                try
                {
                    result = repository.Add(fornecedor);
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

        public Fornecedor GetById(int id)
        {
            return repository.GetById(id);
        }

        public List<Fornecedor> GetAll(FornecedorFilter filter)
        {
            return repository.GetAll(filter);
        }

        public bool Update(Fornecedor fornecedor)
        {
            return repository.Update(fornecedor);
        }

        public List<Fornecedor> List(DataTableFilter filter)
        {
            return repository.List(filter);
        }

        public List<Fornecedor> List(string filter)
        {
            return repository.List(filter);
        }

        public List<Fornecedor> getAllByAtivo()
        {
            return repository.getAllByAtivo();
        }

        public List<Fornecedor> ListByEmpresa(DataTableFilter filter, int? loggedEmpresaId)
        {
            return repository.ListByEmpresa(filter, loggedEmpresaId);
        }

        public List<Fornecedor> ListSelecionadas(int empresaId)
        {
            return repository.ListSelecionadas(empresaId);
        }

        public List<Fornecedor> ListNaoSelecionadas(int empresaId)
        {
            return repository.ListNaoSelecionadas(empresaId);
        }
    }
}
