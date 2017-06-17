// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using UnityEngine;
using System.Collections;
using GJM.Helper;
using System.Collections.Generic;


namespace GJM
{
    /// <summary>
    ///  主页一级页面
    /// </summary>
    public class UIStartMain : UIScene
    {

        private string MainUrl;

        /// <summary> 开始游戏 </summary>
        private UIEventListener mButton_Play;
        /// <summary> 帮助 </summary>
        private UIEventListener mButton_Help;
        /// <summary> 设置 </summary>
        private UIEventListener mButton_Set;
        /// <summary> 打开主页 </summary>
        private UIEventListener mButton_Main;
        /// <summary> 解锁 </summary>
        private UIEventListener mButton_Deblocking;
        /// <summary> 解锁提示 </summary>
        private UIEventListener mButton_Hint;
        /// <summary> 提示说明 </summary>
        private UILabel mLabel_Hint;


        protected override void Start()
        {
            base.Start();
            mButton_Play = GetWidget<UIEventListener>("Button_Play");
            if (mButton_Play) mButton_Play.onClick += ButtonPlayOnClick;
            mButton_Help = GetWidget<UIEventListener>("Button_Help");
            if (mButton_Help) mButton_Help.onClick += ButtonHelpOnClick;
            mButton_Deblocking = GetWidget<UIEventListener>("Button_Deblocking");
            if (mButton_Deblocking) mButton_Deblocking.onClick = ButtonDeblockingOnCkck;
            mButton_Main = GetWidget<UIEventListener>("Button_Main");
            if (mButton_Main) mButton_Main.onClick += ButtonMainOnClick;
            mButton_Set = GetWidget<UIEventListener>("Button_Set");
            if (mButton_Set) mButton_Set.onClick += ButtonSetOnClick;
            MainUrl = Configuration.GetContent("UI", "MainUrl");
            MainManager main = FindObjectOfType<MainManager>();

            mButton_Set = GetWidget<UIEventListener>("Button_Set");

            mButton_Hint = GetWidget<UIEventListener>("Button_Hint");
            mLabel_Hint = Global.FindChild<UILabel>(mButton_Hint.transform, "Label");
            mButton_Hint.onClick += HintOnClick;
            mButton_Hint.gameObject.SetActive(false);

            if (main.IsDeblocking)
            {
                DeblockingButtonClonet();
            }
            else
            {
                SetHint(true, Configuration.GetContent("UI", "Verify"), 3);
            }
          

          
        
        }

        public void DeblockingButtonClonet()
        {
            mButton_Deblocking.GetComponent<BoxCollider>().enabled = false;
            Global.FindChild<UISprite>(mButton_Deblocking.transform, "Background").color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
        }

        private void HintOnClick(GameObject go)
        {
            mButton_Hint.gameObject.SetActive(false);
        }
        private void SetHint(bool isBool, string data, float time)
        {
            mButton_Hint.gameObject.SetActive(isBool);
            mLabel_Hint.text = data;
            if (isBool)
            {
                StartCoroutine(ClonerHint(time));
            }
        }
        private IEnumerator ClonerHint(float time)
        {
            yield return new WaitForSeconds(time);
            mButton_Hint.gameObject.SetActive(false);
        }

        void OnEnable()
        {

            StartCoroutine(PutOffInputKey(1));

        }

        private IEnumerator PutOffInputKey(float time)
        {
            yield return new WaitForSeconds(time);
            InvokeRepeating("PutOffQuit", 0, 0.02f);
        }
        private void PutOffQuit()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                UIView.Instance.SetVisible(UIName.startScene_Prompt, true);
                CancelInvoke("PutOffQuit");
                SetVisible(false);
            }
        }
        private void OnDisable()
        { CancelInvoke("PutOffQuit"); }
        private void OnDestroy()
        { CancelInvoke("PutOffQuit"); }





        private void ButtonSetOnClick(GameObject go)
        {
           // Debug.Log("设置界面!~");
            SetVisible(false);
            UIView.Instance.SetVisible(UIName.startScene_Set, true);
        }

        private void ButtonMainOnClick(GameObject go)
        {
           // Debug.Log("加载首页!~ Url ：" + MainUrl);
            Application.OpenURL(MainUrl);
        }

        private void ButtonDeblockingOnCkck(GameObject go)
        {
           // Debug.Log("解锁界面！");
            SetVisible(false);
            UIView.Instance.SetVisible(UIName.startScene_Deblocking, true);
        }

        private void ButtonHelpOnClick(GameObject go)
        {
           // Debug.Log("帮助界面！");
            SetVisible(false);
            UIView.Instance.SetVisible(UIName.startScene_Help, true);
        }

        private void ButtonPlayOnClick(GameObject go)
        {
           // Debug.Log("加载场景！" + Configuration.GetContent("Scene", "LoadEasyAR"));
            FindObjectOfType<MainManager>().ControlBackAudio(false);
            StartCoroutine(LoadSceneAsync.Instance.LoadScene(Configuration.GetContent("Scene", "LoadEasyAR")));
        }


    }
}
