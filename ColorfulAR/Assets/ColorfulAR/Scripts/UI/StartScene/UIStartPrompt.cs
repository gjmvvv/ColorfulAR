// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using UnityEngine;
using System.Collections;


namespace GJM
{
    /// <summary>
    ///  提示
    /// </summary>
    public class UIStartPrompt : UIScene
    {

        private GameObject Button_Return;
        private GameObject Button_Quit;
        protected override void Start()
        {
            base.Start();
            Button_Quit = Global.FindChild(transform, "Button_Quit");
            Button_Return = Global.FindChild(transform, "Button_Return");

            UIEventListener.Get(Button_Quit).onClick += OnClick;
            UIEventListener.Get(Button_Return).onClick += OnClick;
        }

        void OnEnable()
        { InvokeRepeating("PutOffQuit", 0, 0.02f); }
        /// <summary> 计时 </summary>
        [SerializeField]
        float keepTime;
        [SerializeField]
        bool isQuit = false;
        private void PutOffQuit()
        {
            keepTime += 0.01f;

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (!isQuit) { isQuit = true; return; }
                SetVisible(false);
                UIView.Instance.SetVisible(UIName.startScene_Main, true);
                Application.Quit();
                Debug.Log("退出程序");
            }

            else if (keepTime >= 3)
            {
                SetVisible(false);
                UIView.Instance.SetVisible(UIName.startScene_Main, true);
                keepTime = 0;
            }

        }
        private void OnDisable()
        {
            isQuit = false;
            CancelInvoke("PutOffQuit"); }
        private void OnDestroy()
        { CancelInvoke("PutOffQuit"); }


        private void OnClick(GameObject go)
        {
            switch (go.name)
            {
                case "Button_Quit":
                    Application.Quit();
                    break;
                case "Button_Return":
                    SetVisible(false);
                    UIView.Instance.SetVisible(UIName.startScene_Main, true); 
                 
                    break;
                default:
                    break;
            }
        }



    }
}
