// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GJM.Helper;

namespace GJM
{
    /// <summary> 属性模板 【关联资源管理类】 
    /// </summary>
    [RequireComponent(typeof(ResourceManager))]
    public class Model : MonoSingleton<Model>
    {
        [SerializeField]
        private bool isLoadOK = false;
        public bool IsLoadOK
        {
            get { return this.isLoadOK; }
            set { this.isLoadOK = value; }
        }

        [SerializeField]
        private List<ModelData> modelData = new List<ModelData>();
         
        public void AddModeleData(ModelData md)
        {
            modelData.Add(md);
        }
         
        public List<ModelData> GetAllModelData()
        {
            return this.modelData;
        }



        #region 正式模型数据
        /// <summary> 正式数据 </summary>
     
        [SerializeField]private List<ModelData> officialData = new List<ModelData>();

        /// <summary> 添加正式数据 </summary>
        /// <param name="md"></param>
        private void AddOfficialData (ModelData md)
        {
            officialData.Add(md);
        }
        #endregion

        public void ClearModel()
        {
            for (int i = 0; i < modelData.Count; i++)
            {
                Destroy(modelData[i].mTarget.gameObject);
            }

            modelData.Clear();
        }
    }
}
