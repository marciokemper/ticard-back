using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.DomainServices.Interfaces
{
    public interface IConfiguracaoDomainService
    {
        Configuracao Add(Configuracao configuracao);
        Configuracao GetById(int id);
        bool Update(Configuracao configuracao);

    }
}
