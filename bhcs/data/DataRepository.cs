using MicroOrm.Dapper.Repositories;
using System.Data;
using MicroOrm.Dapper.Repositories.SqlGenerator;
using System;
using MySql.Data.MySqlClient;

namespace data
{
    public class DataRepository<T> : DapperRepository<T>, IDisposable where T : class
    {
        public DataRepository(IDbConnection connection, ISqlGenerator<T> sqlGenerator)
        : base(connection, sqlGenerator)
        {
        }

        public void Dispose( )
        {
            Connection.Dispose();
        }

        public static DataRepository<T> Create()
        {
            return  new DataRepository<T>(new MySqlConnection(DBHelper.ConnectionString), new SqlGenerator<T>(ESqlConnector.MySQL));
        }
    }
     
}
