using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.ncore.ServicedApi.Container
{
    public class KernelRegistry : Dictionary<string, object>
    {
        public void Add( object value )
        {
            base.Add( value.GetType().ToString(), value );
        }

        public void Add( Type type, object value )
        {
            base.Add( type.ToString(), value );
        }
    }
}
