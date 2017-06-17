// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using EasyAR;
using GJM.Common;
using GJM.Helper;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GJM
{

    /// <summary> 控制状态 </summary>
    public enum EnumControlStatus
    {
        移动,
        旋转,
    }
    /// <summary> 识别状态 </summary>
    public enum EnumDiscernStatus
    {
        不脱卡,
        脱卡,
    }

    /// <summary> 
    /// 游戏核心控制
    /// </summary>
    [RequireComponent(typeof(EasyTargetController))]
    [RequireComponent(typeof(UIManagerController))]
    [RequireComponent(typeof(StatusManager))]
    public class Ctrl : MonoSingleton<Ctrl>
    {
        #region [ 需要用到的数据 ]

        public int targetNumber = 0;
        public int GetTargetNumber
        {
            get
            {
                return targetNumber++;
            }
        }
        #endregion
        #region [引用到的组件]

        private Model model = null;

        private View view = null;

        /// <summary>   脱卡后模型最佳位置 </summary>
        [SerializeField]
        public Transform TakeOffTheCardTF;

        /// <summary> EasyAR摄像机 </summary>
        private Camera easyARCamera = null;

        /// <summary> 目标管理池 </summary>
        private TargetManagerPool targetPool = null;

        /// <summary> 模板数据列表 </summary>
        [SerializeField]
        private List<ModelData> mdList = new List<ModelData>();

        /// <summary> 控制（移动,旋转） 识别（不脱卡,脱卡）  状态管理 </summary>
        private StatusManager statusManager = null;

        public MainManager main = null;


        /// <summary> 特效包 </summary>
        [SerializeField]
        private GameObject fx_Summoner;

        #endregion

        #region [ Init ]
        void Awkae()
        {

            #region [ EasyKEY ] Awake 一
            //  InitEasyKey();
            #endregion
        }
        void Start()
        {

            GetCtrlComponents();   // [ Init Get Component ] Start 二   
            //  InitResourcesModel();   // [ Init Data To Model ] Start 三  
            //  InitModelToView();  // [ Init Model To View ] Start 四           

            DynamicallyLoadAllTargetsFromJsonFile("OfficialTarget.json"); // [ Init Behaviour ] Start 五

        }
        #endregion

        #region  [ EasyKEY ] Awake
        private const string title = "Please enter KEY first!";
        private const string boxtitle = "===PLEASE ENTER YOUR KEY HERE===";
        private const string keyMessage = ""
            + "Steps to create the key for this sample:\n"
            + "  1. login www.easyar.com\n"
            + "  2. create app with\n"
            + "      Name: HelloARMultiTarget-SingleTracker (Unity)\n"
            + "      Bundle ID: cn.easyar.samples.unity.helloarmultitarget.st\n"
            + "  3. find the created item in the list and show key\n"
            + "  4. replace all text in TextArea with your key";
        private void InitEasyKey()
        {
            if (!FindObjectOfType<EasyARBehaviour>())
            { Debug.LogError(" --- Error EasyARBehaviour Not Null !"); return; }
            if (FindObjectOfType<EasyARBehaviour>().Key.Contains(boxtitle))
            {
#if UNITY_EDITOR
                UnityEditor.EditorUtility.DisplayDialog(title, keyMessage, "OK");
#endif
                Debug.LogError(title + " " + keyMessage);
            }
        }

        #endregion

        #region [ Init Get Component  ] Start 二
        private void GetCtrlComponents()
        {
            main = FindObjectOfType<MainManager>();
            view = FindObjectOfType<View>(); if (!view) view = View.Instance;
            model = FindObjectOfType<Model>(); if (!model) model = Model.Instance;
            targetPool = TargetManagerPool.Instance;
            statusManager = GetComponent<StatusManager>();
            if (!easyARCamera)
            {
                var renderCameraBehviour = FindObjectOfType<RenderCameraBehaviour>();
                if (renderCameraBehviour) { easyARCamera = renderCameraBehviour.GetComponent<Camera>(); }
            }
            fx_Summoner.SetActive(false);
        }
        #endregion

        #region [ Init Data To Model ] Start 三
        /// <summary> 初始化资源到模板 </summary>
        private void InitResourcesModel()
        {
            IResourcesService iRS = AbstractFactory.CreateResourcesServic();
            iRS.Load();
            // 调用工厂  处理数据到模板上   
            mdList = model.GetAllModelData();
        }
        #endregion

        #region [ Init Model To View ] Start 四
        private void InitModelToView()
        {

            for (int i = 0; i < mdList.Count; i++)
            {
                TargetData td = new TargetData();
                #region Load Target GameObject
                td.mName = mdList[i].mName;
                td.mTarget = CreateTarget(mdList[i].mName, mdList[i].mTarget);
                td = TargetAddComponent(td, mdList[i]);
                #endregion
                //  print(i+" Init Model To View :"+td.mName);
                TargetManagerPool.Instance.AddTargetPool(td);
            }
        }
        Vector3 pos;
        private GameObject CreateTarget(string targetName, GameObject go)
        {
            GameObject target = new GameObject(targetName);
            if (go)
            {
                go.name = go.name.Replace("(Clone)", "");
                go.transform.position += pos;
                go.transform.SetParent(target.transform);
            }
            return target;
        }
        private TargetData TargetAddComponent(TargetData td, ModelData md)
        {
            td.mTargetManager = td.mTarget.AddComponent<TargetManager>();
            td.mTargetManager.InitThisComponent(md);
            td.mTargetManager.SetAudioClip(md.Chinese, md.ChineseExplain, md.English, md.EnglishExplain, md.Sound);
            // td.mTargetManager.SetBehaviour(td.mEasyTargetManager);
            return td;
        }
        #endregion

        #region [ Init Behaviour ] Start 五
        /// <summary> 动态地加载所有目标 从json文件
        /// </summary>
        /// <param name="json"></param>
        /// <param name="ResourcesPath"></param>
        private void DynamicallyLoadAllTargetsFromJsonFile(string json)
        {
            try
            {
                ImageTrackerBehaviour tracker = FindObjectOfType<ImageTrackerBehaviour>();
                tracker.SimultaneousNum = mdList.Count;
                List<Target> targetList = ImageTarget.LoadListFromJsonFile(json, StorageType.Assets);
                print(" 读取到JSON 数量 :" + targetList.Count);
                if (targetList.Count == 0) { Debug.LogError("Json Null " + targetList); return; }

                GameObject model = Resources.Load<GameObject>("ImageTarget");

                foreach (var target in targetList.Where(t => t.IsValid).OfType<ImageTarget>())
                {

                    using (target)
                    {
                        #region Load Target

                        TargetData TD = new TargetData();
                        if (model)
                        {
                            TD.mName = target.Name;
                            GameObject go = Instantiate(model);
                            go.name = target.Name;
                            go.transform.SetParent(view.transform);
                            TD.mEasyTargetManager = go.GetComponent<EasyTargetManager>();
                            TD.mEasyTargetManager.SetupWithJsonFile(json, StorageType.Assets, target.Name);
                            TD.mEasyTargetManager.Bind(tracker);
                            TargetManagerPool.Instance.AddTargetPool(TD);
                        }

                        //  TD.mName = target.Name;
                        //  TD.mTarget = new GameObject(TD.mName);
                        //
                        //
                        //  TD.mTarget.transform.SetParent(view.transform);
                        //  TD.mEasyTargetManager = TD.mTarget.AddComponent<EasyTargetManager>();
                        //  TD.mEasyTargetManager.SetupWithJsonFile(json, StorageType.Assets, target.Name);
                        //  TD.mEasyTargetManager.Bind(tracker);
                        //  TD.mTargetManager = TD.mTarget.AddComponent<TargetManager>();
                        //  TargetManagerPool.Instance.AddTargetPool(TD);

                        //  TargetData td = targetPool.GetTargetData(target.Name);
                        //  if (td != null)
                        //  {
                        //      GameObject go = new GameObject("Target_" + target.Name);
                        //      Debug.Log(" --------Behaviour  " + go);
                        //      td.mEasyTargetManager = go.AddComponent<EasyTargetManager>();
                        //      // td.mEasyTargetManager.ActiveTargetOnStart = false;
                        //      td.mEasyTargetManager.SetupWithJsonFile(json, StorageType.Assets, target.Name);
                        //      td.mEasyTargetManager.Bind(tracker);
                        //      td.mEasyTargetManager.transform.SetParent(view.transform);
                        //      targetPool.UpdataTargetPool(td);
                        //  }
                        //  else
                        //  {
                        //      Debug.LogError(" --- Load Error Behaviour ");
                        //  }
                        #endregion
                    }
                }


            }
            catch (System.Exception e)
            {
                Debug.LogError(" Ctrl Json Error :" + e);
            }


        }

        #region  其他方法 无用

        /*

        /// <summary> 动态加载图像 </summary>
        private void DynamicallyLoadFromImage()
        {
            EasyImageTargetBehaviour targetBehaviour;
            ImageTrackerBehaviour tracker = FindObjectOfType<ImageTrackerBehaviour>();

            CreateTarget("argame01", out targetBehaviour);
            targetBehaviour.Bind(tracker);
            targetBehaviour.SetupWithImage("sightplus/argame01.jpg", StorageType.Assets, "argame01", new Vector2());
            GameObject duck02_1 = Instantiate(Resources.Load("duck02")) as GameObject;
            duck02_1.transform.parent = targetBehaviour.gameObject.transform;
            targetBehaviour = null;
        }
        /// <summary> 动态地加载json文件 </summary>
        private void DynamicallyLoadFromJsonFile()
        {
            EasyImageTargetBehaviour targetBehaviour;
            ImageTrackerBehaviour tracker = FindObjectOfType<ImageTrackerBehaviour>();

            CreateTarget("argame00", out targetBehaviour);
            targetBehaviour.Bind(tracker);
            targetBehaviour.SetupWithJsonFile("targets.json", StorageType.Assets, "argame");
            GameObject duck02_2 = Instantiate(Resources.Load("duck02")) as GameObject;
            duck02_2.transform.parent = targetBehaviour.gameObject.transform;
            targetBehaviour = null;
        }

        /// <summary> 动态地加载json字符串 </summary>
        private void DynamicallyLoadFromJsonString()
        {
            EasyImageTargetBehaviour targetBehaviour;
            ImageTrackerBehaviour tracker = FindObjectOfType<ImageTrackerBehaviour>();
            string jsonString = @"
{
  ""images"" :
  [
    {
      ""image"" : ""sightplus/argame02.jpg"",
      ""name"" : ""argame02""
    }
  ]
}
";
            CreateTarget("argame02", out targetBehaviour);
            targetBehaviour.Bind(tracker);
            targetBehaviour.SetupWithJsonString(jsonString, StorageType.Assets, "argame02");
            GameObject duck02_3 = Instantiate(Resources.Load("duck02")) as GameObject;
            duck02_3.transform.parent = targetBehaviour.gameObject.transform;
            targetBehaviour = null;
        }

        */
        #endregion

        #endregion

        #region [ View 提供函数 ]
        public void SetDiscernStatus(EnumDiscernStatus enumDiscernStatus)
        {
            statusManager.mDiscerns = enumDiscernStatus;
        }
        public void SetControlStatus(EnumControlStatus enumControlStatus)
        {
            statusManager.mControls = enumControlStatus;
        }
        #endregion

        public void InitNowTransfrom()
        {
            List<TargetData> tdList = targetPool.GetNowAllTargetData();
            for (int i = 0; i < tdList.Count; i++)
            {
                if (tdList[i].mTargetManager.onInitTransfrom != null)
                    tdList[i].mTargetManager.onInitTransfrom();
            }
        }

        /// <summary> 设置特效 </summary>
        /// <param name="transform"></param>
        public void SetFxSummoner(Transform parent, bool isShow)
        {
            fx_Summoner.SetActive(isShow);
            fx_Summoner.transform.SetParent(parent);
            fx_Summoner.transform.localPosition = Vector3.zero;
    
        }

    

    }
}
