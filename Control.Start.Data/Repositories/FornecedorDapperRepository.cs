using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Filters;
using Control.Facilites.Domain.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Control.Facilites.Data.Repositories
{

    internal class FornecedorDapperRepository : RepositoryBase, IFornecedorDapperRepository
    {
        StringBuilder sb;
        public FornecedorDapperRepository(IConfigurationRoot configuration) : base(configuration)
        {
        }

        public Fornecedor Add(Fornecedor fornecedor)
        {
            sb = new StringBuilder();
            sb.Append("INSERT INTO fornecedor (Codigo,Nome,Cnpj, PontoVenda) VALUES");
            sb.Append(" (@Codigo,@Nome,@PontoVenda, @Cnpj);");
            sb.Append(" SELECT LAST_INSERT_ID();");
            fornecedor.Id = Connection.QueryFirst<int>(sb.ToString(),
                new
                {
                    Codigo = fornecedor.Codigo,
                    Nome = fornecedor.Nome,
                    Cnpj = fornecedor.Cnpj,
                    PontoVenda = fornecedor.PontoVenda,
                }, transaction: transaction);
            return fornecedor;




        }

        public List<Fornecedor> List(string filter)
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT Codigo,Nome,Cnpj,PontoVenda");
            sb.Append(" FROM fornecedor ");
            sb.Append(" WHERE 1 = 1");

            if (!string.IsNullOrEmpty(filter))
                sb.AppendFormat(" AND (Nome LIKE '%{0}%')", filter);

            sb.Append(" ORDER BY Nome");

            var result = Connection.Query<Fornecedor>(sb.ToString(), null, transaction: transaction).ToList();

            return result;
        }

        public bool Remove(int id)
        {

            var affectedRows = Connection.Execute("DELETE FROM fornecedor WHERE Id = @Id", new { Id = id }, transaction: transaction);

            return affectedRows > 0;
        }

        public Fornecedor GetById(int id)
        {

            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT Id, Codigo,Nome,Cnpj, PontoVenda ");
            sb.Append(" FROM fornecedor ");
            sb.AppendFormat(" WHERE Id={0} ", id);
            return Connection.Query<Fornecedor>(sb.ToString(), null, transaction: transaction).FirstOrDefault();


        }

        public List<Fornecedor> GetAll(FornecedorFilter filter)
        {
            sb = new StringBuilder();
            sb.Append("SELECT Codigo,Nome,Cnpj,PontoVenda");
            sb.Append(" FROM fornecedor WHERE Id = IFNULL(@Id, Id) AND Nome = IFNULL(@Nome, Nome) ORDER BY Nome");

            var result = Connection.Query<Fornecedor>(sb.ToString(), filter, transaction: transaction).ToList();

            return result;

        }

        public bool Update(Fornecedor fornecedor)
        {

            try
            {

                //Codigo,Nome,Cnpj,PontoVenda, Empresa
                sb = new StringBuilder();
                sb.Append("UPDATE fornecedor SET Codigo = @Codigo, Cnpj=@Cnpj,");
                sb.Append(" Nome= @Nome, PontoVenda=@PontoVenda");
                sb.Append(" WHERE Id = @Id");
                var affectedRows = Connection.Execute(sb.ToString(),
                   new
                   {
                       Id = fornecedor.Id,
                       Codigo = fornecedor.Codigo,
                       Nome = fornecedor.Nome,
                       Cnpj = fornecedor.Cnpj,
                       PontoVenda = fornecedor.PontoVenda,

                   }, transaction: transaction);
                return true;
            }
            catch
            {
                return false;
            }

        }

        private string GetNomeColuna(int posicao)
        {
            if (posicao == 0)
                return "Id";
            else if (posicao == 2)
                return "Codigo";
            else
                return "Nome";

        }

        public List<Fornecedor> List(DataTableFilter filter)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append("SELECT Id, Codigo,Nome,Cnpj, PontoVenda ");
            sb.Append(" FROM fornecedor");

            if(!string.IsNullOrEmpty(filter.search.value))
                sb.AppendFormat(" WHERE Nome LIKE '%{0}%' OR Codigo LIKE '%{0}%'", filter.search.value);

            if (filter.order.Count > 0)
                sb.AppendFormat(" ORDER BY {0} {1} ", GetNomeColuna(filter.order[0].column), filter.order[0].dir);

            return Connection.Query<Fornecedor>(sb.ToString(), null, transaction: transaction).ToList();

        }

        public List<Fornecedor> getAllByAtivo()
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT distinct Id, Codigo,Nome, Cnpj,PontoVenda ");
            sb.Append(" FROM fornecedor");

            sb.Append(" ORDER BY Nome ");

            var result = Connection.Query<Fornecedor>(sb.ToString(), null, transaction: transaction).ToList();

            return result;

        }

        public List<Fornecedor> ListByEmpresa(DataTableFilter filter, int? loggedEmpresaId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append("SELECT Id,  Codigo,Nome,PontoVenda, Cnpj ");
            sb.Append(" FROM fornecedor Where 1 = 1");

            if (!string.IsNullOrEmpty(filter.search.value))
                sb.AppendFormat(" AND Nome LIKE '%{0}%' OR Codigo LIKE '%{0}%'", filter.search.value);

            if (loggedEmpresaId != null && loggedEmpresaId.Value > 0)
                sb.AppendFormat(" AND Empresa = {0}", loggedEmpresaId.Value);

            if (filter.order.Count > 0)
                sb.AppendFormat(" ORDER BY {0} {1} ", GetNomeColuna(filter.order[0].column), filter.order[0].dir);

            return Connection.Query<Fornecedor>(sb.ToString(), null, transaction: transaction).ToList();


        }

        public List<Fornecedor> ListSelecionadas(int empresaId)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT f.Id, f.Nome FROM fornecedor f WHERE 1 = 1 ");
            sb.Append(" AND EXISTS(SELECT * FROM empresa_fornecedor ec WHERE f.Id = ec.FornecedorId");
            sb.AppendFormat(" AND ec.EmpresaId = {0}", empresaId);
            sb.Append(")");

            return Connection.Query<Fornecedor>(sb.ToString(), new FornecedorFilter(), transaction: transaction).ToList();

        }

        public List<Fornecedor> ListNaoSelecionadas(int empresaId)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT f.Id, f.Nome FROM fornecedor f WHERE 1 = 1 ");
            sb.Append(" AND NOT EXISTS(SELECT * FROM empresa_fornecedor ec WHERE f.Id = ec.FornecedorId");
            sb.AppendFormat(" AND ec.EmpresaId = {0}", empresaId);
            sb.Append(")");


            return Connection.Query<Fornecedor>(sb.ToString(), new FornecedorFilter(), transaction: transaction).ToList();

        }



    }

}
