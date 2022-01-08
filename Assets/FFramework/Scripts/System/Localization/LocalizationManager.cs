using Sirenix.OdinInspector;
using UnityEngine;
namespace JKFramework
{
    /// <summary>
    /// 本地化管理器
    /// 持有本地化资源
    /// 提供获取本地化内容函数
    /// </summary>
    public class LocalizationManager : ManagerBase<LocalizationManager>
    {
        //本地化配置资源
        public LocalizationSetting localizationSetting;
        [SerializeField]
        [OnValueChanged("UpdateLanguageType")]
        private LanguageType currentLanguageType;
        public LanguageType CurrentLanguageType {
            get => currentLanguageType;
            set {
                currentLanguageType = value;
                UpdateLanguageType();
            }
        }
        /// <summary>
        /// 获取当前语言类型下的内容
        /// </summary>
        /// <param name="dataPackageName">包名称</param>
        /// <param name="contentName">内容名称</param>
        /// <typeparam name="T">内容类型</typeparam>
        /// <returns></returns>
        public T GetContent<T>(string dataPackageName, string contentName) where T : class, ILanguageContent => localizationSetting.GetContent<T>(dataPackageName, contentName, currentLanguageType);
        /// <summary>
        /// 更新语言
        /// </summary>
        private void UpdateLanguageType()
        {
#if UNITY_EDITOR
            GameRoot.InitForEditor();
#endif
            //触发更新语言事件
            EventManager.EventTrigger("UpdateLanguageType");
        }
    }
}