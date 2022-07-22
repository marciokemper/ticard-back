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
    internal class EmpresaFornecedorAppService : Interfaces.IEmpresaFornecedorAppService
    {
        private readonly IEmpresaFornecedorDomainService service;

        public EmpresaFornecedorAppService(IEmpresaFornecedorDomainService service)
        {
            this.service = service;
        }

        public EmpresaFornecedor Add(EmpresaFornecedor empresaFornecedor)
        {
            var result = service.Add(empresaFornecedor.MapTo<EmpresaFornecedor>());
            return result.MapTo<EmpresaFornecedor>();
        }

        public bool Remove(int id)
        {
            return service.Remove(id);
        }

        public EmpresaFornecedor GetById(int id)
        {
            return service.GetById(id).MapTo<EmpresaFornecedor>();
        }

        public List<EmpresaFornecedor> GetAll(EmpresaFornecedorFilterDto filter)
        {
            return service.GetAll(filter.MapTo<EmpresaFornecedorFilter>()).ToList();
        }

        public bool Update(EmpresaFornecedor empresaFornecedor)
        {
            return service.Update(empresaFornecedor.MapTo<EmpresaFornecedor>());
        }

        public List<EmpresaFornecedor> List(string filter)
        {
            return service.List(filter).ToList();

        }

        public List<EmpresaFornecedor> List(DataTableListaDto filter)
        {
            return service.List(filter.MapTo<DataTableFilter>()).ToList();
        }

        public List<EmpresaFornecedor> List(int empresa, DataTableListaDto filter)
        {
            return service.List(empresa,filter.MapTo<DataTableFilter>()).ToList();
        }

        public List<EmpresaFornecedor> GetEmpresaFornecedorByEmpresa(int empresaId)
        {
            return service.GetEmpresaFornecedorByEmpresa(empresaId).ToList();
        }

        public bool Remove(int empresa, int fornecedor)
        {
            return service.Remove(empresa, fornecedor);
        }

        public EmpresaFornecedor Add(int empresa, int fornecedor)
        {
            var result = service.Add(empresa, fornecedor);
            return result.MapTo<EmpresaFornecedor>();
        }
    }
}