// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GJM.Helper;

namespace GJM
{
    /// <summary>
    ///  
    /// </summary>
    public class UIView : MonoSingleton<UIView>
    {
        #region UI 管理

        private Dictionary<string, UIScene> mUISceneDic = new Dictionary<string, UIScene>();
        private Dictionary<UIAnchor.Side, GameObject> mUIAnchor = new Dictionary<UIAnchor.Side, GameObject>();
        private void Start()
        {
            InitializeUIs();
            InitPanel();
        }
      
        private void InitializeUIs()
        {
            mUIAnchor.Clear();
            Object[] objs = FindObjectsOfType(typeof(UIAnchor));
            if (objs != null)
            {
                foreach (Object obj in objs)
                {
                    UIAnchor uiAnchor = obj as UIAnchor;
                    if (!mUIAnchor.ContainsKey(uiAnchor.side))
                        mUIAnchor.Add(uiAnchor.side, uiAnchor.gameObject);
                }
            }
            mUISceneDic.Clear();
            Object[] uis = FindObjectsOfType(typeof(UIScene));
            if (uis != null)
            {
                foreach (Object obj in uis)
                {
                    UIScene ui = obj as UIScene;
                    ui.SetVisible(false);
                    mUISceneDic.Add(ui.gameObject.name, ui);
                }
            }
        }

        public void SetVisible(string name, bool visible)
        {
            if (visible && !IsVisible(name)) { OpenScene(name); }
            else if (!visible && IsVisible(name))
            { CloseScene(name); }
        }

        public bool IsVisible(string name)
        {
            UIScene ui = GetUI(name);
            if (ui != null)
                return ui.IsVisible();
            return false;
        }
        private UIScene GetUI(string name)
        {
            UIScene ui;

            return mUISceneDic.TryGetValue(name, out ui) ? ui : null;
        }

        public T GetUI<T>(string name) where T : UIScene
        {
            return GetUI(name) as T;
        }

        private bool isLoaded(string name)
        {
            if (mUISceneDic.ContainsKey(name))
            {
                return true;
            }
            return false;
        }

        private void OpenScene(string name)
        {
            if (isLoaded(name))
            {
                mUISceneDic[name].SetVisible(true);
            }
        }
        private void CloseScene(string name)
        {
            if (isLoaded(name))
            {
                mUISceneDic[name].SetVisible(false);
            }
        }

        #endregion
        public void InitPanel()
        {
            SetVisible(UIName.startScene_Main, true);
        }

    }
    public class UIName
    {
        public const string startScene_Main = "Panel_Main";
        public const string startScene_Help = "Panel_Help";
        public const string startScene_Deblocking = "Panel_Deblocking";
        public const string startScene_Set = "Panel_Set";
        public const string startScene_Prompt = "Panel_Prompt";
    }
}
