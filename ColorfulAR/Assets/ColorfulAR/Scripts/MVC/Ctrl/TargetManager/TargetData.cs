// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using UnityEngine;
using System.Collections;
using EasyAR;


namespace GJM
{
    /// <summary>
    ///  
    /// </summary>
    [System.Serializable]
    public class TargetData
    {
        public string mName;

        /// <summary>  模型物体 </summary>
        public GameObject mTarget; 

        /// <summary> 目标管理类 （Root 孩子（模型物体的父级））</summary>
        public TargetManager mTargetManager = null;

        /// <summary> 识别物体管理 (Root) </summary>
        public EasyTargetManager mEasyTargetManager = null;

    }
}
