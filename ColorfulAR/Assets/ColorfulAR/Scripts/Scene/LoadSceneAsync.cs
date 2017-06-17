// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using GJM.Helper;

namespace GJM.Helper
{
    /// <summary>
    ///  加载场景
    /// </summary>
    public class LoadSceneAsync : MonoSingleton<LoadSceneAsync>
    {
        /// <summary> 场景加载的返回值 </summary>
        private AsyncOperation async;
        private string LoadUIName = "";
        private string LoadSceneName = "";
        private bool Contain3DScene = false;
        public string GetUISceneName { get { return this.LoadUIName; } }
        public string GetSceneName { get { return this.LoadSceneName; } }
        public bool GetContain3DScene { get { return this.Contain3DScene; } }



        /// <summary> 加载进度条场景 </summary>
        IEnumerator Load()
        {

            async = SceneManager.LoadSceneAsync(Configuration.GetContent("Scene", "LoadLogin"));
            yield return new WaitForSeconds(0.1f);
            yield return async;
        }

        /// <summary> 加载场景 </summary>
        public IEnumerator LoadScene(string uiScene)
        {
            LoadUIName = uiScene;
            yield return StartCoroutine(Load());
        }


    

        /// <summary> 累加3D场景 </summary>
        /// <param name="uiScene"></param>
        /// <param name="scene"></param>
        /// <returns></returns>
        public IEnumerator LoadScene(string uiScene, string scene)
        {
            Contain3DScene = true;
            LoadUIName = uiScene;
            LoadSceneName = scene;
            yield return StartCoroutine(Load());
        }
    }
}
