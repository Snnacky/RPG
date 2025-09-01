using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CounterAttackState : PlayerState
{

    private Player_Combat combat;
    private bool counterSomebody;//�Ƿ񷴻�����
    public Player_CounterAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        combat=player.GetComponent<Player_Combat>();
    }

    public override void Enter()
    {
        base.Enter();
        counterSomebody = combat.CounterAttackEnd();
        stateTimer = combat.GetCounterRecoveryDuration();//�������ɹ��Ļظ�ʱ��
        anim.SetBool("counterAttack_end", counterSomebody);//����
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, rb.velocity.y);
        //���������Ĵ�����
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
        //ʱ�����
        if (stateTimer < 0 && triggerCalled==false)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
