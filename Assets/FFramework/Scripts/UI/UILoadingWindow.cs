using UnityEngine;
using UnityEngine.UI;
namespace JKFramework
{
    [UIElement(true, "UI/UILoading", 4)]
    public class UILoadingWindow : UIWindowBase
    {
        [SerializeField]
        private Text progressText;
        [SerializeField]
        private Image fillImage;

        public override void OnShow()
        {
            base.OnShow();
            UpdateProgress(0f);
        }
        protected override void RegisterEventListener()
        {
            base.RegisterEventListener();
            EventManager.AddEventListener<float>("LoadSceneProgress", UpdateProgress);
            EventManager.AddEventListener("LoadSceneSucceed", OnLoadingSceneSucceeded);
        }
        protected override void CancelEventListener()
        {
            base.CancelEventListener();
            EventManager.RemoveEventListener<float>("LoadSceneProgress", UpdateProgress);
            EventManager.RemoveEventListener("LoadSceneSucceed", OnLoadingSceneSucceeded);
        }
        private void OnLoadingSceneSucceeded() => Close();
        /// <summary>
        /// 更新进度
        /// </summary>
        private void UpdateProgress(float progressValue)
        {
            // print($"{(int)(progressValue * 100)}%");
            // print($"progressValue -> {progressValue}");
            progressText.text = $"{(int)(progressValue * 100)}%";
            fillImage.fillAmount = progressValue;
        }
    }
}