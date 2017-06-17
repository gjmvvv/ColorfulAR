// 代码编写：郭进明  |  技术分享博客：http://www.cnblogs.com/GJM6/ 


using UnityEditor;
using System.Collections.Generic;
using System.IO;
using LitJson;

namespace GJM.Editors
{
    /// <summary>
    /// 根据JPG 格式图片 动态加载JSON 字符串生成JSON 文件
    /// </summary>
    public class GenerateJsonConfig : Editor
    {
        private static Dictionary<string, Dictionary<string, string>> mData = new Dictionary<string, Dictionary<string, string>>();
 
        /// <summary> 加载 </summary>
        /// <param name="file">资源路径</param>
        /// <param name="aKey">主键</param>
        private static void LoadText(string file)
        {

            UnityEngine.Object obj = UnityEngine.Resources.Load(file);

            string mapText = (obj as UnityEngine.TextAsset).text;
            StringReader reader = new StringReader(mapText);
            string line = null;
            if (!mData.ContainsKey(file))
                mData.Add(file, new Dictionary<string, string>());
            else { mData[file].Clear(); }
            while ((line = reader.ReadLine()) != null)
            {
                var keyValue = line.Split('=');
                if (!mData[file].ContainsKey(keyValue[0]))
                    mData[file].Add(keyValue[0], keyValue[1]);
            }
            reader.Close();
        }



        [MenuItem("GJM Tools /Resources Json/写入Json（根据ResMapJpg表的路径）")]
        public static void GenerateJson()
        {
            LoadText("ResMapJpg");

            string data = "{\"images\":[";
            int index = mData["ResMapJpg"].Count;
            int number = 0;
            foreach (var item in mData["ResMapJpg"])
            {
                number++;
                if (index == number)
                    data += "{ \"name\":\"" + item.Key + "\",\"image\":\"" + @item.Value + ".jpg\"}";
                else
                    data += "{ \"name\":\"" + item.Key + "\",\"image\":\"" + @item.Value + ".jpg\"},";
            }
            data += "]}";
            string path = Path.Combine(UnityEngine.Application.dataPath, "StreamingAssets").Replace(@"\", "/");
            File.WriteAllText(Path.Combine(path, "JsonTarget.json"), data);
            AssetDatabase.Refresh();
            mData.Remove("ResMapJpg");
        }


        [MenuItem("GJM Tools /Resources Json/写入Json（根据OfficialJpg表的路径）")]
        public static void GenerateOfficialJson()
        {
            LoadText("OfficialJpg");

            string data = "{\"images\":[";
            int index = mData["OfficialJpg"].Count;
            int number = 0;
            foreach (var item in mData["OfficialJpg"])
            {
                number++;
                if (index == number)
                    data += "{ \"name\":\"" + item.Key + "\",\"image\":\"" + @item.Value + ".jpg\"}";
                else
                    data += "{ \"name\":\"" + item.Key + "\",\"image\":\"" + @item.Value + ".jpg\"},";
            }
            data += "]}";
            string path = Path.Combine(UnityEngine.Application.dataPath, "StreamingAssets").Replace(@"\", "/");
            File.WriteAllText(Path.Combine(path, "OfficialTarget.json"), data);
            AssetDatabase.Refresh();
            mData.Remove("OfficialJpg");
        }



    }
}
