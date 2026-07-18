using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WMS.Application.Interfaces;

namespace WMS.Infrastructure.Data.Connections
{
    public class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
    {
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
