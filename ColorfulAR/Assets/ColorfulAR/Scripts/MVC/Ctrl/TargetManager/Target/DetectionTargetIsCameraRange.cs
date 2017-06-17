// 代码编写：郭进明  |  技术分享博客：http://www.cnblogs.com/GJM6/ 

using UnityEngine;
using System.Collections;

namespace GJM
{
    /// <summary>
    /// 检测物体是否在摄像机的范围内
    /// </summary>
    public class DetectionTargetIsCameraRange : MonoBehaviour
    {
        private float lastTime = 0;
        private float curtTime = 0;
        public bool isRendering = false;

        private void Update()
        {
            isRendering = curtTime != lastTime ? true : false;
            lastTime = curtTime;
        }
        private void OnWillRenderObject()
        {
            curtTime = Time.time;
        }

    }
}
