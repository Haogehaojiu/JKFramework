using JKFramework;
using UnityEngine;
using UnityEngine.UI;

[UIElement(true, "UI/resultWindow", 4)]
public class UIResultWindow : UIWindowBase
{
    [SerializeField]
    private Button playAgainButton;
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private Text playAgainButtonText;
    [SerializeField]
    private Text backButtonText;
    [SerializeField]
    private Text scoreText;
    private const string LocalizationSetPackName = "UIResultWindow";

    public override void Init()
    {
        playAgainButton.onClick.AddListener(PlayAgainButtonClick);
        backButton.onClick.AddListener(BackButtonClick);
    }
    protected override void OnUpdateLanguage()
    {
        playAgainButtonText.JKLocalSet(LocalizationSetPackName, "playAgain");
        backButtonText.JKLocalSet(LocalizationSetPackName, "back");
    }
    public void Init(int score)
    {
        scoreText.text = $"{LocalizationManager.Instance.GetContent<L_Text>(LocalizationSetPackName, "score").content}{score}";
        GameManager.Instance.PauseGame();
    }
    /// <summary>
    /// 回主菜单
    /// </summary>
    private void BackButtonClick()
    {
        GameManager.Instance.ContinueGame();
        SceneManager.LoadScene("MainMenu");
    }
    /// <summary>
    /// 再来一局
    /// </summary>
    private void PlayAgainButtonClick()
    {
        //再次加载当前场景，进行游戏
        GameManager.Instance.ContinueGame();
        GameManager.Instance.PlayAgain();
    }
}