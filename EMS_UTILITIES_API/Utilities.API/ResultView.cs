using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utilities.API
{
    public class ResultView
    {
        public bool IsSuccess { get; set; }

        public bool IsListResult { get; set; }

        public object Result { get; set; }

    }
}
