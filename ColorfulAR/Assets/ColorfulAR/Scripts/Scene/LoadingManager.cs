// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using UnityEngine;
using System.Collections;
using GJM.Helper;
using UnityEngine.SceneManagement;

namespace GJM
{
    /// <summary>
    ///  
    /// </summary>
    public class LoadingManager : MonoBehaviour
    {

        private AsyncOperation async; // 异步返回结果
        private uint _nowProcess = 0; // 现在的进度
        private uint toProcess;       // 正在加载的进度

        private UISlider mProgress;   // 进度条 
        [SerializeField]
        private Transform LogoTF;

        [SerializeField]
        private UILabel label;
        private void Start()
        {
            _nowProcess = 0;
            StartCoroutine(LoadScene());
        }
        private void Update()
        {
            if (async == null) { return; }
            if (async.progress < 0.9f) { toProcess = (uint)(async.progress * 100); }
            else { toProcess = 100; }
            if (_nowProcess < toProcess) { _nowProcess++; }
            if (_nowProcess == 100)         //async.isDone应该是在场景被激活时才为true
            { async.allowSceneActivation = true; }

            if (mProgress) mProgress.value = _nowProcess / 100f;
           // Debug.Log(" --- Loading ... ... :" + async.progress * 100);
            if (LogoTF != null) LogoTF.eulerAngles += new Vector3(0, 0, 1f);
        }
        void FixedUpdate()
        {
            if (label)
                label.text = (_nowProcess + "%").ToString();
        }

        private IEnumerator LoadScene()
        {
            if (LoadSceneAsync.Instance.GetContain3DScene)
            {
                async = SceneManager.LoadSceneAsync(LoadSceneAsync.Instance.GetUISceneName);
                SceneManager.LoadSceneAsync(LoadSceneAsync.Instance.GetSceneName, LoadSceneMode.Additive);
            }
            else
            {


               // Debug.Log("Load Scene :" + LoadSceneAsync.Instance.GetUISceneName); 
                async = SceneManager.LoadSceneAsync(LoadSceneAsync.Instance.GetUISceneName);
            }
            async.allowSceneActivation = false;
            yield return async;
        }


    }
}
