// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using UnityEngine;
using System.Collections;


namespace GJM
{
    /// <summary>
    ///  
    /// </summary>
    public class UIInit : MonoBehaviour 
    {
        UIView view;
        void Start()
        {
            view = GetComponent<UIView>();
            view.InitPanel();
        }

   
    }
}
