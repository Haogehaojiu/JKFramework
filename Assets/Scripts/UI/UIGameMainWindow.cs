using JKFramework;
using UnityEngine;
using UnityEngine.UI;

[UIElement(true, "UI/gameMainWindow", 0)]
public class UIGameMainWindow : UIWindowBase
{
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text bulletNumberText;
    [SerializeField]
    private Image hpFillImage;

    protected override void RegisterEventListener()
    {
        base.RegisterEventListener();
        EventManager.AddEventListener<int, int>("UpdateHealthPoint", UpdateHealthPoint);
        EventManager.AddEventListener<int, int>("UpdateBulletNumber", UpdateBulletNumber);
        EventManager.AddEventListener<int>("UpdateScore", UpdateScore);
    }
    protected override void CancelEventListener()
    {
        base.CancelEventListener();
        EventManager.RemoveEventListener<int, int>("UpdateHealthPoint", UpdateHealthPoint);
        EventManager.RemoveEventListener<int, int>("UpdateBulletNumber", UpdateBulletNumber);
        EventManager.RemoveEventListener<int>("UpdateScore", UpdateScore);
    }
    private void UpdateHealthPoint(int current, int max) { hpFillImage.fillAmount = (float)current / max; }
    private void UpdateBulletNumber(int current, int max) { bulletNumberText.text = $"{current}/{max}"; }
    private void UpdateScore(int num) { scoreText.text = num.ToString(); }
}