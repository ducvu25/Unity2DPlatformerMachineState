using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    float gravity;
    public PlayerDashState(Player player, PlayerStateMachine machine, string animationName) : base(player, machine, animationName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        //Debug.Log("dash");
        stateTimer = player.dashDuration;
        gravity = rb.gravityScale;
        rb.gravityScale = 0;
        
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = gravity;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();
        if (player.IsWallCheck() && !player.IsGroundCheck())
        {
            machine.ChangeState(player.wallState);
        }

        rb.velocity = new Vector2((player.facingRight ? 1 : -1) * player.speedDash, 0);
        if(stateTimer <= 0) {
            machine.ChangeState(player.idleState);
        }
    }
}
