using Control.Facilites.Data.Repositories;
using Control.Facilites.Domain.Interfaces.Repositories;
using Control.Facilites.Domain.Interfaces.Repositories.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MySql.Data.MySqlClient;

namespace Control.Facilites.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields 
        private readonly MySqlConnection connection;

        #endregion

        #region Constructor
        public UnitOfWork(IConfigurationRoot configuration)
        {
            var connectionString = configuration.GetSection(RepositoryBase.CONNECTIONSTRING_KEY);

            if (string.IsNullOrWhiteSpace(connectionString.Value))
                throw new ArgumentNullException("Connection string not found");


            //connection = new MySqlConnection("Server=104.198.37.67; Port=3306; Database=control_start; Uid=admindb; Pwd=Ybr125agC_k1970;Allow User Variables=True;SslMode=Required;");
             connection = new MySqlConnection(connectionString.Value);
        
        }
        #endregion

        #region Begin
        public IUnitOfWorkTransaction Begin(params IRepositoryBase[] repositories)
        {

            if (repositories == null || repositories.Length == 0)
                throw new ArgumentNullException(nameof(repositories), "repositories is required");
            var unitOfWorkTransaction = new UnitOfWorkTransaction(connection, repositories);
            return unitOfWorkTransaction;
        }
        #endregion
    }
}
