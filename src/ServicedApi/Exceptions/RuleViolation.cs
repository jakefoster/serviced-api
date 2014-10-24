using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace org.ncore.ServicedApi.Exceptions
{
    [DebuggerDisplay( "Property = {Property}, Error = {Error}" )]
    public class RuleViolation
    {
        public string Error
        {
            get;
            private set;
        }

        public string Property
        {
            get;
            private set;
        }

        public RuleViolation( string error )
        {
            Error = error;
        }

        public RuleViolation( string property, string error )
        {
            Property = property;
            Error = error;
        }

        public override string ToString()
        {
            if( string.IsNullOrEmpty( Property ) )
            {
                return string.Format( "Error = {0}", Error );
            }
            else
            {
                return string.Format( "Property = {0}, Error = {1}", Property, Error );
            }
        }
    }
}
