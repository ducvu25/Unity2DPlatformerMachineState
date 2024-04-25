using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    float attackCombo = 1f;
    float lastTimeAttacked = 0;
    int attackCounter;
    

    public PlayerAttackState(Player player, PlayerStateMachine machine, string animationName) : base(player, machine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isAttack = true;
        if (Time.time > lastTimeAttacked + attackCombo)
            attackCounter = 0;
        player.animator.SetInteger("AttackCounter", attackCounter);

        float dirFac = player.facingRight ? 1 : -1;
        if(xInput != 0)
            dirFac = xInput;

        rb.velocity =  new Vector2(dirFac * player.speedAttack[attackCounter].x, rb.velocity.y + player.speedAttack[attackCounter].y);
    }

    public override void Exit()
    {
        base.Exit();
        attackCounter = (attackCounter + 1) % 3;
        lastTimeAttacked = Time.time;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();
        if(!player.isAttack)
        {
            machine.ChangeState(player.idleState);
        }
    }
}

