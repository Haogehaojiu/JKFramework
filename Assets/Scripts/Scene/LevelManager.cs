using JKFramework;
using UnityEngine;

public class LevelManager : LogicManagerBase<LevelManager>
{
    private int score;
    public Transform TempGameObjectTransform;

    private bool isActiveSettingWindow;
    public int Score {
        get => score;
        set {
            score = value;
            EventManager.EventTrigger("UpdateScore", score);
        }
    }

    private void Start()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.Show<UIGameMainWindow>();
        Score = 0;
        PlayerController.Instance.Init(ConfigManager.Instance.GetConfig<PlayerConfig>("Player"));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isActiveSettingWindow = !isActiveSettingWindow;
            //暂停游戏，打开设置窗口
            if (isActiveSettingWindow)
            {
                GameManager.Instance.PauseGame();
                UIManager.Instance.Show<UISettingWindow>().InitForGame();
            }
            //继续游戏，关闭窗口
            else
            {
                UIManager.Instance.Close<UISettingWindow>();
                GameManager.Instance.ContinueGame();
            }
        }
    }
    protected override void RegisterEventListener()
    {
        EventManager.AddEventListener("MonsterDie", OnMonsterDie); 
        EventManager.AddEventListener("GameOver", OnGameOver);
    }
    protected override void CancelEventListener()
    {
        EventManager.RemoveEventListener("MonsterDie"); 
        EventManager.RemoveEventListener("GameOver", OnGameOver);
    }
    private void OnMonsterDie() => ++Score;
    private void OnGameOver()
    {
        //更新存档
        GameManager.Instance.UpdateScore(score);
        //打开结果页面
        UIManager.Instance.Show<UIResultWindow>().Init(score);
    }
}