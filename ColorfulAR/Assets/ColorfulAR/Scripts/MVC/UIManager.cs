// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EasyAR;


namespace GJM
{
    /// <summary>
    ///  UI 管理器
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        private View view;
        private Ctrl ctrl;

        void Start()
        {
            view = FindObjectOfType<View>();
            ctrl = FindObjectOfType<Ctrl>();
            InitButtonAndToggle();
        }



        #region UIEventListener  Button  Toggle

        /// <summary> 开关控制 旋转/移动 </summary>
        private UIToggle Toggle_control;
        /// <summary> 开关控制 不脱卡/脱卡 </summary>
        private UIToggle Toggle_discern;

        private UIButton Button_return;
        private UIButton Button_overturn;
        private UIButton Button_chinese;
        private UIButton Button_english;
        private UIButton Button_explain;
        private GameObject Button_photograph;
        private UIButton Button_video;
        private GameObject Button_background;
        /// <summary> 放大按钮 </summary>
        private GameObject Button_Big;
        /// <summary> 缩小按钮</summary>
        private GameObject Button_Small;
        private void InitButtonAndToggle()
        {

            Button_Big = Global.FindChild(transform, "Button_Big");
            if (Button_Big) UIEventListener.Get(Button_Big).onPress = BigOnPress;
            Button_Small = Global.FindChild(transform, "Button_Small");
            if (Button_Small) UIEventListener.Get(Button_Small).onPress = SmallOnPress;

            Toggle_control = Global.FindChild<UIToggle>(transform, "Toggle_control");
            if (Toggle_control) UIEventListener.Get(Toggle_control.gameObject).onClick = ControlOnClick;

            Toggle_discern = Global.FindChild<UIToggle>(transform, "Toggle_discern");
            if (Toggle_discern) UIEventListener.Get(Toggle_discern.gameObject).onClick = DiscernOnClick;

            Button_return = Global.FindChild<UIButton>(transform, "Button_return");
            if (Button_return) UIEventListener.Get(Button_return.gameObject).onClick = ReturnOnClick;

            Button_overturn = Global.FindChild<UIButton>(transform, "Button_overturn");
            if (Button_overturn) UIEventListener.Get(Button_overturn.gameObject).onClick = OverturnOnClick;

            Button_chinese = Global.FindChild<UIButton>(transform, "Button_chinese");
            if (Button_chinese) UIEventListener.Get(Button_chinese.gameObject).onClick = ChineseOnClick;

            Button_english = Global.FindChild<UIButton>(transform, "Button_english");
            if (Button_english) UIEventListener.Get(Button_english.gameObject).onClick = EnglishOnClick;

            Button_explain = Global.FindChild<UIButton>(transform, "Button_explain");
            if (Button_explain) UIEventListener.Get(Button_explain.gameObject).onClick = ExplainOnClick;

            Button_photograph = Global.FindChild(transform, "Button_photograph");
            if (Button_photograph) UIEventListener.Get(Button_photograph.gameObject).onClick = PhotographOnClick;

            Button_video = Global.FindChild<UIButton>(transform, "Button_video");
            if (Button_video) UIEventListener.Get(Button_video.gameObject).onClick = VideoOnClick;

            Button_background = Global.FindChild(transform, "Button_background");
            if (Button_background) UIEventListener.Get(Button_background).onClick = BackagerOnClick;

        }






        private void BackagerOnClick(GameObject go)
        {
          //  Debug.Log("按键背景 -" + Input.touchCount);
#if UNITY_ANDROID || UNITY_IPHONE
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            { if (view.backagerOnClickDelegate != null) view.backagerOnClickDelegate(); }
            else if (Input.touchCount > 1)
            { if (view.backagerOnDoubleDelegate != null) view.backagerOnDoubleDelegate(); }
#elif  UNITY_STANDALONE_WIN
            if(Input.GetMouseButtonDown(0))
                 { if (view.backagerOnClickDelegate != null) view.backagerOnClickDelegate(); }
#endif


        }

        /// <summary> 显示隐藏UI </summary>
        /// <param name="visible"></param>
        public void IsVisbleView(bool visible)
        {
          //  Debug.Log("控制显示UI ：" + visible);
            if (Toggle_control) Toggle_control.gameObject.SetActive(visible);
            if (Toggle_discern) Toggle_discern.gameObject.SetActive(visible);
            if (Button_return) Button_return.gameObject.SetActive(visible);
            if (Button_overturn) Button_overturn.gameObject.SetActive(visible);
            if (Button_chinese) Button_chinese.gameObject.SetActive(visible);
            if (Button_english) Button_english.gameObject.SetActive(visible);
            if (Button_explain) Button_explain.gameObject.SetActive(visible);
            if (Button_photograph) Button_photograph.gameObject.SetActive(visible);
            if (Button_video) Button_video.gameObject.SetActive(visible);
            if (Button_background) Button_background.gameObject.SetActive(visible);
            if (Button_Big) Button_Big.gameObject.SetActive(visible);
            if (Button_Small) Button_Small.gameObject.SetActive(visible);
        }

     
        bool isStart = false;
        private void SmallOnPress(GameObject go, bool state)
        {
           // Debug.Log(" UI--- 缩小 ---" + state);
            if (state)
            {
                if (!isStart)
                {
                    InvokeRepeating("Small", 0, 0.1f);
                    isStart = state;
                 //   print("开始缩小");
                } 
            } 
            else { //print("缩小结束");
                
                isStart = state; CancelInvoke("Small"); }

        }
        private void Small()
        {
          //  print("缩小中");
            if (view.onSmallDelegate != null) view.onSmallDelegate();
        }


        private void BigOnPress(GameObject go, bool state)
        {
           // Debug.Log(" UI--- 放大 ---" + state); 
            if (state)
            {
                if (!isStart)
                {
                    InvokeRepeating("Big", 0, 0.1f);
                    isStart = state;
                   // print("开始缩小");
                }
            }
            else { //print("缩小结束");
                isStart = state; CancelInvoke("Big"); }
        }
        private void Big()
        {
         //   print("放大中");
            if (view.onBigDelegate != null) view.onBigDelegate();
        }


        private void PhotographOnClick(GameObject go)
        {
            //   Debug.Log(" UI--- 拍照 ---");
            if (view.buttonPhotographDelegate != null) view.buttonPhotographDelegate();
        }

        private void VideoOnClick(GameObject go)
        {
            //  Debug.Log(" UI--- 录像 ---");
            if (view.buttonVideoDelegate != null) view.buttonVideoDelegate();
        }

        private void ExplainOnClick(GameObject go)
        {
            //  Debug.Log(" UI--- 播放音频 说明 ---" + go.name);
            if (view.buttonPlayAudioExplainDelegate != null) view.buttonPlayAudioExplainDelegate();
        }

        private void EnglishOnClick(GameObject go)
        {
            //  Debug.Log(" UI--- 播放音频 英文 ---" + go.name);
            if (view.buttonPlayAudioEnglishDelegate != null) view.buttonPlayAudioEnglishDelegate();
        }

        private void ChineseOnClick(GameObject go)
        {
            //  Debug.Log(" UI--- 播放音频 中文 ---" + go.name);

            if (view.buttonPlayAudioChineseDelegate != null) view.buttonPlayAudioChineseDelegate();
        }

        private void OverturnOnClick(GameObject go)
        {
            //   Debug.Log(" UI--- 翻转摄像头 ---");
            if (view.buttonOverturnCameraDeleagte != null) view.buttonOverturnCameraDeleagte();
        }

        private void ReturnOnClick(GameObject go)
        {
            //      Debug.Log(" UI--- 返回 ---");
            if (view.buttonReturnDelegate != null) view.buttonReturnDelegate();
        }

        /// <summary> 识别事件 【不脱卡/脱卡】 </summary>
        /// <param name="go"></param>
        private void DiscernOnClick(GameObject go)
        {
            if (!Toggle_discern) return;

            if (Toggle_discern.value)
            {
               // Debug.Log(" UI--- 识别事件 【不脱卡/脱卡】 ---脱卡");
                ctrl.SetDiscernStatus(EnumDiscernStatus.脱卡);
              
            }
            else
            {
               // Debug.Log(" UI--- 识别事件 【不脱卡/脱卡】 ---不脱卡");
                ctrl.SetDiscernStatus(EnumDiscernStatus.不脱卡);
            }
        }

        /// <summary> 控制事件 【旋转/移动】 </summary>
        /// <param name="go"></param>
        private void ControlOnClick(GameObject go)
        {
            if (!Toggle_control) return;

            if (Toggle_control.value)
            {
               // Debug.Log(" UI--- 控制事件 【旋转/移动】 ---旋转");
                ctrl.SetControlStatus(EnumControlStatus.旋转);
            }
            else
            {
               // Debug.Log(" UI--- 控制事件 【旋转/移动】 ---移动");
                ctrl.SetControlStatus(EnumControlStatus.移动);
            }
        //    ctrl.InitNowTransfrom();

        }

        #endregion

        #region 原有其他数据  【无用】
        private void ButtonOnDrag(GameObject go, Vector2 delta)
        {
         //   NGUIDebug.Log("--- On Drag :" + go + " Vector2:" + delta);
        }
        private void ButtonOnPress(GameObject go, bool state)
        {
            //   NGUIDebug.Log("--- On Press :" + go + " bool:" + state);
            if (state) InvokeRepeating("InvokeUpdate", 0, 0.2f);
            else
            {
                CancelInvoke("InvokeUpdate");
            }
        }
        #region 模型操控  废弃
        /*
        private List<Transform> targetList = new List<Transform>();
        /// <summary> 记录上一个触摸点 </summary>
        private Vector2 oldPosition1;
        /// <summary> 记录上一个触摸点 </summary>
        private Vector2 oldPosition2;
        /// <summary> 上一次触摸点的距离 </summary>
        private float lastDistance = 0;
        /// <summary> EasyAR识别的摄像机 </summary>
        /// Camera EasyARCamera;
        private void Init()
        {
       //     EasyARCamera = FindObjectOfType<EasyAR.RenderCameraBehaviour>().GetComponent<Camera>();
        }

        /// <summary> 当开始点击到屏幕时候调用 手指离开取消方法 </summary>
        private void InvokeUpdate()
        {
            #region 单点触摸
            if (Input.touchCount == 1)
            {
                OneInputTouch();
            }
            #endregion
            #region 多点触摸
            if (Input.touchCount > 1)
            {
                MultiInputTouch();
                #region 方法2
                /*
                if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
                {

                    float currentDistance = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;
                    if (Mathf.Abs(lastDistance - currentDistance) < 1f)
                    {
                        NGUIDebug.Log("-多点触摸-移动" + Input.GetTouch(0).position + " " + Input.GetTouch(1).position);
                        lastDistance = currentDistance;
                    }
                    else if (currentDistance > lastDistance)
                    {
                        NGUIDebug.Log("-多点触摸-放大" + Input.GetTouch(0).position + " " + Input.GetTouch(1).position);
                    }
                    else
                    {
                        NGUIDebug.Log("-多点触摸-缩小" + Input.GetTouch(0).position + " " + Input.GetTouch(1).position);
                    }
                }
           
                #endregion



            }
            #endregion
        }



        /// <summary> 多指触摸 </summary>
        private void MultiInputTouch()
        {
            // 前两只手指触摸类型都为移动触摸
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                var tempPosition1 = Input.GetTouch(0).position;
                var tempPosition2 = Input.GetTouch(1).position;
                //备份上一次触摸点的位置，用于对比    

                #region 移动
                if (tempPosition1.magnitude + tempPosition2.magnitude == lastDistance)
                {
                    lastDistance = tempPosition1.magnitude + tempPosition2.magnitude;
                    NGUIDebug.Log("-- 模型移动：" + lastDistance);
                }
                #endregion


                #region 放大 操作
                //函数返回真为放大，返回假为缩小
                if (isEnlarge(oldPosition1, oldPosition2, tempPosition1, tempPosition2))
                {// 放大   

                    for (int i = 0; i < targetList.Count; i++)
                    {
                        if (targetList[i].localScale.magnitude <= 3.5)
                        {
                            targetList[i].localScale += new Vector3(0.1f, 0.1f, 0.1f);
                            NGUIDebug.Log("放大：" + targetList[i].localScale + "- 触摸位置：" + tempPosition1 + " - " + tempPosition2);
                        }
                    }

                }
                #endregion
                #region 缩小 操作
                else if (!isEnlarge(oldPosition1, oldPosition2, tempPosition1, tempPosition2))
                {// 缩小                       
                    for (int i = 0; i < targetList.Count; i++)
                    {
                        if (targetList[i].localScale.magnitude >= 0.8f)
                        {
                            targetList[i].localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                            NGUIDebug.Log("缩小：" + targetList[i].localScale + "- 触摸位置：" + tempPosition1 + " - " + tempPosition2);
                        }
                    }
                }
                #endregion

                oldPosition1 = tempPosition1;
                oldPosition2 = tempPosition2;


            }
        }
        /// <summary> 单指触摸 </summary>
        private void OneInputTouch()
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                var tempPosition = Input.GetTouch(0).position;
                NGUIDebug.Log("单指转动：- 触摸位置：" + tempPosition);
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    GameObject gameObj = hitInfo.collider.gameObject;
                    if (gameObj)//当射线碰撞目标为boot类型的物品 ，执行拾取操作
                    {
                        NGUIDebug.Log("单指点击 播放动画：" + gameObj.name);
                        EasyTargetManager etm = gameObj.transform.parent.GetComponent<EasyTargetManager>();
                        if (etm)
                            NGUIDebug.Log("单指点击 播放动画：" + gameObj.name + " ---- ETM :" + etm.name);
                    }
                }

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
            NGUIDebug.Log("位置计算： Leng1:" + leng1 + " - Leng2:" + leng2);
            if (leng1 < leng2) { return true; }   // 放大手势
            else { return false; }                // 缩小手势
        }

        private void isMove(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
        {
            var leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
            var leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));
            NGUIDebug.Log("位置计算： Leng1:" + leng1 + " - Leng2:" + leng2);

        }
*/

        #endregion

        #endregion

    }
}
