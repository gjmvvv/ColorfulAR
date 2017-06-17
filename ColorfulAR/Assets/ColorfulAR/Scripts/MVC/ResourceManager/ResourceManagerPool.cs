// 代码编写：郭进明  |  技术分享博客：http://www.cnblogs.com/GJM6/ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GJM.Helper;

namespace GJM
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceManagerPool : MonoSingleton<ResourceManagerPool>
    {
       


        public Dictionary<string, ModelData> managerPool = new Dictionary<string, ModelData>();

        public void AddManagerPool(ModelData TD)
        {
            //Debug.Log(" -- Add Pool  - Key：" + TD.mName + " TypeData:" + TD);
            if (!IsKey(TD.mName)) { managerPool.Add(TD.mName, TD); }
            else { UpdateManagerPool(TD); }
        }
        public void UpdateManagerPool(ModelData TD)
        {
            if (IsKey(TD.mName))
            {
              // if (TD.mTarget) managerPool[TD.mName].mTarget = TD.mTarget;
              // // if (TD.mTarget) OutTargetToPool(managerPool[TD.mName].mTarget);
              // if (TD.Sound) managerPool[TD.mName].Sound = TD.Sound;
              // if (TD.Chinese) managerPool[TD.mName].Chinese = TD.Chinese;
              // if (TD.English) managerPool[TD.mName].English = TD.English;
              // if (TD.ChineseExplain) managerPool[TD.mName].ChineseExplain = TD.ChineseExplain;
              // if (TD.EnglishExplain) managerPool[TD.mName].EnglishExplain = TD.EnglishExplain;
              // if (TD.animator) managerPool[TD.mName].animator = TD.animator;
              // if (TD.animatorController) managerPool[TD.mName].animatorController = TD.animatorController;
                managerPool[TD.mName] = TD;
            }
        }

   

        public void Clear(string key)
        {
            if (IsKey(key))
            {
                managerPool.Remove(key);
            }
        }

        public void ClearAll()
        {
            managerPool.Clear();
        }

        public ModelData GetTypeData(string key)
        {
            if (IsKey(key))
            {
                if (managerPool[key].mTarget && managerPool[key].mTarget.activeSelf == false)
                {
                    managerPool[key].mTarget.SetActive(true);
                }
                return managerPool[key];
            }
            return null;
        }
        public List<ModelData> GetAllTypeData()
        {
            List<ModelData> tdList = new List<ModelData>();
            foreach (var item in managerPool)
            {
                tdList.Add(item.Value);
            }
            return tdList;
        }
        private bool IsKey(string key)
        {
            if (managerPool.ContainsKey(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
