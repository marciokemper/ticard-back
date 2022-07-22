using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Filters;
using Control.Facilites.Domain.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Control.Facilites.Data.Utils;

namespace Control.Facilites.Data.Repositories
{

    internal class EmpresaFornecedorDapperRepository : RepositoryBase, IEmpresaFornecedorDapperRepository
    {
        StringBuilder sb;
        public EmpresaFornecedorDapperRepository(IConfigurationRoot configuration) : base(configuration)
        {
        }

        public EmpresaFornecedor Add(EmpresaFornecedor empresaFornecedor)
        {
            
            empresaFornecedor.Id = Connection.QueryFirst<int>("INSERT INTO empresa_fornecedor (EmpresaId, FornecedorId) VALUES (@EmpresaId, @FornecedorId); SELECT LAST_INSERT_ID();",
                new
                {
                    EmpresaId = empresaFornecedor.Fornecedor.Id,
                    FornecedorId = empresaFornecedor.Empresa.Id,
                }, transaction: transaction);

            return empresaFornecedor;
        }

        private string CreateUserName()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }

        private string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        private Usuario CriarUsuario(EmpresaFornecedor empresaFornecedor)
        {
            var _userName = CreateUserName();
            var _password = CreatePassword(6);

            sb = new StringBuilder();
            sb.Clear();
            sb.Append("SELECT Id, Codigo,Nome,Cnpj, PontoVenda ");
            sb.Append(" FROM fornecedor ");
            sb.AppendFormat(" WHERE Id={0} ", empresaFornecedor.Fornecedor.Id);
            var forn = Connection.Query<Fornecedor>(sb.ToString(), null, transaction: transaction).FirstOrDefault();

            Usuario usuario = new Usuario();
            usuario.Nome = $"AUT_{forn.Nome}";
            usuario.Email = "email@teste.com.br";
            usuario.Login = _userName;
            usuario.Senha = _password;
            usuario.EmpresaFornecedor = new EmpresaFornecedor();
            usuario.EmpresaFornecedor.Id = empresaFornecedor.Id;
            //usuario.EmpresaFornecedor.n = empresaFornecedor.Id;

            sb = new StringBuilder();
            sb.Append("INSERT INTO usuario (Nome,Email,Login, Senha, EmpresaFornecedor, Ativo) VALUES");
            sb.Append(" (@Nome,@Email,@Login,@Senha,@EmpresaFornecedor,@Ativo);");
            sb.Append(" SELECT LAST_INSERT_ID();");
            usuario.Id = Connection.QueryFirst<int>(sb.ToString(),
                new
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Login = usuario.Login,
                    Senha = General.Encrypt(usuario.Senha),
                    EmpresaFornecedor = usuario.EmpresaFornecedor.Id == 0 ? null : usuario.EmpresaFornecedor.Id.ToString(),
                    Ativo = 1,
                }, transaction: transaction);
            return usuario;
        }

        public EmpresaFornecedor Add(int empresa, int fornecedor)
        {
            

            EmpresaFornecedor empresaFornecedor = new EmpresaFornecedor();
            empresaFornecedor.Empresa = new Empresa();
            empresaFornecedor.Empresa.Id = empresa;
            empresaFornecedor.Fornecedor = new Fornecedor();
            empresaFornecedor.Fornecedor.Id = fornecedor;
            
            empresaFornecedor.Id = Connection.QueryFirst<int>("INSERT INTO empresa_fornecedor (EmpresaId, FornecedorId) VALUES (@EmpresaId, @FornecedorId); SELECT LAST_INSERT_ID();",
                new
                {
                    EmpresaId = empresaFornecedor.Empresa.Id,
                    FornecedorId = empresaFornecedor.Fornecedor.Id,
                }, transaction: transaction);

            CriarUsuario(empresaFornecedor);

            return empresaFornecedor;
        }


        public EmpresaFornecedor GetById(int id)
        {
            sb = new StringBuilder();

            sb.Append("SELECT fc.FornecedorClienteId, fc.Parsear, fc.Coletar, fc.DataInclusao, fc.Ativo, fc.ValorAproximado, ");
            sb.Append("fc.Operacional, fc.OperacionalDescricao, fc.fornecedorclientecol, fc.DebitoAutomatico, fc.DocumentoQuantidade, ");
            sb.Append("fc.FornecedorCodigoERP, c.ClienteId, f.FornecedorId, cc.CentroCustoId FROM fornecedorcliente fc ");
            sb.Append("INNER JOIN cliente c ON c.ClienteId = fc.FkClienteId ");
            sb.Append("INNER JOIN fornecedor f ON f.FornecedorId = fc.FkFornecedorId ");
            sb.Append("LEFT JOIN centrocusto cc ON cc.CentroCustoId = fc.FkCentroCustoID ");
            sb.Append("WHERE fc.FornecedorClienteId = " + id + ";");

            var result = Connection.Query<EmpresaFornecedor, Empresa, Fornecedor, EmpresaFornecedor>(sb.ToString(), (empresaFornecedor, empresa, fornecedor) =>
            {
                empresaFornecedor.Empresa = empresa;
                empresaFornecedor.Fornecedor = fornecedor;
                return empresaFornecedor;
            }, splitOn: "Id,Id,Id");

            return result.FirstOrDefault();
        }

        public bool Update(EmpresaFornecedor empresaFornecedor)
        {

            sb = new StringBuilder();

            try
            {
                var affectedRows = Connection.Execute("UPDATE fornecedorcliente SET FkFornecedorId = @FornecedorId, FkClienteId = @ClienteId, Login = @Login, Ativo = @Ativo, Senha = @Senha, NumeroDiaVencimento=@NumeroDiaVencimento, NumeroDiasAntesVencimento=@NumeroDiasAntesVencimento, NumeroUnidadeConsumidora =@NumeroUnidadeConsumidora, Parsear = @Parsear, Coletar = @Coletar WHERE FornecedorClienteId = @Id",
                    new
                    {
                        Id = empresaFornecedor.Id,
                        EmpresaId = empresaFornecedor.Fornecedor.Id,
                        FornecedorId = empresaFornecedor.Empresa.Id,

                    }
                   , transaction: transaction);

                return true;
            }
            catch
            {
                return false;
            }


        }

        private string GetNomeColuna(int posicao)
        {
            if (posicao == 1)
                return "C1.NM_CLIENTE";
            else if (posicao == 2)
                return "C1.DS_CNPJ";
            else
                return "C1.PK_ID";

        }

        public List<EmpresaFornecedor> List(DataTableFilter filter)
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT ");
            sb.Append("    fc.PK_ID,  ");
            sb.Append("    fc.FK_FORNECEDOR_ID CD_FORNECEDOR,   ");
            sb.Append("    f.NM_FORNECEDOR,   ");
            sb.Append("    (CASE WHEN f.TP_FORNECEDOR=1 THEN 'Telefonia'  ");
            sb.Append("          WHEN f.TP_FORNECEDOR=2 THEN 'Gás'  ");
            sb.Append("          WHEN f.TP_FORNECEDOR=3 THEN 'Energia' ELSE 'Agua' END) DS_TP_FORNECEDOR,  ");
            sb.Append("    fc.FK_CLIENTE_ID FK_CLIENTE_ID,   ");
            sb.Append("    c.NM_CLIENTE,   ");
            sb.Append("    fc.DS_LOGIN,   ");
            sb.Append("    fc.DS_SENHA,   ");
            sb.Append("    fc.NR_DIA_VENCIMENTO,   ");
            sb.Append("    fc.NR_DIAS_ANTES_VENCIMENTO, fc.NR_UNIDADE_CONSUMIDORA   ");
            sb.Append(" FROM fornecedor_cliente fc ");
            sb.Append(" INNER JOIN cliente C ON(fc.FK_CLIENTE_ID = C.PK_ID) ");
            sb.Append(" INNER JOIN fornecedor F ON(fc.FK_FORNECEDOR_ID = F.PK_ID) ");

            if (!string.IsNullOrEmpty(filter.search.value))
                sb.AppendFormat(" AND f.NM_FORNECEDOR LIKE '{0}%'", filter.search.value);

            return Connection.Query<EmpresaFornecedor>(sb.ToString(), new EmpresaFornecedorFilter(), transaction: transaction).ToList();

        }


        public List<EmpresaFornecedor> List(int id, DataTableFilter filter)
        {
            sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT ");
            sb.Append("    fc.Id,  ");
            sb.Append("    c.Id,   ");
            sb.Append("    c.Nome, c.Cnpj,   ");
            sb.Append("    f.Id,   ");
            sb.Append("    f.Nome   ");
            sb.Append(" FROM empresa_fornecedor fc ");
            sb.Append(" INNER JOIN empresa c ON(fc.EmpresaId = c.Id) ");
            sb.Append(" INNER JOIN fornecedor f ON(fc.FornecedorId = f.Id) ");
            sb.AppendFormat(" WHERE fc.EmpresaId = {0} ", id);

            if (!string.IsNullOrEmpty(filter.search.value))
                sb.AppendFormat(" AND f.Nome LIKE '{0}%'", filter.search.value);

            sb.AppendFormat(" ORDER BY f.Nome");

            var data = Connection.Query<EmpresaFornecedor, Empresa, Fornecedor, EmpresaFornecedor>(sb.ToString(), (empresaFornecedor, empresa, fornecedor) =>
            {
                empresaFornecedor.Empresa = empresa;
                empresaFornecedor.Fornecedor = fornecedor;
                return empresaFornecedor;
            }, splitOn: "Id,Id,Id").ToList();

            return data;
        }

        public List<EmpresaFornecedor> GetEmpresaFornecedorByEmpresa(int empresaId)
        {
            throw new NotImplementedException();
        }

        public List<EmpresaFornecedor> GetAll(EmpresaFornecedorFilter filter)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int empresa, int fornecedor)
        {
            sb = new StringBuilder();
            sb.Clear();
            sb.Append("DELETE usuario ");
            sb.Append("FROM usuario ");
            sb.Append("INNER JOIN empresa_fornecedor ON usuario.EmpresaFornecedor = empresa_fornecedor.Id ");
            sb.AppendFormat("WHERE empresa_fornecedor.EmpresaId = {0} AND empresa_fornecedor.FornecedorId = {1} ", empresa, fornecedor);

            var affectedRows = Connection.Execute(sb.ToString(), null, transaction: transaction);

            affectedRows = Connection.Execute("DELETE FROM empresa_fornecedor WHERE EmpresaId = @EmpresaId AND FornecedorId = @FornecedorId", new { EmpresaId = empresa, FornecedorId = fornecedor }, transaction: transaction);

            return affectedRows > 0;
        }
    }
}