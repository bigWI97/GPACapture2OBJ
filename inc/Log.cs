using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPACapture2OBJ.inc
{
    internal class Log
    {
        public static void LError(string str)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(str);
            Console.ResetColor();
        }

        public static void LInfo(string str)
        {
            Console.WriteLine(str);
        }
    }
}
