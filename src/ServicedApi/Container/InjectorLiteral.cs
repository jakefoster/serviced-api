﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.ncore.ServicedApi.Container
{
    public class InjectorLiteral
    {
        public object Value { get; set; }
        public InjectorLiteral( object value ) 
        {
            Value = value;
        }
    }
}
