// 代码编写：郭进明  |  技术分享博客：http://www.cnblogs.com/GJM6/ 

using UnityEngine;
using System.Collections;
using EasyAR;

namespace GJM
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(TargetMotor))]
    [RequireComponent(typeof(TargetAnimation))]
    [RequireComponent(typeof(TargetAudioBand))]
    [RequireComponent(typeof(DetectionTargetIsCameraRange))]
    public class TargetManager : MonoBehaviour
    {

        #region [ 字段数据 ]
        /// <summary> 目标动画管理类 </summary>
        [SerializeField]
        private TargetAnimation mAnim;
        public Animator SetAnim
        {
            set
            {
                mAnim.SetThisAnimator = value;
            }
        }
        /// <summary> 目标音频管理类 </summary>
        [SerializeField]
        private TargetAudioBand mAudio;

        /// <summary> 目标马达管理类 </summary>
        [SerializeField]
        private TargetMotor mMotor;

        /// <summary> 判断是否在摄像机范围 </summary>
        [SerializeField]
        private DetectionTargetIsCameraRange dtic;

       

        #endregion

        #region [ Init ]
        void Start()
        {
            if (!mAnim)
            {
                mAnim = GetComponent<TargetAnimation>();
            }
            if (!mAudio) mAudio = GetComponent<TargetAudioBand>();
            if (!mMotor) mMotor = GetComponent<TargetMotor>();
            if (!dtic) dtic = GetComponent<DetectionTargetIsCameraRange>();
            mMotor.SetTargetManager = this;
  
        }
        public void InitThisComponent(ModelData md)
        {
            if (!mAnim)
            {
                mAnim = GetComponent<TargetAnimation>();
                mAnim.SetThisAnimator = md.animator;
            }
            if (!mAudio) mAudio = GetComponent<TargetAudioBand>();
            if (!mMotor) mMotor = GetComponent<TargetMotor>();
            mMotor.SetTargetManager = this;
            if (!dtic) dtic = GetComponent<DetectionTargetIsCameraRange>();

            BoxCollider bc = GetComponent<BoxCollider>();
            bc.isTrigger = true;
        }

        public void SetAudioClip(AudioClip c, AudioClip ce, AudioClip e, AudioClip ee, AudioClip s)
        {
            if (!mAudio)
            {
                mAudio = this.gameObject.GetComponent<TargetAudioBand>();
                if (mAudio) mAudio = this.gameObject.AddComponent<TargetAudioBand>();
            }
            if (c) mAudio.mChinese = c;
            if (ce) mAudio.mChineseExplain = ce;
            if (e) mAudio.mEnglish = e;
            if (ee) mAudio.mEnglishExplain = ee;
            if (s) mAudio.mSound = s;
            mAudio.Init();
        }


        #endregion

        #region [ 对外提供的方法 ]
        public void MotorMove(Vector2 v2)
        {
            mMotor.MovementMove(v2.x, v2.y);
        }
        public void MotorRotating(Vector2 v2)
        {
            mMotor.MovementRotating(v2.x, v2.y);
        }

        public delegate void OnMotorInitTransfrom();
        public OnMotorInitTransfrom onInitTransfrom;

        /// <summary> 播放音频 0中文 1英文 2中文说明 </summary>
        public void PlayAudio(int number)
        {
            switch (number)
            {
                case 0: mAudio.PlayAudioChinese(); break;
                case 1: mAudio.PlayAudioEnglish(); break;
                case 2: mAudio.PlayAudioExplain(); break;
                default:
                    mAudio.PlayAudioChinese();
                    break;
            }
        }

        public void StartCorrectTF()
        {
            InvokeRepeating("CorrectTF", 0, 0.5F);

        }
        private void CorrectTF()
        {
            if (Vector3.Distance(this.transform.localPosition, Vector3.zero) == 0) CancelInvoke("CorrectTF");
            this.transform.localPosition = Vector3.zero;
           // Debug.Log(" 脱卡显示， 纠正位置 -");
        }


        /// <summary>  发现目标调用 </summary>
        public void FoundTarget()
        {

        }
        /// <summary> 卸载目标时候调用后 </summary>
        public void LostTarget()
        {

        }

        #endregion

    }
}
