using UnityEngine;
using Sirenix.OdinInspector;
namespace JKFramework
{
    /// <summary>
    /// UI元素
    /// </summary>
    public class UIElement
    {
        [LabelText("是否需要缓存")]
        public bool isCache;
        [LabelText("预制体")]
        public GameObject prefab;
        [LabelText("UI层级")]
        public int layerLevel;
        /// <summary>
        /// 这个元素的窗口对象
        /// </summary>
        public UIWindowBase objInstance;
    }
}