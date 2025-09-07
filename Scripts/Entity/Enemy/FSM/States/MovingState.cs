using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MovingState : IEnemyState
{
    public void Enter(Enemy enemy)
    {
        enemy.agent.speed = enemy.walkSpeed;
        enemy.agent.isStopped = false;
        enemy.WanderToRandomPosition();
    }

    public void Update(Enemy enemy)
    {
        enemy.animator.SetBool("Moving", true);

        if (!enemy.agent.pathPending && enemy.agent.remainingDistance < 0.1f )
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit(Enemy enemy)
    {

    }
}
