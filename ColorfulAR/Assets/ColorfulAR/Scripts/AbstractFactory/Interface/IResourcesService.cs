// 代码编写：郭进明  |  技术分享博客：http://www.cnblogs.com/GJM6/ 

using UnityEngine;
using System.Collections;

namespace GJM
{
    /// <summary>
    /// 
    /// </summary>
    public interface IResourcesService
    {
        bool LoadIsDown();
        void Load();        
        void Save();

    }
}
