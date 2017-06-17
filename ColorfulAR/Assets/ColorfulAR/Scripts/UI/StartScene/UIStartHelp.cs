// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using UnityEngine;
using System.Collections;


namespace GJM
{
    /// <summary>
    ///  帮助控制面板
    /// </summary>
    public class UIStartHelp : UIScene
    {

        public enum CycleModel
        {
            往返循环,
            到尾转头到头转尾,
        }

        [SerializeField]
        private CycleModel 循环模式 = CycleModel.往返循环;

      //   [SerializeField]
        private GameObject[] helpObjs;
        private UIEventListener mButtonReturn;
        private UIEventListener mButtonLeft;
        private UIEventListener mButtonRight;
        private UIKeyBinding keyCode;
        protected override void Start()
        {
            base.Start();
            helpObjs = new GameObject[3];
            for (int i = 0; i < 3; i++)
            {
                helpObjs[i] = Global.FindChild(transform, "Help_" + (i + 1));
                if (i != 0) helpObjs[i].SetActive(false);
            }
            mButtonReturn = Global.FindChild<UIEventListener>(transform, "Button_Return");
            mButtonLeft = Global.FindChild<UIEventListener>(transform, "Button_Left");
            mButtonRight = Global.FindChild<UIEventListener>(transform, "Button_Right");
            if (mButtonLeft) mButtonLeft.onClick = ButtonLeft;
            if (mButtonRight) mButtonRight.onClick = ButtonRight;
            if (mButtonReturn) mButtonReturn.onClick = ButtonReturn;           
        }

        void OnEnable()
        { InvokeRepeating("PutOffQuit", 0, 0.02f); }
        private void PutOffQuit()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                SetVisible(false);
                UIView.Instance.SetVisible(UIName.startScene_Main, true);
                CancelInvoke("PutOffQuit");
            }
        }
        private void OnDisable()
        { CancelInvoke("PutOffQuit"); }
        private void OnDestroy()
        { CancelInvoke("PutOffQuit"); }



        private void ButtonReturn(GameObject go)
        {
            SetVisible(false);
            UIView.Instance.SetVisible(UIName.startScene_Main, true);
        }

        private void ButtonRight(GameObject go)
        {
            indexNumber++;
            switch (循环模式)
            {
                case CycleModel.往返循环:
                    if (indexNumber >= helpObjs.Length - 1) { indexNumber = helpObjs.Length - 1; }
                    HelpInit();
                    break;
                case CycleModel.到尾转头到头转尾:
                    if (indexNumber > helpObjs.Length - 1) { indexNumber = 0; }
                    HelpInit();
                    break;
                default:
                    break;
            }



        }

        private void ButtonLeft(GameObject go)
        {
            indexNumber--;   
            switch (循环模式)
            {
                case CycleModel.往返循环:
                    if (indexNumber <= 0) { indexNumber = 0; }
                    HelpInit();
                    break;
                case CycleModel.到尾转头到头转尾:
                    if (indexNumber < 0) { indexNumber = helpObjs.Length - 1; }
                    HelpInit();
                    break;
                default:
                    break;
            }
      
        }
        [SerializeField]
        private int indexNumber = 0;
        private void HelpInit()
        {
            for (int i = 0; i < helpObjs.Length; i++)
            {
                if (helpObjs[i] == null) break;
                if (indexNumber == i) { helpObjs[i].SetActive(true); }
                else
                {
                    helpObjs[i].SetActive(false);
                }
            }
        }
    }
}
