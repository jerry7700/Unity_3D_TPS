using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    #region 欄位
    [Header("追趕距離")]
    [SerializeField] float chaseDistance = 10f;
    [Header("失去目標後困惑的時間")]
    [SerializeField] float confuseTime = 5f;

    [Header("目標巡邏點的GameObject物件")]
    [SerializeField] PatrolPath patrol;
    [Header("需要到達waypoint的距離")]
    [SerializeField] float waypointToStay = 3f;
    [Header("待在waypoint的時間")]
    [SerializeField] float waypointToWaitTime = 3f;
    [Header("巡邏的速度")]
    [SerializeField,Range(0,1)] float patrolSpeedRatio = 0.5f;

    Animator animator;
    GameObject player;
    Mover mover;
    Health health;

    //上次看到玩家時間
    private float timeSinceLastSswPlayer = Mathf.Infinity;
    //原點座標
    private Vector3 beginpostion;
    //當前需要到達的Waypoint
    int currentWaypointIndex;
    //距離上次抵達Waypoint的時間
    float timeSinceArriveWaypoint = 0;
    #endregion

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mover = GetComponent<Mover>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();

        beginpostion = transform.position;

        health.onDamage += OnDamage;
        health.onDie += OnDead;
    }

    private void Update()
    {
        if (health.IsDead()) return;

        //如果(玩家在追趕距離內)
        if (IsInRange())
        {
            //移動到玩家前方
            timeSinceLastSswPlayer = 0;
            mover.MoveTo(player.transform.position, 1);
        }
        else if(timeSinceLastSswPlayer < confuseTime)
        {
            ConfuseBehaviour();
        }
        else
        {
            PatrolBehaviour();    
        }

        UpdateTimer();
    }

    /// <summary>
    /// 巡邏行為
    /// </summary>
    private void PatrolBehaviour()
    {
        Vector3 nextwaypointpostion = beginpostion;
        if(patrol != null)
        {
            if(IsAtWayPoint())
            {
                mover.CancelMove();
                animator.SetBool("是否失去敵人", true);
                timeSinceArriveWaypoint = 0;
                currentWaypointIndex = patrol.GetNextWayPointNumder(currentWaypointIndex);
            }

            if(timeSinceArriveWaypoint > waypointToWaitTime)
            {
                animator.SetBool("是否失去敵人", false);
                mover.MoveTo(patrol.GetWayPointPosition(currentWaypointIndex), patrolSpeedRatio);
            }
        }
        else
        {
            //回到原點
            animator.SetBool("是否失去敵人", false);
            mover.MoveTo(beginpostion, 0.5f);
        }
    }

    /// <summary>
    /// 檢查是否已經抵達WayPoint
    /// </summary>
    /// <returns></returns>
    private bool IsAtWayPoint()
    {
        return (Vector3.Distance(transform.position, patrol.GetWayPointPosition(currentWaypointIndex)) < waypointToStay);
    }

    /// <summary>
    /// 困惑的動作行為
    /// </summary>
    private void ConfuseBehaviour()
    {
        //取消移動與攻擊
        mover.CancelMove();
        //困惑動作
        animator.SetBool("是否失去敵人", true);
    }

    /// <summary>
    /// 是否小於追趕距離內
    /// </summary>
    /// <returns></returns>
    private bool IsInRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
    }

    private void UpdateTimer()
    {
        timeSinceLastSswPlayer += Time.deltaTime;
        timeSinceArriveWaypoint += Time.deltaTime;
    }

    private void OnDamage()
    {
        //受到攻擊時，觸發的事情
    }

    private void OnDead()
    {
        mover.CancelInvoke();
        animator.SetTrigger("死亡觸發");
    }
}
