// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using UnityEngine;
using System.Collections;


namespace GJM
{
    /// <summary>
    ///  实现识别控制器
    /// </summary>
    public class EasyTargetController : MonoBehaviour
    {
        private View view;
        private StatusManager statusM;
        private TargetManagerPool targetPool;
        void Start()
        {
            if (!targetPool) targetPool = TargetManagerPool.Instance;
            if (!view) view = View.Instance;
            statusM = GetComponent<StatusManager>();
            InitEasyTargetManager();

        }

        #region 识别控制 EasyTargetManager
        /// <summary> 脱卡后模型的最佳位置 </summary>
        [SerializeField]
        private Transform TakeOffTheCardTF;

        /// <summary> 识别物体委托初始化 </summary>
        private void InitEasyTargetManager()
        {
            view.loadTargetDelegate = this.LoadTarget;
            view.unloadTargetDelegate = this.UnLoadTarget;
            view.foundTargetDelegate = this.FoundTarget;
            view.lostTargetDelegate = this.LostTarget;
        }


        /// <summary> 显示 目标 </summary>
        /// <param name="targetData"></param>
        /// <param name="isTakeOffTheCard">是否脱卡 [True 代表脱卡 False 代表未脱卡]</param>
        private void FoundTarget(TargetData targetData, bool isTakeOffTheCard)
        {
            GameObject target = targetData.mTarget;
            if (target)
            {
                if (isTakeOffTheCard) // 脱卡显示
                {
                    FoundTarget(targetData, false);
                }
                else  // 不是脱卡
                {
                    if (target.transform.parent != targetData.mEasyTargetManager.transform)
                        target.transform.SetParent(targetData.mEasyTargetManager.transform);
                    target.transform.position = Vector3.zero;
                    target.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    target.transform.localScale = new Vector3(1, 1, 1);
                    if (target.gameObject.activeSelf == false)
                    { target.gameObject.SetActive(true); }
                    targetPool.FoundTarget(targetData.mName); // 发现目标 添加到发现目标的池中
                }
            }
        }

        /// <summary> 隐藏目标 </summary>
        /// <param name="targetData">目标数据</param>
        /// <param name="isTakeOffTheCard">是否脱卡 [True 代表脱卡 False 代表未脱卡]</param>
        private void HideTarget(TargetData targetData, bool isTakeOffTheCard)
        {
            if (isTakeOffTheCard)
            {
                targetData.mTarget.transform.SetParent(TakeOffTheCardTF);

                targetData.mTarget.transform.localPosition = new Vector3(0, 0, 0);
                targetData.mTarget.transform.localRotation = new Quaternion(0, 0, 0, 0);
                targetData.mTarget.transform.localScale = new Vector3(1, 1, 1);

                //  targetData.mTarget.gameObject.SetActive(false);
                //   targetPool.LostTarget(targetData.mName); // 从当前目标池中清除
                targetData.mTargetManager.StartCorrectTF(); // 开始纠正位置
            }
            else
            {
                targetData.mTarget.transform.SetParent(targetData.mEasyTargetManager.transform);
                targetData.mTarget.gameObject.SetActive(false);
                targetPool.LostTarget(targetData.mName); // 从当前目标池中清除

            }

        }

        /// <summary> 加载目标时候调用 </summary>
        /// <param name="targetName">目标名称</param>
        /// <param name="go">目标对象</param>
        private void LoadTarget(string targetName)
        {
          //  Debug.Log(" -- 委托实现 加载目标时候调用 - TargetName:" + targetName);
            // TargetData targetData = targetPool.GetTargetData(targetName);
            // HideTarget(targetData, false);


        }
        /// <summary> 停止程序 卸载 目标资源 </summary>
        /// <param name="targetName"></param>
        private void UnLoadTarget(string targetName)
        {
           // Debug.Log(" -- 委托实现 停止程序卸载目标资源 - TargetName:" + targetName);
            targetPool.RefreshTargetPool(targetName);
        }

        /// <summary> 失去了目标时候调用 </summary>
        /// <param name="targetName"></param>
        private void LostTarget(string targetName)
        {


           // Debug.Log(" -- 委托实现 失去了目标时候调用 - TargetName:" + targetName);
            view.IsEasyLableUIExokain(false, "", "");
            TargetData td = targetPool.GetTargetData(targetName);
            if (td == null) { Debug.LogError(" -- 没有从池中找到 失去目标：" + targetName); return; }

            switch (statusM.mDiscerns)
            {
                case EnumDiscernStatus.不脱卡:
                    HideTarget(td, false);
                    Ctrl.Instance.SetFxSummoner(this.transform, false);
                    break;
                case EnumDiscernStatus.脱卡:
                    HideTarget(td, true);
                    Ctrl.Instance.SetFxSummoner(this.transform, false);
                    break;
            }
        }

        /// <summary> 发现目标时候调用 </summary>
        /// <param name="targetName">发现的目标组件</param>
        private void FoundTarget(string targetName)
        {
         //   Debug.Log(" -- 委托实现 发现目标时候调用 - TargetName:" + targetName);
            Ctrl.Instance.SetFxSummoner(this.transform, true );

            TargetData targetData = targetPool.GetTargetData(targetName);

            if (targetData.mTargetManager)
            {
                if (targetData.mTarget == null)
                {
                    GameObject loadGo = Resources.Load<GameObject>("Prefabs/" + targetName);
                    loadGo = Instantiate(loadGo, targetData.mTargetManager.transform) as GameObject;
                    loadGo.transform.eulerAngles = new Vector3(0, 180, 0);
                    targetData.mTargetManager.SetAnim = loadGo.GetComponent<Animator>();
                    targetData.mTarget = targetData.mTargetManager.gameObject;

                    print(
                    " C " + Resources.Load<AudioClip>("AudioClip/C/" + targetData.mName + "_C") +
                    " CC " + Resources.Load<AudioClip>("AudioClip/CC/" + targetData.mName + "_CC") +
                    " E " + Resources.Load<AudioClip>("AudioClip/E/" + targetData.mName + "_E") +
                    " EC " + Resources.Load<AudioClip>("AudioClip/EC/" + targetData.mName + "_EC") +
                    " S " + Resources.Load<AudioClip>("AudioClip/S/" + targetData.mName + "_S"));


                    targetData.mTargetManager.SetAudioClip(
                     Resources.Load<AudioClip>("AudioClip/C/" + targetData.mName + "_C"),
                     Resources.Load<AudioClip>("AudioClip/CC/" + targetData.mName + "_CC"),
                     Resources.Load<AudioClip>("AudioClip/E/" + targetData.mName + "_E"),
                     Resources.Load<AudioClip>("AudioClip/EC/" + targetData.mName + "_EC"),
                     Resources.Load<AudioClip>("AudioClip/S/" + targetData.mName + "_S"));

                }



            }
            // view.IsEasyLableUIExokain(true, "", targetName);
            view.IsEasyLableUIExokain(true, "", "");
            TargetData td = null;
            switch (statusM.mDiscerns)
            {
                case EnumDiscernStatus.不脱卡:
                    td = targetPool.GetTargetData(targetName);
                    if (td != null)
                        FoundTarget(td, false);

                    td.mTarget.SetActive(false);
                    StartCoroutine(HangTime(0.5f, td.mTarget));
                    break;
                case EnumDiscernStatus.脱卡:

                    targetPool.ClearAllNowPool();
                    td = targetPool.GetTargetData(targetName);
                    if (td != null)
                        FoundTarget(td, true);

                    td.mTarget.SetActive(false);
                    StartCoroutine(HangTime(0.5f, td.mTarget));
                    break;
            }

        }
        #endregion

        private IEnumerator HangTime(float time, GameObject obj)
        {
            yield return new WaitForSeconds(time);
            obj.SetActive(true);
            Ctrl.Instance.SetFxSummoner(this.transform, false);
        }

    }
}
