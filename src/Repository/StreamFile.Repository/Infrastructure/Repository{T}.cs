using StreamFile.Core.Utils;
using Dapper;
using Invedia.Data.Dapper.SqlGenerator;
using Invedia.Data.EF.Interfaces.DbContext;
using Invedia.Data.EF.Services.Repository;
using Microsoft.Data.SqlClient;
using Serilog;
using System;
using System.Data;
using Entity = StreamFile.Contract.Repository.Models.Entity;

namespace StreamFile.Repository.Infrastructure
{
    public abstract class Repository<T> : EntityStringRepository<T> where T : Entity, new()
    {
        protected string ConnectionString;
        protected ISqlGenerator<T> SqlGenerator { get; }

        private readonly ILogger _logger;

        //private readonly SqlGeneratorConfig _config = new SqlGeneratorConfig { SqlProvider = SqlProvider.Oracle };
        protected Repository(IDbContext dbContext) : base(dbContext)
        {
            ConnectionString = SystemHelper.AppDb;
            SqlGenerator = new SqlGenerator<T>(SqlProvider.MSSQL, true);
            _logger = Log.Logger;
        }

        public bool Insert(T entity)
        {
            var dateTimeNow = CoreHelper.SystemTimeNow;
            entity.CreatedTime = dateTimeNow;
            entity.LastUpdatedTime = dateTimeNow;

            var sqlQuery = SqlGenerator.GetInsert(entity);
            IDbConnection con = null;
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                var result = con.Execute(sqlQuery.GetSql(), sqlQuery.Param);
                return result != 0;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, sqlQuery.GetSql(), sqlQuery.Param);
                _logger.Error(ex, ex.Message, entity);
                return false;
            }
            finally
            {
                if (con != null && con.State != ConnectionState.Closed) con.Close();
            }
        }
    }
}