using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    #region 欄位
    [Tooltip("最大移動速度")]
    [SerializeField] float maxSpeed = 6;
    [SerializeField] float animatorChangeRatio = 0.1f;

    NavMeshAgent navmeshAgent;

    //上一幀的移動速度
    float lastFrameSpeed;
    #endregion

    private void Awake()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = navmeshAgent.velocity;
        //將全局navmeshAgent速度變量，轉變成local的速度變量
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, localVelocity.z, animatorChangeRatio);

        this.GetComponent<Animator>().SetFloat("走路速度", lastFrameSpeed / maxSpeed);
    }

    public void MoveTo(Vector3 destination, float speedRatio)
    {
        //不開啟停止導航系統
        navmeshAgent.isStopped = false;
        //設定導航系統的速度
        navmeshAgent.speed = maxSpeed * Mathf.Clamp01(speedRatio);
        //設定要導航的目的
        navmeshAgent.destination = destination;
    }

    public void CancelMove()
    {
        //停止導航系統
        navmeshAgent.isStopped = true;
    }
}
