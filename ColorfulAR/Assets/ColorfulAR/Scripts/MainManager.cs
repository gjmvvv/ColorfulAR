// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using UnityEngine;
using System.Collections;
using GJM.Helper;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace GJM
{
    /// <summary>
    ///  
    /// </summary>
    public class MainManager : MonoBehaviour
    {
        private readonly string config = "config.txt";
        private AsyncOperation async;
        private AudioSource audio;

        /// <summary> 是否解锁 </summary>
        [SerializeField]
        private bool isDeblocking = false;

        public bool IsDeblocking
        {
            set
            {
                PlayerPrefs.SetString("Deblocking", value.ToString());
                isDeblocking = value;
            }
            get
            {
                if (isDeblocking == true) return isDeblocking;
                try
                {
                    string data = PlayerPrefs.GetString("Deblocking");
                    isDeblocking = bool.Parse(data);
                    return isDeblocking;
                }
                catch (System.Exception)
                {
                    return isDeblocking; 
                }
               
            
            }
        }


        public bool isInitData = false;
        private IEnumerator Start()
        {
            if (isInitData)
            {
                PlayerPrefs.DeleteAll();          
            }
            DontDestroyOnLoad(gameObject);
            audio = GetComponent<AudioSource>();
            Configuration.LoadConfig(config);

            while (!Configuration.IsDone) { yield return null; }

            string uiscene = Configuration.GetContent("Scene", "LoadGameStart");
            StartCoroutine(LoadSceneAsync.Instance.LoadScene(uiscene));



        }

        public void ControlBackAudio(bool isBool)
        {
            audio.enabled = isBool;

        }


    }
}
