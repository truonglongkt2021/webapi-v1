using Dapper;
using Invedia.Data.EF.Interfaces.DbContext;
using Invedia.DI.Attributes;
using Serilog;
using StreamFile.Contract.Repository.Interface;
using StreamFile.Contract.Repository.Models.DocumentStore;
using StreamFile.Core.Utils;
using StreamFile.Repository.Infrastructure;
using System;
using System.Data;
using System.Data.SqlClient;

namespace StreamFile.Repository
{
    [ScopedDependency(ServiceType = typeof(IDocumentStoreRepository))]
    public class DocumentStoreRepository : Repository<DocumentStoresEntity>, IDocumentStoreRepository
    {
        private readonly ILogger _logger;
        public DocumentStoreRepository(IDbContext dbContext) : base(dbContext)
        {
            _logger = Log.Logger;
        }

        public bool Edit(DocumentStoresEntity entity)
        {
            entity.LastUpdatedTime = CoreHelper.SystemTimeNow;
            var sqlQuery = SqlGenerator.GetUpdate(entity);
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
                _logger.Information(sqlQuery.GetSql(), sqlQuery.Param);
                _logger.Error(ex, ex.Message, entity);
                return false;
            }
            finally
            {
                if (con != null && con.State != ConnectionState.Closed) con.Close();
            }
        }

        public DocumentStoresEntity GetDocByDocumnetId(string documentId)
        {
            var sqlQuery = SqlGenerator.GetSelectFirst(_ => _.DocumentId == documentId, null);
            IDbConnection con = null;
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                var result = con.QuerySingle<DocumentStoresEntity>(sqlQuery.GetSql(), sqlQuery.Param);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Information(sqlQuery.GetSql(), sqlQuery.Param);
                _logger.Error(ex, ex.Message, documentId);
                return null;
            }
            finally
            {
                if (con != null && con.State != ConnectionState.Closed) con.Close();
            }
        }
    }
}
