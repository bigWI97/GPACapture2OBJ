using GPACapture2OBJ.inc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPACapture2OBJ
{
    internal class Program
    {
        const int SUCCESS = 0;
        const int FAILED = -1;
        static int Main(string[] args)
        {
            string arg0 = (args != null && args.Length != 0) ? args[0] : string.Empty;

            string workDir = string.Empty;
            if(arg0.ToLower().Contains("-h") || arg0.ToLower().Contains("help"))
            {
                Func.LogHelp();
                return SUCCESS;
            }
            else if(string.IsNullOrEmpty(arg0))
            {
                workDir = Environment.CurrentDirectory;
                Log.LInfo($"未输入目标路径，将使用: {workDir} 作为路径");
            }
            else if (Directory.Exists(arg0))
            {
                workDir = arg0;
                Log.LInfo($"使用输入路径: {workDir}");
            }
            else
            {
                Log.LError($"路径无效: {arg0}");
                return FAILED;
            }

            string[] objFiles = Directory.GetFiles(workDir, "*.obj");
            if (objFiles == null || objFiles.Length == 0) 
            {
                Log.LError($"路径下: {workDir} 找不到任何.obj文件");
                return FAILED;
            }

            bool ret = true;
            foreach (string objFile in objFiles)
            {
                if (objFile.EndsWith("_handled.obj")) //忽略已处理的
                {
                    Log.LInfo($"忽略 {objFile}");
                    continue;
                }

                string vtCsvFile = objFile.Replace(".obj", "_uvs.csv");
                string vnCsvFile = objFile.Replace(".obj", "_normals.csv");
                string exportObjPath = objFile.Replace(".obj", "_handled.obj");

                bool hSuc = Func.HandleOne(objFile, vtCsvFile, vnCsvFile, exportObjPath);
                ret = ret && hSuc;
                if (!hSuc) { Log.LError($"在解析导出 {objFile} 时出错"); }
            }

            return ret ? SUCCESS : FAILED;
        }
    }
}
