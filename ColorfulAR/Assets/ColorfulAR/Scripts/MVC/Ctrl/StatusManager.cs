// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using UnityEngine;
using System.Collections;


namespace GJM
{
    /// <summary>
    ///  控制/识别 状态管理
    /// </summary>
    public class StatusManager : MonoBehaviour 
    {
   

        /// <summary> 控制状态 【旋转/移动】 </summary>
        [SerializeField]
        private EnumControlStatus mControl = EnumControlStatus.旋转;
        /// <summary> 识别状态 【不脱卡/脱卡】 </summary>
        [SerializeField]
        private EnumDiscernStatus mDiscern = EnumDiscernStatus.不脱卡;

      

        /// <summary> 设置 识别状态 【不脱卡/脱卡】 </summary>
        public EnumControlStatus mControls
        {
            get
            {
                return this.mControl;
            }
            set
            {
                this.mControl = value;
            }
        }
        /// <summary> 设置 控制状态 【旋转/移动】 </summary>
        public EnumDiscernStatus mDiscerns
        {
            get
            {
                return this.mDiscern;
            }
            set
            {
                this.mDiscern = value;
            }
        }
    

   
    }
}
