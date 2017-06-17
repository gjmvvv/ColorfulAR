// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using UnityEngine;
using System.Collections;


namespace GJM
{
    /// <summary>
    ///  动画播放
    /// </summary>
    public class TargetAnimation : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        public Animator SetThisAnimator
        {
            set
            {
                if (value)
                {
                    this.animator = value;
                }
                else
                {
                    Debug.LogError(" Set This Animator Null " + this.name);
                }
            }
        }

        bool touchAnim = false;


        /// <summary>
        /// 播放随机动画
        /// </summary>
        public void PlayRandomAnim()
        {
            if (touchAnim)
            { 
                touchAnim = false;
            }
            else
            {
               // NGUIDebug.Log(" 播放动画 同时循环播放特性");
                this.GetComponent<TargetAudioBand>().PlayAudioSound();
                touchAnim = true;
            }
       
            animator.SetBool("touch", touchAnim);

        }
    }
}
