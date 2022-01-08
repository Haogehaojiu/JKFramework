using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Video;

namespace JKFramework
{
    /// <summary>
    /// 语言类型
    /// </summary>
    public enum LanguageType
    {
        Chinese = 0,
        English = 1
    }
    /// <summary>
    /// 具体语言内容的接口
    /// </summary>
    public interface ILanguageContent
    {
    }
    [Serializable]
    public class L_Text : ILanguageContent
    {
        public string content;
    }
    [Serializable]
    public class L_Image : ILanguageContent
    {
        public Sprite content;
    }
    [Serializable]
    public class L_Video : ILanguageContent
    {
        public VideoClip content;
    }
    [Serializable]
    public class L_Audio : ILanguageContent
    {
        public AudioClip content;
    }
    /// <summary>
    /// 本地化数据
    /// 图片，不同语言下，不同的Sprite
    /// </summary>
    public class LocalizationModel
    {
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine, KeyLabel = "语言类型", ValueLabel = "值")]
        [SerializeField]
        public Dictionary<LanguageType, ILanguageContent> contentDic = new Dictionary<LanguageType, ILanguageContent>
        {
            {
                LanguageType.Chinese, new L_Text()
            },
            {
                LanguageType.English, new L_Text()
            },
        };
    }
    [CreateAssetMenu(fileName = "LocalizationSetting", menuName = "JKFramework/Localization")]
    public class LocalizationSetting : ConfigBase
    {
        //包：UI还是Player、敌人的配置之类
        //【包名称，【内容名称，具体值（不同语言，不同设置）】】
        [SerializeField]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine, KeyLabel = "数据包", ValueLabel = "配置")]
        private Dictionary<string, Dictionary<string, LocalizationModel>> dataPackage;
        /// <summary>
        /// 获取本地化内容
        /// </summary>
        /// <param name="dataPackageName">数据包名称</param>
        /// <param name="contentName">内容名称</param>
        /// <param name="languageType">语言类型</param>
        /// <typeparam name="T">具体返回的类型</typeparam>
        public T GetContent<T>(string dataPackageName, string contentName, LanguageType languageType) where T : class, ILanguageContent => dataPackage[dataPackageName][contentName].contentDic[languageType] as T;
    }
}