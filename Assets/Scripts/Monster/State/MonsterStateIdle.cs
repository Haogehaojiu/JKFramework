using System.Collections;
using JKFramework;
using UnityEngine;
public class MonsterStateIdle : MonsterStateBase
{
    private Coroutine goPatrol;
    public override void Enter()
    {
        //修改状态
        SetMoveState(false);
        //播放对应动画
        PlayAnimation("idle");
        //延迟一个随机时间去巡逻
        goPatrol = this.StartCoroutine(GoPatrol(UnityEngine.Random.Range(1, 10)));
    }
    private IEnumerator GoPatrol(int time)
    {
        yield return new WaitForSeconds(time);
        stateMachine.SwitchState<MonsterStatePatrol>((int)MonsterStateType.Patrol);
    }
    public override void Exit()
    {
        base.Exit();
        if (goPatrol != null)
        {
            this.StopCoroutine(goPatrol);
            goPatrol = null;
        }
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        //检测和玩家的距离，是否需要追击或攻击
        CheckFollowAndSwitchState();
    }
}