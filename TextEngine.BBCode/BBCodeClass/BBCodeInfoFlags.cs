using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.BBCode
{
    [Flags]
    public enum BBCodeInfoFlags
    {
        None = 0,
        /// <summary>
        /// Does not convert sub tags if set.
        /// </summary>
        InnerTextOnly = 1 << 0
    }
}
