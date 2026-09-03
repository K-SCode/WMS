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
            if(string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection String cannot be null or empty");
            }
            return new SqlConnection(connectionString);
        }
    }
}
