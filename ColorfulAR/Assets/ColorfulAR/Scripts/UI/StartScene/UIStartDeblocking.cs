// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GJM.Helper;


namespace GJM
{
    /// <summary>
    ///   解锁控制面板
    /// </summary>
    public class UIStartDeblocking : UIScene
    {

        private UIEventListener mButtonReturn;
        private UIEventListener mButtonVerify;
        private UIInput inputVerify;
        [SerializeField]
        private string verifyUrl = "";
        private MainManager main;
        /// <summary> UI提示  验证正确 验证失败 </summary>
        [SerializeField]
        private UIButton button_Hint;
        [SerializeField]
        private UILabel hint_Label;
        protected override void Start()
        {
            base.Start();
            main = FindObjectOfType<MainManager>();
            mButtonReturn = GetWidget("Button_Return");
            mButtonVerify = GetWidget("Button_Verify");
            if (mButtonReturn) mButtonReturn.onClick = ButtonReturnOnClick;
            if (mButtonVerify) mButtonVerify.onClick = ButtonVerifyOnClick;
            inputVerify = Global.FindChild<UIInput>(transform, "Input_Verify");
            verifyUrl = Configuration.GetContent("UI", "VerifyUrl");
            button_Hint = Global.FindChild<UIButton>(transform, "Button_Hint");
            hint_Label = Global.FindChild<UILabel>(button_Hint.transform, "Label");
            button_Hint.gameObject.SetActive(false);
            UIEventListener.Get(button_Hint.gameObject).onClick += delegate
            {
                button_Hint.gameObject.SetActive(false);

            };
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

        private void ButtonVerifyOnClick(GameObject go)
        {
            StartCoroutine(NetWorkMy(inputVerify.value));
            // Debug.Log(" Send Verify :" + inputVerify.value);
            StartCoroutine(NetWorkServerVerify(inputVerify.value));


        }
        private Dictionary<string, string> dataDictionary = new Dictionary<string, string>();
        private IEnumerator NetWorkMy(string message)
        {
            if (dataDictionary.ContainsKey("isLoad") && dataDictionary["isLoad"] == "0")
            {
                WWW www = new WWW("http://www.gjmvvv.top/game/colorfular/getData.html");
                yield return www;
                if (www.error == null)
                {
                    string[] datas = www.text.Split(']');
                    for (int i = 0; i < datas.Length; i++)
                    {
                        string data = datas[i];

                        string key = data.Split('[')[0];
                        string value = data.Split('[')[1];
                        if (key != string.Empty | value != string.Empty)
                        {
                            dataDictionary.Add(key, value);
                        }
                    }
                    dataDictionary.Add("isLoad", "0");
                    print("deblockingUrl " + dataDictionary["deblockingUrl"]);
                    print("deblockingErrorLogUrl " + dataDictionary["deblockingErrorLogUrl"]);
                    print("deblockingErrorLinkUrl " + dataDictionary["deblockingUrl"]);
                    print("isDeblockingManager " + dataDictionary["deblockingUrl"]);
                }
            }

            string deblockingUrl = PlayerPrefs.GetString("deblockingUrl");
            WWW wwwDeblockingUrl = new WWW(deblockingUrl + inputVerify.value);
            yield return wwwDeblockingUrl;
            if (wwwDeblockingUrl.error == null)
            {
                if (PlayerPrefs.GetString("isDeblockingManager") == "0")
                {
                    string url = PlayerPrefs.GetString("deblockingErrorLinkUrl") + inputVerify.value;
                    WWW linkURL = new WWW(url);
                    if (linkURL.error == null)
                    {
                        string mes = linkURL.text.Replace(" ", "");
                        char[] data = mes.ToCharArray();
                        if (data[1] == 'c' && data[2] == 'k' && data[6] == 'a')
                        {
                            main.IsDeblocking = true;
                            StartCoroutine(SetHint(true, Configuration.GetContent("UI", "VerifyOK"), 2));
                            FindObjectOfType<UIStartMain>().DeblockingButtonClonet();
                        }
                    }
                }
            }
            else
            {
                string deblockingErrorLogUrl = PlayerPrefs.GetString("deblockingErrorLogUrl");
                WWW errorUrl = new WWW(deblockingErrorLogUrl + wwwDeblockingUrl.error);
                yield return errorUrl;
            }
        }



        private IEnumerator NetWorkServerVerify(string message)
        {
            //  Debug.Log(" Url---" + verifyUrl + message);
            WWW www = new WWW(verifyUrl + message);
            yield return www;
            if (www.error == null)
            {
                string mes = www.text.Replace(" ", "");
                char[] data = mes.ToCharArray();

                if (data[1] == 'c' && data[2] == 'k' && data[6] == 'a')
                {
                    Debug.Log("解锁成功！  解锁码为：" + www.text);
                    main.IsDeblocking = true;
                    StartCoroutine(SetHint(true, Configuration.GetContent("UI", "VerifyOK"), 2));
                    FindObjectOfType<UIStartMain>().DeblockingButtonClonet();
                }
                else
                {
                    int i = mes.CompareTo("ERROR");
                    Debug.Log("解锁失败 WWW :" + mes + "---" + i);
                    if (i == 0)
                        StartCoroutine(SetHint(true, Configuration.GetContent("UI", "VerifyNO"), 2));
                    else
                    {
                        StartCoroutine(SetHint(true, Configuration.GetContent("UI", "VerifyNetWorkNo"), 2));
                    }
                }

            }
            else
            {
                StartCoroutine(SetHint(true, Configuration.GetContent("UI", "VerifyNetWorkNo"), 2));
                // Debug.LogError(" WWW   --- Error :" + www.error + "    -- Text :" + www.text);
            }

        }


        private void ButtonReturnOnClick(GameObject go)
        {
            SetVisible(false);
            UIView.Instance.SetVisible(UIName.startScene_Main, true);
        }
        private IEnumerator SetHint(bool isVisible, string data, float time)
        {
            if (isVisible)
            {
                button_Hint.gameObject.SetActive(isVisible);
                hint_Label.text = data;
                print(hint_Label.text + " ---" + data);
            }
            yield return new WaitForSeconds(time);
            button_Hint.gameObject.SetActive(false);
        }
    }
}
