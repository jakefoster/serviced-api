using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using org.ncore.ServicedApi.Persistence;

namespace org.ncore.ServicedApi
{
    public class ServiceContext : IDisposable
    {
        private const string STORAGE_KEY = "org.ncore.ServicedApi.ServiceContext";

        public static ServiceContext Current
        {
            get
            {
                return (ServiceContext)ContextStorage.Current[ STORAGE_KEY ];
            }
            set
            {
                ContextStorage.Current[ STORAGE_KEY ] = value;
            }
        }

        public IDataContext DataContext { get; private set; }

        public ServiceContext( IDataContext dataContext )
        {
            this.DataContext = dataContext;
        }

        public void Dispose()
        {
            if( this.DataContext != null )
            {
                this.DataContext.Dispose();
                this.DataContext = null;
            }
        }
    }
}
