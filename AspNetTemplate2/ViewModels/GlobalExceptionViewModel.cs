using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetTemplate2.ViewModels
{
    public class GlobalExceptionViewModel
    {
        public int StatusCode { get; set; }
        public bool IsStatusCodeException { get; set; }
        public string Source { get; set; }
        public Exception ApplicationException { get; set; }
    }
}
