using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace FlexDevSagas.Common.Providers
{
    public class CustomSqlLockStatementProvider :
        ILockStatementProvider
    {
        private readonly string _tableName;

        public CustomSqlLockStatementProvider(string tableName)
        {
            _tableName = tableName;
        }

        public string GetRowLockStatement<T>(DbContext context) where T : class
        {
            return $"select * from \"{_tableName}\" WHERE \"CorrelationId\" = @p0";
        }
    }
}
