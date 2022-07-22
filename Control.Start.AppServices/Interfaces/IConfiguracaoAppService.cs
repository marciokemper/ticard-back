using Control.Facilites.AppServices.Dtos;
using Control.Facilites.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.AppServices.Interfaces
{
    public interface IConfiguracaoAppService
    {
        Configuracao Add(Configuracao configuracao);
        Configuracao Get(int id);
        bool Update(Configuracao cliente);
    }
}
