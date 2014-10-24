using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace org.ncore.ServicedApi.Persistence
{
    public interface IDataContext : IDisposable
    {
        TransactionScope GetScope();
        TransactionScope GetScope( TransactionScopeOption scopeOption );
        IDataSilo GetSilo<T>() where T : IDataSilo;
    }
}
