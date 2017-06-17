// 代码编写：郭进明  |  技术分享博客：http://www.cnblogs.com/GJM6/ 

using UnityEngine;
using System.Collections;
using System.IO;
using System;

namespace GJM
{
    /// <summary>
    /// 抽象工厂处理
    /// </summary>
    public  class AbstractFactory
    {
        private static string dbVersion;

        private static string projectName = "GJM";
        /// <summary> 配置注入 </summary>
        static AbstractFactory()
        {          
            dbVersion = File.ReadAllText(Application.streamingAssetsPath + "/DBVersion.txt");        
        }

        public static IResourcesService CreateResourcesServic()
        {
            string className = projectName + "." + dbVersion + "IResourcesService";
            var type = Type.GetType(className);
            return Activator.CreateInstance(type) as IResourcesService;

        }

    }
}
