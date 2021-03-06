using Dapper;
using Invedia.Data.EF.Interfaces.DbContext;
using Invedia.DI.Attributes;
using Serilog;
using StreamFile.Contract.Repository.Interface;
using StreamFile.Contract.Repository.Models.TransferLog;
using StreamFile.Core.Constants;
using StreamFile.Core.Utils;
using StreamFile.Repository.Infrastructure;
using System;
using System.Data;
using System.Data.SqlClient;

namespace StreamFile.Repository
{
    [ScopedDependency(ServiceType = typeof(ITransferLogRepository))]

    public class TransferLogRepository : Repository<TransferLogsEntity>, ITransferLogRepository
    {
        private readonly ILogger _logger;
        public TransferLogRepository(IDbContext dbContext) : base(dbContext)
        {
            _logger = Log.Logger;
        }

        public bool Edit(TransferLogsEntity entity)
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

        public TransferLogsEntity GetPaymentedlogById(string id)
        {
            var sqlQuery = SqlGenerator.GetSelectFirst(_ => _.Id == id && _.Status == StatusConstant.PAYMENT && _.DeletedTime == null, null);
            IDbConnection con = null;
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                var result = con.QueryFirstOrDefault<TransferLogsEntity>(sqlQuery.GetSql(), sqlQuery.Param);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Information(sqlQuery.GetSql(), sqlQuery.Param);
                _logger.Error(ex, ex.Message, id);
                return null;
            }
            finally
            {
                if (con != null && con.State != ConnectionState.Closed) con.Close();
            }
        }

        public TransferLogsEntity GetlogById(string Id)
        {
            var sqlQuery = SqlGenerator.GetSelectFirst(_ => _.Id == Id && _.DeletedTime == null, null);
            IDbConnection con = null;
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                var result = con.QueryFirstOrDefault<TransferLogsEntity>(sqlQuery.GetSql(), sqlQuery.Param);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Information(sqlQuery.GetSql(), sqlQuery.Param);
                _logger.Error(ex, ex.Message, Id);
                return null;
            }
            finally
            {
                if (con != null && con.State != ConnectionState.Closed) con.Close();
            }
        }

        public TransferLogsEntity GetlogByRefId(string refId)
        {
            var sqlQuery = SqlGenerator.GetSelectFirst(_ => _.RefId == refId && _.DeletedTime == null, null);
            IDbConnection con = null;
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                var result = con.QueryFirstOrDefault<TransferLogsEntity>(sqlQuery.GetSql(), sqlQuery.Param);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Information(sqlQuery.GetSql(), sqlQuery.Param);
                _logger.Error(ex, ex.Message, refId);
                return null;
            }
            finally
            {
                if (con != null && con.State != ConnectionState.Closed) con.Close();
            }
        }

        public TransferLogsEntity GetLogFromCallback(string orderId) //Id
        {
            var sqlQuery = SqlGenerator.GetSelectFirst(_ => _.Id == orderId
                                        && _.Status == StatusConstant.NEW
                                        && _.DeletedTime == null, null);
            IDbConnection con = null;
            try
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                var result = con.QueryFirstOrDefault<TransferLogsEntity>(sqlQuery.GetSql(), sqlQuery.Param);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Information(sqlQuery.GetSql(), sqlQuery.Param);
                _logger.Error(ex, ex.Message, orderId);

                return null;
            }
            finally
            {
                if (con != null && con.State != ConnectionState.Closed) con.Close();
            }
        }
    }
}
