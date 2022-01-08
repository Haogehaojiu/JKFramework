using UnityEngine;
public class MonsterStatePatrol : MonsterStateBase
{
    //巡逻的目标点
    private Vector3 target;
    public override void Enter()
    {
        //修改移动状态
        SetMoveState(true);
        //播放对应动画
        PlayAnimation("run");
        //获取巡逻目标点
        target = monster.GetPatrolTarget();
        navMeshAgent.SetDestination(target);
    }
    public override void LateUpdate()
    {
        base.LateUpdate();
        //检测和玩家的距离，是否需要追击或攻击
        //如果切换为追击状态，则退出方法，不执行下面巡逻逻辑
        if (CheckFollowAndSwitchState()) return;
        //到达巡逻点后，切换到待机状态
        if (Vector3.Distance(monster.transform.position, target) < 1f) stateMachine.SwitchState<MonsterStateIdle>((int)MonsterStateType.Idle);
    }
}