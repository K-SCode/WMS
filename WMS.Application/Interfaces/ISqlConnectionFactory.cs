using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace WMS.Application.Interfaces
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
