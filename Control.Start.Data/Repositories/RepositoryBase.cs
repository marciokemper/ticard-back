using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Control.Facilites.Data.Repositories
{
    internal class RepositoryBase
    {
        internal const string CONNECTIONSTRING_KEY = "ConnectionString";

        private MySqlConnection connection;
        internal MySqlConnection Connection
        {
            get
            {
                return connection;
            }

            set
            {
                connection = value;
            }
        }

        public RepositoryBase(IConfigurationRoot configuration)
        {
            var connectionString = configuration.GetSection(CONNECTIONSTRING_KEY);
            if (string.IsNullOrWhiteSpace(connectionString.Value))
                throw new ArgumentNullException("Connection string not found");
            Connection = new MySqlConnection(connectionString.Value);

            //connection = new MySqlConnection("Server=104.198.37.67; Port=3306; Database=control_start; Uid=admindb; Pwd=Ybr125agC_k1970;Allow User Variables=True;SslMode=Required;");

        }

        public IDbTransaction transaction { get; private set; }



        public void SetTransaction(MySqlTransaction transaction)
        {
            this.transaction = transaction;
            this.connection = transaction.Connection;
        }

        internal void SetConnection(MySqlConnection connection)
        {
            this.connection = connection;
            this.transaction = null;
        }
    }
}
