using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Filters;
using Control.Facilites.Domain.Enum;
using Control.Facilites.Domain.Interfaces.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.Domain.Interfaces
{
    public interface IDocumentoDapperRepository : IRepositoryBase<Documento, DocumentoFilter, Filters.DataTableFilter>
    {

        List<Documento> GetAllByFilter(int? fornecedor, string Cnpj, int? cliente, int? situacao);
        List<dynamic> GetStatusArquivoColetado(int? LoggedclienteId, int? LoggedEmpresaId);
        List<dynamic> GetValorTipo(int? LoggedclienteId, int? LoggedEmpresaId);
        bool SetarSituacaoPaid(int fornecedorclienteid);
        List<dynamic> GetValorTipoMes(int? LoggedclienteId, int? LoggedEmpresaId);
        List<Documento> GetMesesByLoggedclienteId(int? LoggedclienteId, string centrosCusto);
        List<dynamic> GetUltimosArquivoColetado(int? LoggedclienteId, int? LoggedEmpresaId);
        List<Documento> ValidarItemIncluido(int fornecedorclienteid, string dtVencimento, decimal total);
        List<Documento> ValidarItemIncluidoByReferencia(int fornecedorclienteid, string referenceDate, decimal total);
        List<Documento> ValidarItemIncluidoManual(int clienteId, int fornecedorId, string dtVencimento, decimal total);
        bool UpdateSituacao(string arquivoColetadoId, string situacao);
        bool UpdateDadosPedido(string arquivoColetadoId, string numero, string data);
        List<dynamic> GetValorPorMes(int? LoggedclienteId, int? LoggedEmpresaId, int typeInvoice);
        Documento GetArquivoColetadoById(int clienteId, int fornecedorId);
        List<Documento> List(int? loggedfornecedorId, int? loggedEmpresaId, string palavra, DataTableFilter filter);
        List<dynamic> List(int? loggedclienteId, string centrosCusto);
        List<dynamic> GetCSVFilesDownload(int? LoggedclienteId, string centrosCusto, int? clienteId, string vencimentode, string vencimentoate, string tipo, string palavra, string situacao);
        List<Documento> List(int? clienteId, DateTime startDate);
        List<dynamic> GetValorPorTipo(int? LoggedclienteId, string centrosCusto);
        List<dynamic> GetValorPorUnidade(int? LoggedclienteId, string centrosCusto);
        List<dynamic> GetDocumentos(DateTime datade, DateTime dataate, string emissor, string cnpj);
        List<dynamic> GetCalendario(int? LoggedclienteId, int? LoggedEmpresaId);
        List<dynamic> GetNotificacoes(int? LoggedclienteId, string centrosCusto);
        void SendEmailNotificacoes();
        List<dynamic> ListByMes(int? LoggedclienteId, string ano, string mes, int? cliente, int? emissor, string situacao, DataTableFilter filter);
        List<Documento> ListByMesFornecedorCliente(int fornecedorClienteId, string ano, string mes, DataTableFilter filter);
        List<dynamic> ListResumoByMes(int? LoggedclienteId, string ano, string mes, int? cliente, int? emissor, string situacao, DataTableFilter filter);
        bool ClearLogExecute(int fornecedorclienteid);
        bool UpdateByIdsSituacao(string ids, string situacao);
        List<Documento> GetAllByFornecedor(int? loggedFornecedorId, int? loggedEmpresaId);
        bool AprovarDocumento(Documento notafiscal);
        bool ValidarNotaFiscal(Documento notafiscal);
        bool RejeitarDocumento(Documento notafiscal);
        Documento Add(Entities.ContasConfig config, Documento notafiscal);
        List<Documento> ListByNotaDebito(int? notadebito);
        List<Documento> GetAllByOficina(int oficina, string codigo);

    }
}
