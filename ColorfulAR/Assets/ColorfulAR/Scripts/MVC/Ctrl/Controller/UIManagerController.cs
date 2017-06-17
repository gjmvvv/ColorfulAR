// 代码编写：郭进明  |  技术分享博客：http://www.cnblogs.com/GJM6/ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EasyAR;
using System.IO;
using cn.sharerec;
using GJM.Helper;
namespace GJM
{
    /// <summary>
    /// UI 触摸 控制管理 （实现View 层的委托方法）
    /// </summary>
    [RequireComponent(typeof(InputController))]
    public class UIManagerController : MonoBehaviour
    {
        private View view;
        private Camera easyARCamera;
        private InputController inputController;
        private TargetManagerPool targetPool;
        /// <summary> 控制/识别 状态管理 </summary>
        private StatusManager statuM;
        void Start()
        {
            view = View.Instance;
            targetPool = TargetManagerPool.Instance;
            statuM = Ctrl.Instance.GetComponent<StatusManager>();
            easyARCamera = FindObjectOfType<ShareREC>().GetComponent<Camera>();
            InitViewDelegate();
        }

        #region [ Init View UIRoot Delegate ] Start
        private void InitViewDelegate()
        {
            inputController = GetComponent<InputController>();
            inputController.joystick += JoystickManager;
            view.buttonReturnDelegate += this.RealizeReturnDelegate;
            view.buttonOverturnCameraDeleagte += this.RealizeOverturnCameraDeleagte;
            view.buttonPlayAudioChineseDelegate += this.RealizePlayAudioChineseDelegate;
            view.buttonPlayAudioEnglishDelegate += this.RealizePlayAudioEnglishDelegate;
            view.buttonPlayAudioExplainDelegate += this.RealizePlayAudioExplainDelegate;
            view.buttonPhotographDelegate += this.RealizePhotographDelegate;
            view.buttonVideoDelegate += this.RealizeVideoDelegate;
            view.backagerOnClickDelegate += this.OneInputTouch;
            view.backagerOnDoubleDelegate += this.MultiInputTouch;

            view.onBigDelegate += this.OnBig;
            view.onSmallDelegate += this.OnSmall;
        }





        private void JoystickManager(Vector2 move)
        {
          //  Debug.Log(" --- 输入摇杆 ：" + move);

            List<TargetData> tdList = targetPool.GetNowAllTargetData();
            EnumControlStatus ecs = statuM.mControls;

            for (int i = 0; i < tdList.Count; i++)
            {
                switch (ecs)
                {
                    case EnumControlStatus.移动:

                        tdList[i].mTargetManager.MotorMove(move);
                        break;
                    case EnumControlStatus.旋转:
                        tdList[i].mTargetManager.MotorRotating(move);
                        break;
                    default:
                        break;
                }

            }

        }

        #region  [On Click] Big Small
        /// <summary> 放大 </summary>
        private void OnBig()
        {
            List<TargetData> targetDataList = targetPool.GetNowAllTargetData();
            // 放大                       
            for (int i = 0; i < targetDataList.Count; i++)
            {
                Transform target = targetDataList[i].mTarget.transform;
                if (target.localScale.magnitude <= 3.5)
                {
                    target.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                }
            }
        } 
        /// <summary> 缩小 </summary>
        private void OnSmall()
        {
            List<TargetData> targetDataList = targetPool.GetNowAllTargetData();
            // 缩小
            for (int i = 0; i < targetDataList.Count; i++)
            {
                Transform target = targetDataList[i].mTarget.transform;
                if (target.localScale.magnitude >= 0.8f)
                {
                    target.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                }
            }
        }

        #endregion

        #region [ One OnClick ] Play Animator



        /// <summary> 记录上一个触摸点 </summary>
        private Vector2 oldPosition1;
        /// <summary> 记录上一个触摸点 </summary>
        private Vector2 oldPosition2;
        /// <summary> 上一次触摸点的距离 </summary>
        private float lastDistance = 0;

        /// <summary> 多指触摸 </summary>
        private void MultiInputTouch()
        {
           // NGUIDebug.Log("委托 多指触摸");
            // 前两只手指触摸类型都为移动触摸
            // if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            //  {
            var tempPosition1 = Input.GetTouch(0).position;
            var tempPosition2 = Input.GetTouch(1).position;
            //备份上一次触摸点的位置，用于对比    

            List<TargetData> targetDataList = targetPool.GetNowAllTargetData();

            if (targetDataList.Count <= 0)
            { //NGUIDebug.Log(" -- 当前没有操控的目标："); 
                
                return; }
            else
            {
                #region 放大 操作
                //函数返回真为放大，返回假为缩小
                if (isEnlarge(oldPosition1, oldPosition2, tempPosition1, tempPosition2))
                {// 放大                       
                    for (int i = 0; i < targetDataList.Count; i++)
                    {
                        Transform target = targetDataList[i].mTarget.transform;
                        if (target.localScale.magnitude <= 3.5)
                        {
                            target.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                           // NGUIDebug.Log("放大：" + target.transform.localScale + "- 触摸位置：" + tempPosition1 + " - " + tempPosition2);
                        }
                    }
                }
                #endregion
                #region 缩小 操作
                else if (!isEnlarge(oldPosition1, oldPosition2, tempPosition1, tempPosition2))
                {// 缩小                       
                    for (int i = 0; i < targetDataList.Count; i++)
                    {
                        Transform target = targetDataList[i].mTarget.transform;
                        if (target.localScale.magnitude >= 0.8f)
                        {
                            target.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                           // NGUIDebug.Log("缩小：" + target.localScale + "- 触摸位置：" + tempPosition1 + " - " + tempPosition2);
                        }
                    }
                }
                #endregion
                //  }
                oldPosition1 = tempPosition1;
                oldPosition2 = tempPosition2;
            }
        }
        /// <summary> 判断模型放大缩小 </summary>
        /// <param name="oP1"></param>
        /// <param name="oP2"></param>
        /// <param name="nP1"></param>
        /// <param name="nP2"></param>
        /// <returns> True 为放大手势 False为缩小手势 </returns>
        private bool isEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
        {
            var leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
            var leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));
          //  NGUIDebug.Log("位置计算： Leng1:" + leng1 + " - Leng2:" + leng2);
            if (leng1 < leng2) { return true; }   // 放大手势
            else { return false; }                // 缩小手势


        }

        /// <summary> 单指触摸 </summary>
        private void OneInputTouch()
        {
           // Debug.Log("委托 单指触摸");

            Ray ray = easyARCamera.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
               // Debug.DrawRay(Input.mousePosition, hitInfo.collider.transform.position);
                GameObject gameObj = hitInfo.collider.gameObject;
               // NGUIDebug.Log(" 射线 碰到的物体 ：" + gameObj);
                if (gameObj)//当射线碰撞目标为boot类型的物品 ，执行拾取操作
                {
                    TargetAnimation anim = gameObj.transform.parent.GetComponent<TargetAnimation>();
                    if (anim) anim.PlayRandomAnim();
                }
            }


        }

        #endregion


        #region [ Touch ] TwoTouch Manager( size Manager )

        #endregion

        #region [ Return ] Delegate
        private void RealizeReturnDelegate()
        {
            FindObjectOfType<MainManager>().ControlBackAudio(true);
           // Debug.Log("---实现委托 --- 返回 ");
            string uiscene = Configuration.GetContent("Scene", "LoadGameStart");
            StartCoroutine(LoadSceneAsync.Instance.LoadScene(uiscene));
        }
        #endregion

        #region [ Video ] Delegate
        public float videoTime = 10;
        private void RealizeVideoDelegate()
        {
           // Debug.Log("---实现委托 --- 录像 ");
            StartCoroutine(StartVideo());
        }
        private IEnumerator StartVideo()
        {
            view.IsVisibleUI(false);                               // 屏蔽UI 根节点
            view.IsVisibleViewUIExplain(true, "开始录制视频！");           // 显示说明 开始录制视频
            yield return new WaitForSeconds(1);
            view.IsVisibleViewUIExplain(false, "");                        // 关闭说明提示     
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(view.IsVisibleViewUIRrogressBar(videoTime));    // 进度条控制 等待10秒后关闭          
            ShareREC.StartRecorder();                                      // 调用开始录像
            yield return new WaitForSeconds(videoTime);                    // 录像中 等待10秒
            ShareREC.StopRecorder();                                       // 调用结束录像
            view.IsVisibleViewUIExplain(true, "录像结束！请打开相册中ColorfulAR目录查看.");// 显示说明 录制结束,请到相册中查看！
            yield return new WaitForSeconds(3);                            // 等待3秒后
            view.IsVisibleViewUIExplain(false, "");                        // 关闭说明                         
            view.IsVisibleUI(true);                                // 显示UI
            yield return null;
        }
        #endregion

        #region [ Photograph ] Delegate
        private void RealizePhotographDelegate()
        {
          //  Debug.Log("---实现委托 --- 照相 ");
            StartCoroutine(StartPhoto());


        }

        private IEnumerator StartPhoto()
        {
            view.IsVisibleUI(false);// 关闭显示层的UI   
            view.IsVisibleViewUIExplain(true, "开始拍照！");
            yield return new WaitForSeconds(0.5f);
            view.IsVisibleViewUIExplain(false, "");
            yield return new WaitForSeconds(0.2f);
            RealizePhoto();                 // 实现拍照     
            yield return null;
            view.IsVisibleViewUIExplain(true, "拍照结束！请打开相册中ARphoto目录查看"); // 提示说明 等待3秒后关闭          
            yield return new WaitForSeconds(3f);
            view.IsVisibleViewUIExplain(false, ""); // 隐藏说明UI
            view.IsVisibleUI(true);         // 显示显示层的UI
        }


        #region 截图方法 一
        private string Path_save;
        /// <summary> 实现拍照 </summary>
        private void RealizePhoto()
        {

            //获取系统时间并命名相片名  
            System.DateTime now = System.DateTime.Now;
            string times = now.ToString();
            times = times.Trim();
            times = times.Replace("/", "-");
            string filename = "Screenshot" + times + ".png";
            //判断是否为Android平台  
            if (Application.platform == RuntimePlatform.Android)
            {

                //截取屏幕  
                Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                texture.Apply();
                //转为字节数组  
                byte[] bytes = texture.EncodeToPNG();

                string destination = "/sdcard/ColorfulAR";
                //判断目录是否存在，不存在则会创建目录  
                if (!Directory.Exists(destination))
                {
                    Directory.CreateDirectory(destination);
                }
                Path_save = destination + "/" + filename;
               // Debug.Log("截图路径：" + Path_save);
                //存图片  
                System.IO.File.WriteAllBytes(Path_save, bytes);
            }
        }

        #endregion
        #endregion

        #region [ Overturn Camera ] Delegate

        bool isSwitch = true;

        /// <summary> 翻转摄像头 </summary>
        private void RealizeOverturnCameraDeleagte()
        {
           // Debug.Log("---实现委托 --- 翻转摄像头 ");
            if (isSwitch)
            {
                ARBuilder.Instance.CameraDeviceBehaviours[0].Close();
                ARBuilder.Instance.CameraDeviceBehaviours[0].CameraDeviceType = CameraDevice.Device.Front;
                ARBuilder.Instance.CameraDeviceBehaviours[0].OpenAndStart();
                isSwitch = false;
            }
            else
            {
                ARBuilder.Instance.CameraDeviceBehaviours[0].Close();
                ARBuilder.Instance.CameraDeviceBehaviours[0].CameraDeviceType = CameraDevice.Device.Back;
                ARBuilder.Instance.CameraDeviceBehaviours[0].OpenAndStart();
                isSwitch = true;
            }
        }
        #endregion

        #region [ Play Audio Sound ] Play AudioClip - Sound、Chinese、English、ChineseExplain、EnglishExplain

        private void RealizePlayAudioChineseDelegate()
        {
           // Debug.Log("---实现委托 --- 播放中文 ");
            List<TargetData> tdList = targetPool.GetNowAllTargetData();

            tdList[0].mTargetManager.PlayAudio(0);
        }

        private void RealizePlayAudioEnglishDelegate()
        {
          //  Debug.Log("---实现委托 --- 播放英文 ");
            List<TargetData> tdList = targetPool.GetNowAllTargetData();
            tdList[0].mTargetManager.PlayAudio(1);
        }

        private void RealizePlayAudioExplainDelegate()
        {
         //   Debug.Log("---实现委托 --- 播放说明 ");
            List<TargetData> tdList = targetPool.GetNowAllTargetData();
            tdList[0].mTargetManager.PlayAudio(2);
        }

        #endregion
        #endregion
    }
}
