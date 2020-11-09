using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum UnitState
{
    Idle,
    Move,
    MoveToResource,
    Gather,
    MoveToEnemy,
    Attack
}

public class Unit : MonoBehaviour
{

    [Header("Components")]
    public GameObject selectionVisual;
    NavMeshAgent navAgent;

    [Header("States")]
    public UnitState state;

    public int gatherAmount;
    public int curHp;
    public int maxHp;
    public int minAttackDamage;
    public int maxAttackDamage;
        
    public float gatherRate;
    public float attackRate;
    public float pathUpdateRate = 1.0f;
    public float attackDistance;

    private float lastPathUpdateTime;
    private float lastGatherTime;
    private float lastAttackTime;

    private Unit curEnemyTarget;

    private ResourceSource curResourceSource;

    public UnitHealthBar healthBar;

    public Player player;
    
    //events
    [System.Serializable]
    public class StateChangeEvent : UnityEvent<UnitState> { }
    public StateChangeEvent onStateChange;

   
    //methods
    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        SetState(UnitState.Idle);
    }

    void Update()
    {
        switch (state)
        {
            case UnitState.Move:
                {
                    MoveUpdate();
                    break;
                }
            case UnitState.MoveToResource:
                {
                    MoveToResourceUpdate();
                    break;
                }
            case UnitState.Gather:
                {
                    GatherUpdate();
                    break;
                }
            case UnitState.MoveToEnemy:
                {
                    MoveToEnemyUpdate();
                    break;
                }
            case UnitState.Attack:
                {
                    AttackUpdate();
                    break;
                }
        }
    }

    public void ToggleSelectionVisual(bool selected)
    {
        selectionVisual.SetActive(selected);
    }

    public void MoveToPosition(Vector3 pos)
    {
        navAgent.isStopped = false;
        navAgent.SetDestination(pos);
        SetState(UnitState.Move);
    }

    public void GatherResource(ResourceSource resource, Vector3 pos)
    {
        curResourceSource = resource;
        SetState(UnitState.MoveToResource);

        navAgent.isStopped = false;
        navAgent.SetDestination(pos);
    }

    void SetState(UnitState toState)
    {
        state = toState;

        //calling the event
        if (onStateChange != null)
        {
            onStateChange.Invoke(state);
        }
        if (toState == UnitState.Idle)
        {
            navAgent.isStopped = true;
            navAgent.ResetPath();
        }
    }

    void MoveUpdate()
    {
        if(Vector3.Distance(transform.position, navAgent.destination) ==0.0f)
                SetState(UnitState.Idle);
    }

    void MoveToResourceUpdate()
    {
        if(curResourceSource == null)
        {
            SetState(UnitState.Idle);
            return;
        }
        if (Vector3.Distance(transform.position, navAgent.destination) == 0.0f)
            SetState(UnitState.Gather);
    }

    void GatherUpdate()
    {
        if(curResourceSource == null)
        {
            SetState(UnitState.Idle);
            return;
        }
        LookAt(curResourceSource.transform.position);
        if(Time.time - lastGatherTime > gatherRate)
        {
            lastGatherTime = Time.time;
            curResourceSource.GatherResource(gatherAmount, player);
        }
    }

    void MoveToEnemyUpdate()
    {
        if(curEnemyTarget == null)
        {
            SetState(UnitState.Idle);
            return;
        }
        if(Time.time - lastPathUpdateTime > pathUpdateRate)
        {
            lastPathUpdateTime = Time.time;
            navAgent.isStopped = false;
            navAgent.SetDestination(curEnemyTarget.transform.position);
        }
        if (Vector3.Distance(transform.position, curEnemyTarget.transform.position) > attackDistance)
            SetState(UnitState.Attack);

    }

    void AttackUpdate()
    {
        if(curEnemyTarget == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        if (!navAgent.isStopped)
            navAgent.isStopped = true;

        if(Time.time - lastAttackTime > attackRate)
        {
            lastAttackTime = Time.time;
            curEnemyTarget.TakeDamage(Random.Range(minAttackDamage, maxAttackDamage + 1));
        }

        LookAt(curEnemyTarget.transform.position);

        if (Vector3.Distance(transform.position, curEnemyTarget.transform.position) > attackDistance)
            SetState(UnitState.MoveToEnemy);
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;
        if (curHp <= 0)
            Die();
    }

    void Die()
    {
        player.units.Remove(this);
        Destroy(gameObject);
    }

    void LookAt(Vector3 pos)
    {
        Vector3 dir = (pos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public void AttackUnit(Unit target)
    {
        curEnemyTarget = target;
        SetState(UnitState.MoveToEnemy);
    }
}
