using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.BBCode
{
    public class BBCodeValidator
    {
        /// <summary>
        /// Current bbcode info.
        /// </summary>
        public BBCodeInfo BBCode { get; set;}
        /// <summary>
        /// Current tag data.
        /// </summary>
        public dynamic TagData { get; set; }
        /// <summary>
        /// Does not print if set.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
