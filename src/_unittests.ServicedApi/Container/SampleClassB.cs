﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _unittests.org.ncore.ServicedApi.Container
{
    public class SampleClassB : ISampleInterfaceA
    {
        public string WhoAmI()
        {
            return "I am a SampleClassB";
        }
    }
}
