using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Control.Facilites.Results
{
    public class GenericResult
    {
        public int Count { get; set; }
        public string[] Errors { get; set; }
        public bool Success { get; set; }
    }

    public class GenericResult<TResult> : GenericResult
    {
        public TResult Result { get; set; }
    }

   

}