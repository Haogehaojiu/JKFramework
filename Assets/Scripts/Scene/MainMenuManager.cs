using JKFramework;

/// <summary>
/// 菜单场景的总管理器
/// 负责调度整个场景的逻辑、流程
/// </summary>
public class MainMenuManager : LogicManagerBase<MainMenuManager>
{
    private void Start()
    {
        // GameManager.Instance.ContinueGame();
        UIManager.Instance.CloseAll();
        //播放音乐
        AudioManager.Instance.PlayBgAudio("Audio/BG/Menu");
        //显示主菜单窗口
        UIManager.Instance.Show<UIMainMenuWindow>();
    }
    protected override void RegisterEventListener()
    {
        EventManager.AddEventListener<string>("CreateNewSaveAndEnterGame", CreateNewSaveAndEnterGame);
        EventManager.AddEventListener<SaveItem, UserData>("EnterGame", EnterGame);
    }
    protected override void CancelEventListener()
    {
        EventManager.RemoveEventListener<string>("CreateNewSaveAndEnterGame", CreateNewSaveAndEnterGame);
        EventManager.RemoveEventListener<SaveItem, UserData>("EnterGame", EnterGame);
    }
    /// <summary>
    /// 创建一个存档，并进入游戏
    /// </summary>
    public void CreateNewSaveAndEnterGame(string userName)
    {
        //建立存档
        SaveItem saveItem = SaveManager.CreateSaveItem();
        //创建首次存档时的用户数据（金币等）
        UserData userData = new UserData(userName);
        SaveManager.SaveObject(userData, saveItem);

        EventManager.EventTrigger("UpdateSaveItem");
        EventManager.EventTrigger("UpdateRankItem");

        //进入游戏
        EnterGame(saveItem, userData);
    }
    private void EnterGame(SaveItem saveItem, UserData userData)
    {
        //显示加载面板
        UIManager.Instance.Show<UILoadingWindow>();
        GameManager.Instance.EnterGame(saveItem, userData);
        SceneManager.LoadSceneAsync("Game");
    }
}