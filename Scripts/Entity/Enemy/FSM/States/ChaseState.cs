using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{
    public void Enter(Enemy enemy)
    {
        Debug.Log("Chase 시작");
        if (enemy.agent.isOnNavMesh)
        {
            enemy.agent.isStopped = false;
        }
        enemy.animator.SetBool("Moving", true);
    }

    public void Update(Enemy enemy)
    {
        if (enemy is TeleportEnemy && enemy.CanSeePlayer() && enemy.GetDetectDistance() > 6f)
        {
            enemy.ChangeState(new TeleportState());
            return;
        }
        else if (enemy is InvisibleEnemy && enemy.CanSeePlayer())
        {
            enemy.ChangeState(new InvisibleState());
            return;
        }

        if (!enemy.CanSeePlayer())
        {
            enemy.ChangeState(new IdleState());
            return;
        }

        enemy.MoveToPlayer();

        if (enemy.InAttackRange() && enemy.IsPlayerInFieldOfView())
        {
            enemy.ChangeState(new AttackState());
        }
    }

    public void Exit(Enemy enemy)
    {
        if (enemy.agent.isOnNavMesh)
        {
            enemy.agent.isStopped = true;
        }
        Debug.Log("추격 종료");
    }
}
