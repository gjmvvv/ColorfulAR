// 代码编写：郭进明  |  技术分享博客：http://www.cnblogs.com/GJM6/ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;

namespace GJM
{
    /// <summary>
    /// 本地 加载资源
    /// </summary>
    public class LocalIResourcesService : IResourcesService
    {
        private ResourceManager rm;

        private Model model;


        public void Load()
        {

            model = Model.Instance;
            rm = ResourceManager.Instance;

            //  LoadTextToResourcesManager(); // 初始化配表存储到 资源管理类中
            //  LoadToResourcesToList();      // 获取所有的数据
            //  LoadDataToModel();            // 根据配表 读取资源放到模板中
            //  UninstAllResource();          // 卸载资源      

            LoadOfficialTextToResourcesManager();
            LoadTOfficialoResourcesToList();
            LoadOfficialoDataToModel();
        }




        #region [ Load ]

        #region Load Text To ResourceManager 一
        private void LoadTextToResourcesManager()
        {
            //   rm.LoadText(ResName.ResMapJpg);       
            rm.LoadText(ResName.ResMapFBX);
            rm.LoadText(ResName.ResAudio_Chinese);
            rm.LoadText(ResName.ResAudio_ChineseExplain);
            rm.LoadText(ResName.ResAudio_English);
            rm.LoadText(ResName.ResAudio_EnglishExplain);
            rm.LoadText(ResName.ResAudio_Sound);
            rm.LoadText(ResName.ResMapAnimators);
        }
        #endregion

        #region  Load All Resources To This List 二

        /// <summary> 获取所有的数据
        /// </summary>
        private void LoadToResourcesToList()
        {
            //    List<string> jpgList = rm.GetAllTypeKey(ResName.ResMapJpg);
            fbxList = rm.GetAllTypeKey(ResName.ResMapFBX);
            sList = rm.GetAllTypeKey(ResName.ResAudio_Sound);
            cList = rm.GetAllTypeKey(ResName.ResAudio_Chinese);
            ceList = rm.GetAllTypeKey(ResName.ResAudio_ChineseExplain);
            eList = rm.GetAllTypeKey(ResName.ResAudio_English);
            eeList = rm.GetAllTypeKey(ResName.ResAudio_EnglishExplain);
            animList = rm.GetAllTypeKey(ResName.ResMapAnimators);
        }
        #endregion

        #region Load To Resources Pool 三
        #region [ 数据字段 ]
        private List<string> fbxList = null;
        private List<string> animList = null;
        private List<string> sList = null;
        private List<string> cList = null;
        private List<string> ceList = null;
        private List<string> eList = null;
        private List<string> eeList = null;

        #endregion
        private IEnumerator LoadDataToModel()
        {
            #region  Load Resources To Pool
            int count = fbxList.Count;
            for (int i = 0; i < count; i++)
            {
                ModelData md = new ModelData();
                md.mName = fbxList[i].Split('_')[1];
                #region Load Target And Animator
                Object go = rm.Load(ResName.ResMapFBX, fbxList[i]);
                if (go)
                {
                    md.mTarget = GameObject.Instantiate(go as GameObject);
                    md.animator = md.mTarget.GetComponent<Animator>();
                    md.mTarget.transform.SetParent(model.transform);
                    yield return new WaitForSeconds(0.2f);
                }
                #endregion
                ResourceManagerPool.Instance.AddManagerPool(md);
            }

            for (int i = 0; i < count; i++)
            {
                #region Load AudioClip
                #region Load AudioClip Sound
                ModelData sTypeData = ResourceManagerPool.Instance.GetTypeData(sList[i].Split('_')[0]);
                if (sTypeData != null)
                { // 音频_S
                    Object obj = rm.Load(ResName.ResAudio_Sound, sList[i]);
                    if (obj)
                    {
                        sTypeData.Sound = obj as AudioClip;
                        ResourceManagerPool.Instance.UpdateManagerPool(sTypeData);
                    }
                }
                #endregion
                #region  Load AudioClip Chinese
                ModelData cTypeData = ResourceManagerPool.Instance.GetTypeData(cList[i].Split('_')[0]);
                if (cTypeData != null)
                { // 中文_C
                    Object obj = rm.Load(ResName.ResAudio_Chinese, cList[i]);
                    if (obj)
                    {
                        cTypeData.Chinese = obj as AudioClip;
                        ResourceManagerPool.Instance.UpdateManagerPool(cTypeData);
                    }
                }

                #endregion
                #region Load AudioClip ChineseExplain
                ModelData ceTypeData = ResourceManagerPool.Instance.GetTypeData(ceList[i].Split('_')[0]);
                if (ceTypeData != null)
                { // 中文说明_CE
                    ceTypeData.ChineseExplain = rm.Load<AudioClip>(ResName.ResAudio_ChineseExplain, ceList[i]) != null
                        ? rm.Load<AudioClip>(ResName.ResAudio_ChineseExplain, ceList[i]) : null;
                    ResourceManagerPool.Instance.UpdateManagerPool(ceTypeData);
                }

                #endregion
                #region Load AudioClip English
                ModelData eTypeData = ResourceManagerPool.Instance.GetTypeData(eList[i].Split('_')[0]);
                if (eTypeData != null)
                { // 英文_E
                    eTypeData.English = rm.Load<AudioClip>(ResName.ResAudio_English, eList[i]) != null
                        ? rm.Load<AudioClip>(ResName.ResAudio_English, eList[i]) : null;
                    ResourceManagerPool.Instance.UpdateManagerPool(eTypeData);
                }

                #endregion
                #region Load AudioClip EnglishExplain
                ModelData eeTypeData = ResourceManagerPool.Instance.GetTypeData(eeList[i].Split('_')[0]);
                if (eeTypeData != null)
                {
                    eeTypeData.EnglishExplain = rm.Load<AudioClip>(ResName.ResAudio_EnglishExplain, eeList[i]) != null
                        ? rm.Load<AudioClip>(ResName.ResAudio_EnglishExplain, eeList[i]) : null;
                    ResourceManagerPool.Instance.UpdateManagerPool(eeTypeData);
                }

                #endregion
                #endregion

                #region Load Animator Controller
                ModelData acTypeData = ResourceManagerPool.Instance.GetTypeData(animList[i].Split('_')[0]);
                if (acTypeData != null)
                {
                    Object ac = rm.Load(ResName.ResMapAnimators, animList[i]);
                    //   AnimatorController animC = ac ? ac as AnimatorController : null;
                    //   if (acTypeData.animator)
                    //     acTypeData.animator.runtimeAnimatorController = animC;
                    ResourceManagerPool.Instance.UpdateManagerPool(acTypeData);
                    model.AddModeleData(acTypeData);
                }
                #endregion
            }
            #endregion
            model.ClearModel();
            //  List<ModelData> mdList = ResourceManagerPool.Instance.GetAllTypeData();
            //  for (int i = 0; i < mdList.Count; i++) { model.AddModeleData(mdList[i]); }
            // mdList.Clear();

            ResourceManagerPool.Instance.ClearAll();
        }
        #endregion

        #region  Uninst All Resource 四
        private void UninstAllResource()
        {
            #region Uninstall Resource

            sList.Clear();
            cList.Clear();
            ceList.Clear();
            eList.Clear();
            eeList.Clear();

            rm.UninstallResource(ResName.ResAudio_Sound);
            rm.UninstallResource(ResName.ResAudio_Chinese);
            rm.UninstallResource(ResName.ResAudio_ChineseExplain);
            rm.UninstallResource(ResName.ResAudio_English);
            rm.UninstallResource(ResName.ResAudio_EnglishExplain);

            animList.Clear();
            rm.UninstallResource(ResName.ResMapAnimators);

            fbxList.Clear();
            rm.UninstallResource(ResName.ResMapFBX);
            #endregion
        }
        #endregion
        #endregion


        #region [ Load ] Official
        /// <summary> 加载配表到资源管理类中 </summary>
        private void LoadOfficialTextToResourcesManager()
        {
            rm.LoadText(ResName.OfficialPrefab);
            rm.LoadText(ResName.OfficialAudioClicp_C);
            rm.LoadText(ResName.OfficialAudioClicp_CC);
            rm.LoadText(ResName.OfficialAudioClicp_E);
            rm.LoadText(ResName.OfficialAudioClicp_EE);
            rm.LoadText(ResName.OfficialAudioClicp_S);

        }

        private List<string> prefabList = null; 
        private List<string> audioclip_c = null;
        private List<string> audioclip_cc = null;
        private List<string> audioclip_e = null;
        private List<string> audioclip_ec = null;
        private List<string> audioclip_s = null;
        private void LoadTOfficialoResourcesToList()
        {
            prefabList = rm.GetAllTypeKey(ResName.OfficialPrefab); 
            audioclip_c = rm.GetAllTypeKey(ResName.OfficialAudioClicp_C);
            audioclip_cc = rm.GetAllTypeKey(ResName.OfficialAudioClicp_CC);
            audioclip_e = rm.GetAllTypeKey(ResName.OfficialAudioClicp_E);
            audioclip_ec = rm.GetAllTypeKey(ResName.OfficialAudioClicp_EE);
            audioclip_s = rm.GetAllTypeKey(ResName.OfficialAudioClicp_S);
        }

        private void LoadOfficialoDataToModel()
        {
            int count = prefabList.Count;
            for (int i = 0; i < count; i++)
            {

                ModelData md = new ModelData();
                md.mName = prefabList[i];
                Object go = rm.Load(ResName.OfficialPrefab, prefabList[i]);
                if (go)
                {
                    md.mTarget = GameObjectPool.instance.CreateObject(prefabList[i], go as GameObject, Vector3.zero, Quaternion.identity);
                    md.animator = md.mTarget.GetComponent<Animator>();
                    md.mTarget.transform.SetParent(model.transform);
                    md.mTarget.name = md.mTarget.name.Replace("(Clone)", "");
                    Object c = rm.Load(ResName.OfficialAudioClicp_C, audioclip_c[i]);
                    Object ce = rm.Load(ResName.OfficialAudioClicp_CC, audioclip_cc[i]);
                    Object e = rm.Load(ResName.OfficialAudioClicp_E, audioclip_e[i]);
                    Object ee = rm.Load(ResName.OfficialAudioClicp_EE, audioclip_ec[i]);
                    Object s = rm.Load(ResName.OfficialAudioClicp_S, audioclip_s[i]);

                    if (c) md.Chinese = c as AudioClip;
                    if (ce) md.ChineseExplain = ce as AudioClip;
                    if (e) md.English = e as AudioClip;
                    if (ee) md.EnglishExplain = ee as AudioClip;
                    if (s) md.Sound = s as AudioClip;
                }
                ResourceManagerPool.Instance.AddManagerPool(md);
                model.AddModeleData(md);
            }
            model.ClearModel(); // 先初始化一下模型层容器  
          //  List<ModelData> mdList = ResourceManagerPool.Instance.GetAllTypeData(); // 从资源池加载出来的模型
           // for (int i = 0; i < mdList.Count; i++) { model.AddModeleData(mdList[i]); } // 附加到模型成上
            UninstOfficialAllResource();

        }



        private void UninstOfficialAllResource()
        {
            prefabList.Clear();
            audioclip_c.Clear();
            audioclip_cc.Clear();
            audioclip_e.Clear();
            audioclip_ec.Clear();
            audioclip_s.Clear();


            rm.UninstallResource(ResName.OfficialAudioClicp_C);
            rm.UninstallResource(ResName.OfficialAudioClicp_CC);
            rm.UninstallResource(ResName.OfficialAudioClicp_E);
            rm.UninstallResource(ResName.OfficialAudioClicp_EE);
            rm.UninstallResource(ResName.OfficialAudioClicp_S);

            rm.UninstallResource(ResName.OfficialPrefab);
            ResourceManagerPool.Instance.ClearAll();

        }


        #endregion
        public void Save()
        {
          //  Debug.Log(" -- LocalIResourcesService - Save ");
        }
        bool isDown = false;
        public bool LoadIsDown()
        {
            return isDown;
        }
    }
}
