using Control.Facilites.AppServices.Dtos;
using Control.Facilites.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.AppServices.Interfaces
{
    public interface IEmpresaAppService
    {
        Empresa Add(Empresa empresa);
        List<Empresa> List(string filter);
        Empresa GetById(int id);
        bool Update(Empresa empresa);
        bool Remove(int id);
        List<Empresa> List(DataTableListaDto filter);
        List<Empresa> List(int id, DataTableListaDto filter);

    }
}
