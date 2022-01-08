using UnityEditor;
using UnityEngine;

namespace JKFramework
{
    public class GameRoot : SingletonMonoBehaviour<GameRoot>
    {
        /// <summary>
        /// 框架设置
        /// </summary>
        [SerializeField]
        private GameSetting gameSetting;
        public GameSetting GameSetting => gameSetting;
        protected override void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            base.Awake();
            DontDestroyOnLoad(gameObject);
            InitManagers();
        }
        private void InitManagers()
        {
            ManagerBase[] managerBases = GetComponents<ManagerBase>();
            for(var i = 0; i < managerBases.Length; i++) managerBases[i].Init();
        }
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        public static void InitForEditor()
        {
            //当前是否要进行播放或准备播放中
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (Instance == null && GameObject.Find("GameRoot") != null)
            {
                Instance = GameObject.Find("GameRoot").GetComponent<GameRoot>();
                //清空事件
                EventManager.Clear();
                Instance.InitManagers();
                Instance.gameSetting.InitForEditor();
                //场景中的所有窗口都进行一次show -> OnUpdateLanguage() && RegisterEventListener();
                UIWindowBase[] uiWindowBases = Instance.transform.GetComponentsInChildren<UIWindowBase>();
                foreach(UIWindowBase uiWindowBase in uiWindowBases) uiWindowBase.OnShow();
            }
        }
#endif
    }
}