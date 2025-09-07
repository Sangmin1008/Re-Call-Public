using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    public void Enter(Enemy enemy)
    {
        Debug.Log("Idle시작");
        enemy.agent.speed = enemy.walkSpeed;
        enemy.animator.SetBool("Moving", false);
    }

    public void Update(Enemy enemy)
    {
        if (enemy.CanSeePlayer())
            enemy.ChangeState(new ChaseState());
        else
            enemy.ChangeState(new MovingState());
    }

    public void Exit(Enemy enemy)
    {
        Debug.Log("Idle종료");
        
    }
}
