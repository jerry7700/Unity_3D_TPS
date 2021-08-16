using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region 欄位
    [Header("移動參數")]
    [Tooltip("移動速度")]
    [SerializeField] float MoveSpeed = 8;
    [Tooltip("Shift加速的倍數"),Range(1,3)]
    [SerializeField] float sprintSpeedModifier = 2;
    [Tooltip("蹲下時的減速倍數"), Range(0, 1)]
    [SerializeField] float crouchedSpeedModifier = 0.5f;
    [Tooltip("旋轉速度")]
    [SerializeField] float rotateSpeed = 5f;
    [Tooltip("加速度的百分比")]
    [SerializeField] float addSpeedRatio = 0.1f;

    [Space(20)]
    [Header("跳躍參數")]
    [Tooltip("跳躍時向上施加的力量")]
    [SerializeField] float jumpForce = 15;
    [Tooltip("在空中下降施加的力量")]
    [SerializeField] float gravityDownForce = 50;
    [Tooltip("檢查與地面之間的距離")]
    [SerializeField] float distanceToGround = 0.1f;

    InputController input;
    CharacterController controller;
    Animator animator;

    //下一幀要移動到的目標位置
    Vector3 targetMovemenet;
    //下一幀跳躍到的方向
    Vector3 jumpDirection;
    //上一幀的移動速度
    float lastFrameSpeed;
    #endregion
    private void Awake()
    {
        input = GameManagerSingleton.Instance.InputController;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        MoveBehaviour();
        JumpBehaviour();
    }

    /// <summary>
    /// 移動行為
    /// </summary>
    private void MoveBehaviour()
    {
        targetMovemenet = Vector3.zero;
        targetMovemenet += input.GetMoveInput().z * GetCurrentCameraForward();
        targetMovemenet += input.GetMoveInput().x * GetCurrentCameraRight();

        //避免對角線超過1
        targetMovemenet = Vector3.ClampMagnitude(targetMovemenet, 1);

        //下一幀的移動速度
        float nextFrameSpeed = 0;

        if(targetMovemenet == Vector3.zero)
        {
            nextFrameSpeed = 0f;
        }
        else if(input.GetSprintInput()) //是否按下加速
        {
            nextFrameSpeed = 1f;

            targetMovemenet *= sprintSpeedModifier;
            smoothRotation(targetMovemenet);
        }
        else
        {
            nextFrameSpeed = 0.5f;

            smoothRotation(targetMovemenet);
        }

        if(lastFrameSpeed != nextFrameSpeed)
        {
            lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, nextFrameSpeed, addSpeedRatio);
        }

        animator.SetFloat("走路速度", lastFrameSpeed);

        controller.Move(targetMovemenet * Time.deltaTime * MoveSpeed);
    }

    /// <summary>
    /// 跳躍行為
    /// </summary>
    private void JumpBehaviour()
    {
        if(input.GetJumpInputDown() && IsGrounded())
        {
            animator.SetTrigger("跳躍觸發");
            jumpDirection = Vector3.zero;
            jumpDirection += jumpForce * Vector3.up;
        }

        jumpDirection.y -= gravityDownForce * Time.deltaTime;
        //jumpDirection.y = Math.Max(jumpDirection.y, -gravityDownForce);

        controller.Move(jumpDirection * Time.deltaTime);
    }

    /// <summary>
    ///  檢測是否在地上
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position,-Vector3.up ,distanceToGround);
    }

    #region 取得目前相機的方向

    /// <summary>
    /// 取得目前相機的正方方向
    /// </summary>
    /// <returns></returns>
    private Vector3 GetCurrentCameraForward()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();
        return cameraForward;
    }

    /// <summary>
    /// 取得目前相機的右方方向
    /// </summary>
    /// <returns></returns>
    private Vector3 GetCurrentCameraRight()
    {
        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();
        return cameraRight;
    }

    #endregion

    /// <summary>
    /// 平滑旋轉角度
    /// </summary>
    /// <param name="targetMovemenet"></param>
    private void smoothRotation(Vector3 targetMovemenet)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetMovemenet, Vector3.up), rotateSpeed * Time.deltaTime);
    }
}
