using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
namespace JKFramework
{
    /// <summary>
    /// UI提示
    /// </summary>
    public class UITips : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private Text infoText;
        private Queue<string> tipsQueue;
        private static readonly int CanTip = Animator.StringToHash("canTip");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            infoText = transform.Find("BG/Text").GetComponent<Text>();
            tipsQueue = new Queue<string>();
        }
        /// <summary>
        /// 添加提示
        /// </summary>
        /// <param name="info"></param>
        public void AddTip(string info)
        {
            tipsQueue.Enqueue(info);
            if (tipsQueue.Count > 0 && !animator.GetBool(CanTip)) ShowTip();
        }
        public void ShowTip()
        {
            infoText.text = tipsQueue.Dequeue();
            animator.SetBool(CanTip, true);
        }
        #region 动画事件
        [UsedImplicitly]
        private void EndTip()
        {
            animator.SetBool(CanTip, false);
            tipsQueue.Clear();
        }
        #endregion
    }
}