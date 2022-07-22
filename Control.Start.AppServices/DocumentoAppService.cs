using Control.Facilites.AppServices.Dtos;
using Control.Facilites.AppServices.Extensions;
using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Filters;
using Control.Facilites.Domain.Enum;
using Control.Facilites.DomainServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace Control.Facilites.AppServices
{
    internal class DocumentoAppService : Interfaces.IDocumentoAppService
    {
        private readonly IDocumentoDomainService service;

        public DocumentoAppService(IDocumentoDomainService service)
        {
            this.service = service;
        }

        public Documento Add(Documento arquivoColetado)
        {
            var result = service.Add(arquivoColetado.MapTo<Documento>());
            return result.MapTo<Documento>();
        }

        public bool Remove(int id)
        {
            return service.Remove(id);
        }

        public Documento GetById(int id)
        {
            return service.GetById(id).MapTo<Documento>();
        }

        public List<Documento> GetAll(DocumentoFilterDto filter)
        {
            return service.GetAll(filter.MapTo<DocumentoFilter>()).ToList();
        }

        public List<dynamic> GetStatusArquivoColetado(int? LoggedclienteId, int? LoggedEmpresaId)
        {
            return service.GetStatusArquivoColetado(LoggedclienteId, LoggedEmpresaId).EnumerableTo<dynamic>().ToList();
        }

        public List<dynamic> GetUltimosArquivoColetado(int? LoggedclienteId, int? LoggedEmpresaId)
        {
            return service.GetUltimosArquivoColetado(LoggedclienteId, LoggedEmpresaId).EnumerableTo<dynamic>().ToList();
        }

        public List<dynamic> GetValorPorMes(int? LoggedclienteId, int? LoggedEmpresaId, int typeInvoice)
        {
            return service.GetValorPorMes(LoggedclienteId, LoggedEmpresaId, typeInvoice).EnumerableTo<dynamic>().ToList();
        }

        public bool Update(Documento arquivoColetado)
        {
            return service.Update(arquivoColetado.MapTo<Documento>());
        }

        public List<Documento> List(DataTableListaDto filter)
        {
            return service.List(filter.MapTo<DataTableFilter>()).ToList();
        }


        public Documento GetArquivoColetadoById(int clienteId, int fornecedorId)
        {
            return service.GetArquivoColetadoById(clienteId, fornecedorId).MapTo<Documento>();
        }

        public List<Documento> List(int? clienteId, DateTime startDate)
        {
            return service.List(clienteId, startDate).ToList();
        }

        public bool UpdateSituacao(string arquivoColetadoId, string situacao)
        {
            return service.UpdateSituacao(arquivoColetadoId, situacao);
        }

        public List<Documento> GetMesesByLoggedclienteId(int? LoggedclienteId, string centrosCusto)
        {
            return service.GetMesesByLoggedclienteId(LoggedclienteId, centrosCusto).EnumerableTo<Documento>().ToList();
        }

        public List<Documento> ValidarItemIncluido(int fornecedorclienteid, string dtVencimento, decimal total)
        {
            return service.ValidarItemIncluido(fornecedorclienteid, dtVencimento, total).EnumerableTo<Documento>().ToList();

        }



        public List<dynamic> GetValorPorTipo(int? LoggedclienteId, string centrosCusto)
        {
            return service.GetValorPorTipo(LoggedclienteId, centrosCusto).EnumerableTo<dynamic>().ToList();
        }

        public List<dynamic> GetValorPorUnidade(int? LoggedclienteId, string centrosCusto)
        {
            return service.GetValorPorUnidade(LoggedclienteId, centrosCusto).EnumerableTo<dynamic>().ToList();
        }

        public bool UpdateDadosPedido(string arquivoColetadoId, string numero, string data)
        {
            return service.UpdateDadosPedido(arquivoColetadoId, numero, data);
        }

        public List<Documento> ValidarItemIncluidoManual(int clienteId, int fornecedorId, string dtVencimento, decimal total)
        {
            return service.ValidarItemIncluidoManual(clienteId, fornecedorId, dtVencimento, total).EnumerableTo<Documento>().ToList();
        }

        public List<dynamic> GetDocumentos(DateTime datade, DateTime dataate, string emissor, string cnpj)
        {
            return service.GetDocumentos(datade, dataate, emissor, cnpj).EnumerableTo<dynamic>().ToList();
        }

        public List<dynamic> GetCSVFilesDownload(int? LoggedclienteId, string centrosCusto, int? clienteId, string vencimentode, string vencimentoate, string tipo, string palavra, string situacao)
        {
            return service.GetCSVFilesDownload(LoggedclienteId, centrosCusto, clienteId, vencimentode, vencimentoate, tipo, palavra, situacao);
        }

        public List<dynamic> GetCalendario(int? LoggedclienteId, int? LoggedEmpresaId)
        {
            return service.GetCalendario(LoggedclienteId, LoggedEmpresaId).EnumerableTo<dynamic>().ToList();
        }

        public List<dynamic> List(int? loggedclienteId, string centrosCusto)
        {
            return service.List(loggedclienteId, centrosCusto).EnumerableTo<dynamic>().ToList();
        }

        public List<dynamic> GetNotificacoes(int? LoggedclienteId, string centrosCusto)
        {
            return service.GetNotificacoes(LoggedclienteId, centrosCusto).EnumerableTo<dynamic>().ToList();
        }

        public void SendEmailNotificacoes()
        {
            service.SendEmailNotificacoes();
        }

        public List<dynamic> GetValorTipo(int? LoggedclienteId, int? LoggedEmpresaId)
        {
            return service.GetValorTipo(LoggedclienteId, LoggedEmpresaId).EnumerableTo<dynamic>().ToList();
        }

        public List<dynamic> GetValorTipoMes(int? LoggedclienteId, int? LoggedEmpresaId)
        {
            return service.GetValorTipoMes(LoggedclienteId, LoggedEmpresaId).EnumerableTo<dynamic>().ToList();
        }

        public List<Documento> ValidarItemIncluidoByReferencia(int fornecedorclienteid, string referenceDate, decimal total)
        {
            return service.ValidarItemIncluidoByReferencia(fornecedorclienteid, referenceDate, total).EnumerableTo<Documento>().ToList();
        }

        public bool SetarSituacaoPaid(int fornecedorclienteid)
        {
            return service.SetarSituacaoPaid(fornecedorclienteid);
        }

        public List<dynamic> ListByMes(int? LoggedclienteId, string ano, string mes, int? cliente, int? emissor, string situacao, DataTableListaDto filter)
        {
            return service.ListByMes(LoggedclienteId, ano, mes, cliente, emissor, situacao, filter.MapTo<DataTableFilter>()).EnumerableTo<dynamic>().ToList();
        }

        public List<Documento> ListByMesFornecedorCliente(int fornecedorClienteId, string ano, string mes, DataTableListaDto filter)
        {
            return service.ListByMesFornecedorCliente(fornecedorClienteId, ano, mes, filter.MapTo<DataTableFilter>()).ToList();
        }

        public List<dynamic> ListResumoByMes(int? LoggedclienteId, string ano, string mes, int? cliente, int? emissor, string situacao, DataTableListaDto filter)
        {
            return service.ListResumoByMes(LoggedclienteId, ano, mes, cliente, emissor, situacao, filter.MapTo<DataTableFilter>()).EnumerableTo<dynamic>().ToList();
        }

        public bool ClearLogExecute(int fornecedorclienteid)
        {
            return service.ClearLogExecute(fornecedorclienteid);
        }

        public bool UpdateByIdsSituacao(string ids, string situacao)
        {
            return service.UpdateByIdsSituacao(ids, situacao);
        }

        public List<Documento> List(int? loggedfornecedorId, int? loggedEmpresaId, string palavra, DataTableListaDto filter)
        {
            return service.List(loggedfornecedorId, loggedEmpresaId, palavra, filter.MapTo<DataTableFilter>()).EnumerableTo<Documento>().ToList();
        }

        public List<Documento> GetAllByFornecedor(int? loggedFornecedorId, int? loggedEmpresaId)
        {
            return service.GetAllByFornecedor(loggedFornecedorId, loggedEmpresaId).ToList();
        }

        public List<Documento> GetAllByOficina(int oficina, string codigo)
        {
            return service.GetAllByOficina(oficina, codigo).ToList();
        }

        public bool AprovarDocumento(Documento documento)
        {
            return service.AprovarDocumento(documento.MapTo<Documento>());
        }
        public bool ValidarNotaFiscal(Documento notafiscal)
        {
            return service.ValidarNotaFiscal(notafiscal.MapTo<Documento>());
        }
        public bool RejeitarDocumento(Documento documento)
        {
            return service.RejeitarDocumento(documento.MapTo<Documento>());
        }

        public Documento Add(ContasConfig config, Documento notafiscal)
        {
            var result = service.Add(config, notafiscal.MapTo<Documento>());
            return result.MapTo<Documento>();
        }

        public List<Documento> ListByNotaDebito(int? notadebito)
        {
            return service.ListByNotaDebito(notadebito).ToList();
        }

        public List<Documento> GetAllByFilter(int? fornecedor, string Cnpj, int? cliente, int? situacao)
        {
            return service.GetAllByFilter(fornecedor, Cnpj, cliente, situacao).ToList();
        }
    }
}
