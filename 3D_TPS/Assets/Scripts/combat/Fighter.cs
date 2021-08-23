using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    #region
    [Header("攻擊傷害")]
    [SerializeField] float attackDamage = 10f;
    [Header("攻擊距離")]
    [SerializeField] float attackRange = 2f;
    [Header("攻擊時間間隔")]
    [SerializeField] float timeBetweenAttack = 2f;

    Mover mover;
    Animator animator;
    Health health;
    Health targetHealth;

    //上一次的攻擊時間
    float timeSinceLastAttack = Mathf.Infinity;
    #endregion

    private void Awake()
    {
        mover = GetComponent<Mover>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        health.onDie += OnDie;
    }

    private void Update()
    {
        if (targetHealth == null ||targetHealth.IsDead()) return;

        //如果(攻擊距離內)
        if(IsInAttackRange())
        {
            //取消移動
            mover.CancelMove();
            //攻擊
            AttackBehaviour();
        }
        else if(timeSinceLastAttack > timeBetweenAttack)
        {
            //繼續移動
            mover.MoveTo(targetHealth.transform.position, 1f);
        }

        UpdateTimer();
    }

    private void UpdateTimer()
    {
        timeSinceLastAttack += Time.deltaTime;
    }

    /// <summary>
    /// 攻擊
    /// </summary>
    private void AttackBehaviour()
    {
        transform.LookAt(targetHealth.transform);

        if(timeSinceLastAttack > timeBetweenAttack)
        {
            timeSinceLastAttack = 0;
            triggerAttack();
        }
    }

    private void triggerAttack()
    {
        animator.ResetTrigger("攻擊觸發");
        animator.SetTrigger("攻擊觸發");
    }

    private void Hit()
    {
        if (targetHealth == null) return;

        if(IsInAttackRange())
        {
            targetHealth.TakeDamage(attackDamage);
        }
    }

    /// <summary>
    /// 攻擊距離內
    /// </summary>
    /// <returns></returns>
    private bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, targetHealth.transform.position) < attackRange;
    }

    /// <summary>
    /// 攻擊
    /// </summary>
    /// <param name="target"></param>
    public void Attack(Health target)
    {
        targetHealth = target;
    }

    /// <summary>
    /// 取消移動
    /// </summary>
    public void CancelTarget()
    {
        targetHealth = null;
    }

    private void OnDie()
    {
        this.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

