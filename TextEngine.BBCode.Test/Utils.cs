using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.BBCode.Test
{
    public class Utils
    {
        public static int ToInt32(string s, int def = 0)
        {
            if(int.TryParse(s, out int num))
            {
                return num;
            }
            return def;
        }
    }
}
