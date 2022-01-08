using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace JKFramework
{
    /// <summary>
    /// 游戏中所有非框架的配置，游戏运行时只有一个
    /// </summary>
    [CreateAssetMenu(fileName = "ConfigSetting", menuName = "JKFramework/ConfigSetting")]
    public class ConfigSetting : ConfigBase
    {
        /// <summary>
        /// 所有配置的容器
        /// [配置类型的名称， [id，具体配置]]
        /// </summary>
        [DictionaryDrawerSettings(KeyLabel = "类型", ValueLabel = "列表")]
        public Dictionary<string, Dictionary<int, ConfigBase>> ConfigDictionary;

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <param name="id">id</param>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns></returns>
        public T GetConfig<T>(string configTypeName, int id) where T : ConfigBase
        {
            //检查类型
            if (!ConfigDictionary.ContainsKey(configTypeName)) throw new System.Exception($"{GetType()}/{nameof(ConfigSetting)}/{nameof(ConfigDictionary)}不包含此类型");
            if (!ConfigDictionary[configTypeName].ContainsKey(id)) throw new System.Exception($"{GetType()}/{nameof(ConfigSetting)}/{nameof(ConfigDictionary)}/{nameof(configTypeName)}不包含此id");
            return ConfigDictionary[configTypeName][id] as T;
        }
    }
}