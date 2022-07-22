using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Filters;
using Control.Facilites.Domain.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.Data.Repositories
{
    internal class ConfiguracaoDapperRepository : RepositoryBase, IConfiguracaoDapperRepository
    {
        StringBuilder sb;
        public ConfiguracaoDapperRepository(IConfigurationRoot configuration) : base(configuration)
        {
        }

        public Configuracao GetById(int id)
        {
            sb = new StringBuilder();
            sb.Clear();
            sb.Append("SELECT ");
            sb.Append("    Id, CaminhoNexxera  ");
            sb.Append(" FROM configuracao ");

            var result = Connection.QueryFirstOrDefault<Configuracao>(sb.ToString(), new { Id = id }, transaction: transaction);
            return result;
        }



        public bool Update(Configuracao configuracao)
        {

            try
            {

                var affectedRows = Connection.Execute("UPDATE configuracao SET CaminhoNexxera=@CaminhoNexxera WHERE Id = @Id",
                    new { 
                        Id = configuracao.Id,
                        CaminhoNexxera = configuracao.CaminhoNexxera
                    }
                   , transaction: transaction);

                return true;
            }


            catch
            {
                return false;
            }

            

        }

        public Configuracao Add(Configuracao obj)
        {
            throw new NotImplementedException();
        }

        public List<Configuracao> GetAll(ConfiguracaoFilter filter)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }

        public List<Configuracao> List(DataTableFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
