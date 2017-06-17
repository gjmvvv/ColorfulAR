// 代码编写：郭进明  |  技术分享博客：http://www.cnblogs.com/GJM6/ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GJM
{
    /// <summary>
    /// 目标 马达运动类
    /// </summary>
    public class TargetMotor : MonoBehaviour
    {

        private TargetManagerPool targetPool;
        [SerializeField]
        private TargetManager targetM;

        public TargetManager SetTargetManager
        {
            set
            {
                this.targetM = value;
            }
        }
        /// <summary> 移动 </summary>
        [SerializeField]
        private float moveSpeed = 0.05f;
        /// <summary> 转向速度 </summary>
        [SerializeField]
        private float rotationSpeed = 0.3f;
        /// <summary> 旋转速度 </summary>
        [SerializeField]
        private float rotateSpeed = 50;

        void Start()
        {
            targetPool = FindObjectOfType<TargetManagerPool>();

        }

        void OnEnable()
        {
            if (targetM)
                targetM.onInitTransfrom += InitTransfrom;
            else
            {
                Debug.LogWarning("OnEnable TargetManager Null " + name);
            }
        }

        void OnDisable()
        {
            if (targetM)
                targetM.onInitTransfrom -= InitTransfrom;
            else
            {
                Debug.LogError("OnDisable TargetManager Null " + name);
            }
        }

        void OnDestroy()
        {
            if (targetM)
                targetM.onInitTransfrom -= InitTransfrom;
            else
            {
                Debug.LogError("OnDestroy TargetManager Null " + name);
            }
        }

        private void InitTransfrom()
        {
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = new Quaternion(0, 0, 0, 0);
            this.transform.localScale = new Vector3(1, 1, 1);
        }

        /// <summary> 输入类调用的运动方法 </summary>
        /// <param name="horizontal">水平</param>
        /// <param name="vertical">垂直</param>
        public void MovementMove(float horizontal, float vertical)
        {
            //    if (targetPool.GetNowTargetList() == null || targetPool.GetNowTargetList().Count <= 0) targetPool.GetNowTargetList().Add(this.gameObject);
            if (horizontal != 0 || vertical != 0)
            {
              //  Debug.Log(" --- 移动：" + horizontal + "." + vertical);
                Vector3 direct = new Vector3(horizontal * moveSpeed, 0, vertical * moveSpeed); // 移动
                List<TargetData> td = targetPool.GetNowAllTargetData();
                if (td.Count <= 0) return;
                for (int i = 0; i < td.Count; i++)
                {
                    td[i].mTarget.transform.Translate(direct);
                }

            }
            else
            { // 停止运动
               // Debug.Log("--- 停止运动 ！");
            }

        }

        /// <summary> 输入类调用的旋转方法 </summary>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        public void MovementRotating(float horizontal, float vertical)
        {
            //  if (targetPool.GetNowTargetList() == null || targetPool.GetNowTargetList().Count <= 0) targetPool.GetNowTargetList().Add(this.gameObject);
            if (horizontal != 0 || vertical != 0)
            {
                //    Debug.Log(" --- 旋转：" + horizontal + "." + vertical);
                List<TargetData> td = targetPool.GetNowAllTargetData();
                if (td.Count <= 0) return;
                for (int i = 0; i < td.Count; i++)
                {
                    // td[i].mTarget.transform.rotation = Quaternion.Euler(new Vector3(vertical * rotateSpeed, 0, -horizontal * rotateSpeed));
                    //  td[i].mTarget.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -horizontal * rotateSpeed));

                    //    td[i].mTarget.transform.rotation = Quaternion.Euler(new Vector3(0,vertical * rotateSpeed,  0));

                    if (vertical < 0)
                        td[i].mTarget.transform.localEulerAngles += new Vector3(0, vertical * -rotateSpeed, 0);
                    if (vertical > 0)
                        td[i].mTarget.transform.localEulerAngles += new Vector3(0, -vertical * rotateSpeed, 0);
                }
            }
            else
            {
               // Debug.Log(" --- 停止旋转 ！ ");
            }
        }
    }
}
