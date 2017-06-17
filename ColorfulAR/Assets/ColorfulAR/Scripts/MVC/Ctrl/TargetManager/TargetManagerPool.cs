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
    public class TargetManagerPool : MonoSingleton<TargetManagerPool>
    {

        #region [ Init ]
        void Start()
        {
            InitComponent();
        }
        #endregion

        #region [ Init Component]
        private void InitComponent()
        {


        }
        #endregion

        #region [ 管理 TargetData Model 总控制池 ]

        private Dictionary<string, TargetData> targetDictionary = new Dictionary<string, TargetData>();

        public List<TargetData> td = new List<TargetData>();
        void Update()
        {
            td.Clear();

            foreach (KeyValuePair<string, TargetData> item in targetDictionary)
            {
                td.Add(item.Value);
            }
        }

        public void AddTargetPool(TargetData targetData)
        {
            if (!targetDictionary.ContainsKey(targetData.mName))
            {
                targetDictionary.Add(targetData.mName, targetData);
                td.Add(targetData);
               // OutTargetPool(targetData.mTarget.transform);
            }
        }

        public void UpdataTargetPool(TargetData targetData)
        {
            if (targetDictionary.ContainsKey(targetData.mName))
            {
                targetDictionary[targetData.mName] = targetData;
                OutTargetPool(targetData.mTarget.transform);
            }
        }

        /// <summary> 刷新目标池 </summary>
        /// <param name="key"></param>
        public void RefreshTargetPool(string key)
        {
            TargetData td = GetTargetData(key);
            if (td.mTarget)
            {
                OutTargetPool(td.mTarget.transform);
            }
        }

        /// <summary> 从池中取得 KEY的TargetData </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TargetData GetTargetData(string key)
        {
            if (targetDictionary.ContainsKey(key))
            {
                return targetDictionary[key];
            }
            else
            {
                Debug.LogError(" *** Target Pool Null :" + key);
                return null;
            }
        }


        public void Clear(string key)
        {
            if (targetDictionary.ContainsKey(key))
            {
                targetDictionary.Remove(key);
            }
        }


        private void OutTargetPool(Transform tf)
        {
            if (tf)
            {
                tf.transform.SetParent(transform);
                if (tf.gameObject.activeSelf == true) { tf.gameObject.SetActive(false); }
            }
        }

        #endregion

        #region [ 管理当前显示的目标 ]
        [SerializeField]
        private List<TargetData> NowTargetData = new List<TargetData>();


        /// <summary> 发现目标 </summary>
        /// <param name="targetName"></param>
        public void FoundTarget(string targetName)
        {
            TargetData td = GetTargetData(targetName);
            if (td != null && !NowTargetData.Contains(td))
                NowTargetData.Add(td);
        }

        /// <summary> 丢失目标 </summary>
        /// <param name="targetName"></param>
        public void LostTarget(string targetName)
        {
            TargetData td = GetTargetData(targetName);
            if (td != null)
                NowTargetData.Remove(td);
        }

        public List<TargetData> GetNowAllTargetData()
        {
            return NowTargetData;
        }
        public void ClearAllNowPool()
        {
            for (int i = 0; i < NowTargetData.Count; i++)
            {
                NowTargetData[i].mTarget.gameObject.SetActive(false);
              

                LostTarget(NowTargetData[i].mName);

            }
        }

        #endregion




    }
}
