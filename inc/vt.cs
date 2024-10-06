using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPACapture2OBJ.inc
{
    internal class vt
    {
        public float x = float.PositiveInfinity, y = float.PositiveInfinity;

        public override string ToString()
        {
            return $"vt {x} {y}";
        }
    }
}
