using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeleportState : IEnemyState
{
    private bool _isTeleporting;
    private float _teleportDistance=3f;

    public void Enter(Enemy enemy)
    {
        _isTeleporting = false;
    }

    public void Update(Enemy enemy)
    {
        if (!_isTeleporting)
        {
            Vector3 playerPos = enemy.GetPlayerPosition();

            //normalized는 거리와 상관없이 방향만 추출
            Vector3 offset = (playerPos- enemy.transform.position).normalized;  //enemy기준으로 offset값을 구해야함
            Vector3 targetPos = playerPos - offset * _teleportDistance;

            //NavMesh Walkable Area체크
            NavMeshHit hit;
            int walkableArea = NavMesh.GetAreaFromName("Walkable");
            int walkableMask = 1 << walkableArea;

            bool isWalkable = NavMesh.SamplePosition(targetPos, out hit, _teleportDistance, walkableMask);

            enemy.agent.isStopped = true;

            if (isWalkable)
            {
                // enemy.transform.position = hit.position;
                enemy.agent.Warp(hit.position);
                Debug.Log("텔레포트 실행");
                _isTeleporting = true;
                enemy.ChangeState(new ChaseState());
            }
            else
            {
                Debug.Log("텔레포트 실패(nonWalkable)지역");
                enemy.agent.isStopped = false;
                enemy.ChangeState(new IdleState());
            }


        }
    }

    public void Exit(Enemy enemy)
    {
        Debug.Log("Teleporte Exit");
    }
}
