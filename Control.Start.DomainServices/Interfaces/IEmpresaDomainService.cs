using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.DomainServices.Interfaces
{
    public interface IEmpresaDomainService
    {
        Empresa Add(Empresa empresa);
        List<Empresa> List(DataTableFilter filter);
        List<Empresa> List(string filter);
        List<Empresa> List(int id, DataTableFilter filter);
        List<Empresa> GetAll(EmpresaFilter filter);
        Empresa GetById(int id);
        bool Update(Empresa empresa);
        bool Remove(int id);
 


    }
}
