using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.External.API.Shared
{
    public class ResultView
    {
        public bool IsSuccess { get; set; }

        public bool IsListResult { get; set; }
        public string Id { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }

    }
}
