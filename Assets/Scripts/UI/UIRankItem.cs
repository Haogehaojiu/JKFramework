using UnityEngine;
using JKFramework;
using UnityEngine.UI;

[Pool]
public class UIRankItem : MonoBehaviour
{
    [SerializeField]
    private Text rankNumberText;
    [SerializeField]
    private Text userNameText;
    [SerializeField]
    private Text scoreText;
    public void Init(UserData userData, int rankNumber)
    {
        rankNumberText.text = rankNumber.ToString();
        userNameText.text = userData.userName;
        scoreText.text = userData.score.ToString();
    }

    public void Destroy() => this.JKGameObjectPushPool();
}