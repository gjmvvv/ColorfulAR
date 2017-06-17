
using UnityEngine;
using System.Collections;

namespace GJM
{
    /// <summary>
    /// 
    /// </summary>
    public class Global : MonoBehaviour
    {

        /// <summary> 场景的UI 名称 </summary>
        public static string LoadUIName = "";
        /// <summary> 3D场景的 名称 </summary>
        public static string LoadSceneName = "";

        /// <summary> 是否存在3D场景 </summary>
        public static bool Contain3DScene = false;

        /// <summary> 递归 根据名称找子物体 </summary>
        /// <param name="trans"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static GameObject FindChild(Transform trans, string childName)
        {
            Transform child = trans.Find(childName);
            if (child != null) { return child.gameObject; }
            int count = trans.childCount;
            GameObject go = null;
            for (int i = 0; i < count; ++i)
            {
                child = trans.GetChild(i);
                go = FindChild(child, childName);
                if (go != null) return go;
            }
            return null;
        }
   
        /// <summary> 找子物体下的组件 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="trans"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static T FindChild<T>(Transform trans, string childName) where T : Component
        {
            GameObject go = FindChild(trans, childName);
            if (go == null) return null;
            return go.GetComponent<T>();
        }

        /// <summary> 获取时间格式字符串，显示mm:ss </summary>
        /// <returns>The minute time.</returns>
        /// <param name="time">Time.</param>
        public static string GetMinuteTime(float time)
        {
            int mm, ss;
            string stime = "0:00";
            if (time <= 0) return stime;
            mm = (int)time / 60;
            ss = (int)time % 60;
            if (mm > 60)
                stime = "59:59";
            else if (mm < 10 && ss >= 10)
            {
                stime = "0" + mm + ":" + ss;
            }
            else if (mm < 10 && ss < 10)
            {
                stime = "0" + mm + ":0" + ss;
            }
            else if (mm >= 10 && ss < 10)
            {
                stime = mm + ":0" + ss;
            }
            else
            {
                stime = mm + ":" + ss;
            }
            return stime;
        }
    }
}
