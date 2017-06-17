// 代码编写：郭进明  |  技术分享博客：http://www.cnblogs.com/GJM6/ 

using UnityEngine;
using System.Collections;


namespace GJM
{
    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class ModelData
    {
        /// <summary> 识别的名称【预制件名称】 </summary>
        public string mName;
        /// <summary> 预制件 </summary>
        public GameObject mTarget;

        public Animator animator;

        public AudioClip Chinese;
        public AudioClip English;
        public AudioClip Sound;
        public AudioClip ChineseExplain;
        public AudioClip EnglishExplain;
 
    }
}
