// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using UnityEngine;
using System.Collections;


namespace GJM
{
    /// <summary>
    ///  设置界面
    /// </summary>
    public class UIStartSet : UIScene
    {

        private UIEventListener mButtonReturn;
        private UIEventListener mButtonDownload;
        private UIToggle mToggleAudio;

        private AudioSource audioSource;
        protected override void Start()
        {
            base.Start();
            audioSource = FindObjectOfType<AudioSource>();
            mButtonReturn = GetWidget("Button_Return");
            mButtonDownload = GetWidget("Button_Download");
            mToggleAudio = Global.FindChild<UIToggle>(transform, "Toggle_Audio");
            UIEventListener.Get(mToggleAudio.gameObject).onClick = ToggleAudioManager;
            mButtonReturn.onClick = ButtonReturnOnClick;
            mButtonDownload.onClick = ButtonDownloadOnClick; 
        }

            void OnEnable()
        { InvokeRepeating("PutOffQuit", 0, 0.02f); } 
        private void PutOffQuit()
        { 
            if (Input.GetKeyUp(KeyCode.Escape) )
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




        private void ButtonReturnOnClick(GameObject go)
        {
            SetVisible(false);
            UIView.Instance.SetVisible(UIName.startScene_Main, true); 
        }

        private void ButtonDownloadOnClick(GameObject go)
        {

            string url = GJM.Helper.Configuration.GetContent("UI", "DonwnloadUrl");
            Debug.Log(" Button Download . . ." + url);
            Application.OpenURL(url);
        }

        private void ToggleAudioManager(GameObject go)
        {

            audioSource.mute = mToggleAudio.value;
        }

    }
}
