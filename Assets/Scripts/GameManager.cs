using UnityEngine;
using JKFramework;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public SaveItem saveItem { get; private set; }
    public UserData userData { get; private set; }
    public UserSetting userSetting { get; private set; }
    private void Start()
    {
        //如果没有获取到用户设置，意味着首次进入游戏或者用户删除了设置文件
        userSetting = SaveManager.LoadSetting<UserSetting>();
        if (userSetting == null) userSetting = new UserSetting(0.7f, 0.7f, 1f, LanguageType.English);
        SaveManager.SaveSetting(userSetting);
        AudioManager.Instance.GlobalVolume = userSetting.globalVolume;
        AudioManager.Instance.BgVolume = userSetting.bgVolume;
        AudioManager.Instance.EffectVolume = userSetting.effectVolume;
        LocalizationManager.Instance.CurrentLanguageType = userSetting.languageType;
    }
    /// <summary>
    /// 进入游戏
    /// </summary>
    public void EnterGame(SaveItem saveItem, UserData userData)
    {
        //全局保存当前的存档和用户数据
        this.saveItem = saveItem;
        this.userData = userData;
    }
    /// <summary>
    /// 暂停游戏
    /// </summary>
    public void PauseGame() => Time.timeScale = 0;
    /// <summary>
    /// 继续游戏
    /// </summary>
    public void ContinueGame() => Time.timeScale = 1;
    /// <summary>
    /// 再来一局
    /// </summary>
    public void PlayAgain()
    {
        //显示加载面板
        UIManager.Instance.Show<UILoadingWindow>();
        // EnterGame(saveItem, userData);
        SceneManager.LoadSceneAsync("Game");
    }
    /// <summary>
    /// 更新分数
    /// </summary>
    /// <param name="score"></param>
    public void UpdateScore(int score)
    {
        //现在分数是否大于本存档的分数,是的话才保存
        if (score > userData.score)
        {
            //保存分数
            userData.score = score;
            SaveManager.SaveObject(userData, saveItem);
            //排名可能有变化
            EventManager.EventTrigger("UpdateSaveItem");
            EventManager.EventTrigger("UpdateRankItem");
        }
    }
}