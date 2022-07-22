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

    internal class EmpresaDapperRepository : RepositoryBase, IEmpresaDapperRepository
    {
        StringBuilder sb;
        public EmpresaDapperRepository(IConfigurationRoot configuration) : base(configuration)
        {
        }

        public Empresa Add(Empresa empresa)
        {
            sb = new StringBuilder();
            sb.Append("INSERT INTO empresa (Nome, EmpresaPaiId, Cnpj, PontoVenda) VALUES");
            sb.Append(" (@Nome, @EmpresaPaiId, @Cnpj, @PontoVenda);");
            sb.Append(" SELECT LAST_INSERT_ID();");
            empresa.Id = Connection.QueryFirst<int>(sb.ToString(),
                new
                {
                    Nome = empresa.Nome,
                    Cnpj = empresa.Cnpj,
                    EmpresaPaiId = (string.IsNullOrEmpty(empresa.EmpresaPaiId) ? null : empresa.EmpresaPaiId),
                    PontoVenda = empresa.PontoVenda,
                }, transaction: transaction);

            return empresa;




        }

        private void AddConfiguracao(int empresa)
        {

            try
            {
                sb = new StringBuilder();
                sb.Append("INSERT INTO configuracao ( Id, EmailPagamento, Empresa) VALUES");
                sb.Append(" (@Id, @EmailPagamento, @Empresa);");
                sb.Append(" SELECT LAST_INSERT_ID();");
                Connection.QueryFirst<int>(sb.ToString(),
                    new
                    {
                        Id = empresa,
                        PasswordDefault = "",
                        EmailPagamento = "empresa@empresa.com.br",
                        Empresa = empresa
                    }, transaction: transaction);

            }catch(Exception ex)
            {

            }

        }

        public List<Empresa> List(string filter)
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT Id, Nome, EmpresaPaiId, Cnpj, PontoVenda");
            sb.Append(" FROM empresa Where EmpresaPaiId is null");

            if (!string.IsNullOrEmpty(filter))
                sb.AppendFormat(" Where (Nome LIKE '%{0}%')", filter);

            sb.Append(" ORDER BY Nome");

            var result = Connection.Query<Empresa>(sb.ToString(), null, transaction: transaction).ToList();

            return result;
        }

        public bool Remove(int id)
        {

            var affectedRows = Connection.Execute("DELETE FROM empresa WHERE Id = @Id", new { Id = id }, transaction: transaction);

            return affectedRows > 0;
        }

        public Empresa GetById(int id)
        {

            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT Id, Nome, EmpresaPaiId, Cnpj, PontoVenda ");
            sb.Append(" FROM empresa ");
            sb.AppendFormat(" WHERE Id={0} ", id);
            return Connection.Query<Empresa>(sb.ToString(), null, transaction: transaction).FirstOrDefault();


        }

        public List<Empresa> GetAll(EmpresaFilter filter)
        {
            sb = new StringBuilder();
            sb.Append("SELECT Nome, EmpresaPaiId, Cnpj, Email, Contato ");
            sb.Append(" FROM empresa WHERE Id = IFNULL(@Id, Id) AND Nome = IFNULL(@Nome, Nome) ORDER BY Nome");

            var result = Connection.Query<Empresa>(sb.ToString(), filter, transaction: transaction).ToList();

            return result;

        }

              
        public bool Update(Empresa empresa)
        {

            try
            {
                sb = new StringBuilder();
                sb.Append("UPDATE empresa SET Nome = @Nome, EmpresaPaiId=@EmpresaPaiId, Cnpj=@Cnpj, PontoVenda=@PontoVenda");                
                sb.Append(" WHERE Id = @Id");
                var affectedRows = Connection.Execute(sb.ToString(),
                   new
                   {
                       Id = empresa.Id,
                       Nome = empresa.Nome,
                       Cnpj = empresa.Cnpj,
                       EmpresaPaiId = (string.IsNullOrEmpty(empresa.EmpresaPaiId) ? null : empresa.EmpresaPaiId),
                       PontoVenda = empresa.PontoVenda,
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
            else
                return "Nome";

        }

        public List<Empresa> List(DataTableFilter filter)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append("SELECT Id, Nome, EmpresaPaiId, Cnpj, PontoVenda");
            sb.Append(" FROM empresa Where EmpresaPaiId is null");

            if(!string.IsNullOrEmpty(filter.search.value))
                sb.AppendFormat(" AND Nome LIKE '%{0}%' ", filter.search.value);

            if (filter.order.Count > 0)
                sb.AppendFormat(" ORDER BY {0} {1} ", GetNomeColuna(filter.order[0].column), filter.order[0].dir);

            return Connection.Query<Empresa>(sb.ToString(), null, transaction: transaction).ToList();

        }

        public List<Empresa> List(int id, DataTableFilter filter)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append("SELECT Id, Nome, Cnpj, PontoVenda");
            sb.AppendFormat(" FROM empresa Where EmpresaPaiId = {0}", id);

            if (!string.IsNullOrEmpty(filter.search.value))
                sb.AppendFormat(" AND Nome LIKE '%{0}%' ", filter.search.value);

            //if (filter.order.Count > 0)
                sb.AppendFormat(" ORDER BY Id ", GetNomeColuna(filter.order[0].column), filter.order[0].dir);

            return Connection.Query<Empresa>(sb.ToString(), null, transaction: transaction).ToList();
        }
    }

}
