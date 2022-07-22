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
    internal class FornecedorAppService : Interfaces.IFornecedorAppService
    {
        private readonly IFornecedorDomainService service;

        public FornecedorAppService(IFornecedorDomainService service)
        {
            this.service = service;
        }

        public Fornecedor Add(Fornecedor fornecedor)
        {
            var result = service.Add(fornecedor.MapTo<Fornecedor>());
            return result.MapTo<Fornecedor>();
        }

        public bool Remove(int id)
        {
            return service.Remove(id);
        }

        public Fornecedor GetById(int id)
        {
            return service.GetById(id).MapTo<Fornecedor>();
        }

        public List<Fornecedor> GetAll(FornecedorFilterDto filter)
        {
            return service.GetAll(filter.MapTo<FornecedorFilter>()).ToList();
        }

        public List<Fornecedor> ListByEmpresa(DataTableListaDto filter, int? loggedEmpresaId)
        {
            return service.ListByEmpresa(filter.MapTo<DataTableFilter>(), loggedEmpresaId).ToList();
        }

        public bool Update(Fornecedor fornecedor)
        {
            return service.Update(fornecedor.MapTo<Fornecedor>());
        }


        public List<Fornecedor> List(string filter)
        {
            return service.List(filter).ToList();
        }

        public List<Fornecedor> getAllByAtivo()
        {
            return service.getAllByAtivo().ToList();
        }

        public List<Fornecedor> List(DataTableListaDto filter)
        {
            throw new NotImplementedException();
        }

        public List<Fornecedor> ListSelecionadas(int empresaId)
        {
            return service.ListSelecionadas(empresaId).ToList();
        }

        public List<Fornecedor> ListNaoSelecionadas(int empresaId)
        {
            return service.ListNaoSelecionadas(empresaId).ToList();
        }
    }
}