using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float walkSpeed = 3f;
    public float runSpeed = 5f;
    public float maxHealth = 100f;
    public float currentHealth;

    public List<GenericItemDataSO> dropOnDeath;

    [Header("AI")]
    public NavMeshAgent agent;
    [SerializeField] private float chasingRange;

    [Header("Combat")]
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float attackRange;
    [SerializeField] private float fieldOfView = 120f; //시야각

    public float minWanderDistance;
    public float maxWanderDistance;

    public Color color;

    //애니메이션
    public Animator animator;
    public SkinnedMeshRenderer[] meshRenderers;    //캐릭터의 시각요소 제어용(빨간화면 등)

    private IEnemyState _currentEnemyState;

    public bool isDead;

    protected virtual void Start()
    {
        Renderer rend = GetComponentInChildren<Renderer>();

        // 머테리얼을 복제해서 나만의 인스턴스 생성
        rend.material = new Material(rend.material);

        // 복제된 머테리얼의 컬러 변경
        rend.material.color = color;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        currentHealth = maxHealth;

        ChangeState(new IdleState());
    }

    protected virtual void Update()
    {
        if (isDead) return;
        _currentEnemyState.Update(this);
    }

    public void ChangeState(IEnemyState enemyState)
    {
        if (_currentEnemyState != null)
            _currentEnemyState.Exit(this);
        _currentEnemyState = enemyState;
        _currentEnemyState.Enter(this);
    }

    public bool CanSeePlayer()
    {
        return GetDetectDistance() <= chasingRange;
    }

    public bool InAttackRange()
    {
        return GetDetectDistance() <= attackRange;
    }

    public float GetDetectDistance()
    {
        return Vector3.Distance(transform.position, GetPlayerPosition());
    }

    public Vector3 GetPlayerPosition()
    {
        return Camera.main.transform.position;
    }
    public void MoveToPlayer()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(GetPlayerPosition());
            SetAnimatorSpeed(agent.velocity.magnitude, runSpeed);
        }
    }

    void MoveToNewLocation(Enemy enemy)
    {
        enemy.agent.SetDestination(GetMoveLocation(enemy));
    }

    public void WanderToRandomPosition()
    {
        Vector3 targetPos = GetMoveLocation(this);
        if (agent.isOnNavMesh)
            agent.SetDestination(targetPos);
    }

    Vector3 GetMoveLocation(Enemy enemy)
    {
        NavMeshHit hit;

        NavMesh.SamplePosition(enemy.transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;

        while (Vector3.Distance(enemy.transform.position, hit.position) < enemy.GetDetectDistance())
        {
            NavMesh.SamplePosition(enemy.transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30) break;
        }

        return hit.position;
    }

    public void AttackPlayer()
    {
        EventBus.Publish("PlayerTakeDamage", damage);
    }

    public bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = GetPlayerPosition() - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle <= fieldOfView * 0.8;
    }

    public void SetAnimatorSpeed(float agentSpeed, float speed)
    {
        animator.speed = agentSpeed / walkSpeed;
    }

    public void TakePhysicalDamage(float damage) //몬스터가 맞는거
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Debug.Log("데미지 효과");
            animator.SetTrigger("TakeDamage");
        }

        //데미지 효과
        StartCoroutine(DamageFlash());
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        //몬스터 죽으면 아이템 드롭 - 인벤토리에서 아이템 드랍하는거 보고하기, 랜덤값으로 리스트안에 있는 아이템 퍼트리기
        for (int i = 0; i < dropOnDeath.Count; i++)
        {
            Vector3 spawnOffset = new Vector3(0, 0, 0.5f * i);
            Vector3 spawnPosition = transform.position + spawnOffset;
            ItemDatabase.Instance.SpawnItem(spawnPosition, dropOnDeath[i]);
        }

        if (agent != null)
            agent.enabled = false;

        animator.SetTrigger("Dead");
        //2초 뒤에 사라짐
        EventBus.Publish<int>("QuestClear", 4);
        GameManager.Instance.HuntMonster();
        Invoke("DestroyEnemy", 2f);
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    IEnumerator DamageFlash()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.SetColor("_Color", Color.red);
        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.SetColor("_Color", Color.white);
        }
    }

    //적이 볼수있는 범위 확인용 함수
    private void OnDrawGizmosSelected()
    {
        //추격범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chasingRange);

        //공격범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


}
