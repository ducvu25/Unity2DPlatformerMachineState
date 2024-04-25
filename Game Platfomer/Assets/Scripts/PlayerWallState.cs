using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallState : PlayerState
{
    float vY;
    float gravity;
    bool isSet;
    public PlayerWallState(Player player, PlayerStateMachine machine, string animationName) : base(player, machine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.wallDelay;
        vY = rb.velocity.y;
        rb.velocity = Vector2.zero;
        gravity = rb.gravityScale;
        rb.gravityScale = 0;
        isSet = true;
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = gravity;
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer <= 0 && isSet)
        {
            isSet = false;
            rb.gravityScale = gravity;
            rb.velocity = new Vector2(rb.velocity.x, vY);
        }
        if ((xInput > 0 && !player.facingRight) || (xInput < 0 && player.facingRight))
        {
            player.SetVelocity(xInput * player.moveSpeed * 0.8f, rb.velocity.y);
            machine.ChangeState(player.airState);
        }
        if(yInput > 0)
        {
            if(player.IsWallCheck2())
                rb.velocity = new Vector2(player.moveSpeed * (player.facingRight ? -1 : 1), player.jumpFoce);
            else
                rb.velocity = new Vector2(0, player.jumpFoce*0.6f);
            machine.ChangeState(player.jumpState);
        }
        else if(yInput < 0)
        {
            stateTimer = 0;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }else
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y*0.7f);

        if (player.IsGroundCheck())
            machine.ChangeState(player.idleState);
    }
}