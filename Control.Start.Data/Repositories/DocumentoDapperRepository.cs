using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Enum;
using Control.Facilites.Domain.Filters;
using Control.Facilites.Domain.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;


namespace Control.Facilites.Data.Repositories
{
    internal class DocumentoDapperRepository : RepositoryBase, IDocumentoDapperRepository
    {
        public DocumentoDapperRepository(IConfigurationRoot configuration) : base(configuration)
        {
        }

        public Documento Add(Domain.Entities.ContasConfig config, Documento docto)
        {

            try
            {
                docto.CaminhoAnexo = Control.Facilites.Events.Utils.Upload.File(docto.CaminhoAnexo, "", docto, false, config);

                var sb = new StringBuilder();
                sb.Append("INSERT INTO documento (EmpresaFornecedor, Ativo,DataInclusao, SituacaoDocumento,CaminhoAnexo) VALUES");
                sb.Append(" (@EmpresaFornecedor, @Ativo,@DataInclusao, @SituacaoDocumento,@CaminhoAnexo);");
                sb.Append(" SELECT LAST_INSERT_ID();");
                docto.Id = Connection.QueryFirst<int>(sb.ToString(),
                    new
                    {
                        EmpresaFornecedor = docto.EmpresaFornecedor.Id,
                        Ativo = 1,
                        DataInclusao = DateTime.Now,
                        SituacaoDocumento = Convert.ToInt32(FluxoDocumento.Aguardando),
                        CaminhoAnexo = string.IsNullOrEmpty(docto.CaminhoAnexo) ? "" : docto.CaminhoAnexo

                    }, transaction: transaction); ;
                return docto;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"ERRO - {ex.StackTrace}");
                throw new Exception(ex.Message);
            }



        }

        public bool Update(Documento docto)
        {

            try
            {
                var sb = new StringBuilder();
                sb.Append("UPDATE documento SET EmpresaFornecedor = @EmpresaFornecedor,");
                sb.Append(" Ativo=@Ativo, SituacaoOrcamento=@SituacaoOrcamento, ");
                if(!string.IsNullOrWhiteSpace(docto.CaminhoAnexo))
                    sb.Append($"CaminhoAnexo='{docto.CaminhoAnexo}' ");

                sb.Append(" WHERE Id = @Id");
                var affectedRows = Connection.Execute(sb.ToString(),
                   new
                   {
                       Id = docto.Id,
                       EmpresaFornecedor = docto.EmpresaFornecedor.Id,
                       Ativo = 1,
                       DataInclusao = DateTime.Now,
                       SituacaoDocumento = Convert.ToInt32(FluxoDocumento.Aguardando),
                       CaminhoAnexo = string.IsNullOrEmpty(docto.CaminhoAnexo) ? "" : docto.CaminhoAnexo

                   }, transaction: transaction);
                return true;
            }
            catch
            {
                return false;
            }

        }

        private void SendEmailPagamento(Documento notafiscal)
        {

            //StringBuilder sb = new StringBuilder();
            //string _body = string.Empty;
            //try
            //{

            //    var nf = GetById(notafiscal.Id);


            //    Configuracao config = GetConfiguracao();


            //    if (config == null)
            //        throw new Exception("� necess�rio definir o servidor de envio de email em configura��o");

            //    sb.Clear();

            //    if (string.IsNullOrEmpty(config.EmailPagamento))
            //        return;
            //    _body = config.EmailAprovacao.Replace("{PEDIDO}", nf.PedidoCompra);
            //    _body = _body.Replace("{COD_USUARIO}", nf.CodigoUsuario);

            //    var _split = _body.Split("<br>");
            //    _body = "";
            //    foreach (var item in _split)
            //    {
            //        _body = $"{_body}{item.Replace("<br>", "")}\r\n";
            //    }

            //    _body = $"{_body}\r\nAnexos\r\n{nf.CaminhoAnexo}";

            //    var _emails = config.EmailPagamento.Split(";");

            //    foreach (var emailPagamento in _emails)
            //    {

            //        MailMessage mm = new MailMessage(config.FromEmail, emailPagamento, config.AssuntoEmailAprovacao, _body);

            //        //mm.Attachments.Add(new Attachment(notafiscal.CaminhoAnexo));
            //        SmtpClient sm = new SmtpClient(config.HostEmail, config.PortEmail);
            //        sm.UseDefaultCredentials = false;
            //        sm.Credentials = new NetworkCredential(config.FromEmail, config.PassEmail);
            //        sm.Send(mm);

            //    }
            //}
            //catch (Exception ex)
            //{
            //    //throw new Exception(ex.Message);
            //}

        }

        public bool AprovarDocumento(Documento documento)
        {

            try
            {
                var affectedRows = Connection.Execute("UPDATE documento SET SituacaoDocumento = @SituacaoDocumento WHERE Id = @Id",
                    new
                    {
                        Id = documento.Id,
                        SituacaoDocumento = 2,
                    }, transaction: transaction);


                return true;

            }
            catch
            {
                return false;
            }

        }
        public bool ValidarNotaFiscal(Documento notafiscal)
        {

            try
            {
                var affectedRows = Connection.Execute("UPDATE orcamento SET ObsAdmin = @ObsAdmin, PedidoCompra = @PedidoCompra, SituacaoOrcamento = @SituacaoOrcamento WHERE Id = @Id",
                    new
                    {
                        //ObsAdmin = notafiscal.ObsAdmin,
                        //SituacaoOrcamento = 4,
                        //PedidoCompra = notafiscal.PedidoCompra,
                        Id = notafiscal.Id
                    }, transaction: transaction);
                

                //Enviar Email
                //SendEmailPagamento(notafiscal);
                return true;

            }
            catch
            {
                return false;
            }

        }
        public bool RejeitarDocumento(Documento documento)
        {

            try
            {
                var affectedRows = Connection.Execute("UPDATE documento SET SituacaoDocumento = @SituacaoDocumento WHERE Id = @Id",
                    new
                    {
                        Id = documento.Id,
                        SituacaoDocumento = 3,
                    }, transaction: transaction);


                return true;

            }
            catch
            {
                return false;
            }

        }

        //private ContasConfig GetConfig()
        //{
        //    //var environmentName = Environment.GetEnvironmentVariable("CURRENT_ENVIRONMENT");
        //    //if (string.IsNullOrWhiteSpace(environmentName))
        //    //    environmentName = "prod";

        //    IConfiguration config = new ConfigurationBuilder()
        //        .AddJsonFile($"eventsappsettings.prod.json", optional: true, reloadOnChange: true)
        //        .AddEnvironmentVariables().Build();


        //    var contasConfig = new ContasConfig()
        //    {
        //        ContasAPIAddress = config.GetSection("ContasConfig").GetSection("ContasAPIAddress").Value,
        //        ContasAPIUser = config.GetSection("ContasConfig").GetSection("ContasAPIUser").Value,
        //        ContasAPIPass = config.GetSection("ContasConfig").GetSection("ContasAPIPass").Value,
        //        CorreioAPIAddress = config.GetSection("ContasConfig").GetSection("CorreioAPIAddress").Value,
        //        LocalServerAddress = config.GetSection("ContasConfig").GetSection("LocalServerAddress").Value,
        //        ControlStorageFolder = config.GetSection("ContasConfig").GetSection("ControlStorageFolder").Value,
        //        ParallelRobotsPerRequest = Int32.Parse(config.GetSection("ContasConfig").GetSection("ParallelRobotsPerRequest").Value),

        //        StorageToUse = (Domain.Enum.StorageType)Int32.Parse(config.GetSection("ContasConfig").GetSection("StorageToUse").Value),
        //        S3Url = config.GetSection("ContasConfig").GetSection("S3Url").Value,
        //        S3Key = config.GetSection("ContasConfig").GetSection("S3Key").Value,
        //        S3Secret = config.GetSection("ContasConfig").GetSection("S3Secret").Value,
        //        S3Bucket = config.GetSection("ContasConfig").GetSection("S3Bucket").Value,
        //        Headless = bool.Parse(config.GetSection("ContasConfig").GetSection("Headless").Value),
        //        SendToCorreio = bool.Parse(config.GetSection("ContasConfig").GetSection("SendToCorreio").Value)
        //    };

        //    return contasConfig;
        //}

        /// <summary>
        /// Verifica se unidade de negocio/emissor 
        /// </summary>
        /// <param name="fornecedorclienteId"></param>
        /// <returns></returns>
        private bool VerifyAutomaticDebit(int fornecedorclienteId)
        {

            try
            {

                //var sb = new StringBuilder();
                //sb.Clear();
                //sb.Append($"SELECT DebitoAutomatico FROM fornecedorcliente WHERE FornecedorClienteId={fornecedorclienteId}");
                //var result = Connection.QueryFirstOrDefault<FornecedorCliente>(sb.ToString(), null, transaction: transaction);

                //if (result != null && result.DebitoAutomatico)
                //    return true;

                return false;


            }
            catch
            {

                return false;
            }
        }


        public bool UpdateSituacao(string arquivoColetadoId, string situacao)
        {

            try
            {
                var affectedRows = Connection.Execute("UPDATE arquivocoletado SET Situacao = @Situacao WHERE ArquivoColetadoId = @ArquivoColetadoId",
                    new
                    {
                        Situacao = situacao,
                        ArquivoColetadoId = arquivoColetadoId
                    }, transaction: transaction);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool UpdateByIdsSituacao(string ids, string situacao)
        {

            try
            {
                var affectedRows = Connection.Execute($"UPDATE arquivocoletado SET Situacao = @Situacao WHERE ArquivoColetadoId IN ({ids})",
                    new
                    {
                        Situacao = situacao,
                    }, transaction: transaction);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool UpdateDadosPedido(string arquivoColetadoId, string numero, string data)
        {

            var _data = data.Split("/");
            try
            {
                var affectedRows = Connection.Execute("UPDATE arquivocoletado SET DataEntrega = @DataEntrega, NumeroPedido = @NumeroPedido WHERE ArquivoColetadoId = @ArquivoColetadoId",
                    new
                    {
                        DataEntrega = $"{_data[2]}-{ _data[1].PadLeft(2, '0')}-{ _data[0].PadLeft(2, '0')} 00:00:00", //  Convert.ToDateTime(data).ToString("yyyy-MM-dd 00:00:00"),
                        NumeroPedido = numero,
                        ArquivoColetadoId = arquivoColetadoId
                    }, transaction: transaction);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool SetarSituacaoPaid(int fornecedorclienteid)
        {

            try
            {
                var affectedRows = Connection.Execute($"UPDATE arquivocoletado SET Situacao = 2 WHERE FkFornecedorClienteId = {fornecedorclienteid} AND Situacao = 1", null, transaction: transaction);

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool ClearLogExecute(int fornecedorclienteid)
        {

            try
            {
                var affectedRows = Connection.Execute($"DELETE FROM log WHERE fklogFornecedorClienteId = {fornecedorclienteid}", null, transaction: transaction);

                affectedRows = Connection.Execute($"UPDATE fornecedorcliente SET Operacional = 1 WHERE FornecedorClienteId = {fornecedorclienteid}", null, transaction: transaction);

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool Remove(int id)
        {
            var affectedRows = Connection.Execute("DELETE FROM orcamento WHERE Id = @Id", new { Id = id }, transaction: transaction);

            return affectedRows > 0;
        }

        public List<Documento> GetAllByFilter(int? fornecedor, string Cnpj, int? cliente, int? situacao)
        {

            var sb = new StringBuilder();
            sb.Clear();
            sb.Append("SELECT ");
            sb.Append("    d.Id,  ");
            sb.Append("    d.Ativo, d.DataInclusao,d.SituacaoDocumento,d.CaminhoAnexo,   ");
            sb.Append("    ef.Id,   ");
            sb.Append("    e.Id,e.Nome,e.Cnpj,   ");
            sb.Append("    f.Id,   ");
            sb.Append("    f.Nome   ");
            sb.Append(" FROM documento d ");
            sb.Append(" INNER JOIN empresa_fornecedor ef ON(d.EmpresaFornecedor = ef.Id) ");
            sb.Append(" INNER JOIN empresa e ON(ef.EmpresaId = e.Id) ");
            sb.Append(" INNER JOIN fornecedor f ON(ef.FornecedorId = f.Id) ");
            sb.AppendFormat(" WHERE 1 = 1");

            if (fornecedor != null) //Fornecedor
                sb.AppendFormat(" AND f.Id = {0}", fornecedor);

            if (cliente != null) //cliente
                sb.AppendFormat(" AND e.Id = {0}", cliente);

            if (Cnpj != null) //Cnpj
                sb.AppendFormat(" AND e.Cnpj = '{0}'", Cnpj);

            if (situacao != null) //situacao
                sb.AppendFormat(" AND d.SituacaoDocumento = {0}", situacao);

            //sb.AppendFormat("   order by u.Id");

            var data = Connection.Query<Documento, EmpresaFornecedor, Empresa, Fornecedor, Documento>(sb.ToString(), (documento, empresaFornecedor, empresa, forn) =>
            {
                empresaFornecedor.Empresa = empresa;
                empresaFornecedor.Fornecedor = forn;
                documento.EmpresaFornecedor = empresaFornecedor;
                ;
                return documento;
            }, splitOn: "Id,Id, Id, Id").ToList();

            return data;

        }

        public List<Documento> GetAllByOficina(int oficina, string codigo)
        {

            var _splt = codigo.Split("/");
            var sb = new StringBuilder();
            sb.Clear();
            sb.Append("SELECT ");
            sb.Append("    u.Id,  ");
            sb.Append("    u.ClienteNome, u.TipoDocumento,   ");
            sb.Append("    u.Vencimento,   ");
            sb.Append("    u.Valor,   ");
            sb.Append("    u.NumeroOrcamento, u.Placa, u.CaminhoAnexo, u.SituacaoOrcamento, u.PedidoCompra, u.ObsAdmin, u.ObsFornecedor,  ");
            sb.Append("    f.Id,   ");
            sb.Append("    f.Nome,   ");
            sb.Append("    f.Cnpj   ");
            sb.Append(" FROM orcamento u ");
            sb.Append(" INNER JOIN fornecedor f ON(u.Fornecedor = f.Id) ");
            sb.AppendFormat(" WHERE 1 = 1");
            sb.AppendFormat(" AND u.Ativo = 1");
            sb.AppendFormat(" AND u.SituacaoOrcamento = 4"); //Validada
            sb.AppendFormat(" AND u.Fornecedor = {0}", oficina);

            sb.AppendFormat($" AND YEAR(u.DataInclusao) = {_splt[0]} ");
            sb.AppendFormat($"AND MONTH(u.DataInclusao) = {_splt[1]} ");

            var data = Connection.Query<Documento, Fornecedor, Documento>(sb.ToString(), (notafiscal, fornecedor) =>
            {
                //notafiscal.Fornecedor = fornecedor;
                return notafiscal;
            }, splitOn: "Id,Id").ToList();

            return data;

        }

        public Documento GetById(int id)
        {

            var sb = new StringBuilder();
            sb.Clear();
            sb.Append("SELECT o.Id, o.pedidoCompra, o.TipoDocumento, o.ObsFornecedor, o.ObsAdmin, o.Vencimento,o.Valor, o.NumeroOrcamento,o.Placa,o.Ativo,o.DataInclusao,o.ClienteNome,o.ClientePessoa,o.ClienteCpfCnpj,o.SituacaoOrcamento, o.CaminhoAnexo, o.PedidoCompra, o.ObsAdmin, o.ObsFornecedor,");
            sb.Append("    f.Id,  ");
            sb.Append("    f.Nome   ");
            sb.Append(" FROM orcamento o ");
            sb.Append(" INNER JOIN fornecedor f ON(o.Fornecedor = f.Id) ");
            sb.AppendFormat(" WHERE o.Id={0} ", id);

            var data = Connection.Query<Documento, Fornecedor, Documento>(sb.ToString(), (notafiscal, fornecedor) =>
            {
                //notafiscal.Fornecedor = fornecedor;
                return notafiscal;
            }, splitOn: "Id,Id").FirstOrDefault();

            return data;


        }
        public List<Documento> GetAll(DocumentoFilter filter)
        {
            var result = Connection.Query<Documento>("SELECT * FROM arquivocoletado WHERE ArquivoColetadoId = IFNULL(@Id, PK_ID)", filter, transaction: transaction).ToList();

            return result;
        }
        public List<Documento> ValidarItemIncluido(int fornecedorclienteid, string dtVencimento, decimal total)
        {

            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT ArquivoColetadoId FROM arquivocoletado ");
            sb.AppendFormat("WHERE fkfornecedorclienteid = {0} ", fornecedorclienteid);
            sb.AppendFormat("AND DtVencimento = {0} ", dtVencimento);
            if (total != 0)
                sb.AppendFormat("AND total = {0} ", total.ToString().Replace(",", "."));

            var result = Connection.Query<Documento>(sb.ToString(), null, transaction: transaction).ToList();

            //if (result.Count > 0)
            //    Connection.Execute($"UPDATE arquivocoletado SET Situacao = 1 WHERE ArquivoColetadoId = {result[0].ArquivoColetadoId}", null, transaction: transaction);

            return result;
        }

        public List<Documento> ValidarItemIncluidoByReferencia(int fornecedorclienteid, string referenceDate, decimal total)
        {

            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT ArquivoColetadoId FROM arquivocoletado ");
            sb.AppendFormat("WHERE fkfornecedorclienteid = {0} ", fornecedorclienteid);
            sb.AppendFormat("AND NrMesReferencia = {0} ", referenceDate);
            if (total != 0)
                sb.AppendFormat("AND total = {0} ", total.ToString().Replace(",", "."));

            var result = Connection.Query<Documento>(sb.ToString(), null, transaction: transaction).ToList();

            return result;
        }

        public List<dynamic> GetStatusArquivoColetado(int? LoggedclienteId, int? LoggedEmpresaId)
        {
            var datacorrente = DateTime.Now;
            var qtddocumento = "0";
            var qtNfServicoAprovadas = "0";
            var qtNfProdutoAprovadas = "0";
            var vlrmes = "0";
            bool isNotAdmin = LoggedclienteId != null;

            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT Count(*) AS qtddocumento, Sum(o.SituacaoOrcamento=3) as qtNfServicoAprovadas, ");
            sb.Append(" Sum(o.SituacaoOrcamento=4) as qtNfProdutoAprovadas, Sum(o.Valor) as total ");
            sb.Append(" FROM orcamento o ");
            sb.Append(" INNER JOIN fornecedor f on o.Fornecedor = f.Id  ");
            if (isNotAdmin) 
                sb.AppendFormat(" INNER JOIN usuario u on u.Fornecedor = o.Fornecedor and u.Id = {0} ", LoggedclienteId.Value); 
            sb.Append($" where YEAR(o.DataInclusao) = {datacorrente.Year} and month(o.DataInclusao) = {datacorrente.Month} ");

            if (LoggedEmpresaId != null && LoggedEmpresaId.Value > 0)
                sb.AppendFormat(" AND f.Empresa = {0}", LoggedEmpresaId.Value);

            var result = Connection.QueryFirstOrDefault<dynamic>(sb.ToString(), null, transaction: transaction);
            vlrmes = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0}", Convert.ToDecimal(Convert.ToString(result.total)));
            
            qtddocumento = result.qtddocumento.ToString("n0", CultureInfo.CreateSpecificCulture("pt-BR"));
            qtddocumento = qtddocumento != "" ? qtddocumento.Replace(',', '.') : "0";
            qtNfServicoAprovadas = result.qtNfServicoAprovadas.ToString("n0", CultureInfo.CreateSpecificCulture("pt-BR"));
            qtNfServicoAprovadas = qtNfServicoAprovadas != "" ? qtNfServicoAprovadas.Replace(',', '.') : "0";
            qtNfProdutoAprovadas = result.qtNfProdutoAprovadas.ToString("n0", CultureInfo.CreateSpecificCulture("pt-BR"));
            qtNfProdutoAprovadas = qtNfProdutoAprovadas != "" ? qtNfProdutoAprovadas.Replace(',', '.') : "0";
            if (vlrmes == "")
                vlrmes = "0,00";
            // else{
            //     vlrmes = vlrmes.Replace(".","_");
            //     vlrmes = vlrmes.Replace(",", ".").Replace("_", ",");
            // }

            sb.Clear();
            sb.AppendFormat("SELECT '{0}' AS qtddocumento, '{1}' AS qtNfServicoAprovadas, '{2}' AS qtNfProdutoAprovadas, '{3}' AS vlrmes ", qtddocumento, qtNfServicoAprovadas, qtNfProdutoAprovadas, vlrmes);
            result = Connection.Query<dynamic>(sb.ToString(), null, transaction: transaction).ToList();

            return result;
        }
        public List<dynamic> GetNotificacoes(int? LoggedclienteId, string centrosCusto)
        {
            int _year = DateTime.Now.Year;
            int _month = DateTime.Now.Month;

            Int64 qtdRequisicoes = 0;
            Int64 qtdFaturasAtrasadas = 0;
            Int64 qtdFaturasHoje = 0;
            Int64 qtdErroContrato = 0;
            Int64 qtdRobos = 0;

            StringBuilder sb = new StringBuilder();


            //Requisi��es em aberto
            sb.Clear();
            sb.Append("SELECT COUNT(*) Qtd ");
            sb.Append("FROM requisicao ");
            sb.Append("WHERE RequisicaoStatus IN(3,4) ");

            if (LoggedclienteId != null)
                sb.AppendFormat(" AND RequisicaoClienteId = {0} ", LoggedclienteId.Value);

            var result = Connection.QueryFirstOrDefault<dynamic>(sb.ToString(), null, transaction: transaction);
            qtdRequisicoes = result.Qtd;

            sb.Clear();
            sb.Append(" SELECT COUNT(*) Qtd ");
            sb.Append(" FROM log l ");
            sb.Append(" INNER JOIN fornecedorcliente fc ON(l.fklogFornecedorClienteId = fc.FornecedorClienteId) ");
            sb.Append(" inner join cliente c on c.ClienteId = fc.FkClienteId ");
            sb.Append(" inner join fornecedor f on f.FornecedorId = fc.FkFornecedorId ");
            sb.Append(" WHERE l.tipoErro = 2 ");

            if (LoggedclienteId != null)
                sb.AppendFormat(" and(c.ClienteId = {0} or c.ClientePaiId = {0}) ", LoggedclienteId.Value);

            result = Connection.QueryFirstOrDefault<dynamic>(sb.ToString(), null, transaction: transaction);
            qtdRobos = result.Qtd;


            //Faturas N�o pagas e atrasadas.
            sb.Clear();
            sb.Append("SELECT COUNT(*) Qtd ");
            sb.Append(" FROM arquivocoletado ac ");
            sb.Append(" inner join fornecedorcliente fc on(ac.FkFornecedorClienteId = fc.FornecedorClienteId) ");
            sb.Append(" inner join cliente c on(fc.FkClienteId = c.ClienteId) ");
            sb.Append(" inner join fornecedor f on(fc.FkFornecedorId = f.FornecedorId) ");
            sb.Append(" where ac.Situacao = 1 ");
            sb.Append(" and ac.DtVencimento < now() ");
            sb.Append($" and YEAR(ac.DtVencimento) =  {_year}");
            sb.Append($" and MONTH(ac.DtVencimento) =  {_month}");

            if (LoggedclienteId != null)
                sb.AppendFormat(" and(c.ClienteId = {0} or c.ClientePaiId = {0}) ", LoggedclienteId.Value);

            result = Connection.QueryFirstOrDefault<dynamic>(sb.ToString(), null, transaction: transaction);
            qtdFaturasAtrasadas = result.Qtd;

            //Faturas para pagamento hoje.
            sb.Clear();
            sb.Append("SELECT COUNT(*) Qtd ");
            sb.Append(" FROM arquivocoletado ac ");
            sb.Append(" inner join fornecedorcliente fc on(ac.FkFornecedorClienteId = fc.FornecedorClienteId) ");
            sb.Append(" inner join cliente c on(fc.FkClienteId = c.ClienteId) ");
            sb.Append(" inner join fornecedor f on(fc.FkFornecedorId = f.FornecedorId) ");
            sb.Append(" where ac.Situacao = 1 ");
            sb.Append($" and YEAR(ac.DtVencimento) =  {_year}");
            sb.Append($" and MONTH(ac.DtVencimento) =  {_month}");


            if (LoggedclienteId != null)
                sb.AppendFormat(" and(c.ClienteId = {0} or c.ClientePaiId = {0}) ", LoggedclienteId.Value);

            result = Connection.QueryFirstOrDefault<dynamic>(sb.ToString(), null, transaction: transaction);
            qtdFaturasHoje = result.Qtd;

            //Faturas para pagamento hoje.
            sb.Clear();
            sb.Append("SELECT ");
            sb.Append("SUM((CASE WHEN ac.Total * 0.9 > fc.ValorAproximado THEN 1 ELSE 0 END)) Qtd ");
            sb.Append("FROM arquivocoletado ac ");
            sb.Append("inner join fornecedorcliente fc on(ac.FkFornecedorClienteId = fc.FornecedorClienteId) ");
            sb.Append("inner join cliente c on(fc.FkClienteId = c.ClienteId) ");
            sb.Append("inner join fornecedor f on(fc.FkFornecedorId = f.FornecedorId) ");
            sb.Append("where (fc.ValorAproximado is not null or fc.ValorAproximado > 0) and ac.Situacao = 1 ");

            if (LoggedclienteId != null)
                sb.AppendFormat(" and(c.ClienteId = {0} or c.ClientePaiId = {0}) ", LoggedclienteId.Value);

            result = Connection.QueryFirstOrDefault<dynamic>(sb.ToString(), null, transaction: transaction);
            qtdErroContrato = Convert.ToInt64(result.Qtd);

            sb.Clear();
            sb.AppendFormat("SELECT {0} AS QtdRequisicoes, {1} AS QtdFaturasAtrasadas, {2} AS QtdFaturasHoje, {3} AS QtdErroContrato, {4} AS QtdRobos  ", qtdRequisicoes,
                qtdFaturasAtrasadas, qtdFaturasHoje, qtdErroContrato, qtdRobos);
            result = Connection.Query<dynamic>(sb.ToString(), null, transaction: transaction).ToList();

            return result;

        }

        /// <summary>
        /// Requisi��es em aberto
        /// </summary>
        /// <param name="clienteId"></param>
        /// <returns></returns>
        private string GetNotificaRequisicoes(int clienteId)
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT COUNT(*) Qtd FROM requisicao ");
            sb.Append("inner join cliente on (cliente.ClienteId = requisicao.RequisicaoClienteId) ");
            sb.Append("WHERE RequisicaoStatus IN(3, 4) ");
            sb.Append($" and (cliente.ClienteId = {clienteId} or cliente.ClientePaiId = {clienteId} )");

            var result = Connection.QueryFirstOrDefault<dynamic>(sb.ToString(), null, transaction: transaction);

            if (Convert.ToInt32(result.Qtd) > 0)
            {
                sb.Clear();
                sb.Append("<table border='1' style='border-collapse: collapse; width: 100%;'> ");
                sb.Append("<tbody> ");
                sb.Append("<tr style='background-color: #f0f0f0;'> ");
                sb.Append("<td style='text-align: center;' colspan='2'>&nbsp;<strong>REQUISI&Ccedil;&Otilde;ES DE EMISSORES PENDENTES</strong></td> ");
                sb.Append("</tr> ");
                sb.Append("<tr> ");
                sb.Append("<td style='width: 80%;'><span style='color: #ffffff;'>&nbsp;QUANTIDADE DE REQUISI&Ccedil;&Otilde;ES AGUARDANDO DEFINI&Ccedil;&Atilde;O</span></td> ");
                sb.Append($"<td style='width: 20%; text-align: right;'>&nbsp;{result.Qtd}</td> ");
                sb.Append("</tr> ");
                sb.Append("</tbody> ");
                sb.Append("</table> ");

                return sb.ToString();
            }

            return "";
        }

        private string GetRobos(int clienteId)
        {
            StringBuilder sb = new StringBuilder();


            sb.Clear();
            sb.Append("SELECT fc.OperacionalDescricao, cliente.ClienteNome, fornecedor.FornecedorNome FROM fornecedorcliente fc  ");
            sb.Append("inner join cliente on(cliente.ClienteId = fc.FkClienteId) ");
            sb.Append("inner join fornecedor on(fornecedor.FornecedorId = fc.FkFornecedorId) ");
            sb.Append("WHERE fc.Operacional = 0 ");
            sb.Append($" and (cliente.ClienteId = {clienteId} or cliente.ClientePaiId = {clienteId} )");

            var result = Connection.Query<dynamic>(sb.ToString(), null, transaction: transaction).ToList();

            if (result.Count > 0)
            {
                sb.Clear();
                sb.Append("<table border='1' style='border-collapse: collapse; width: 100%;'> ");
                sb.Append("<tbody> ");
                sb.Append("<tr style='background-color: #4a4a9d;'> ");
                sb.Append("<td style='width: 100%; text-align: center;' colspan='3'>&nbsp;<span style='color: #ffffff;'><strong>PROBLEMAS NA EXECUCAO DE ROBOS</strong></span></td> ");
                sb.Append("</tr> ");
                sb.Append("<tr style='background-color: #f0f0f0;'> ");
                sb.Append("<td style='width: 20%; text-align: center;'>Cliente</td> ");
                sb.Append("<td style='width: 20%; text-align: center;'>Emissor</td> ");
                sb.Append("<td style='width: 60%; text-align: center;'>Descri��o</td> ");
                sb.Append("</tr> ");
                foreach (var item in result)
                {
                    sb.Append("<tr> ");
                    sb.Append($"<td style='text-align: left;'>{item.ClienteNome}</td> ");
                    sb.Append($"<td style='text-align: left;'>{item.FornecedorNome}</td> ");
                    sb.Append($"<td style='text-align: left;'>{item.OperacionalDescricao}</td> ");
                    sb.Append("</tr> ");
                }
                sb.Append("</tbody> ");
                sb.Append("</table> ");
                return sb.ToString();
            }

            return "";
        }

        private string GetNotificaFaturasAtrasadas(int clienteId, int? clienteDiasBaixar, string clienteEmail)
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            if (clienteDiasBaixar != null && clienteDiasBaixar.Value > 0)
            {

                sb.Append("UPDATE arquivocoletado ");
                sb.Append("inner join fornecedorcliente on(arquivocoletado.FkFornecedorClienteId = fornecedorcliente.FornecedorClienteId) ");
                sb.Append("inner join cliente on(fornecedorcliente.FkClienteId = cliente.ClienteId) ");
                sb.Append("SET arquivocoletado.Situacao = 2 ");
                sb.Append("where arquivocoletado.Situacao = 1 ");
                sb.AppendFormat("and arquivocoletado.DtVencimento <= now() - INTERVAL {0} DAY ", clienteDiasBaixar.Value);
                sb.Append("and DATE_FORMAT(arquivocoletado.DtVencimento,'%d/%m/%Y') <> DATE_FORMAT(now(), '%d/%m/%Y') ");
                sb.AppendFormat(" and(cliente.ClienteId = {0} or cliente.ClientePaiId = {0}) ", clienteId);

                var affectedRows = Connection.Execute(sb.ToString(), null, transaction: transaction);
            }

            if (string.IsNullOrEmpty(clienteEmail))
                return "";

            sb.Clear();
            sb.Append("SELECT c.ClienteNome, (CASE WHEN c.TipoPessoa=1 THEN MASK(c.ClienteCnpj, '###.###.###-##') ELSE MASK(c.ClienteCnpj, '##.###.###/####-##') END) ClienteCnpj, f.FornecedorNome, DATE_FORMAT(ac.DtVencimento,'%d/%m/%Y') DtVencimento, ac.Total  ");
            sb.Append(" FROM arquivocoletado ac ");
            sb.Append(" inner join fornecedorcliente fc on(ac.FkFornecedorClienteId = fc.FornecedorClienteId) ");
            sb.Append(" inner join cliente c on(fc.FkClienteId = c.ClienteId) ");
            sb.Append(" inner join fornecedor f on(fc.FkFornecedorId = f.FornecedorId) ");
            sb.Append(" where ac.Situacao = 1 ");
            sb.Append(" and ac.DtVencimento < now() ");
            sb.Append(" and DATE_FORMAT(ac.DtVencimento,'%d/%m/%Y') <> DATE_FORMAT(now(),'%d/%m/%Y') ");
            sb.AppendFormat(" and(c.ClienteId = {0} or c.ClientePaiId = {0}) ", clienteId);


            var result = Connection.Query<dynamic>(sb.ToString(), null, transaction: transaction).ToList();

            if (result.Count > 0)
            {
                sb.Clear();
                sb.Append("<table border='1' style='border-collapse: collapse; width: 100%;'> ");
                sb.Append("<tbody> ");
                sb.Append("<tr style='background-color: #4a4a9d;'> ");
                sb.Append("<td style='width: 100%; text-align: center;' colspan='5'>&nbsp;<span style='color: #ffffff;'><strong>FATURAS ATRASADAS OU N&Atilde;O BAIXADAS</strong></span></td> ");
                sb.Append("</tr> ");
                sb.Append("<tr style='background-color: #f0f0f0;'> ");
                sb.Append("<td style='width: 30%; text-align: center;'>Cliente</td> ");
                sb.Append("<td style='width: 15%; text-align: center;'>Cnpj</td> ");
                sb.Append("<td style='width: 35%; text-align: center;'>Fornecedor</td> ");
                sb.Append("<td style='width: 10%; text-align: center;'>Vencimento</td> ");
                sb.Append("<td style='width: 10%; text-align: center;'>Valor</td> ");
                sb.Append("</tr> ");
                foreach (var item in result)
                {
                    sb.Append("<tr> ");
                    sb.Append($"<td style='text-align: left;'>{item.ClienteNome}</td> ");
                    sb.Append($"<td style='text-align: left;'>{item.ClienteCnpj}</td> ");
                    sb.Append($"<td style='text-align: left;'>{item.FornecedorNome}</td> ");
                    sb.Append($"<td style='text-align: center;'>{item.DtVencimento}</td> ");
                    sb.Append($"<td style='text-align: right;'>{item.Total.ToString("N2")}</td> ");
                    sb.Append("</tr> ");
                }
                sb.Append("</tbody> ");
                sb.Append("</table> ");
                return sb.ToString();
            }

            return "";
        }

        private string GetNotificaFaturasHoje(int clienteId)
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT c.ClienteNome, (CASE WHEN c.TipoPessoa=1 THEN MASK(c.ClienteCnpj, '###.###.###-##') ELSE MASK(c.ClienteCnpj, '##.###.###/####-##') END) ClienteCnpj, f.FornecedorNome, DATE_FORMAT(ac.DtVencimento,'%d/%m/%Y') DtVencimento, ac.Total  ");
            sb.Append(" FROM arquivocoletado ac ");
            sb.Append(" inner join fornecedorcliente fc on(ac.FkFornecedorClienteId = fc.FornecedorClienteId) ");
            sb.Append(" inner join cliente c on(fc.FkClienteId = c.ClienteId) ");
            sb.Append(" inner join fornecedor f on(fc.FkFornecedorId = f.FornecedorId) ");
            sb.Append(" where ac.Situacao = 1 ");
            sb.Append(" and DATE_FORMAT(ac.DtVencimento,'%d/%m/%Y') = DATE_FORMAT(now(),'%d/%m/%Y') ");
            sb.AppendFormat(" and(c.ClienteId = {0} or c.ClientePaiId = {0}) ", clienteId);

            var result = Connection.Query<dynamic>(sb.ToString(), null, transaction: transaction).ToList();

            if (result.Count > 0)
            {
                sb.Clear();
                sb.Append("<table border='1' style='border-collapse: collapse; width: 100%;'> ");
                sb.Append("<tbody> ");
                sb.Append("<tr style='background-color: #4a4a9d;'> ");
                sb.Append("<td style='width: 100%; text-align: center;' colspan='5'>&nbsp;<span style='color: #ffffff;'><strong>FATURAS PARA PAGAMENTO HOJE</strong></span></td> ");
                sb.Append("</tr> ");
                sb.Append("<tr style='background-color: #f0f0f0;'> ");
                sb.Append("<td style='width: 30%; text-align: center;'>Cliente</td> ");
                sb.Append("<td style='width: 15%; text-align: center;'>Cnpj</td> ");
                sb.Append("<td style='width: 35%; text-align: center;'>Fornecedor</td> ");
                sb.Append("<td style='width: 10%; text-align: center;'>Vencimento</td> ");
                sb.Append("<td style='width: 10%; text-align: center;'>Valor</td> ");
                sb.Append("</tr> ");
                foreach (var item in result)
                {
                    sb.Append("<tr> ");
                    sb.Append($"<td style='text-align: left;'>{item.ClienteNome}</td> ");
                    sb.Append($"<td style='text-align: left;'>{item.ClienteCnpj}</td> ");
                    sb.Append($"<td style='text-align: left;'>{item.FornecedorNome}</td> ");
                    sb.Append($"<td style='text-align: center;'>{item.DtVencimento}</td> ");
                    sb.Append($"<td style='text-align: right;'>{item.Total.ToString("N2")}</td> ");
                    sb.Append("</tr> ");
                }
                sb.Append("</tbody> ");
                sb.Append("</table> ");
                return sb.ToString();
            }

            return "";
        }

        private string GetNotificaFaturasInconsistentes(int clienteId)
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT c.ClienteNome, (CASE WHEN c.TipoPessoa=1 THEN MASK(c.ClienteCnpj, '###.###.###-##') ELSE MASK(c.ClienteCnpj, '##.###.###/####-##') END) ClienteCnpj, f.FornecedorNome, DATE_FORMAT(ac.DtVencimento,'%d/%m/%Y') DtVencimento, fc.ValorAproximado, ac.Total  ");
            sb.Append(" FROM arquivocoletado ac ");
            sb.Append(" inner join fornecedorcliente fc on(ac.FkFornecedorClienteId = fc.FornecedorClienteId) ");
            sb.Append(" inner join cliente c on(fc.FkClienteId = c.ClienteId) ");
            sb.Append(" inner join fornecedor f on(fc.FkFornecedorId = f.FornecedorId) ");
            sb.Append("where (fc.ValorAproximado is not null or fc.ValorAproximado > 0) and ac.Situacao = 1 ");
            sb.AppendFormat(" and(c.ClienteId = {0} or c.ClientePaiId = {0}) ", clienteId);

            var result = Connection.Query<dynamic>(sb.ToString(), null, transaction: transaction).ToList();

            if (result.Count > 0)
            {
                sb.Clear();
                sb.Append("<table border='1' style='border-collapse: collapse; width: 100%;'> ");
                sb.Append("<tbody> ");
                sb.Append("<tr style='background-color: #4a4a9d;'> ");
                sb.Append("<td style='width: 100%; text-align: center;' colspan='6'>&nbsp;<span style='color: #ffffff;'><strong>FATURAS INCONSISTENTES</strong></span></td> ");
                sb.Append("</tr> ");
                sb.Append("<tr style='background-color: #f0f0f0;'> ");
                sb.Append("<td style='width: 20%; text-align: center;'>Cliente</td> ");
                sb.Append("<td style='width: 10%; text-align: center;'>Cnpj</td> ");
                sb.Append("<td style='width: 40%; text-align: center;'>Fornecedor</td> ");
                sb.Append("<td style='width: 10%; text-align: center;'>Vencimento</td> ");
                sb.Append("<td style='width: 10%; text-align: center;'>Valor Aproximando</td> ");
                sb.Append("<td style='width: 10%; text-align: center;'>Valor Coletado</td> ");

                sb.Append("</tr> ");
                foreach (var item in result)
                {
                    sb.Append("<tr> ");
                    sb.Append($"<td style='text-align: left;'>{item.ClienteNome}</td> ");
                    sb.Append($"<td style='text-align: left;'>{item.ClienteCnpj}</td> ");
                    sb.Append($"<td style='text-align: left;'>{item.FornecedorNome}</td> ");
                    sb.Append($"<td style='text-align: center;'>{item.DtVencimento}</td> ");
                    sb.Append($"<td style='text-align: right;'>{item.ValorAproximado.ToString("N2")}</td> ");
                    sb.Append($"<td style='text-align: right;'>{item.Total.ToString("N2")}</td> ");
                    sb.Append("</tr> ");
                }
                sb.Append("</tbody> ");
                sb.Append("</table> ");
                return sb.ToString();
            }

            return "";
        }

        private Configuracao GetConfiguracao()
        {
            StringBuilder sb = new StringBuilder();
            sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append("    Id,  ");
            sb.Append("    HostEmail,PortEmail,FromEmail,PassEmail,AssuntoEmailAprovacao,EmailPagamento,   ");
            sb.Append("    EmailAprovacao,PasswordDefault   ");
            sb.Append(" FROM configuracao ");

            var result = Connection.QueryFirstOrDefault<Configuracao>(sb.ToString(), null, transaction: transaction);
            return result;
        }

        public void SendEmailNotificacoes()
        {

            //Cliente cliente;
            //StringBuilder sb = new StringBuilder();
            //string _textRobos = string.Empty;
            //string _textRequisicoes = string.Empty;
            //string _textFaturasAtrasadas = string.Empty;
            //string _textFaturasHoje = string.Empty;
            //string _textFaturasInconsistentes = string.Empty;
            //try
            //{
            //    Configuracao config = GetConfiguracao();


            //    if (config == null)
            //        throw new Exception("� necess�rio definir o servidor de envio de email em configura��o");

            //    sb.Clear();
            //    sb.Append("SELECT DISTINCT ");
            //    sb.Append("  ClienteDiasBaixar,ClienteNotificar,  (CASE WHEN ClientePaiId = null THEN ClientePaiId ELSE ClienteId END) ClienteId, IFNULL(ClienteEmail, '') ClienteEmail ");
            //    sb.Append("FROM cliente ");
            //    sb.Append("where(ClienteEmail IS NOT NULL AND ClienteEmail <> '') ");
            //    sb.Append("or(ClienteDiasBaixar IS NOT NULL AND ClienteDiasBaixar > 0) ");

            //    var clientes = Connection.Query<Cliente>(sb.ToString(), null, transaction: transaction).ToList();

            //    for (int i = 0; i < clientes.Count; i++)
            //    {
            //        _textRobos = string.Empty;
            //        _textRequisicoes = string.Empty;
            //        _textFaturasAtrasadas = string.Empty;
            //        _textFaturasHoje = string.Empty;
            //        _textFaturasInconsistentes = string.Empty;


            //        sb.Clear();
            //        cliente = clientes[i];
            //        _textFaturasAtrasadas = GetNotificaFaturasAtrasadas(cliente.ClienteId, cliente.ClienteDiasBaixar, cliente.ClienteEmail);

            //        if (!string.IsNullOrEmpty(cliente.ClienteEmail))
            //        {
            //            _textRequisicoes = GetNotificaRequisicoes(cliente.ClienteId);
            //            _textRobos = GetRobos(cliente.ClienteId);
            //            _textFaturasHoje = GetNotificaFaturasHoje(cliente.ClienteId);
            //            _textFaturasInconsistentes = GetNotificaFaturasInconsistentes(cliente.ClienteId);

            //            sb.Append(GetHeader(cliente));
            //        }
            //        else
            //            continue;


            //        if (!string.IsNullOrEmpty(_textRequisicoes))
            //            sb.Append($"{_textRequisicoes}</br></br>");

            //        if (!string.IsNullOrEmpty(_textRobos))
            //            sb.Append($"{_textRobos}</br></br>");

            //        if (!string.IsNullOrEmpty(_textFaturasAtrasadas))
            //            sb.Append($"{_textFaturasAtrasadas}</br></br>");

            //        if (!string.IsNullOrEmpty(_textFaturasHoje))
            //            sb.Append($"{_textFaturasHoje}</br></br>");

            //        if (!string.IsNullOrEmpty(_textFaturasInconsistentes))
            //            sb.Append($"{_textFaturasInconsistentes}</br></br>");

            //        var toEmails = cliente.ClienteEmail.Split(";");

            //        if (cliente.ClienteNotificar)
            //        {

            //            for (int j = 0; j < toEmails.Length; j++)
            //            {
            //                if (!string.IsNullOrEmpty(toEmails[j]))
            //                {
            //                    try
            //                    {
            //                        MailMessage mm = new MailMessage(config.FromEmail, toEmails[j], "Notificacao de situacao de coleta de faturas", sb.ToString());
            //                        mm.IsBodyHtml = true;
            //                        SmtpClient sm = new SmtpClient(config.HostEmail, config.PortEmail);
            //                        sm.UseDefaultCredentials = false;
            //                        sm.Credentials = new NetworkCredential(config.FromEmail, config.PassEmail);
            //                        sm.Send(mm);
            //                    }
            //                    catch//ro123.01
            //                    {

            //                    }
            //                }

            //            }
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}

        }

        public List<dynamic> GetUltimosArquivoColetado(int? LoggedclienteId, int? LoggedEmpresaId)
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();

            sb.Append("SELECT o.SituacaoOrcamento, DATE_FORMAT(DATE_ADD(o.DataInclusao, INTERVAL -3 HOUR),'%d/%m/%Y %H:%i') AS DtInclusao, DATE_FORMAT(o.Vencimento,'%d/%m/%Y') AS DtVencimento , CONCAT('R$ ', REPLACE(REPLACE(REPLACE(FORMAT(o.Valor, 2),'.',';'),',','.'),';',',')) AS Total, f.Nome, o.ClienteNome, ");

            sb.Append("    (CASE WHEN o.SituacaoOrcamento=1 THEN 'D' WHEN o.SituacaoOrcamento=2 THEN 'A' WHEN o.SituacaoOrcamento=3 THEN 'P' WHEN o.SituacaoOrcamento=4 THEN 'C' ELSE 'D' END) descricaoSituacao, ");
            sb.Append("    (CASE WHEN o.SituacaoOrcamento=1 THEN '#21493E' WHEN o.SituacaoOrcamento=2 THEN '#EB323A' WHEN o.SituacaoOrcamento=3 THEN '#3C8DE6' WHEN o.SituacaoOrcamento=4 THEN '#FF7900' ELSE '#21493E' END) CorSituacaoLinha, ");
            sb.Append("    (CASE WHEN o.SituacaoOrcamento=1 THEN 'avatar-sm-box bg-inverse' WHEN o.SituacaoOrcamento=2 THEN 'avatar-sm-box bg-danger' WHEN o.SituacaoOrcamento=3 THEN 'avatar-sm-box bg-success' WHEN o.SituacaoOrcamento=4 THEN 'avatar-sm-box bg-teal' ELSE 'avatar-sm-box bg-warning' END) CorSituacao,  ");
            sb.Append("    (CASE WHEN o.SituacaoOrcamento=1 THEN 'Desconhecido' WHEN o.SituacaoOrcamento=2 THEN 'A Pagar' WHEN o.SituacaoOrcamento=3 THEN 'Pago' WHEN o.SituacaoOrcamento=4 THEN 'Contestado' ELSE 'Desconhecido' END) titleSituacao  ");

            sb.Append(" FROM orcamento o ");
            sb.Append(" INNER JOIN fornecedor f ON o.Fornecedor = f.Id ");
            sb.Append(" WHERE 1 = 1");

            if (LoggedclienteId != null)
                sb.AppendFormat(" AND f.Id = {0} ", LoggedclienteId.Value);

            if (LoggedEmpresaId != null && LoggedEmpresaId.Value > 0)
                sb.AppendFormat(" AND f.Empresa = {0}", LoggedEmpresaId.Value);

            sb.Append(" ORDER BY o.DataInclusao DESC ");
            sb.Append("LIMIT 4 ");
            var result = Connection.Query<dynamic>(sb.ToString(), null, transaction: transaction).ToList();


            return result;
        }

        public List<dynamic> GetCalendario(int? LoggedclienteId, int? LoggedEmpresaId)
        {
            bool isNotAdmin = LoggedclienteId != null;
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT (CASE WHEN o.TipoDocumento = 2 THEN DATE_ADD(o.DataInclusao, INTERVAL 20 DAY) ELSE DATE_ADD(o.DataInclusao, INTERVAL 30 DAY) END) Vencimento,  ");
            sb.Append("      DATE_FORMAT((CASE WHEN o.TipoDocumento = 2 THEN DATE_ADD(o.DataInclusao, INTERVAL 20 DAY) ELSE DATE_ADD(o.DataInclusao, INTERVAL 30 DAY) END), '%d/%m/%Y') DtVencimento,  ");
            sb.Append("      f.Nome,  ");
            sb.Append("      CONCAT('R$ ', REPLACE(REPLACE(REPLACE(FORMAT(o.Valor, 2), '.', ';'), ',', '.'), ';', ',')) Valor, ");
            sb.Append("      CONCAT(f.Nome, ' - R$ ', REPLACE(REPLACE(REPLACE(FORMAT(o.Valor, 2), '.', ';'), ',', '.'), ';', ',')) Title, ");
            sb.Append("      (CASE WHEN o.SituacaoOrcamento = 3 THEN 'Enviado p/ JVL' WHEN o.SituacaoOrcamento = 4 THEN 'Validada' ");
            sb.Append("      ELSE 'bg-primary' END) Situacao, ");
            sb.Append("      (CASE WHEN o.TipoDocumento = 1 THEN 'bg-primary' WHEN o.TipoDocumento = 2 THEN 'bg-orange' WHEN o.TipoDocumento = 3 THEN 'bg-teal' ");
            sb.Append("      ELSE 'bg-primary' END) ClassName ");
            sb.Append(" FROM orcamento o ");
            sb.Append(" INNER JOIN fornecedor f on f.Id = o.Fornecedor ");
            if(isNotAdmin) 
                sb.AppendFormat(" INNER JOIN usuario u on u.Fornecedor = o.Fornecedor and u.Id = {0} ", LoggedclienteId.Value);            
            sb.Append(" WHERE o.SituacaoOrcamento IN (3,4) ");

            if (LoggedEmpresaId != null && LoggedEmpresaId.Value > 0)
                sb.AppendFormat(" AND f.Empresa = {0}", LoggedEmpresaId.Value);

            sb.Append(" ORDER BY o.Vencimento ASC ");
            var result = Connection.Query<dynamic>(sb.ToString(), null, transaction: transaction).ToList();

            return result;
        }

        public List<Documento> GetMesesByLoggedclienteId(int? LoggedclienteId, string centrosCusto)
        {

            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT DISTINCT (CONCAT(lpad(MONTH(ac.DtVencimento), 2, '0'), '/', YEAR(ac.DtVencimento))) Vencimento FROM arquivocoletado ac ");
            sb.Append(" INNER JOIN fornecedorcliente fc ON(ac.FkFornecedorClienteId = fc.FornecedorClienteId) ");
            sb.Append(" inner join cliente c on c.ClienteId = fc.FkClienteId WHERE 1 = 1");
            if (LoggedclienteId != null)
                sb.AppendFormat(" AND (fc.FkClienteId = {0} OR c.ClientePaiId = {0})", LoggedclienteId.Value);

            if (!string.IsNullOrEmpty(centrosCusto))
                sb.AppendFormat(" AND c.FkCentroCustoId IN ({0}) ", centrosCusto);

            sb.Append(" ORDER BY DtVencimento DESC ");

            var result = Connection.Query<Documento>(sb.ToString(), null, transaction: transaction).ToList();


            return result;
        }
        public List<dynamic> GetValorPorMes(int? LoggedclienteId, int? LoggedEmpresaId, int typeInvoice)
        {
            bool isNotAdmin = LoggedclienteId != null;

            StringBuilder sb = new StringBuilder();
            // v1
            sb.Append("SELECT concat(LPAD(Mes,2,'0'),'/',Ano) Mes, Total ");
            sb.Append(" FROM( " );
            sb.Append("	SELECT YEAR(o.DataInclusao) Ano, MONTH(o.DataInclusao) Mes, SUM(IFNULL(o.Valor, 0)) Total ");
            sb.Append("		FROM orcamento o ");
            sb.Append(" INNER JOIN fornecedor f on o.Fornecedor = f.Id  ");
            if (isNotAdmin) 
                sb.AppendFormat(" INNER JOIN usuario u on u.Fornecedor = o.Fornecedor and u.Id = {0} ", LoggedclienteId.Value);



            sb.Append($"     WHERE o.SituacaoOrcamento in (3,4) AND o.TipoDocumento = {typeInvoice}"); // Status aprovado enviado para pagamento

            if (LoggedEmpresaId != null && LoggedEmpresaId.Value > 0)
                sb.AppendFormat(" AND f.Empresa = {0}", LoggedEmpresaId.Value);

            sb.Append("        GROUP BY YEAR(o.DataInclusao) , MONTH(o.DataInclusao) ");
            sb.Append("        ORDER BY YEAR(o.DataInclusao) DESC, MONTH(o.DataInclusao) DESC ");
            sb.Append("        LIMIT 4 ");
            sb.Append(" ) T  ORDER BY Mes, Total ");


            // v2

            // sb.Append("	SELECT YEAR(o.DataInclusao) Ano, MONTH(o.DataInclusao) Mes, SUM(IFNULL(o.Valor, 0)) Total, o.TipoDocumento ");
            // sb.Append("		FROM orcamento o ");        
            // if(isNotAdmin) {
            //     sb.Append(" LEFT JOIN usuario u on u.Id = o.UsuarioInclusao ");
            //     sb.Append(" INNER JOIN fornecedor f on f.Id = u.Fornecedor ");
            // }
            // sb.Append("     WHERE o.SituacaoOrcamento in (3,4) "); // Status aprovado enviado para pagamento
            // if (isNotAdmin)
            //     sb.AppendFormat(" AND o.UsuarioInclusao = {0}", LoggedclienteId.Value);    
            // sb.Append("        GROUP BY YEAR(o.DataInclusao) , MONTH(o.DataInclusao), o.TipoDocumento ");
            // sb.Append("        ORDER BY YEAR(o.DataInclusao) DESC, MONTH(o.DataInclusao) DESC ");
            // sb.Append("        LIMIT 4 ");

            var result = Connection.Query<dynamic>(sb.ToString(), null, transaction: transaction).ToList();

            return result;
        }

        public List<dynamic> GetValorTipo(int? LoggedclienteId, int? LoggedEmpresaId)
        {

            StringBuilder sb = new StringBuilder();

            sb.Clear();
            //sb.Append(" SELECT (CASE WHEN o.SituacaoOrcamento<3 THEN 'Peças' ELSE 'Serviços' END) As nome, ");
            //sb.Append(" count(*) qtd FROM orcamento o ");
            //sb.Append(" inner join fornecedor f on f.Id = o.Fornecedor ");
            //sb.Append(" WHERE 1 = 1 ");

            //if (LoggedclienteId != null)
            //    sb.AppendFormat(" AND f.Id = {0} ", LoggedclienteId.Value);

            //sb.Append(" group by o.TipoDocumento ");
            //sb.Append(" ORDER BY o.TipoDocumento ");

            sb.Append(" SELECT nome, count(*) qtd FROM( ");
            sb.Append(" SELECT (CASE WHEN o.SituacaoOrcamento<3 THEN 'Peças' ELSE 'Serviços' END) As nome ");
            sb.Append(" FROM orcamento o ");
            sb.Append(" inner join fornecedor f on f.Id = o.Fornecedor ");
            sb.Append(" WHERE 1 = 1 ");

            if (LoggedclienteId != null)
                sb.AppendFormat(" AND f.Id = {0} ", LoggedclienteId.Value);

            if (LoggedEmpresaId != null && LoggedEmpresaId.Value > 0)
                sb.AppendFormat(" AND f.Empresa = {0}", LoggedEmpresaId.Value);

            sb.Append(" ) T ");
            sb.Append(" group by nome  ORDER BY nome ");

            var result = Connection.Query<dynamic>(sb.ToString(), null, transaction: transaction).ToList();

            return result;
        }
        public List<dynamic> GetValorTipoMes(int? LoggedclienteId, int? LoggedEmpresaId)
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();

            sb.Append("SELECT vencimento, TipoDocumento, SUm(Total) Total FROM( ");
            sb.Append("SELECT ");
            sb.Append("CAST(CONCAT(YEAR(o.Vencimento), '-', Month(o.Vencimento), '-01') AS DATE)  vencimento,   ");
            sb.Append("(CASE WHEN o.SituacaoOrcamento < 3 THEN 1 ELSE 2 END) As TipoDocumento, ");
            sb.Append("IFNULL(o.Valor, 0) Total ");
            sb.Append("FROM orcamento o ");
            sb.Append("left join fornecedor f on f.Id = o.Fornecedor ");
            sb.Append("where o.Vencimento is not null ");
            if (LoggedclienteId != null)
                sb.AppendFormat(" AND c.ClienteId = {0} OR c.ClientePaiId = {0}", LoggedclienteId.Value);

            if (LoggedEmpresaId != null && LoggedEmpresaId.Value > 0)
                sb.AppendFormat(" AND f.Empresa = {0}", LoggedEmpresaId.Value);

            sb.Append(") T ");
            sb.Append("group by vencimento , TipoDocumento order by vencimento desc, TipoDocumento  ");

            //sb.Append("SELECT CAST(CONCAT(YEAR(o.Vencimento), '-', Month(o.Vencimento), '-01') AS DATE)  vencimento, ");
            //sb.Append(" o.TipoDocumento, ");
            //sb.Append(" sum(IFNULL(o.Valor, 0)) Total FROM orcamento o ");
            //sb.Append("left join fornecedor f on f.Id = o.Fornecedor ");
            //sb.Append("where o.Vencimento is not null ");

            //if (LoggedclienteId != null)
            //    sb.AppendFormat(" AND c.ClienteId = {0} OR c.ClientePaiId = {0}", LoggedclienteId.Value);

            //sb.Append(" group by CAST(CONCAT(YEAR(o.Vencimento), '-', Month(o.Vencimento), '-01') AS DATE) desc, o.TipoDocumento ");


            var result = Connection.Query<dynamic>(sb.ToString(), null, transaction: transaction).ToList();

            dynamic _tipoMes = new System.Dynamic.ExpandoObject();
            List<dynamic> lstTipoMes = new List<dynamic>();
            if (result.Count > 0)
            {
                var latestDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                latestDate = latestDate.AddMonths(-6);

                var orderByResult = (from x in result
                                     where x.vencimento >= latestDate
                                     orderby x.vencimento
                                     select x).ToList();

                for (int i = 0; i <= 6; i++)
                {
                    var orderByResult1 = (from x in orderByResult
                                          where x.vencimento == latestDate.AddMonths(i)
                                          orderby x.vencimento
                                          select x).ToList();

                    var tipo1 = orderByResult1.Where(x => x.TipoDocumento == 1).FirstOrDefault();
                    _tipoMes = new System.Dynamic.ExpandoObject();
                    if (tipo1 != null)
                    {
                        _tipoMes.Tipo = "Peças";
                        _tipoMes.Mes = GetMesTipoMes(latestDate.AddMonths(i).ToString("MM/yyyy"));
                        _tipoMes.Valor = tipo1.Total;
                    }
                    else
                    {
                        _tipoMes.Tipo = "Peças";
                        _tipoMes.Mes = GetMesTipoMes(latestDate.AddMonths(i).ToString("MM/yyyy"));
                        _tipoMes.Valor = 0;
                    }
                    lstTipoMes.Add(_tipoMes);

                    var tipo2 = orderByResult1.Where(x => x.TipoDocumento == 2).FirstOrDefault();
                    _tipoMes = new System.Dynamic.ExpandoObject();
                    if (tipo2 != null)
                    {
                        _tipoMes.Tipo = "Serviços";
                        _tipoMes.Mes = GetMesTipoMes(latestDate.AddMonths(i).ToString("MM/yyyy"));
                        _tipoMes.Valor = tipo2.Total;
                    }
                    else
                    {
                        _tipoMes.Tipo = "Serviços";
                        _tipoMes.Mes = GetMesTipoMes(latestDate.AddMonths(i).ToString("MM/yyyy"));
                        _tipoMes.Valor = 0;
                    }
                    lstTipoMes.Add(_tipoMes);

                    //var tipo3 = orderByResult1.Where(x => x.TipoFornecedorId == 3).FirstOrDefault();
                    //_tipoMes = new System.Dynamic.ExpandoObject();
                    //if (tipo3 != null)
                    //{
                    //    _tipoMes.Tipo = "Ambos";
                    //    _tipoMes.Mes = GetMesTipoMes(latestDate.AddMonths(i).ToString("MM/yyyy"));
                    //    _tipoMes.Valor = tipo3.Total;
                    //}
                    //else
                    //{
                    //    _tipoMes.Tipo = "Ambos";
                    //    _tipoMes.Mes = GetMesTipoMes(latestDate.AddMonths(i).ToString("MM/yyyy"));
                    //    _tipoMes.Valor = 0;
                    //}
                    //lstTipoMes.Add(_tipoMes);

                    //lstTipoMes.Add(_tipoMes);
                }


            }

            return lstTipoMes;
        }

        private string GetMesTipoMes(string mes)
        {
            var _split = mes.Split("/");
            string _return = "";
            int caseSwitch = Convert.ToInt32(_split[0]);

            switch (caseSwitch)
            {
                case 1:
                    _return = $"JAN/{_split[1]}";
                    break;
                case 2:
                    _return = $"FEV/{_split[1]}";
                    break;
                case 3:
                    _return = $"MAR/{_split[1]}";
                    break;
                case 4:
                    _return = $"ABR/{_split[1]}";
                    break;
                case 5:
                    _return = $"MAI/{_split[1]}";
                    break;
                case 6:
                    _return = $"JUN/{_split[1]}";
                    break;
                case 7:
                    _return = $"JUL/{_split[1]}";
                    break;
                case 8:
                    _return = $"AGO/{_split[1]}";
                    break;
                case 9:
                    _return = $"SET/{_split[1]}";
                    break;
                case 10:
                    _return = $"OUT/{_split[1]}";
                    break;
                case 11:
                    _return = $"NOV/{_split[1]}";
                    break;
                default:
                    _return = $"DEZ/{_split[1]}";
                    break;
            }

            return _return;
        }

        public List<dynamic> GetValorPorTipo(int? LoggedclienteId, string centrosCusto)
        {

            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT Mes, Total FROM( ");
            sb.Append("SELECT concat(LPAD(MONTH(ar.DtVencimento),2,'0'),'/',YEAR(ar.DtVencimento)) Mes, SUM(IFNULL(ar.Total, 0)) Total FROM arquivocoletado ar ");
            sb.Append("INNER JOIN fornecedorcliente fc ON(ar.FkFornecedorClienteId = fc.FornecedorClienteId) ");
            sb.Append(" inner join cliente c on c.ClienteId = fc.FkClienteId WHERE 1 =1 ");

            if (LoggedclienteId != null)
                sb.AppendFormat(" AND fc.FkClienteId = {0} OR c.ClientePaiId = {0}", LoggedclienteId.Value);

            if (!string.IsNullOrEmpty(centrosCusto))
                sb.AppendFormat(" AND c.FkCentroCustoId IN ({0}) ", centrosCusto);

            sb.Append(" GROUP BY MONTH(ar.DtVencimento) ORDER BY YEAR(ar.DtVencimento) DESC, MONTH(ar.DtVencimento) DESC LIMIT 4 ");

            sb.Append(") T ");
            sb.Append("ORDER BY Mes, Total ");

            var result = Connection.Query<dynamic>(sb.ToString(), null, transaction: transaction).ToList();

            return result;
        }
        public List<dynamic> GetValorPorUnidade(int? LoggedclienteId, string centrosCusto)
        {

            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT Mes, Total FROM( ");
            sb.Append("SELECT concat(LPAD(MONTH(ar.DtVencimento),2,'0'),'/',YEAR(ar.DtVencimento)) Mes, SUM(IFNULL(ar.Total, 0)) Total FROM arquivocoletado ar ");
            sb.Append("INNER JOIN fornecedorcliente fc ON(ar.FkFornecedorClienteId = fc.FornecedorClienteId) ");
            sb.Append(" inner join cliente c on c.ClienteId = fc.FkClienteId WHERE 1 =1 ");

            if (LoggedclienteId != null)
                sb.AppendFormat(" AND fc.FkClienteId = {0} OR c.ClientePaiId = {0}", LoggedclienteId.Value);

            if (!string.IsNullOrEmpty(centrosCusto))
                sb.AppendFormat(" AND c.FkCentroCustoId IN ({0}) ", centrosCusto);

            sb.Append(" GROUP BY MONTH(ar.DtVencimento) ORDER BY YEAR(ar.DtVencimento) DESC, MONTH(ar.DtVencimento) DESC LIMIT 4 ");

            sb.Append(") T ");
            sb.Append("ORDER BY Mes, Total ");

            var result = Connection.Query<dynamic>(sb.ToString(), null, transaction: transaction).ToList();

            return result;
        }
        public List<dynamic> GetDocumentos(DateTime datade, DateTime dataate, string emissor, string cnpj)
        {

            StringBuilder sb = new StringBuilder();


            sb.Clear();
            sb.Append("SELECT ac.CaminhoArquivo FROM arquivocoletado ac ");
            sb.Append(" inner join fornecedorcliente fc on(ac.FkFornecedorClienteId = fc.FornecedorClienteId) ");
            sb.Append(" inner join fornecedor f on(f.FornecedorId = fc.FkFornecedorId) ");
            sb.Append(" inner join cliente c on(fc.FkClienteId = c.ClienteId) ");
            sb.AppendFormat(" where ac.DtVencimento >= '{0}'", datade.ToString("yyyy-MM-dd"));
            sb.AppendFormat(" and ac.DtVencimento <= '{0}'", dataate.ToString("yyyy-MM-dd"));
            sb.AppendFormat(" and f.FornecedorNome like '{0}%' ", emissor);
            sb.AppendFormat(" and c.ClienteCnpj = '{0}' ", cnpj);

            var result = Connection.Query<dynamic>(sb.ToString(), null, transaction: transaction).ToList();

            return result;
        }

        private string GetNomeColuna(int posicao)
        {
            if (posicao == 1)
                return "c.ClienteNome";
            else if (posicao == 2)
                return "f.FornecedorNome";
            else if (posicao == 3)
                return "ac.Status";
            else
                return "ac.DtVencimento";

        }
        public List<Documento> List(DataTableFilter filter)
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT ac.ArquivoColetadoId, ac.DtInclusao, ac.Status,ac.NrMesReferencia,ac.CaminhoArquivo, ac.CodigoBarras, ac.DtVencimento, ac.Total, ");
            sb.Append("    (CASE WHEN ac.Status=1 THEN '1' WHEN ac.Status=2 THEN '2' ELSE '3' END) descricaoStatus, ");
            sb.Append("    c.ClienteId,  ");
            sb.Append("    c.ClienteNome, ");
            sb.Append("    f.FornecedorId, ");
            sb.Append("    f.FornecedorNome ");
            sb.Append(" FROM arquivocoletado ac ");
            sb.Append(" INNER JOIN fornecedorcliente fc ON(fc.FornecedorClienteId = ac.FkFornecedorClienteId) ");
            sb.Append(" INNER JOIN cliente c ON(c.ClienteId = fc.FkClienteId) ");
            sb.Append(" INNER JOIN fornecedor f ON(f.FornecedorId = fc.FkFornecedorId) ");

            if (!string.IsNullOrEmpty(filter.search.value))
                sb.AppendFormat(" AND f.FornecedorNome LIKE '{0}%'", filter.search.value);

            if (filter.order.Count > 0)
                sb.AppendFormat(" ORDER BY {0} {1} ", GetNomeColuna(filter.order[0].column), filter.order[0].dir);

            //var data = Connection.Query<Orcamento, Cliente, Fornecedor, Orcamento>(sb.ToString(), (arquivoColetado, cliente, fornecedor) =>
            //{
            //    arquivoColetado.FkFornecedorClienteId = new FornecedorCliente();
            //    arquivoColetado.FkFornecedorClienteId.FkClienteId = cliente;
            //    arquivoColetado.FkFornecedorClienteId.FkFornecedorId = fornecedor;
            //    return arquivoColetado;
            //}, splitOn: "ArquivoColetadoId,ClienteId, FornecedorId", transaction: transaction).ToList();

            //return data;
            return null;
        }
        public Documento GetArquivoColetadoById(int clienteId, int fornecedorId)
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT ac.ArquivoColetadoId, ac.CodigoBarras, ac.DtInclusao, ac.Status,ac.NrMesReferencia,ac.CaminhoArquivo, ac.DtVencimento, ac.Total, ");
            sb.Append("    (CASE WHEN ac.Status=1 THEN '1' WHEN ac.Status=2 THEN '2' ELSE '3' END) descricaoStatus, ");
            sb.Append("    fc.FornecedorClienteId,  ");
            sb.Append("    c.ClienteId,  ");
            sb.Append("    c.ClienteNome, ");
            sb.Append("    f.FornecedorId, ");
            sb.Append("    f.FornecedorNome, ");
            sb.Append("    tf.TipoFornecedorId,  ");
            sb.Append("    tf.TipoFornecedorNome  ");
            sb.Append(" FROM arquivocoletado ac ");
            sb.Append(" INNER JOIN fornecedorcliente fc ON(fc.FornecedorClienteId = ac.FkFornecedorClienteId) ");
            sb.Append(" INNER JOIN cliente c ON(c.ClienteId = fc.FkClienteId) ");
            sb.Append(" INNER JOIN fornecedor f ON(f.FornecedorId = fc.FkFornecedorId) ");
            sb.Append(" INNER JOIN tipofornecedor tf ON (f.FkTipoFornecedorId = tf.TipoFornecedorId) ");
            sb.AppendFormat(" WHERE fc.FkClienteId = {0}", clienteId);
            sb.AppendFormat(" AND fc.FkFornecedorId = {0}", fornecedorId);



            //var data = Connection.Query<Orcamento, FornecedorCliente, Cliente, Fornecedor, TipoFornecedor, Orcamento>(sb.ToString(), (arquivoColetado, fornecedorCliente, cliente, fornecedor, tipofornecedor) =>
            //{
            //    fornecedor.FkTipoFornecedorId = tipofornecedor;

            //    fornecedorCliente.FkClienteId = cliente;
            //    fornecedorCliente.FkFornecedorId = fornecedor;
            //    arquivoColetado.FkFornecedorClienteId = fornecedorCliente;
            //    //arquivoColetado.FkFornecedorClienteId.FkClienteId = cliente;
            //    //fornecedor.FkTipoFornecedorId = tipofornecedor;
            //    //arquivoColetado.FkFornecedorClienteId.FkFornecedorId = fornecedor;
            //    return arquivoColetado;
            //}, splitOn: "ArquivoColetadoId,FornecedorClienteId, ClienteId, FornecedorId, TipoFornecedorId", transaction: transaction).FirstOrDefault();

            //return data;
            return null;
        }
        public List<Documento> List(int? loggedfornecedorId, int? loggedEmpresaId, string palavra, DataTableFilter filter)
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("select o.Id, o.Vencimento, o.Ativo, o.Valor, o.ClienteNome, o.ClientePessoa, o.caminhoAnexo, o.SituacaoOrcamento, o.DataInclusao, ");
            sb.Append(" f.Id, f.Nome  ");
            sb.Append(" from orcamento o  ");
            sb.Append(" inner join fornecedor f on(o.Fornecedor = f.Id)  ");
            sb.Append(" WHERE 1 = 1 ");

            if (loggedfornecedorId != null)
                sb.AppendFormat(" AND f.Id = {0} ", loggedfornecedorId.Value);

            if (loggedEmpresaId != null && loggedEmpresaId.Value > 0)
                sb.AppendFormat(" AND f.Empresa = {0}", loggedEmpresaId.Value);

            if (!string.IsNullOrEmpty(palavra) && palavra != "undefined")
                sb.AppendFormat(" AND (UPPER(f.Nome) like '%{0}%' or UPPER(o.ClienteNome) like '%{0}%') ", palavra.ToUpper());

            var data = Connection.Query<Documento, Fornecedor, Documento>(sb.ToString(), (nf, fornecedor) =>
            {
                //nf.Fornecedor = fornecedor;
                return nf;
            }, splitOn: "Id,Id", transaction: transaction).ToList();

            return data;
        }

        public List<Documento> ListByMesFornecedorCliente(int fornecedorClienteId, string ano, string mes, DataTableFilter filter)
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT DATE_FORMAT(ac.DataEntrega,'%d/%m/%Y') AS DataEntregaDescricao , DATE_FORMAT(ac.DtInclusao,'%d/%m/%Y') AS DtInclusaoDescricao, ac.Observacao, ac.DataEntrega, ac.NumeroPedido, ac.ArquivoColetadoId, ac.CodigoBarras, ac.Situacao, ac.DtInclusao, ac.Status,ac.NrMesReferencia,ac.CaminhoArquivo, ac.DtVencimento, ac.Total, DATE_FORMAT(ac.DtVencimento,'%d/%m/%Y') Vencimento, ");
            sb.Append("    CONCAT((CASE WHEN MONTH(ac.NrMesReferencia)=1 THEN 'Jan' WHEN MONTH(ac.NrMesReferencia)=2 THEN 'Fev' WHEN MONTH(ac.NrMesReferencia)=3 THEN 'Mar' WHEN MONTH(ac.NrMesReferencia)=4 THEN 'Abr' WHEN MONTH(ac.NrMesReferencia)=5 THEN 'Mai' WHEN MONTH(ac.NrMesReferencia)=6 THEN 'Jun' WHEN MONTH(ac.NrMesReferencia)=7 THEN 'Jul' WHEN MONTH(ac.NrMesReferencia)=8 THEN 'Ago' WHEN MONTH(ac.NrMesReferencia)=9 THEN 'Set' WHEN MONTH(ac.NrMesReferencia)=10 THEN 'Out' WHEN MONTH(ac.NrMesReferencia)=11 THEN 'Nov'  ELSE 'Dez' END),'/', YEAR(ac.NrMesReferencia)) Competencia, ");
            sb.Append("    (CASE WHEN ac.Situacao=0 THEN '#21493E' WHEN ac.Situacao=1 THEN '#EB323A' WHEN ac.Situacao=2 THEN '#3C8DE6' WHEN ac.Situacao=3 THEN '#FF7900'  WHEN ac.Situacao=4 THEN '#42478B' WHEN ac.Situacao=5 THEN '#2A9E79' WHEN ac.Situacao=7 THEN '#5A042B' ELSE '#21493E' END) CorSituacaoLinha, ");
            sb.Append("    (CASE WHEN ac.Status=1 THEN '1' WHEN ac.Status=2 THEN '2' ELSE '3' END) descricaoStatus, ");
            sb.Append("    (CASE WHEN ac.Situacao=0 THEN 'D' WHEN ac.Situacao=1 THEN 'A' WHEN ac.Situacao=2 THEN 'P' WHEN ac.Situacao=3 THEN 'C'  WHEN ac.Situacao=4 THEN 'A' WHEN ac.Situacao=5 THEN 'I' WHEN ac.Situacao=7 THEN 'S' ELSE 'U' END) descricaoSituacao, ");
            sb.Append("    (CASE WHEN ac.Situacao=0 THEN 'avatar-sm-box bg-inverse' WHEN ac.Situacao=1 THEN 'avatar-sm-box bg-danger' WHEN ac.Situacao=2 THEN 'avatar-sm-box bg-success' WHEN ac.Situacao=3 THEN 'avatar-sm-box bg-brown' WHEN ac.Situacao=4 THEN 'avatar-sm-box bg-warning'  WHEN ac.Situacao=5 THEN 'avatar-sm-box bg-primary' WHEN ac.Situacao=7 THEN 'avatar-sm-box bg-inverse' ELSE 'avatar-sm-box bg-info' END) CorSituacao, ");
            sb.Append("    (CASE WHEN ac.Situacao=0 THEN 'Desconhecido' WHEN ac.Situacao=1 THEN 'A Pagar' WHEN ac.Situacao=2 THEN 'Pago' WHEN ac.Situacao=3 THEN 'Contestado'  WHEN ac.Situacao=4 THEN 'Atrasado' WHEN ac.Situacao=5 THEN 'Isento' WHEN ac.Situacao=7 THEN 'Processado' ELSE 'Debito Automatico' END) titleSituacao, ");
            sb.Append("    c.ClienteId,  ");
            sb.Append("    c.ClienteNome, ");
            sb.Append("    (CASE WHEN c.TipoPessoa=1 THEN MASK(c.ClienteCnpj, '###.###.###-##') ELSE MASK(c.ClienteCnpj, '##.###.###/####-##') END) ClienteCnpj, ");
            sb.Append("    c.ClienteCidade, ");
            sb.Append("    c.ClienteNome, ");
            sb.Append("    c.ClienteUF, ");
            sb.Append("    f.FornecedorId, ");
            sb.Append("    f.FornecedorNome, ");
            sb.Append("    tf.TipoFornecedorId,  ");
            sb.Append("    ( CASE WHEN stf.SubTipoFornecedorNome IS NULL THEN tf.tipofornecedornome ELSE CONCAT( tf.tipofornecedornome,' - ',stf.SubTipoFornecedorNome) END) tipofornecedornome,  ");
            sb.Append("    cc.CentroCustoId,  ");
            sb.Append("    (CASE WHEN cc.CentroCustoCodigo IS NULL THEN  cc.CentroCustoNome WHEN cc.CentroCustoCodigo='' THEN cc.CentroCustoNome ELSE CONCAT(cc.CentroCustoCodigo, ' - ',cc.CentroCustoNome ) END) CentroCustoNome ");
            sb.Append(" FROM arquivocoletado ac ");
            sb.Append(" INNER JOIN fornecedorcliente fc ON(fc.FornecedorClienteId = ac.FkFornecedorClienteId) ");
            sb.Append(" INNER JOIN cliente c ON(c.ClienteId = fc.FkClienteId) ");
            sb.Append(" INNER JOIN fornecedor f ON(f.FornecedorId = fc.FkFornecedorId) ");
            sb.Append(" INNER JOIN tipofornecedor tf ON (f.FkTipoFornecedorId = tf.TipoFornecedorId) ");
            sb.Append(" LEFT JOIN SubTipoFornecedor stf ON (stf.SubTipoFornecedorId = ac.FkSubTipoFornecedorId  ) ");
            sb.Append(" LEFT JOIN centrocusto cc ON (c.FkCentroCustoId = cc.CentroCustoId) ");
            sb.Append($" WHERE fc.FornecedorClienteId = {fornecedorClienteId} ");
            sb.Append($" AND YEAR(ac.DtVencimento) = {ano} ");
            sb.Append($" AND month(ac.DtVencimento) = {mes} ");



            //var data = Connection.Query<Orcamento, Cliente, Fornecedor, TipoFornecedor, CentroCusto, Orcamento>(sb.ToString(), (arquivoColetado, cli, forn, tipofornecedor, centrocusto) =>
            //{
            //    arquivoColetado.FkFornecedorClienteId = new FornecedorCliente();
            //    arquivoColetado.FkFornecedorClienteId.FkClienteId = cli;
            //    arquivoColetado.FkFornecedorClienteId.FkClienteId.FkCentroCustoId = centrocusto == null ? new CentroCusto() : centrocusto;
            //    tipofornecedor.TipoFornecedorNome = GetSubTipo(tipofornecedor.TipoFornecedorNome, arquivoColetado.SubTipo);
            //    forn.FkTipoFornecedorId = tipofornecedor;
            //    arquivoColetado.FkFornecedorClienteId.FkFornecedorId = forn;
            //    return arquivoColetado;
            //}, splitOn: "ArquivoColetadoId,ClienteId, FornecedorId, TipoFornecedorId, CentroCustoId", transaction: transaction).ToList();

            //return data;
            return null;
        }
        public List<dynamic> ListByMes(int? LoggedclienteId, string ano, string mes, int? cliente, int? emissor, string situacao, DataTableFilter filter)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();

            sb.Append("SELECT FornecedorIsRobo, FornecedorView, FornecedorClienteId, FornecedorNome, TipoFornecedorNome, ClienteNome, DocumentoQuantidade, ");
            sb.Append("IFNULL(DATE_FORMAT(InicioColeta, '%d/%m/%Y'),'') InicioColeta, ");
            sb.Append("Sum(qtdcoletado) qtdcoletado FROM( ");
            sb.Append("SELECT f.FornecedorIsRobo, f.FornecedorView,fc.FornecedorClienteId, f.FornecedorNome, tf.TipoFornecedorNome, c.ClienteNome, fc.DocumentoQuantidade, ");
            sb.Append("(select ");
            sb.Append("(CASE WHEN CURRENT_DATE() > Date_format(Concat(Year(CURRENT_DATE()), '-', Month(CURRENT_DATE()), '-', fornecedorclientedadosacesso.fornecedorclientedadosacessodiavencimento), '%Y-%m-%d') THEN Date_add(Date_format(Concat(Year(CURRENT_DATE()), '-', Month(CURRENT_DATE()), '-', fornecedorclientedadosacesso.fornecedorclientedadosacessodiavencimento), '%Y-%m-%d'), INTERVAL 1 month) ELSE Date_format(Concat(Year(CURRENT_DATE()), '-', Month(CURRENT_DATE()), '-', fornecedorclientedadosacesso.fornecedorclientedadosacessodiavencimento), '%Y-%m-%d') end)  ");
            sb.Append("from fornecedorclientedadosacesso where fornecedorclientedadosacesso.FkFornecedorClienteId = fc.FornecedorClienteId limit 1) InicioColeta, ");
            sb.Append("IF(ac.DtVencimento is null, 0, 1) qtdcoletado ");
            sb.Append("FROM fornecedorcliente fc ");
            sb.Append($"left join arquivocoletado ac on(ac.FkFornecedorClienteId = fc.FornecedorClienteId and YEAR(ac.DtVencimento) = {ano} and month(ac.DtVencimento) = {mes}) ");
            sb.Append("INNER JOIN cliente c ON(c.ClienteId = fc.FkClienteId) ");
            sb.Append("INNER JOIN fornecedor f ON(f.FornecedorId = fc.FkFornecedorId) ");
            sb.Append("INNER JOIN tipofornecedor tf ON(f.FkTipoFornecedorId = tf.TipoFornecedorId) ");
            sb.Append($"where fc.Ativo = 1 ");

            if (LoggedclienteId != null)
                sb.AppendFormat(" AND (c.ClienteId = {0} OR c.ClientePaiId = {0})", LoggedclienteId.Value);

            if (emissor != null)
                sb.AppendFormat(" AND (fc.FkFornecedorId = {0})", emissor.Value);

            if (cliente != null)
                sb.AppendFormat(" AND (fc.FkClienteId = {0})", cliente.Value);

            sb.Append(") T ");
            sb.Append("group by FornecedorIsRobo, FornecedorView, FornecedorClienteId, FornecedorNome, TipoFornecedorNome, ClienteNome, DocumentoQuantidade, InicioColeta ");

            if (situacao == "2")
                sb.Append("having Sum(qtdcoletado)> 0 ");
            else if (situacao == "3")
                sb.Append("having Sum(qtdcoletado)= 0 ");


            var result = Connection.Query<dynamic>(sb.ToString(), filter, transaction: transaction).ToList();

            return result;
        }
        public List<dynamic> ListResumoByMes(int? LoggedclienteId, string ano, string mes, int? cliente, int? emissor, string situacao, DataTableFilter filter)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();


            sb.Append("SELECT DocumentoQuantidade, qtdcoletado, (DocumentoQuantidade - qtdcoletado) dtqFaltante FROM( ");
            sb.Append("SELECT Sum(fc.DocumentoQuantidade) DocumentoQuantidade, ");
            sb.Append("Sum(IF(ac.DtVencimento is null, 0,1)) qtdcoletado ");
            sb.Append("FROM fornecedorcliente fc ");
            sb.Append($"left join arquivocoletado ac on(ac.FkFornecedorClienteId = fc.FornecedorClienteId and YEAR(ac.DtVencimento) = {ano} and month(ac.DtVencimento) = {mes}) ");
            sb.Append("INNER JOIN cliente c ON(c.ClienteId = fc.FkClienteId) ");
            sb.Append("INNER JOIN fornecedor f ON(f.FornecedorId = fc.FkFornecedorId) ");
            sb.Append("INNER JOIN tipofornecedor tf ON(f.FkTipoFornecedorId = tf.TipoFornecedorId) ");
            sb.Append($"where fc.Ativo = 1 ");

            if (LoggedclienteId != null)
                sb.AppendFormat(" AND (c.ClienteId = {0} OR c.ClientePaiId = {0})", LoggedclienteId.Value);

            if (emissor != null)
                sb.AppendFormat(" AND (fc.FkFornecedorId = {0})", emissor.Value);

            if (cliente != null)
                sb.AppendFormat(" AND (fc.FkClienteId = {0})", cliente.Value);

            sb.Append(") T ");

            var result = Connection.Query<dynamic>(sb.ToString(), filter, transaction: transaction).ToList();

            return result;
        }
        public List<dynamic> List(int? loggedclienteId, string centrosCusto)
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT ac.ArquivoColetadoId, ac.Situacao, ac.Status,ac.NrMesReferencia,ac.CaminhoArquivo, ac.DtVencimento, CONCAT('R$ ', REPLACE(REPLACE(REPLACE(FORMAT(ac.Total, 2),'.',';'),',','.'),';',',')) AS Total , ac.Total AS T2, DATE_FORMAT(ac.DtVencimento,'%d/%m/%Y') Vencimento, ");
            sb.Append("    CONCAT((CASE WHEN MONTH(ac.NrMesReferencia)=1 THEN 'Jan' WHEN MONTH(ac.NrMesReferencia)=2 THEN 'Fev' WHEN MONTH(ac.NrMesReferencia)=3 THEN 'Mar' WHEN MONTH(ac.NrMesReferencia)=4 THEN 'Abr' WHEN MONTH(ac.NrMesReferencia)=5 THEN 'Mai' WHEN MONTH(ac.NrMesReferencia)=6 THEN 'Jun' WHEN MONTH(ac.NrMesReferencia)=7 THEN 'Jul' WHEN MONTH(ac.NrMesReferencia)=8 THEN 'Ago' WHEN MONTH(ac.NrMesReferencia)=9 THEN 'Set' WHEN MONTH(ac.NrMesReferencia)=10 THEN 'Out' WHEN MONTH(ac.NrMesReferencia)=11 THEN 'Nov'  ELSE 'Dez' END),'/', YEAR(ac.NrMesReferencia)) Competencia, ");
            sb.Append("    (CASE WHEN ac.Situacao=0 THEN '#21493E' WHEN ac.Situacao=1 THEN '#EB323A' WHEN ac.Situacao=2 THEN '#3C8DE6' WHEN ac.Situacao=3 THEN '#FF7900'  WHEN ac.Situacao=4 THEN '#42478B' WHEN ac.Situacao=5 THEN '#2A9E79' WHEN ac.Situacao=7 THEN '#5A042B' ELSE '#21493E' END) CorSituacaoLinha, ");
            sb.Append("    (CASE WHEN ac.Status=1 THEN '1' WHEN ac.Status=2 THEN '2' ELSE '3' END) descricaoStatus, ");

            sb.Append("    (CASE WHEN ac.Situacao=0 THEN 'D' WHEN ac.Situacao=1 THEN 'A' WHEN ac.Situacao=2 THEN 'P' WHEN ac.Situacao=3 THEN 'C'  WHEN ac.Situacao=4 THEN 'A' WHEN ac.Situacao=5 THEN 'I' WHEN ac.Situacao=7 THEN 'S' ELSE 'U' END) descricaoSituacao, ");
            sb.Append("    (CASE WHEN ac.Situacao=0 THEN 'avatar-sm-box bg-inverse' WHEN ac.Situacao=1 THEN 'avatar-sm-box bg-danger' WHEN ac.Situacao=2 THEN 'avatar-sm-box bg-success' WHEN ac.Situacao=3 THEN 'avatar-sm-box bg-brown' WHEN ac.Situacao=4 THEN 'avatar-sm-box bg-warning'  WHEN ac.Situacao=5 THEN 'avatar-sm-box bg-primary' WHEN ac.Situacao=7 THEN 'avatar-sm-box bg-inverse' ELSE 'avatar-sm-box bg-info' END) CorSituacao, ");
            sb.Append("    (CASE WHEN ac.Situacao=0 THEN 'Desconhecido' WHEN ac.Situacao=1 THEN 'A Pagar' WHEN ac.Situacao=2 THEN 'Pago' WHEN ac.Situacao=3 THEN 'Contestado'  WHEN ac.Situacao=4 THEN 'Atrasado' WHEN ac.Situacao=5 THEN 'Isento' WHEN ac.Situacao=7 THEN 'Processado' ELSE 'Debito Automatico' END) titleSituacao, ");


            sb.Append("    c.ClienteId,  ");
            sb.Append("    c.ClienteNome, ");
            sb.Append("    (CASE WHEN c.TipoPessoa=1 THEN MASK(c.ClienteCnpj, '###.###.###-##') ELSE MASK(c.ClienteCnpj, '##.###.###/####-##') END) ClienteCnpj, ");
            sb.Append("    c.ClienteCidade, ");
            sb.Append("    c.ClienteUF, ");
            sb.Append("    f.FornecedorId, ");
            sb.Append("    f.FornecedorNome, ");
            sb.Append("    tf.TipoFornecedorId,  ");
            sb.Append("    ( CASE WHEN stf.SubTipoFornecedorNome IS NULL THEN tf.tipofornecedornome ELSE CONCAT( tf.tipofornecedornome,' - ',stf.SubTipoFornecedorNome) END) tipofornecedornome,  ");
            sb.Append("    cc.CentroCustoId,  ");
            sb.Append("    (CASE WHEN cc.CentroCustoCodigo IS NULL THEN  cc.CentroCustoNome WHEN cc.CentroCustoCodigo='' THEN cc.CentroCustoNome ELSE CONCAT(cc.CentroCustoCodigo, ' - ',cc.CentroCustoNome ) END) CentroCustoNome ");
            sb.Append(" FROM arquivocoletado ac ");
            sb.Append(" INNER JOIN fornecedorcliente fc ON(fc.FornecedorClienteId = ac.FkFornecedorClienteId) ");
            sb.Append(" INNER JOIN cliente c ON(c.ClienteId = fc.FkClienteId) ");
            sb.Append(" INNER JOIN fornecedor f ON(f.FornecedorId = fc.FkFornecedorId) ");
            sb.Append(" INNER JOIN tipofornecedor tf ON (f.FkTipoFornecedorId = tf.TipoFornecedorId) ");
            sb.Append(" LEFT JOIN SubTipoFornecedor stf ON (stf.SubTipoFornecedorId = ac.FkSubTipoFornecedorId  ) ");
            sb.Append(" LEFT JOIN centrocusto cc ON (c.FkCentroCustoId = cc.CentroCustoId) ");
            sb.Append(" WHERE 1 = 1 ");

            if (loggedclienteId != null)
                sb.AppendFormat(" AND (fc.FkClienteId = {0} OR c.ClientePaiId = {0})", loggedclienteId.Value);

            if (!string.IsNullOrEmpty(centrosCusto))
                sb.AppendFormat(" AND c.FkCentroCustoId IN ({0}) ", centrosCusto);

            sb.AppendFormat(" ORDER BY ac.DtVencimento DESC ");

            var result = Connection.Query<dynamic>(sb.ToString(), null, transaction: transaction).ToList();

            return result;
        }
        public List<dynamic> GetCSVFilesDownload(int? LoggedclienteId, string centrosCusto, int? clienteId, string vencimentode, string vencimentoate, string tipo, string palavra, string situacao)
        {


            StringBuilder sb = new StringBuilder();
            var lst = new List<dynamic>();
            string _fileName = string.Empty;

            sb.Clear();
            sb.Append("SELECT ac.CaminhoArquivo, ac.NomeArquivoManual, f.FornecedorNome, c.ClienteNome, c.ClienteCidade, ac.DtVencimento, ac.Total ");
            sb.Append(" FROM arquivocoletado ac ");
            sb.Append(" INNER JOIN fornecedorcliente fc ON(fc.FornecedorClienteId = ac.FkFornecedorClienteId) ");
            sb.Append(" INNER JOIN cliente c ON(c.ClienteId = fc.FkClienteId) ");
            sb.Append(" INNER JOIN fornecedor f ON(f.FornecedorId = fc.FkFornecedorId) ");
            sb.Append(" INNER JOIN tipofornecedor tf ON (f.FkTipoFornecedorId = tf.TipoFornecedorId) ");
            sb.Append(" LEFT JOIN SubTipoFornecedor stf ON (stf.SubTipoFornecedorId = ac.FkSubTipoFornecedorId  ) ");
            sb.Append(" LEFT JOIN centrocusto cc ON (c.FkCentroCustoId = cc.CentroCustoId) ");
            sb.Append(" WHERE 1 = 1 ");

            if (LoggedclienteId != null)
                sb.AppendFormat(" AND (fc.FkClienteId = {0} OR c.ClientePaiId = {0})", LoggedclienteId.Value);

            if (clienteId != null)
                sb.AppendFormat(" AND fc.FkClienteId = {0}", clienteId.Value);

            if (!string.IsNullOrEmpty(centrosCusto))
                sb.AppendFormat(" AND c.FkCentroCustoId IN ({0}) ", centrosCusto);

            if ((!string.IsNullOrEmpty(vencimentode) && vencimentode != "undefined") || ((!string.IsNullOrEmpty(vencimentoate) && vencimentoate != "undefined")))
            {
                var _vcto_de = vencimentode.Split('-');
                var _vcto_ate = vencimentoate.Split('-');

                if (_vcto_de.Length == 3)
                    sb.AppendFormat(" AND ac.DtVencimento>= '{0}' ", vencimentode);

                if (_vcto_ate.Length == 3)
                    sb.AppendFormat(" AND ac.DtVencimento<= '{0}' ", vencimentoate);


            }

            if (!string.IsNullOrEmpty(situacao) && situacao != "undefined")
                sb.AppendFormat(" AND ac.Situacao = {0}", situacao);


            if (!string.IsNullOrEmpty(tipo) && tipo != "undefined")
                sb.AppendFormat(" AND tf.TipoFornecedorId = {0}", tipo);

            if (!string.IsNullOrEmpty(palavra) && palavra != "undefined")
                sb.AppendFormat(" AND (UPPER(f.FornecedorNome) like '%{0}%' or UPPER(cc.CentroCustoNome) like '%{0}%' or UPPER(ac.NumeroPedido) like '{0}%') ", palavra.ToUpper());

            var result = Connection.Query<dynamic>(sb.ToString(), null, transaction: transaction).ToList();

            if (result == null)
                return lst;

            lst.Clear();

            try
            {

                for (int i = 0; i < result.Count; i++)
                {
                    _fileName = $"{result[i].FornecedorNome} -{result[i].ClienteNome} - {result[i].ClienteCidade} - {result[i].DtVencimento.ToString("dd-MM-yyyy")} - R$ {result[i].Total.ToString("N2")}";
                    _fileName = _fileName.Replace("'", "");
                    lst.Add(new
                    {
                        Caminho = result[i].CaminhoArquivo,
                        Nome = _fileName,
                        NomeArquivoManual = result[i].NomeArquivoManual
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lst;
        }
        public List<Documento> List(int? clienteId, DateTime startDate)
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT ac.ArquivoColetadoId, ac.CodigoBarras, ac.DtInclusao, ac.Status,ac.NrMesReferencia,ac.CaminhoArquivo, ac.DtVencimento, ac.Total, ");
            sb.Append("    (CASE WHEN ac.Status=1 THEN '1' WHEN ac.Status=2 THEN '2' ELSE '3' END) descricaoStatus, ");
            sb.Append("    c.ClienteId,  ");
            sb.Append("    c.ClienteNome, ");
            sb.Append("    f.FornecedorId, ");
            sb.Append("    f.FornecedorNome, ");
            sb.Append("    tf.TipoFornecedorId,  ");
            sb.Append("    tf.TipoFornecedorNome  ");
            sb.Append(" FROM arquivocoletado ac ");
            sb.Append(" INNER JOIN fornecedorcliente fc ON(fc.FornecedorClienteId = ac.FkFornecedorClienteId) ");
            sb.Append(" INNER JOIN cliente c ON(c.ClienteId = fc.FkClienteId) ");
            sb.Append(" INNER JOIN fornecedor f ON(f.FornecedorId = fc.FkFornecedorId) ");
            sb.Append(" INNER JOIN tipofornecedor tf ON (f.FkTipoFornecedorId = tf.TipoFornecedorId) ");
            sb.Append(" WHERE 1 = 1 ");

            if (clienteId != null)
                sb.AppendFormat(" AND (fc.FkClienteId = {0} OR c.ClientePaiId = {0})", clienteId.Value);

            sb.AppendFormat(" AND (ac.DtInclusao >= \"{0}\" ) ", startDate.ToString());
            sb.AppendFormat(" ORDER BY ac.DtInclusao DESC; ");

            //var data = Connection.Query<Orcamento, Cliente, Fornecedor, TipoFornecedor, Orcamento>(sb.ToString(), (arquivoColetado, cliente, fornecedor, tipofornecedor) =>
            //{
            //    arquivoColetado.FkFornecedorClienteId = new FornecedorCliente();
            //    arquivoColetado.FkFornecedorClienteId.FkClienteId = cliente;
            //    tipofornecedor.TipoFornecedorNome = GetSubTipo(tipofornecedor.TipoFornecedorNome, arquivoColetado.SubTipo);
            //    fornecedor.FkTipoFornecedorId = tipofornecedor;
            //    arquivoColetado.FkFornecedorClienteId.FkFornecedorId = fornecedor;
            //    return arquivoColetado;
            //}, splitOn: "ArquivoColetadoId,ClienteId, FornecedorId, TipoFornecedorId", transaction: transaction).ToList();

            //return data;
            return null;
        }
        private string GetSubTipo(string tipoFornecedorNome, string subTipo)
        {
            string _dsSubTipo = string.Empty;

            if (string.IsNullOrEmpty(subTipo))
                return tipoFornecedorNome;

            if (subTipo == "1")
                _dsSubTipo = "IPVA";
            else if (subTipo == "2")
                _dsSubTipo = "Licenciamento";
            else if (subTipo == "3")
                _dsSubTipo = "DPVAT";
            else
                _dsSubTipo = "Multa";

            return string.Format("{0}-{1}", tipoFornecedorNome, _dsSubTipo);

        }
        public List<Documento> ValidarItemIncluidoManual(int clienteId, int fornecedorId, string dtVencimento, decimal total)
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT sc.ArquivoColetadoId FROM arquivocoletado sc ");
            sb.Append(" INNER JOIN fornecedorcliente fc ON(sc.FkFornecedorClienteId = fc.FornecedorClienteId) ");
            sb.AppendFormat("WHERE fc.FkFornecedorId = {0} ", fornecedorId);
            sb.AppendFormat("AND fc.FkClienteId = {0} ", clienteId);
            sb.AppendFormat("AND sc.DtVencimento = {0} ", dtVencimento);
            if (total != 0)
                sb.AppendFormat("AND sc.total = {0} ", total.ToString().Replace(",", "."));

            var result = Connection.Query<Documento>(sb.ToString(), null, transaction: transaction).ToList();

            return result;
        }

        public Documento Add(Documento documento)
        {

            try
            {
                documento.CaminhoAnexo = Control.Facilites.Events.Utils.Upload.File(documento.CaminhoAnexo, "", documento, false, null);

                var sb = new StringBuilder();
                sb.Append("INSERT INTO documento (EmpresaFornecedor, Ativo,DataInclusao, SituacaoDocumento,CaminhoAnexo) VALUES");
                sb.Append(" (@EmpresaFornecedor, @Ativo,@DataInclusao, @SituacaoDocumento,@CaminhoAnexo);");
                sb.Append(" SELECT LAST_INSERT_ID();");
                documento.Id = Connection.QueryFirst<int>(sb.ToString(),
                    new
                    {
                        EmpresaFornecedor = documento.EmpresaFornecedor.Id,
                        Ativo = documento.Ativo,
                        DataInclusao = documento.DataInclusao,
                        SituacaoDocumento = documento.SituacaoDocumento,
                        CaminhoAnexo = documento.CaminhoAnexo,

                    }, transaction: transaction);
                return documento;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"ERRO - {ex.StackTrace}");
                throw new Exception(ex.Message);
            }
        }

        
        public List<Documento> ListByNotaDebito(int? notadebito)
        {
            //StringBuilder sb = new StringBuilder();

            //sb.Clear();
            //sb.Append("SELECT o.Id, nd.Id FROM orcamento o inner join nota_debito_tem nd on o.Id  =  nd.NotaFiscal");
            //if (notadebito!=null && notadebito != 0)
            //    sb.AppendFormat(" WHERE nd.NotaDebito = {0} ", notadebito.Value);

            //var data = Connection.Query<Documento, NotaDebitoItem, Documento>(sb.ToString(), (notafiscal,notafiscalitem) =>
            //{
            //    return notafiscal;
            //}, splitOn: "Id, Id", transaction: transaction).ToList();

            //return data;
            return null;
        }

        public List<Documento> GetAllByFornecedor(int? loggedFornecedorId, int? loggedEmpresaId)
        {
            throw new NotImplementedException();
        }
    }

    class TipoMes
    {
        public string Tipo { get; set; }
        public string Mes { get; set; }
        public decimal Valor { get; set; }

    }
}
