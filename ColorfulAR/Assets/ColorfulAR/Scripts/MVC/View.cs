// 程序作者：郭进明 |  技术分析博客 http://www.cnblogs.com/GJM6/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using GJM.Helper;


namespace GJM
{

    /// <summary>
    ///  
    /// </summary>
    public class View : MonoSingleton<View>
    {


        private void Awake()
        {
            ctrl = FindObjectOfType<Ctrl>();
            statusManager = FindObjectOfType<StatusManager>();
            uiManager = FindObjectOfType<UIManager>();
            mExplainUI = Global.FindChild<UILabel>(transform, "Explain");
            UIEasyChineExplainLable = Global.FindChild<UILabel>(transform, "EasyExplain_C");
            UIEasyEnglishExplainLable = Global.FindChild<UILabel>(transform, "EasyExplain_E");
            mVideoSlider = Global.FindChild<UISlider>(transform, "Progress Bar");
            mVideoSlider.gameObject.SetActive(false);

        }

        #region 初始化字段
        private UIManager uiManager = null;

        private Ctrl ctrl;
        /// <summary> 说明 UI </summary>

        private UILabel mExplainUI = null;
        /// <summary> 识别模型UI 提示说明 中文</summary>
        private UILabel UIEasyChineExplainLable = null;
        /// <summary> 识别模型UI 提示说明 英文</summary> 
        private UILabel UIEasyEnglishExplainLable = null;


        /// <summary> 控制（移动,旋转） 识别（不脱卡,脱卡）  状态管理 </summary>
        private StatusManager statusManager = null;
        /// <summary> 控制（移动,旋转） 识别（不脱卡,脱卡）  状态管理 </summary>
        public StatusManager mStatusManager
        {
            get
            {
                if (statusManager)
                {
                    return this.statusManager;
                }
                else
                {
                    Debug.LogError(" --- View StatusManager Null");
                    statusManager = FindObjectOfType<StatusManager>();
                    return this.statusManager;
                }
            }
        }
        #endregion

        #region 创建物体之后 Target 管理 从创建吧自身添加到Ctrl中   EasyTargetController 实现委托

        /// <summary> 发现了目标时候调用 </summary>
        /// <param name="targetName">目标名称</param>
        /// <param name="tf">识别挂载的物体</param>
        public delegate void FoundTargetDelegate(string targetName);
        /// <summary> 发现了目标时候调用 </summary>
        public FoundTargetDelegate foundTargetDelegate;

        /// <summary> 失去了目标时候调用 </summary>
        /// <param name="targetName">目标名称</param>
        public delegate void LostTargetDelegate(string targetName);
        /// <summary> 失去了目标时候调用 </summary>
        public LostTargetDelegate lostTargetDelegate;

        /// <summary> 加载目标时候调用 </summary>
        /// <param name="targetName">目标名称</param>
        public delegate void LoadTargetDelegate(string targetName);
        /// <summary> 加载目标时候调用 </summary>
        public LoadTargetDelegate loadTargetDelegate;

        /// <summary> 卸载目标时候调用 </summary>
        /// <param name="targetName">目标名称</param>
        public delegate void UnloadTargetDelegate(string targetName);
        /// <summary> 卸载目标时候调用 </summary>
        public UnloadTargetDelegate unloadTargetDelegate;

        #endregion

        #region 按钮事件控制
        /// <summary> 按钮 控制返回 委托 </summary>
        public delegate void ButtonReturnDelegate();

        /// <summary> 按钮 控制翻转摄像头 委托 </summary>
        public delegate void ButtonOverturnCameraDeleagte();

        /// <summary> 按钮 控制播放音频 中文 委托 </summary>
        public delegate void ButtonPlayAudioChineseDelegate();

        /// <summary> 按钮 控制播放音频 英文 委托 </summary>
        public delegate void ButtonPlayAudioEnglishDelegate();

        /// <summary> 按钮 控制播放音频 说明 委托 </summary>
        public delegate void ButtonPlayAudioExplainDelegate();

        /// <summary> 按钮 控制拍照 委托 </summary>
        public delegate void ButtonPhotographDelegate();

        /// <summary> 按钮 控制录像 委托 </summary>
        public delegate void ButtonVideoDelegate();

        /// <summary> 按钮 点击背景 委托 </summary>
        public delegate void BackagerOnClickDelegate();

        /// <summary> 按钮 多指背景 委托 </summary>
        public delegate void BackagerOnDoubleDelegate();
        /// <summary>  按钮 放大 </summary>
        public delegate void OnBigDelegate();
        /// <summary>  按钮 缩小 </summary>
        public delegate void OnSmallDelegate();

        /// <summary> 按钮 控制返回 实现委托 </summary>
        public ButtonReturnDelegate buttonReturnDelegate;

        /// <summary> 按钮 控制翻转摄像头 实现委托 </summary>
        public ButtonOverturnCameraDeleagte buttonOverturnCameraDeleagte;

        /// <summary> 按钮 控制播放音频 中文 实现委托 </summary>
        public ButtonPlayAudioChineseDelegate buttonPlayAudioChineseDelegate;

        /// <summary> 按钮 控制播放音频 英文 实现委托 </summary>
        public ButtonPlayAudioEnglishDelegate buttonPlayAudioEnglishDelegate;

        /// <summary> 按钮 控制播放音频 说明 实现委托 </summary>  
        public ButtonPlayAudioExplainDelegate buttonPlayAudioExplainDelegate;

        /// <summary> 按钮 控制拍照 实现委托 </summary>    
        public ButtonPhotographDelegate buttonPhotographDelegate;

        /// <summary> 按钮 控制录像 实现委托 </summary>
        public ButtonVideoDelegate buttonVideoDelegate;

        /// <summary> 按钮 点击背景 实现委托 </summary>
        public BackagerOnClickDelegate backagerOnClickDelegate;

        public BackagerOnDoubleDelegate backagerOnDoubleDelegate;
        /// <summary> 按钮 缩小 </summary>
        public OnSmallDelegate onSmallDelegate;
        /// <summary> 按钮 放大 </summary>
        public OnBigDelegate onBigDelegate;
      
        #endregion

        #region 对控制层提供的方法 （录像使用）

        private float number = 0;
        /// <summary> 进度条 </summary>
        [SerializeField]
        private UISlider mVideoSlider = null;

        /// <summary> 进度条控制 </summary>
        /// <param name="time">等待时间后关闭</param>
        public IEnumerator IsVisibleViewUIRrogressBar(float time)
        {
            number = 0;
            if (mVideoSlider) mVideoSlider.gameObject.SetActive(true);        // 显示录制进度条   
            while (number <= time)
            {  // 等待录制 10 秒
                number += 0.12f;
                yield return new WaitForSeconds(0.1f);
                if (mVideoSlider) mVideoSlider.value = number / 10;
                Debug.Log("--- View 进度条 等待时间：" + number + " Time:" + Time.time);
            }
            if (mVideoSlider) mVideoSlider.gameObject.SetActive(false);     // 关闭进度条         
        }

        /// <summary> 控制显示UI </summary>
        /// <param name="visible"></param>
        public void IsVisibleUI(bool visible)
        {
            uiManager.IsVisbleView(visible);
        }


        /// <summary>  显示/隐藏  UI Lable 说明  </summary>
        /// <param name="visible"> True显示说明 false隐藏说明 </param>
        /// <param name="message">说明消息</param>
        public void IsVisibleViewUIExplain(bool visible, string message)
        {
            mExplainUI.text = message;
            mExplainUI.gameObject.SetActive(visible);
        }

        public void IsEasyLableUIExokain(bool visible, string chineMessage, string englishMessage)
        {

            UIEasyChineExplainLable.text = chineMessage;
            UIEasyEnglishExplainLable.text = englishMessage;
            UIEasyChineExplainLable.gameObject.SetActive(visible);
            UIEasyEnglishExplainLable.gameObject.SetActive(visible);
        }
        #endregion



    }
}
