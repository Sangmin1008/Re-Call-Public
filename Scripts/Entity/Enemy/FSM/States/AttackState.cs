using UnityEngine;
using UnityEngine.AI;

public class AttackState : IEnemyState
{
    private float _lastAttackTime; // 공격 쿨타임
    private float attactRate;

    public void Enter(Enemy enemy)
    {
        Debug.Log("Attack 시작");
        attactRate = 1.5f;
        enemy.agent.speed = enemy.runSpeed;
        enemy.agent.isStopped = true;
    }

    public void Update(Enemy enemy)
    {
        if (Time.time - _lastAttackTime > attactRate)
        {
            _lastAttackTime = Time.time;
            enemy.AttackPlayer();
            enemy.animator.speed = 1;
            enemy.animator.SetTrigger("Attack");
        }
        else
        {
            if (enemy.CanSeePlayer())
            {
                if (enemy.agent.isOnNavMesh)
                {
                    enemy.agent.isStopped = false;
                    NavMeshPath path = new NavMeshPath();
                    if (enemy.agent.CalculatePath(enemy.GetPlayerPosition(), path))
                    {
                        enemy.agent.SetDestination(enemy.GetPlayerPosition());
                    }
                }
                else
                {
                    Debug.LogWarning($"{enemy.name}의 NavMeshAgent가 NavMesh 위에 있지 않습니다.");
                }
            }
            else
            {
                if (enemy.agent.isOnNavMesh)
                {
                    enemy.agent.SetDestination(enemy.transform.position);
                    enemy.agent.isStopped = true;
                }
                enemy.ChangeState(new ChaseState());
            }
        }

        if (!enemy.InAttackRange())
            enemy.ChangeState(new IdleState());
    }

    public void Exit(Enemy enemy)
    {
        Debug.Log("Attack 종료");
        if (enemy.agent.isOnNavMesh)
            enemy.agent.isStopped = false;
    }
}
