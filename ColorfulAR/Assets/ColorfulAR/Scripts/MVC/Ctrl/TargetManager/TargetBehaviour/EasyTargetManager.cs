/**
* Copyright (c) 2015-2016 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
* EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
* and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
*/

using GJM;
using System.Collections.Generic;
using UnityEngine;

namespace EasyAR
{
    public class EasyTargetManager : ImageTargetBaseBehaviour
    {
        public View view;

        [SerializeField]
        private int targetID = 0;

        public int TaegetID
        {
            set
            {
                targetID = value;
            }
            get
            {
                return targetID;
            }
        }

        private bool isDeblocking;

        public bool IsDeblocking
        {
            set
            {
                isDeblocking = value;
            }
            get
            {
                return isDeblocking;
            }
        }


        protected override void Awake()
        {
            TargetFound += OnTargetFound;
            TargetLost += OnTargetLost;
            TargetLoad += OnTargetLoad;           //加载
            TargetUnload += OnTargetUnload;

            base.Awake();
            if (!view) view = FindObjectOfType<View>();

        }
        void HideObjects(Transform trans)
        {
            for (int i = 0; i < trans.childCount; ++i)
                HideObjects(trans.GetChild(i));
            if (transform != trans)
                gameObject.SetActive(false);
        }

        void ShowObjects(Transform trans)
        {
            for (int i = 0; i < trans.childCount; ++i)
                ShowObjects(trans.GetChild(i));
            if (transform != trans)
                gameObject.SetActive(true);
        }

        /// <summary> 发现目标时候调用 </summary>
        /// <param name="behaviour"></param>
        void OnTargetFound(ImageTargetBaseBehaviour behaviour)
        {
           // Debug.Log("EasyTargetManager ---  Found Name:" + Name + " " + Target.Id);
            if (view.foundTargetDelegate != null)
                view.foundTargetDelegate(Name);
        }
        /// <summary> 当目标丢失 </summary>
        /// <param name="behaviour"></param>
        void OnTargetLost(ImageTargetBaseBehaviour behaviour)
        {
            // HideObjects(transform);
         //   Debug.Log("EasyTargetManager ---  Lost Name:" + Name + " " + Target.Id);
            if (view.lostTargetDelegate != null) view.lostTargetDelegate(Name);
        }

        /// <summary> 当目标加载 </summary>
        /// <param name="behaviour"></param>
        /// <param name="tracker"></param>
        /// <param name="status"></param>
        void OnTargetLoad(ImageTargetBaseBehaviour behaviour, ImageTrackerBaseBehaviour tracker, bool status)
        {

           // Debug.Log("EasyTargetManager --- Load Target :" + this.gameObject.name);
            TargetData td = TargetManagerPool.Instance.GetTargetData(name);
            if (td != null && !td.mTargetManager)
            {
                if (td.mEasyTargetManager == this && !td.mTargetManager)
                {
                    isDeblocking = true;
                    targetID = Ctrl.Instance.GetTargetNumber;
                 
                    if (targetID >= 10)
                    {
                       // Debug.Log(" 加载超过10个 验证是否解锁：" + Ctrl.Instance.main.IsDeblocking);
                        if (Ctrl.Instance.main.IsDeblocking == true)
                        {
                            isDeblocking = true;
                        }
                        else
                        {
                            isDeblocking = false;
                        }
                    }

                    GameObject go = new GameObject("TargetManager");
                    go.transform.SetParent(td.mEasyTargetManager.transform);
                    if (isDeblocking)
                    { 
                        td.mTargetManager = go.AddComponent<TargetManager>();
                    }
           


                }
            }
            if (view.loadTargetDelegate != null) view.loadTargetDelegate(Name);
        }
        /// <summary> 当目标卸载 </summary>
        /// <param name="behaviour"></param>
        /// <param name="tracker"></param>
        /// <param name="status"></param>
        void OnTargetUnload(ImageTargetBaseBehaviour behaviour, ImageTrackerBaseBehaviour tracker, bool status)
        {
         //   Debug.Log("EasyTargetManager --- Unload target (" + status + "): " + Target.Id + " (" + Target.Name + ") " + " -> " + tracker);
            if (view.unloadTargetDelegate != null) view.unloadTargetDelegate(Name);
        }
    }
}
