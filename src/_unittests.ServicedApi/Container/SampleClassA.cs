﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _unittests.org.ncore.ServicedApi.Container
{
    public class SampleClassA : ISampleInterfaceA
    {
        public string WhoAmI()
        {
            return "I am a SampleClassA";
        }
    }
}
