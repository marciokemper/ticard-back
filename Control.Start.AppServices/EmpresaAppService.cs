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
    internal class EmpresaAppService : Interfaces.IEmpresaAppService
    {
        private readonly IEmpresaDomainService service;

        public EmpresaAppService(IEmpresaDomainService service)
        {
            this.service = service;
        }

        public Empresa Add(Empresa empresa)
        {
            var result = service.Add(empresa.MapTo<Empresa>());
            return result.MapTo<Empresa>();
        }

        public bool Remove(int id)
        {
            return service.Remove(id);
        }

        public Empresa GetById(int id)
        {
            return service.GetById(id).MapTo<Empresa>();
        }

        public List<Empresa> GetAll(EmpresaFilterDto filter)
        {
            return service.GetAll(filter.MapTo<EmpresaFilter>()).ToList();
        }

        public bool Update(Empresa empresa)
        {
            return service.Update(empresa.MapTo<Empresa>());
        }

        public List<Empresa> List(string filter)
        {
            return service.List(filter).ToList();

        }

        public List<Empresa> List(DataTableListaDto filter)
        {
            return service.List(filter.MapTo<DataTableFilter>()).ToList();
        }

        public List<Empresa> List(int id, DataTableListaDto filter)
        {
            return service.List(id,filter.MapTo<DataTableFilter>()).ToList();
        }
    }
}