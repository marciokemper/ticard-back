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
    internal class ConfiguracaoDomainService : IConfiguracaoDomainService
    {
        private readonly IConfiguracaoDapperRepository repository;
        private readonly IUnitOfWork uow;

        public ConfiguracaoDomainService(IConfiguracaoDapperRepository repository, IUnitOfWork uow)
        {
            this.repository = repository;
            this.uow = uow;
        }
        public Configuracao Add(Configuracao configuracao)
        {
            Configuracao result;
            using (var uowTransaction = this.uow.Begin(repository))
            {
                try
                {
                    result = repository.Add(configuracao);
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

        public Configuracao GetById(int id)
        {
            return repository.GetById(id);
        }

        public bool Update(Configuracao configuracao)
        {
            return repository.Update(configuracao);
        }

        
    }
}
