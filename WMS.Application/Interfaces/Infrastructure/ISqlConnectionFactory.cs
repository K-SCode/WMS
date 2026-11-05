using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace WMS.Application.Interfaces.Infrastructure
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
