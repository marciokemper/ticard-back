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
    internal class ConfiguracaoAppService : Interfaces.IConfiguracaoAppService
    {
        private readonly IConfiguracaoDomainService service;

        public ConfiguracaoAppService(IConfiguracaoDomainService service)
        {
            this.service = service;
        }

        public Configuracao Add(Configuracao configuracao)
        {
            var result = service.Add(configuracao.MapTo<Configuracao>());
            return result.MapTo<Configuracao>();
        }

        public Configuracao Get(int id)
        {
            return service.GetById(id).MapTo<Configuracao>();
        }

        public bool Update(Configuracao configuracao)
        {
            return service.Update(configuracao.MapTo<Configuracao>());
        }
    }
   
}
