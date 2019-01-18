using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.DataAccess.DbContexts
{
    public interface INiteLigaContextProvider
    {
        NiteLigaContext GetContext();
    }

    public class NiteLigaContextProvider : INiteLigaContextProvider
    {
        /// <summary>
        /// Возвращает ConnectionString.
        /// </summary>
        /// <remarks>Метод только на время отладки. Необходимо вынести в параметры бизнес логики.</remarks>
        private static string GetConnectionString()
        {
            SqlConnectionStringBuilder bldr = new SqlConnectionStringBuilder();
            bldr.InitialCatalog = "NiteLiga2019Test";
            bldr.DataSource = @"localhost";
            bldr.IntegratedSecurity = true;
            return bldr.ConnectionString;
        }

        public NiteLigaContext GetContext()
        {
            return new NiteLigaContext(GetConnectionString());
        }
    }
}
