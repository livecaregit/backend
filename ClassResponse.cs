using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LC_Service
{
    public class ClassResponse
    {
        [DataContract]
        public class RES_RESULT
        {
            [DataMember(Order = 0)]
            public int result { get; set; }
            [DataMember(Order = 1)]
            public string message { get; set; }
            [DataMember(Order = 2)]
            public string data { get; set; }
        }
    }
}