using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Filters;
using Control.Facilites.Domain.Interfaces;
using Control.Facilites.Domain.Interfaces.Repositories;
using Control.Facilites.DomainServices.Interfaces;
using Control.Facilites.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.DomainServices
{
    internal class DocumentoDomainService : IDocumentoDomainService
    {
        private readonly IDocumentoDapperRepository repository;
        private readonly IUnitOfWork uow;

        public DocumentoDomainService(IDocumentoDapperRepository repository, IUnitOfWork uow)
        {
            this.repository = repository;
            this.uow = uow;
        }
        public Documento Add(Documento arquivoColetado)
        {
            Documento result;
            using (var uowTransaction = this.uow.Begin(repository))
            {
                try
                {
                    result = repository.Add(arquivoColetado);
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

        public bool Remove(int id)
        {
            return repository.Remove(id);
        }

        public Documento GetById(int id)
        {
            return repository.GetById(id);
        }

        public List<Documento> GetAll(DocumentoFilter filter)
        {
            return repository.GetAll(filter);
        }

        public List<dynamic> GetStatusArquivoColetado(int? LoggedclienteId, int? LoggedEmpresaId)
        {
            return repository.GetStatusArquivoColetado(LoggedclienteId, LoggedEmpresaId);
        }

        public List<dynamic> GetUltimosArquivoColetado(int? LoggedclienteId, int? LoggedEmpresaId)
        {
            return repository.GetUltimosArquivoColetado(LoggedclienteId, LoggedEmpresaId);
        }

        public List<dynamic> GetValorPorMes(int? LoggedclienteId, int? LoggedEmpresaId, int typeInvoice)
        {
            return repository.GetValorPorMes(LoggedclienteId, LoggedEmpresaId,typeInvoice);
        }

        public bool Update(Documento arquivoColetado)
        {
            return repository.Update(arquivoColetado);
        }

        public List<Documento> List(DataTableFilter filter)
        {
            return repository.List(filter);
        }

        public Documento GetArquivoColetadoById(int clienteId, int fornecedorId)
        {
            return repository.GetArquivoColetadoById(clienteId, fornecedorId);
        }

        public List<Documento> List(int? clienteId, DateTime startDate)
        {
            return repository.List(clienteId, startDate);
        }

        public bool UpdateSituacao(string arquivoColetadoId, string situacao)
        {
            return repository.UpdateSituacao(arquivoColetadoId, situacao);
        }

        public List<Documento> GetMesesByLoggedclienteId(int? LoggedclienteId, string centrosCusto)
        {
            return repository.GetMesesByLoggedclienteId(LoggedclienteId, centrosCusto);
        }

        public List<Documento> ValidarItemIncluido(int fornecedorclienteid, string dtVencimento, decimal total)
        {
            return repository.ValidarItemIncluido(fornecedorclienteid, dtVencimento, total);
        }

        public List<dynamic> GetValorPorTipo(int? LoggedclienteId, string centrosCusto)
        {
            return repository.GetValorPorTipo(LoggedclienteId, centrosCusto);
        }

        public List<dynamic> GetValorPorUnidade(int? LoggedclienteId, string centrosCusto)
        {
            return repository.GetValorPorUnidade(LoggedclienteId, centrosCusto);
        }

        public bool UpdateDadosPedido(string arquivoColetadoId, string numero, string data)
        {
            return repository.UpdateDadosPedido(arquivoColetadoId, numero, data);
        }

        public List<Documento> ValidarItemIncluidoManual(int clienteId, int fornecedorId, string dtVencimento, decimal total)
        {
            return repository.ValidarItemIncluidoManual(clienteId, fornecedorId, dtVencimento, total);
        }

        public List<dynamic> GetDocumentos(DateTime datade, DateTime dataate, string emissor, string cnpj)
        {
            return repository.GetDocumentos(datade, dataate, emissor, cnpj);
        }

        public List<dynamic> GetCSVFilesDownload(int? LoggedclienteId, string centrosCusto, int? clienteId, string vencimentode, string vencimentoate, string tipo, string palavra, string situacao)
        {
            return repository.GetCSVFilesDownload(LoggedclienteId, centrosCusto, clienteId, vencimentode, vencimentoate, tipo, palavra, situacao);
        }

        public List<dynamic> GetCalendario(int? LoggedclienteId, int? LoggedEmpresaId)
        {
            return repository.GetCalendario(LoggedclienteId, LoggedEmpresaId);
        }

        public List<dynamic> List(int? loggedclienteId, string centrosCusto)
        {
            return repository.List(loggedclienteId, centrosCusto);
        }

        public List<dynamic> GetNotificacoes(int? LoggedclienteId, string centrosCusto)
        {
            return repository.GetNotificacoes(LoggedclienteId, centrosCusto);
        }

        public void SendEmailNotificacoes()
        {
            repository.SendEmailNotificacoes();
        }

        public List<dynamic> GetValorTipo(int? LoggedclienteId, int? LoggedEmpresaId)
        {
            return repository.GetValorTipo(LoggedclienteId, LoggedEmpresaId);
        }

        public List<dynamic> GetValorTipoMes(int? LoggedclienteId, int? LoggedEmpresaId)
        {
            return repository.GetValorTipoMes(LoggedclienteId, LoggedEmpresaId);
        }

        public List<Documento> ValidarItemIncluidoByReferencia(int fornecedorclienteid, string referenceDate, decimal total)
        {
            return repository.ValidarItemIncluidoByReferencia(fornecedorclienteid, referenceDate, total);
        }

        public bool SetarSituacaoPaid(int fornecedorclienteid)
        {
            return repository.SetarSituacaoPaid(fornecedorclienteid);
        }

        public List<dynamic> ListByMes(int? LoggedclienteId, string ano, string mes, int? cliente, int? emissor, string situacao, DataTableFilter filter)
        {
            return repository.ListByMes(LoggedclienteId, ano, mes, cliente, emissor, situacao, filter);
        }

        public List<Documento> ListByMesFornecedorCliente(int fornecedorClienteId, string ano, string mes, DataTableFilter filter)
        {
            return repository.ListByMesFornecedorCliente(fornecedorClienteId, ano, mes, filter);
        }

        public List<dynamic> ListResumoByMes(int? LoggedclienteId, string ano, string mes, int? cliente, int? emissor, string situacao, DataTableFilter filter)
        {
            return repository.ListResumoByMes(LoggedclienteId, ano, mes, cliente, emissor, situacao, filter);
        }

        public bool ClearLogExecute(int fornecedorclienteid)
        {
            return repository.ClearLogExecute(fornecedorclienteid);
        }

        public bool UpdateByIdsSituacao(string ids, string situacao)
        {
            return repository.UpdateByIdsSituacao(ids, situacao);
        }

        public List<Documento> List(int? loggedfornecedorId, int? loggedEmpresaId, string palavra, DataTableFilter filter)
        {
            return repository.List(loggedfornecedorId, loggedEmpresaId, palavra, filter);
        }

        public List<Documento> GetAllByFornecedor(int? loggedFornecedorId, int? loggedEmpresaId)
        {
            return repository.GetAllByFornecedor(loggedFornecedorId, loggedEmpresaId);
        }

        public List<Documento> GetAllByOficina(int oficina, string codigo)
        {
            return repository.GetAllByOficina(oficina, codigo);
        }

        public bool AprovarDocumento(Documento documento)
        {
            return repository.AprovarDocumento(documento);
        }
        public bool ValidarNotaFiscal(Documento notafiscal)
        {
            return repository.ValidarNotaFiscal(notafiscal);
        }
        public bool RejeitarDocumento(Documento documento)
        {
            return repository.RejeitarDocumento(documento);
        }

        public Documento Add(ContasConfig config, Documento notafiscal)
        {
            Documento result;
            using (var uowTransaction = this.uow.Begin(repository))
            {
                try
                {
                    result = repository.Add(config, notafiscal);
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

        public List<Documento> ListByNotaDebito(int? notadebito)
        {
            return repository.ListByNotaDebito(notadebito);
        }

        public List<Documento> GetAllByFilter(int? fornecedor, string Cnpj, int? cliente, int? situacao)
        {
            return repository.GetAllByFilter(fornecedor, Cnpj, cliente, situacao);
        }
    }
}
