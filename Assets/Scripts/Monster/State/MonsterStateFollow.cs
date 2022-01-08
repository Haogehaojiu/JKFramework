using UnityEngine;
public class MonsterStateFollow : MonsterStateBase
{
    public override void Enter()
    {
        //修改移动状态
        SetMoveState(true);
        //播放对应动画
        PlayAnimation("run");
    }

    public override void LateUpdate()
    {
        //如果距离玩家达到条件，攻击玩家
        if (Vector3.Distance(monster.transform.position, player.transform.position) < 1.5f)
        {
            stateMachine.SwitchState<MonsterStateAttack>((int)MonsterStateType.Attack);
            return;
        }
        //否则去追玩家
        navMeshAgent.SetDestination(player.transform.position);
    }
}