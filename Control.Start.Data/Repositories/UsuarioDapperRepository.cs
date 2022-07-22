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
    internal class UsuarioDapperRepository : RepositoryBase, IUsuarioDapperRepository
    {
        StringBuilder sb;
        public UsuarioDapperRepository(IConfigurationRoot configuration) : base(configuration)
        {
        }


        public void ValidaEmail(Usuario obj)
        {

            sb = new StringBuilder();
            sb.Clear();
            sb.Append("SELECT Id FROM usuario");
            sb.AppendFormat(" WHERE Email = '{0}'", obj.Email);

            if (obj.Id > 0)
                sb.AppendFormat(" AND Id <> {0}", obj.Id);

            var result = Connection.QueryFirstOrDefault<Usuario>(sb.ToString(), null, transaction: transaction);

            if (result != null)
                throw new Exception("Email já cadastrado para outro usuário");

        }

        public Usuario Add(Usuario usuario)
        {
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


        public bool Remove(int id)
        {

            var affectedRows = Connection.Execute("DELETE FROM usuario WHERE Id = @Id", new { Id = id }, transaction: transaction);

            return affectedRows > 0;
        }

        public Usuario GetById(int id)
        {
            sb = new StringBuilder();
            sb.Clear();
            sb.Append("SELECT ");
            sb.Append("    u.Id,  ");
            sb.Append("    u.Nome,   ");
            sb.Append("    u.Email,   ");
            sb.Append("    u.Login, u.Senha,   ");
            sb.Append("    u.Ativo,   ");
            sb.Append("    f.Id,   ");
            sb.Append("    f.Nome   ");
            sb.Append(" FROM usuario u ");
            sb.Append(" LEFT JOIN fornecedor f ON(u.Fornecedor = f.Id) ");
            sb.AppendFormat(" WHERE u.Id={0} ", id);

            var data = Connection.Query<Usuario, Fornecedor, Usuario>(sb.ToString(), (usuario, fornecedor) =>
            {
                return usuario;
            }, splitOn: "Id,Id").FirstOrDefault();

            return data;


        }

        public List<Usuario> GetAll(UsuarioFilter filter)
        {

            var result = Connection.Query<Usuario>("SELECT Nome,Email,Login, Senha, Fornecedor, Ativo FROM usuario WHERE Id = IFNULL(@Id, Id) AND Nome = IFNULL(@Nome, Nome)", filter, transaction: transaction).ToList();

            return result;

        }

        public bool Update(Usuario usuario)
        {
            int? fornecedorId = null;

            //if (usuario.Fornecedor != null && usuario.Fornecedor.Id!=0)
            //    fornecedorId = usuario.Fornecedor.Id;


            try
            {

                sb = new StringBuilder();
                sb.Append("UPDATE usuario SET Nome = @Nome,");
                sb.Append(" Email= @Email, Login= @Login, Senha=@Senha, EmpresaFornecedor=@EmpresaFornecedor, Ativo=@Ativo");
                sb.Append(" WHERE Id = @Id");
                var affectedRows = Connection.Execute(sb.ToString(),
                   new
                   {
                       Id = usuario.Id,
                       Nome = usuario.Nome,
                       Email = usuario.Email,
                       Login = usuario.Login,
                       Senha = General.Encrypt(usuario.Senha),
                       EmpresaFornecedor = usuario.EmpresaFornecedor == null ? null : usuario.EmpresaFornecedor.Id.ToString(),
                       Ativo = 1,

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
            if (posicao == 1)
                return "u.Nome";
            else
                return "u.Id";

        }

        public List<Usuario> List(DataTableFilter filter)
        {
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append("SELECT ");
            sb.Append("    u.Id,  ");
            sb.Append("    u.Nome,   ");
            sb.Append("    u.Email,   ");
            sb.Append("    u.Login,   ");
            sb.Append("    u.Ativo   ");
            sb.Append("    f.Id,   ");
            sb.Append("    f.Nome   ");
            sb.Append(" FROM usuario u ");
            sb.Append(" LEFT JOIN fornecedor f ON(u.Fornecedor = f.Id) ");

            if (!string.IsNullOrEmpty(filter.search.value))
                sb.AppendFormat(" AND u.Nome LIKE '{0}%'", filter.search.value);

            if (filter.order.Count > 0)
                sb.AppendFormat(" ORDER BY {0} {1} ", GetNomeColuna(filter.order[0].column), filter.order[0].dir);

            var data = Connection.Query<Usuario, Fornecedor, Usuario>(sb.ToString(), (usuario, fornecedor) =>
            {
                //usuario.Fornecedor = fornecedor;
                return usuario;
            }, splitOn: "Id,Id").ToList();

            return data;

        }

        public Usuario Login(string login, string password)
        {
            sb = new StringBuilder();

            var _password = General.Encrypt(password);

            sb.Clear();

            sb.Append(" SELECT  ");
            sb.Append("    u.Id,  ");
            sb.Append("    u.Nome, ");
            sb.Append("    u.Email, ");
            sb.Append("    u.Login, ");
            sb.Append("    f.Id   ");
            sb.Append(" FROM usuario u ");
            sb.Append(" LEFT JOIN empresa_fornecedor f ON(u.EmpresaFornecedor = f.Id) ");
            sb.Append(" where u.Ativo = 1 ");
            sb.Append($" and u.Login = '{login}' and u.Senha = '{_password}' ");

            var data = Connection.Query<Usuario, EmpresaFornecedor, Usuario>(sb.ToString(), (usuario, fornecedor) =>
            {
                usuario.EmpresaFornecedor = fornecedor;

                return usuario;
            }, splitOn: "Id,Id", transaction: transaction).FirstOrDefault();

            return data;
        }

        private bool ResetPassword(int id, string password)
        {

            try
            {
                var affectedRows = Connection.Execute("UPDATE usuario SET Senha = @Senha, PrimeiroAcesso=0 WHERE Id = @Id",
                    new
                    {
                        Id = id,
                        Senha = password
                    }
                   , transaction: transaction);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public Usuario LoginReset(int id, string login, string password)
        {
            sb = new StringBuilder();

            var _password = General.Encrypt(password);

            if (!ResetPassword(id, _password))
                return null;
            sb.Clear();

            sb.Append(" SELECT  ");
            sb.Append("    u.Id,  ");
            sb.Append("    u.Nome, ");
            sb.Append("    u.Email, ");
            sb.Append("    u.Login, ");
            //sb.Append("    f.Id,  ");
            //sb.Append("    f.Nome, ");
            //sb.Append("    f.Codigo, ");
            //sb.Append("    f.Cnpj ");
            sb.Append(" FROM usuario u ");
            sb.Append(" left join fornecedor f on(u.Fornecedor = f.Id) ");
            sb.Append(" where u.Ativo = 1 ");
            sb.Append($" and u.Email = '{login}' and u.Senha = '{_password}' ");

            var data = Connection.Query<Usuario, Fornecedor, Usuario>(sb.ToString(), (usuario, fornecedor) =>
            {
                //usuario.Fornecedor = fornecedor;

                return usuario;
            }, splitOn: "Id,Id", transaction: transaction).FirstOrDefault();

            return data;
        }


        public List<Usuario> ListByFornecedor(DataTableFilter filter, int? loggedfornecedorId, int? loggedEmpresaId)
        {
            StringBuilder sb = new StringBuilder();
            List<Usuario> usrs = new List<Usuario>();

            sb.Clear();
            sb.Append("SELECT ");
            sb.Append("    u.Id,Senha,  ");
            sb.Append("    u.Nome,   ");
            sb.Append("    u.Email,   ");
            sb.Append("    u.Login,   ");
            sb.Append("    u.Ativo,   ");
            sb.Append("    ef.Id,   ");
            sb.Append("    e.Id,   ");
            sb.Append("    e.Nome,   ");
            sb.Append("    f.Id,   ");
            sb.Append("    f.Nome   ");
            sb.Append(" FROM usuario u ");
            sb.Append(" LEFT JOIN empresa_fornecedor ef ON(u.EmpresaFornecedor = ef.Id) ");
            sb.Append(" LEFT JOIN empresa e ON(e.Id = ef.EmpresaId) ");
            sb.Append(" LEFT JOIN fornecedor f ON(f.Id = ef.FornecedorId) ");
            sb.Append(" WHERE 1 = 1 ");

            if (loggedfornecedorId != null && loggedfornecedorId.Value > 0)
                sb.AppendFormat(" AND u.Fornecedor = {0}", loggedfornecedorId.Value);

            if (loggedEmpresaId != null && loggedEmpresaId.Value > 0)
                sb.AppendFormat(" AND u.Empresa = {0}", loggedEmpresaId.Value);

            if (!string.IsNullOrEmpty(filter.search.value))
                sb.AppendFormat(" AND u.Nome LIKE '{0}%'", filter.search.value);

            if (filter.order.Count > 0)
                sb.AppendFormat(" ORDER BY {0} {1} ", GetNomeColuna(filter.order[0].column), filter.order[0].dir);

            var data = Connection.Query<Usuario, EmpresaFornecedor, Empresa, Fornecedor, Usuario>(sb.ToString(), (usuario, empresa_fornecedor, empresa, fornecedor) =>
            {
                usuario.EmpresaFornecedor = empresa_fornecedor;
                if (empresa_fornecedor != null)
                {
                    usuario.EmpresaFornecedor.Empresa = empresa;
                    usuario.EmpresaFornecedor.Fornecedor = fornecedor;
                }
                return usuario;
            }, splitOn: "Id,Id,Id,Id").ToList();

            foreach (var item in data)
            {
                item.Senha = General.Decrypt(item.Senha);
                usrs.Add(item);


            }

            return usrs;


        }


    }
}
