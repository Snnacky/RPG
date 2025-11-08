using UnityEngine;

public class Player_SwordThrowState : PlayerState
{
    private Camera mainCamera;

    public Player_SwordThrowState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        skillManager.swordThrow.EnableDots(true);
        if(mainCamera!=Camera.main)
            mainCamera = Camera.main;
    }

    public override void Update()
    {
        base.Update();
        Vector2 dirToMouse = DirectionToMouse();//获取鼠标与玩家的相对坐标方向
        player.SetVelocity(0, rb.velocity.y);
        player.HandleFlip(dirToMouse.x);

        skillManager.swordThrow.PredictTrajectory(dirToMouse);//预测所有辅助点的位置

        //攻击
        if (input.Player.Attack.WasPressedThisFrame())
        { 
            anim.SetBool("swordThrow_end", true);//执行抛出动画
            skillManager.swordThrow.EnableDots(false);//关闭抛物线辅助线
            skillManager.swordThrow.ConfirmTrajectory(dirToMouse);//攻击方向
        }
        //取消攻击
        if (input.Player.RangeAttack.WasReleasedThisFrame() || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("swordThrow_end", false);
            skillManager.swordThrow.EnableDots(false);
    }

    //获取鼠标与玩家的相对坐标
    private Vector2 DirectionToMouse()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 worldMousePosition = mainCamera.ScreenToWorldPoint(player.mousePosition);//将鼠标位置转换为世界坐标,鼠标位置为屏幕坐标

        Vector2 direction=worldMousePosition - playerPosition;

        return direction.normalized;
    }
}
