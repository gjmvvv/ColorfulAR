// 代码编写：郭进明  |  技术分享博客：http://www.cnblogs.com/GJM6/ 

using UnityEngine;
using System.Collections;

namespace GJM
{
    /// <summary>
    /// 
    /// </summary>
    public class TargetAudioBand : MonoBehaviour
    {
        [SerializeField]
        private AudioClip sound;
        [SerializeField]
        private AudioClip chinese;
        [SerializeField]
        private AudioClip english;
        [SerializeField]
        private AudioClip chineseExplain;
        [SerializeField]
        private AudioClip englishExplain;
        [SerializeField]
        private AudioSource audioSource;

        public AudioClip mSound { set { this.sound = value; } }
        public AudioClip mChinese { set { this.chinese = value; } }
        public AudioClip mEnglish { set { this.english = value; } }
        public AudioClip mChineseExplain { set { this.chineseExplain = value; } }
        public AudioClip mEnglishExplain { set { this.englishExplain = value; } }



        public void PlayAudioChinese()
        {
           // Debug.Log("-- 目标 播放声音 中文");

            audioSource.PlayOneShot(chinese);

        }

        public void PlayAudioEnglish()
        {
          //  Debug.Log("-- 目标 播放声音 英文");
            audioSource.PlayOneShot(english);
        }

        public void PlayAudioExplain()
        {
           // Debug.Log("-- 目标 播放声音 说明");
            audioSource.PlayOneShot(chineseExplain);
        }
        /// <summary> 反复播放音效  </summary>
        public void PlayAudioSound()
        {
            audioSource.loop = true;
            audioSource.clip = sound;
            audioSource.Play();
        }

        public void Init()
        {
            //   audioSource = this.gameObject.AddComponent<AudioSource>();
            audioSource = this.GetComponentInChildren<AudioSource>();
            if (chinese) audioSource.clip = chinese;
            if (audioSource) audioSource.playOnAwake = true;
        }
    }
}
