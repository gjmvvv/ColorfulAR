// 代码编写：郭进明  |  技术分享博客：http://www.cnblogs.com/GJM6/ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GJM
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourcesFactory : MonoBehaviour
    {
        private string PrefabsPath = "Prefabs/";  // + Type + name

        #region 初始化
        private Dictionary<string, Object> GoCache;
        private ResourcesFactory()
        {
            GoCache = new Dictionary<string, Object>();
        }
        private static ResourcesFactory instance;
        public static ResourcesFactory Instance
        {
            get
            {
                if (instance == null)
                    instance = new ResourcesFactory();
                return instance;
            }
        }
        #endregion

        /// <summary> 创建预制件物体 </summary>
        /// <param name="prefabsPath"></param>
        /// <returns></returns>
        public GameObject CreatePrefabs(string prefabsPath)
        {
            return LoadResouce<GameObject>(PrefabsPath + prefabsPath);
        }
        private T LoadResouce<T>(string resouceName) where T : Object
        {
            if (!GoCache.ContainsKey(resouceName))
            {
                T go = (T)Resources.Load(resouceName);
                if (!(go is Texture))
                    go = (T)GameObject.Instantiate(go);
                go.name = go.name.Replace("(Clone)", "");
                GoCache.Add(resouceName, go);
            }
            return (T)GoCache[resouceName];

        }

    }
}
