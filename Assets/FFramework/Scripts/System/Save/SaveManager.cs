using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace JKFramework
{
    /// <summary>
    /// 一个存档的数据
    /// </summary>
    [Serializable]
    public class SaveItem
    {
        public int saveId { get; private set; }
        public DateTime lastSaveTime { get; private set; }

        public SaveItem(int saveId, DateTime lastSaveTime)
        {
            this.saveId = saveId;
            this.lastSaveTime = lastSaveTime;
        }
        public void UpdateTime(DateTime lastSaveTime) => this.lastSaveTime = lastSaveTime;
    }
    /// <summary>
    /// 存档管理器
    /// </summary>
    public static class SaveManager
    {
        /// <summary>
        /// 存档管理器的设置数据
        /// </summary>
        [Serializable]
        private class SaveManagerData
        {
            //当前存档ID
            public int currentId;
            //所有存档列表
            public List<SaveItem> saveItemList = new List<SaveItem>();
        }
        private static SaveManagerData saveManagerData;
        //存放存档文件的目录名称
        private const string saveDirName = "SaveData";
        //存放设置文件的目录名（1、全局数据的保存（分辨率、按键设置）2、存档的设置保存）
        //常规情况下，存档管理器自行维护
        private const string settingDirName = "Setting";
        // 存档文件夹路径
        private static readonly string saveDirPath;
        // 设置文件路径
        private static readonly string settingDirPath;

        //存档中对象的缓存字典
        //[存档ID [文件名称， 实际的对象]]
        private static Dictionary<int, Dictionary<string, object>> cacheDic = new Dictionary<int, Dictionary<string, object>>();

        //初始化
        static SaveManager()
        {
            saveDirPath = $"{Application.persistentDataPath}/{saveDirName}";
            settingDirPath = $"{Application.persistentDataPath}/{settingDirName}";
            //检查路径是否存在
            if (!Directory.Exists(saveDirPath)) Directory.CreateDirectory(saveDirPath);
            //检查路径是否存在
            if (!Directory.Exists(settingDirPath)) Directory.CreateDirectory(settingDirPath);
            //初始化SaveManagerData
            InitSaveManagerData();
        }

        #region 存档设置
        /// <summary>
        /// 获取存档管理器数据
        /// </summary>
        /// <returns></returns>
        private static void InitSaveManagerData()
        {
            saveManagerData = LoadFile<SaveManagerData>($"{saveDirPath}/SaveManagerData");
            if (saveManagerData == null)
            {
                saveManagerData = new SaveManagerData();
                UpdateSaveManagerData();
            }
        }
        /// <summary>
        /// 更新存档管理器数据
        /// </summary>
        public static void UpdateSaveManagerData() => SaveFile(saveManagerData, $"{saveDirPath}/SaveManagerData");
        /// <summary>
        /// 获取所有存档(排序：最新的在后面)
        /// </summary>
        /// <returns></returns>
        public static List<SaveItem> GetAllSaveItem() => saveManagerData.saveItemList;
        /// <summary>
        /// 获取所有存档（排序：最新的在前面）
        /// </summary>
        /// <returns></returns>
        public static List<SaveItem> GetAllSaveItemByCreateTime()
        {
            List<SaveItem> saveItemList = new List<SaveItem>(saveManagerData.saveItemList.Count);
            for(var i = 0; i < saveManagerData.saveItemList.Count; i++) saveItemList.Add(saveManagerData.saveItemList[saveManagerData.saveItemList.Count - (i + 1)]);
            return saveItemList;
        }
        /// <summary>
        /// 获取所有存档（排序：最新更新的在前面）
        /// </summary>
        /// <returns></returns>
        public static List<SaveItem> GetAllSaveItemByUpdateTime()
        {
            List<SaveItem> saveItemList = new List<SaveItem>(saveManagerData.saveItemList.Count);
            for(var i = 0; i < saveManagerData.saveItemList.Count; i++) saveItemList.Add(saveManagerData.saveItemList[i]);
            OrderByUpdateTimeComparer orderByUpdateTimeComparer = new OrderByUpdateTimeComparer();
            saveItemList.Sort(orderByUpdateTimeComparer);
            return saveItemList;
        }
        private class OrderByUpdateTimeComparer : IComparer<SaveItem>
        {
            public int Compare(SaveItem x, SaveItem y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                var saveIdComparison = x.saveId.CompareTo(y.saveId);
                if (saveIdComparison != 0) return saveIdComparison;
                return x.lastSaveTime.CompareTo(y.lastSaveTime);
            }
            // public int Compare(SaveItem x, SaveItem y)
            // {
            //     if (x.lastSaveTime > y.lastSaveTime) return -1;
            //     return 1;
            // }
        }
        /// <summary>
        /// 获取所有存档（万能解决方案）
        /// </summary>
        /// <param name="orderFunc"></param>
        /// <param name="isDescending"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<SaveItem> GetAllSaveItem<T>(Func<SaveItem, T> orderFunc, bool isDescending = false)
        {
            if (isDescending)
            {
                return saveManagerData.saveItemList.OrderByDescending(orderFunc).ToList();
            } else
            {
                return saveManagerData.saveItemList.OrderBy(orderFunc).ToList();
            }
        }
        #endregion
        #region 关于存档
        /// <summary>
        /// 获取SaveData
        /// </summary>
        /// <param name="id"></param>
        public static SaveItem GetSaveItem(int id)
        {
            for(var i = 0; i < saveManagerData.saveItemList.Count; i++)
                if (saveManagerData.saveItemList[i].saveId == id)
                    return saveManagerData.saveItemList[i];
            return null;
        }
        /// <summary>
        /// 添加一个存档
        /// </summary>
        public static SaveItem CreateSaveItem()
        {
            SaveItem saveItem = new SaveItem(saveManagerData.currentId, DateTime.Now);
            saveManagerData.saveItemList.Add(saveItem);
            saveManagerData.currentId += 1;
            //更新SaveManagerData 写入磁盘
            UpdateSaveManagerData();
            return saveItem;
        }
        /// <summary>
        /// 删除存档
        /// </summary>
        /// <param name="saveId">存档ID</param>
        public static void DeleteSaveItem(int saveId)
        {
            var itemDir = GetSavePath(saveId, false);
            //如果文件夹存在且有效
            //把这个文件夹下的存档文件递归删除
            if (itemDir != null) Directory.Delete(itemDir, true);
            saveManagerData.saveItemList.Remove(GetSaveItem(saveId));
            //移除缓存
            RemoveCache(saveId);
            //更新SaveManagerData 写入磁盘
            UpdateSaveManagerData();
        }
        /// <summary>
        /// 删除存档
        /// </summary>
        /// <param name="saveItem"></param>
        public static void DeleteSaveItem(SaveItem saveItem)
        {
            var dataDir = GetSavePath(saveItem.saveId, false);
            //如果文件夹存在且有效
            //把这个文件夹下的存档文件递归删除
            if (dataDir != null) Directory.Delete(dataDir, true);
            saveManagerData.saveItemList.Remove(saveItem);
            //移除缓存
            RemoveCache(saveItem.saveId);
            //更新SaveManagerData 写入磁盘
            UpdateSaveManagerData();
        }
        #endregion
        #region 关于缓存
        /// <summary>
        /// 设置存档缓存
        /// </summary>
        /// <param name="saveId">存档编号</param>
        /// <param name="saveFileName">存档文件名称</param>
        /// <param name="saveObject">存档对象</param>
        private static void SetCache(int saveId, string saveFileName, object saveObject)
        {
            //缓存字典中是否存在此存档编号
            if (cacheDic.ContainsKey(saveId))
            {
                //这个存档文件夹中事后存在此文件
                if (cacheDic[saveId].ContainsKey(saveFileName)) cacheDic[saveId][saveFileName] = saveObject;
                else cacheDic[saveId].Add(saveFileName, saveObject);
            } else cacheDic.Add(saveId, new Dictionary<string, object> {{saveFileName, saveObject}});
        }
        /// <summary>
        /// 获取存档缓存
        /// </summary>
        /// <param name="saveId">存档编号</param>
        /// <param name="saveFileName">存档文件名称</param>
        /// <typeparam name="T">存档具体类型</typeparam>
        private static T GetCache<T>(int saveId, string saveFileName) where T : class
        {
            // 缓存字典中是否有这个SaveID
            if (cacheDic.ContainsKey(saveId))
            {
                // 这个存档中有没有这个文件
                if (cacheDic[saveId].ContainsKey(saveFileName)) return cacheDic[saveId][saveFileName] as T;
                return null;
            }
            return null;
        }
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="saveId"></param>
        private static void RemoveCache(int saveId) { cacheDic.Remove(saveId); }
        #endregion
        #region 关于对象
        /// <summary>
        /// 保存对象到某个存档中
        /// </summary>
        /// <param name="saveObject">要保存的对象</param>
        /// <param name="saveFileName">保存的文件名称</param>
        /// <param name="saveId">存档编号</param>
        public static void SaveObject(object saveObject, string saveFileName, int saveId = 0)
        {
            //存档所在文件夹的路径
            var dirPath = GetSavePath(saveId);
            //具体对象要保存的路径
            var savePath = $"{dirPath}/{saveFileName}";
            //具体的保存
            SaveFile(saveObject, savePath);
            //更新存档时间
            GetSaveItem(saveId).UpdateTime(DateTime.Now);
            //更新SaveManagerData 写入磁盘
            UpdateSaveManagerData();
            //更新缓存
            SetCache(saveId, saveFileName, saveObject);
        }
        /// <summary>
        /// 保存对象到某个存档中
        /// </summary>
        /// <param name="saveObject">要保存的对象</param>
        /// <param name="saveFileName">保存的文件名称</param>
        /// <param name="saveItem">存档信息</param>
        public static void SaveObject(object saveObject, string saveFileName, SaveItem saveItem)
        {
            //存档所在文件夹的路径
            var dirPath = GetSavePath(saveItem.saveId);
            //具体对象要保存的路径
            var savePath = $"{dirPath}/{saveFileName}";
            //具体的保存
            SaveFile(saveObject, savePath);
            //更新存档时间
            saveItem.UpdateTime(DateTime.Now);
            //更新SaveManagerData 写入磁盘
            UpdateSaveManagerData();
            //更新缓存
            SetCache(saveItem.saveId, saveFileName, saveObject);
        }
        /// <summary>
        /// 保存对象到某个存档中
        /// </summary>
        /// <param name="saveObject">要保存的对象</param>
        /// <param name="saveId">存档的编号</param>
        public static void SaveObject(object saveObject, int saveId = 0) { SaveObject(saveObject, saveObject.GetType().Name, saveId); }
        /// <summary>
        /// 保存对象到某个存档中
        /// </summary>
        /// <param name="saveObject">要保存的对象</param>
        /// <param name="saveItem">存档信息</param>
        public static void SaveObject(object saveObject, SaveItem saveItem) { SaveObject(saveObject, saveObject.GetType().Name, saveItem); }
        /// <summary>
        /// 从某个具体的存档中加载某个对象
        /// </summary>
        /// <param name="saveFileName">文件名称</param>
        /// <param name="saveId">存档编号</param>
        /// <typeparam name="T">要返回的具体类型</typeparam>
        public static T LoadObject<T>(string saveFileName, int saveId = 0) where T : class
        {
            T obj = GetCache<T>(saveId, saveFileName);
            if (obj == null)
            {
                //存档所在文件夹的路径
                var dirPath = GetSavePath(saveId);
                if (dirPath == null) return null;
                //具体对象要保存的路径
                var savePath = $"{dirPath}/{saveFileName}";
                obj = LoadFile<T>(savePath);
                SetCache(saveId, saveFileName, obj);
            }
            return obj;
        }
        /// <summary>
        /// 从某个具体的存档中加载某个对象
        /// </summary>
        /// <param name="saveFileName">文件名称</param>
        /// <param name="saveItem">存档信息</param>
        /// <typeparam name="T">要返回的具体类型</typeparam>
        public static T LoadObject<T>(string saveFileName, SaveItem saveItem) where T : class { return LoadObject<T>(saveFileName, saveItem.saveId); }
        /// <summary>
        /// 从某个具体的存档中加载某个对象
        /// </summary>
        /// <param name="saveId">存档编号</param>
        /// <typeparam name="T">要返回的具体类型</typeparam>
        public static T LoadObject<T>(int saveId = 0) where T : class { return LoadObject<T>(typeof(T).Name, saveId); }
        /// <summary>
        /// 从某个具体的存档中加载某个对象
        /// </summary>
        /// <param name="saveItem">存档信息</param>
        /// <typeparam name="T">要返回的具体类型</typeparam>
        /// <returns></returns>
        public static T LoadObject<T>(SaveItem saveItem) where T : class { return LoadObject<T>(typeof(T).Name, saveItem.saveId); }
        #endregion
        #region 全局数据
        /// <summary>
        /// 加载设置,全局生效，不关乎任何一个存档
        /// </summary>
        public static T LoadSetting<T>(string fileName) where T : class { return LoadFile<T>($"{settingDirPath}/{fileName}"); }
        /// <summary>
        /// 加载设置,全局生效，不关乎任何一个存档
        /// </summary>
        public static T LoadSetting<T>() where T : class { return LoadSetting<T>(typeof(T).Name); }
        /// <summary>
        /// 保存设置，全局生效，不关乎任何一个存档
        /// </summary>
        public static void SaveSetting(object saveObject, string fileName) { SaveFile(saveObject, $"{settingDirPath}/{fileName}"); }
        /// <summary>
        /// 保存设置，全局生效，不关乎任何一个存档
        /// </summary>
        public static void SaveSetting(object saveObject) { SaveSetting(saveObject, saveObject.GetType().Name); }
        #endregion
        #region 工具函数
        private static BinaryFormatter binaryFormatter = new BinaryFormatter();
        /// <summary>
        /// 获取某个存档的路径
        /// </summary>
        /// <param name="saveId">存档ID</param>
        /// <param name="isNeedCreateDir">是否需要创建路径</param>
        private static string GetSavePath(int saveId, bool isNeedCreateDir = true)
        {
            //验证是否存在某个存档
            if (GetSaveItem(saveId) == null) throw new Exception("JKFramework: saveId->存档不存在");

            var saveDir = $"{saveDirPath}/{saveId}";
            //确定文件夹是否存在
            if (!Directory.Exists(saveDir))
            {
                if (isNeedCreateDir) Directory.CreateDirectory(saveDir);
                else return null;
            }
            return saveDir;
        }
        /// <summary>
        /// 保存存档
        /// </summary>
        /// <param name="saveObj">保存的对象</param>
        /// <param name="path">保存的路径</param>
        private static void SaveFile(object saveObj, string path)
        {
            try
            {
                // FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
                FileStream fileStream = null;
                fileStream = File.Open(path, FileMode.Create);
                //二进制的方式把对象写进文件
                binaryFormatter.Serialize(fileStream, saveObj);
                // fileStream.Dispose();
                fileStream.Close();
            } catch(Exception e)
            {
                Debug.Log($"存储失败 -> {e}");
                throw;
            }
        }
        /// <summary>
        /// 加载存档
        /// </summary>
        /// <param name="path">加载路径</param>
        /// <typeparam name="T">加载后要转为的类型</typeparam>
        private static T LoadFile<T>(string path) where T : class
        {
            if (!File.Exists(path)) return null;
            FileStream fileStream = null;
            T obj = null;
            binaryFormatter = new BinaryFormatter();
            // FileStream fileStream = new FileStream(path, FileMode.Open);
            fileStream = File.Open(path, FileMode.Open);
            //将内容解码成对象
            obj = (T)binaryFormatter.Deserialize(fileStream);
            // fileStream.Dispose();
            fileStream.Close();
            return obj;
        }
        #endregion
    }
}