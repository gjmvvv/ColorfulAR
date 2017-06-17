// 代码编写：郭进明  |  技术分享博客：http://www.cnblogs.com/GJM6/ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GJM
{
    /// <summary>
    /// 
    /// </summary>
    public class TextFactory : MonoBehaviour
    {
        public List<ModelData> tdList;
        void Start()
        {
            IResourcesService resourcesService = AbstractFactory.CreateResourcesServic();
            resourcesService.Load();
            tdList = ResourceManagerPool.Instance.GetAllTypeData();
        }

    }
}