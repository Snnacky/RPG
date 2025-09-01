using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CounterAttackState : PlayerState
{

    private Player_Combat combat;
    private bool counterSomebody;//是否反击到人
    public Player_CounterAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        combat=player.GetComponent<Player_Combat>();
    }

    public override void Enter()
    {
        base.Enter();
        counterSomebody = combat.CounterAttackEnd();
        stateTimer = combat.GetCounterRecoveryDuration();//反击不成功的回复时间
        anim.SetBool("counterAttack_end", counterSomebody);//反击
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, rb.velocity.y);
        //攻击结束的触发器
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
        //时间结束
        if (stateTimer < 0 && triggerCalled==false)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
