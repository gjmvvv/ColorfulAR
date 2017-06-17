using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using GJM.Helper;

namespace GJM
{
    public class ResName
    {
        public static string ResMapJpg = "ResMapJpg";
        public const string ResPrefab = "ResMapPrefab";
        public const string ResMapFBX = "ResMapFBX";
        public const string ResMapAnimators = "ResMapAnimators";

        public const string ResAudio_Chinese = "ResMapMp3_Chine";
        public const string ResAudio_ChineseExplain = "ResMapMp3_ChineExplain";
        public const string ResAudio_English = "ResMapMp3_English";
        public const string ResAudio_EnglishExplain = "ResMapMp3_EnglishExplain";
        public const string ResAudio_Sound = "ResMapMp3_Sound";



        public const string OfficialPrefab = "OfficialPrefab";

        public const string OfficialAudioClicp_C = "Official_C";
        public const string OfficialAudioClicp_CC = "Official_CC";
        public const string OfficialAudioClicp_E = "Official_E";
        public const string OfficialAudioClicp_EE = "Official_EC";
        public const string OfficialAudioClicp_S = "Official_S";


    }
    /// <summary> 资源管理类 </summary>
  // [RequireComponent(typeof(ResourceManagerPool))]
    public class ResourceManager :MonoSingleton<ResourceManager>
    {
        private static Dictionary<string, Dictionary<string, string>> mData = new Dictionary<string, Dictionary<string, string>>();
    
        /// <summary> 加载 </summary>
        /// <param name="file">资源路径</param>
        /// <param name="aKey">主键</param>
        public void LoadText(string file)
        {
           
          //  Debug.Log(" -- RM Load :" + file);
            Object obj = Resources.Load(file);
            if (!obj) Debug.LogError(" - - -Load Text Null :" + file);
            string mapText = (obj as TextAsset).text;
            StringReader reader = new StringReader(mapText);
            string line = null;
            if (!mData.ContainsKey(file))
                mData.Add(file, new Dictionary<string, string>());
            else { mData[file].Clear();  }
            while ((line = reader.ReadLine()) != null)
            {
                var keyValue = line.Split('=');
                if (!mData[file].ContainsKey(keyValue[0]))
                    mData[file].Add(keyValue[0], keyValue[1]);
                else { Debug.LogError(" --- Load :" + file + " ValueKey Error:" + keyValue[0] + " - ValueValue：" + keyValue[1]); }
            }
            reader.Close();
        }

        /// <summary> 获取单个次次键 </summary>
        /// <param name="mainKey">主键</param>
        /// <param name="resName">次键</param>
        /// <returns></returns>
        private string GetAkeyValue(string mainKey, string resName)
        {
            if (mData.ContainsKey(mainKey))
            {
                if (mData[mainKey].ContainsKey(resName))
                {
                    return mData[mainKey][resName];
                }
              //  Debug.Log(" -- GetAkeyValue  Null resName " + resName); 
                return null;
            }
          //  Debug.Log(" -- GetAkeyValue  Null mainKey " + mainKey); 
            return null;
        }

        /// <summary> 公共接口 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">主键</param>
        /// <param name="path">次键</param>
        /// <returns></returns>
        public T Load<T>(string mainKey, string vlaueKey) where T : Object
        {
            string s = GetAkeyValue(mainKey, vlaueKey);
            return Resources.Load<T>(s);
        }
        public Object Load(string type, string path)
        { return Resources.Load(GetAkeyValue(type, path)); }
              
        public List<string> GetAllTypeKey(string type)
        {
            List<string> keyList = new List<string>();
            if (mData.ContainsKey(type))
            {
                foreach (KeyValuePair<string, string> kvp in mData[type])
                {
                    keyList.Add(kvp.Key);
                }
                return keyList;
            }
            return null;
        }

        public void UninstallResource(string mainKey)
        {
            if (mData.ContainsKey(mainKey))
            {
              //  Debug.Log(" --- 资源卸载成功：" + mainKey + " -" + mData[mainKey].Count);
                mData[mainKey].Clear();
                mData.Remove(mainKey);

            }
        }


        public Dictionary<string, string> GetAllTypeDictionary(string mainKey)
        {
            List<string> keyList = new List<string>();
            if (mData.ContainsKey(mainKey))
            {
                return mData[mainKey];
            }
            return null;
        }
    }
}