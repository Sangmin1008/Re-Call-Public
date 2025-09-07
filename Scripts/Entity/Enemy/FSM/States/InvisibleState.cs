using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleState : IEnemyState
{
    private Renderer _renderer;
    private Coroutine _coroutine;

    public void Enter(Enemy enemy)
    {
        Debug.Log("Invisible Enter");

        _renderer = enemy.GetComponentInChildren<Renderer>();

        if (_renderer != null)
        {
            _coroutine = enemy.StartCoroutine(InvisibilityLoop(enemy));
        }

        enemy.agent.isStopped = false;
    }

    public void Update(Enemy enemy)
    {
        if (!enemy.CanSeePlayer())
        {
            StopInvisibility(enemy);

            // 안 보이면 IdleState로
            enemy.ChangeState(new IdleState());
            return;
        }

        if (enemy.InAttackRange() && enemy.IsPlayerInFieldOfView())
        {
            enemy.ChangeState(new AttackState());
        }

        enemy.agent.SetDestination(enemy.GetPlayerPosition());
    }

    public void Exit(Enemy enemy)
    {
        Debug.Log("Invisible Exit");
        StopInvisibility(enemy);
    }

    private void StopInvisibility(Enemy enemy)
    {
        if (_coroutine != null)
        {
            enemy.StopCoroutine(_coroutine);
            _coroutine = null;
        }

        if (_renderer != null)
            _renderer.enabled = true;
    }

    private IEnumerator InvisibilityLoop(Enemy enemy)
    {
        while (true)
        {
            // 보이는 상태
            if (_renderer != null)
                _renderer.enabled = true;
            yield return new WaitForSeconds(3f);

            // 안 보이는 상태
            if (_renderer != null)
                _renderer.enabled = false;
            yield return new WaitForSeconds(2f);
        }
    }
}

