using JKFramework;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/Player config", fileName = "PlayerConfig")]
public class PlayerConfig : ConfigBase
{
    [LabelText("移动速度")]
    public float moveSpeed = 4f;
    [LabelText("生命值")]
    public int health = 100;
    [LabelText("最大子弹数")]
    public int maxBulletNumber = 30;
    [LabelText("射击间隔")]
    public float shootInterval = 0.05f;
    [LabelText("子弹移动力量")]
    public float bulletMovePower = 50f;
    [LabelText("攻击力")]
    public int attack = 10;
}