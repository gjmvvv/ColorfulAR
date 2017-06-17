// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace GJM
{
    /// <summary>
    ///  
    /// </summary>
    public class UIScene : MonoBehaviour
    {

        protected string mUIName = "";

        private Dictionary<string, UIEventListener> mUIWidgets = new Dictionary<string, UIEventListener>();

        public UIAnchor.Side side = UIAnchor.Side.Center;

        protected virtual void Start()
        {
            this.FindChildWidgets(gameObject.transform);
        }

        protected virtual void Update()
        {
        }

        public virtual bool IsVisible()
        {
            return gameObject.activeSelf;
            //  return UIView.Instance.IsVisible(this.name);
        }

        public virtual void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
            //  UIView.Instance.SetVisible(this.name, visible);
        }

        protected UIEventListener GetWidget(string name)
        {
            // If allready find out, return 
            if (mUIWidgets.ContainsKey(name)) return mUIWidgets[name];
            // Find out widget with name and add to dictionary
            Transform t = gameObject.transform.Find(name);
            if (t == null) return null;
            UIEventListener widget = t.gameObject.GetComponent<UIEventListener>();
            if (widget != null) { mUIWidgets.Add(widget.gameObject.name, widget); }
            return t.gameObject.GetComponent<UIEventListener>();
        }

        protected T GetWidget<T>(string name) where T : Component
        {
            // Find out widget with name and add to dictionary
            GameObject go = GameObject.Find(name);
            if (go == null) return null;
            T widget = go.GetComponent<T>();
            return widget;
        }

        private void FindChildWidgets(Transform t)
        {
            UIEventListener widget = t.gameObject.GetComponent<UIEventListener>();
            if (widget != null)
            {
                //	Debug.LogWarning("FindChildWidgets Parent[" + t.name + "] " + t.gameObject.name);
                string name = t.gameObject.name;
                if (!mUIWidgets.ContainsKey(name))
                {
                    mUIWidgets.Add(name, widget);
                }
                else
                {
                    //	Debug.LogWarning("Scene[" + this.transform.name + "]UISceneWidget[" + name + "]is exist!");
                }
            }
            for (int i = 0; i < t.childCount; ++i)
            {
                Transform child = t.GetChild(i);
                FindChildWidgets(child);
            }
        }
    }
}
