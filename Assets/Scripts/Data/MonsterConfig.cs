using JKFramework;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterConfig", menuName = "Config/Monster Config")]
public class MonsterConfig : ConfigBase
{
    public int hp;
    public int attack;
    public GameObject monsterPrefab;
}
