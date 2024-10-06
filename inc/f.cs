using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPACapture2OBJ.inc
{
    internal class f
    {
        public int[] vIndex = new int[3] { int.MaxValue, int.MaxValue, int.MaxValue };
        public int[] vtIndex = new int[3] { int.MaxValue, int.MaxValue, int.MaxValue };
        public int[] vnIndex = new int[3] { int.MaxValue, int.MaxValue, int.MaxValue };

        public override string ToString()
        {
            string v0 = vIndex[0] == int.MaxValue ? string.Empty : vIndex[0].ToString();
            string vt0 = vtIndex[0] == int.MaxValue ? string.Empty : vtIndex[0].ToString();
            string vn0 = vnIndex[0] == int.MaxValue ? string.Empty : vnIndex[0].ToString();
            string v1 = vIndex[1] == int.MaxValue ? string.Empty : vIndex[1].ToString();
            string vt1 = vtIndex[1] == int.MaxValue ? string.Empty : vtIndex[1].ToString();
            string vn1 = vnIndex[1] == int.MaxValue ? string.Empty : vnIndex[1].ToString();
            string v2 = vIndex[2] == int.MaxValue ? string.Empty : vIndex[2].ToString();
            string vt2 = vtIndex[2] == int.MaxValue ? string.Empty : vtIndex[2].ToString();
            string vn2 = vnIndex[2] == int.MaxValue ? string.Empty : vnIndex[2].ToString();
            return $"f {v0}/{vt0}/{vn0} {v1}/{vt1}/{vn1} {v2}/{vt2}/{vn2}";
        }
    }
}
