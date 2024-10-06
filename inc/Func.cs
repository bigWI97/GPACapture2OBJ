using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GPACapture2OBJ.inc
{
    internal class Func
    {
        //从模型obj文件提取顶点位置v信息
        public static List<v> GetVsFromOBJFile(string path)
        {
            List<v> vs = new List<v>();

            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Log.LError($"未找到OBJ文件路径: {path}");
                return vs;
            }

            foreach(var line in File.ReadLines(path))
            {
                if(line.Contains("v "))
                {
                    string[] sp = line.Split(' ');
                    v @v = new v() 
                    { 
                        x = float.Parse(sp[1]), 
                        y = float.Parse(sp[2]), 
                        z = float.Parse(sp[3]) 
                    };
                    vs.Add(v);
                }
            }

            return vs;
        }

        //从模型obj文件提取面索引f信息
        public static List<f> GetFsFromOBJFile(string path)
        {
            List<f> fs = new List<f>();

            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Log.LError($"未找到OBJ文件路径: {path}");
                return fs;
            }

            foreach (var line in File.ReadLines(path))
            {
                if (line.Contains("f "))
                {
                    f @f = new f();
                    string[] sp1 = line.Split(' ');
                    //sp1[0]是"f",sp1[1..3]是三项索引v/vt/vn
                    for (int i = 0; i <= 2; i++)
                    {
                        string finfo = sp1[i + 1];
                        string[] sp2 = finfo.Split('/');
                        if (!string.IsNullOrEmpty(sp2[0])) { f.vIndex[i] = int.Parse(sp2[0]); }
                        if (!string.IsNullOrEmpty(sp2[1])) { f.vtIndex[i] = int.Parse(sp2[1]); }
                        if (!string.IsNullOrEmpty(sp2[2])) { f.vnIndex[i] = int.Parse(sp2[2]); }
                    }
                    fs.Add(f);
                }
            }

            return fs;
        }

        //从法向量csv文件提取顶点法线vn信息
        public static List<vn> GetVNsFromCSVFile(string path)
        {
            List<vn> vns = new List<vn>();

            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Log.LError($"未找到CSV文件路径: {path}");
                return vns;
            }

            int loop3 = 0;
            foreach (var line in File.ReadLines(path).Skip(1))//忽略第一行的“Index,TEXCOORD1,byte28p”这类
            {
                string[] sp = line.Split(',');
                int index = int.Parse(sp[0]);
                float f = float.Parse(sp[1]);
                //Log.LInfo($"line: {line},  index：{index}, f{loop3}: {f}");
                if(vns.Count == index) { vns.Add(new vn()); }
                vn @vn = vns[index];
                if      (loop3 == 0) @vn.x = f;
                else if (loop3 == 1) @vn.y = f;
                else if (loop3 == 2) @vn.z = f;
                loop3 = (loop3 + 1) % 3;
            }

            return vns;
        }

        //从uv csv文件提取顶点uv vt信息
        public static List<vt> GetVTsFromCSVFile(string path)
        {
            List<vt> vts = new List<vt>();

            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Log.LError($"未找到CSV文件路径: {path}");
                return vts;
            }

            int loop2 = 0;
            foreach (var line in File.ReadLines(path).Skip(1))//忽略第一行的“Index,TEXCOORD2,byte12p”这类
            {
                string[] sp = line.Split(',');
                int index = int.Parse(sp[0]);
                float f = float.Parse(sp[1]);
                //Log.LInfo($"line: {line},  index：{index}, f{loop2}: {f}");
                if (vts.Count == index) { vts.Add(new vt()); }
                vt @vt = vts[index];
                if (loop2 == 0) @vt.x = f;
                else if (loop2 == 1) @vt.y = f;
                loop2 = (loop2 + 1) % 2;
            }

            return vts;
        }

        public static bool HandleOne(string objPath, string vtCsvPath, string vnCsvPath, string exportObjPath)
        {
            if (!File.Exists(objPath)) { Log.LError($"文件不存在: {objPath}"); return false; }
            if (!File.Exists(vtCsvPath)) { Log.LError($"文件不存在: {vtCsvPath}"); return false; }
            if (!File.Exists(vnCsvPath)) { Log.LError($"文件不存在: {vnCsvPath}"); return false; }

            List<v> vs = GetVsFromOBJFile(objPath);
            List<f> fs = GetFsFromOBJFile(objPath);
            List<vt> vts = GetVTsFromCSVFile(vtCsvPath);
            List<vn> vns = GetVNsFromCSVFile(vnCsvPath);

            if (vs.Count == 0 || vts.Count == 0 || vns.Count == 0 || vs.Count != vts.Count || vts.Count != vns.Count) 
            {
                Log.LError($"顶点信息数量异常！vs.Count=={vs.Count}, vts.Count=={vts.Count}, vns.Count=={vns.Count}");
                return false;
            }

            for (int i = 0; i < fs.Count; i++) 
            {
                f @f = fs[i];
                //gpa导出模型文件f面索引的顶点和法线索引是相同的(v==vn),把vt补上相同的就好了
                @f.vtIndex[0] = @f.vIndex[0];
                @f.vtIndex[1] = @f.vIndex[1];
                @f.vtIndex[2] = @f.vIndex[2];
            }

            //导出新的obj文件
            StringBuilder sb = new StringBuilder(1024 * 1024);
            for (int i = 0; i < vs.Count; i++) { sb.AppendLine(vs[i].ToString()); }
            for (int i = 0; i < vts.Count; i++) { sb.AppendLine(vts[i].ToString()); }
            for (int i = 0; i < vns.Count; i++) { sb.AppendLine(vns[i].ToString()); }
            for (int i = 0; i < fs.Count; i++) { sb.AppendLine(fs[i].ToString()); }
            try
            {
                File.WriteAllText(exportObjPath, sb.ToString());
                Log.LInfo($"导出新OBJ文件成功，路径: {exportObjPath}");
                Log.LInfo($">>源OBJ文件: {objPath}");
                Log.LInfo($">>UV CSV文件: {vtCsvPath}");
                Log.LInfo($">>法线CSV文件: {vnCsvPath}");
                Log.LInfo("");
            }
            catch (Exception e)
            {
                Log.LError($"OBJ文件导出失败，异常: {e}");
                return false;
            }

            return true;
        }

        public static void LogHelp()
        {
            Log.LInfo("======================help=======================");
            Log.LInfo("GPACapture2OBJ.exe <待处理路径>");
            Log.LInfo("GPACapture2OBJ.exe <Path>");
            Log.LInfo("");
            Log.LInfo("路径内文件命名例");
            Log.LInfo("./test.obj (GPA导出的模型文件)");
            Log.LInfo("./test_normals.csv (GPA导出的法线csv文件)");
            Log.LInfo("./test_uvs.csv (GPA导出的uv csv文件)");
            Log.LInfo("");
            Log.LInfo("执行成功后会在路径下生成./test_handled.obj文件");
            Log.LInfo("=================================================");
        }
    }
}
