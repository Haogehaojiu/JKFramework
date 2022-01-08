using System.Collections;
using JKFramework;
using UnityEngine;

public class MonsterStateDie : MonsterStateBase
{
    private Coroutine goDie;
    public override void Enter()
    {
        //修改状态
        SetMoveState(false);
        //播放对应动画
        PlayAnimation("die");
        //玩家得分以及通知怪物管理器
        EventManager.EventTrigger("MonsterDie");
        goDie =  this.StartCoroutine(Die());
    }
    public override void Exit()
    {
        if (goDie!=null)
        {
            this.StopCoroutine(goDie);
            goDie = null;
        }
    }
    private IEnumerator Die()
    {
        yield return new WaitForSeconds(2f);
        monster.Die();
    }
}