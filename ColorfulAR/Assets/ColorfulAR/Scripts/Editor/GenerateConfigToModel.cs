// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using UnityEngine;
using System.Collections;
using UnityEditor;


namespace GJM.Editors
{
    /// <summary>
    ///  根据配表 动态生成模版管理
    /// </summary>
    public class GenerateConfigToModel : MonoBehaviour
    {

        [MenuItem("GJM Tools /Resources Model/工厂加载")]
        public static void GenerateModel()
        {
            IResourcesService iRS = AbstractFactory.CreateResourcesServic();
            iRS.Load(); 
        }
       

    }
}
