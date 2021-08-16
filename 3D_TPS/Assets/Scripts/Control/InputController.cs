using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float vertical;
    public float horizontal;
    bool canInput = true;

    private void Awake()
    {
        //設定游標狀態(鎖住)
        Cursor.lockState = CursorLockMode.Locked;
        //是否顯示游標(否)
        Cursor.visible = false;
    }

    private void Update()
    {
        CheckCursorState();
    }

    /// <summary>
    /// 移動按鍵設定
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMoveInput()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        //限制移動最大幅度為1，否則對角線移動可能會超過定義的最大移動速度
        move = Vector3.ClampMagnitude(move, 1);
        return move;
    }

    /// <summary>
    /// 是否按下Sprint加速
    /// </summary>
    /// <returns></returns>
    public bool GetSprintInput()
    {
        if(CanProcessInput())
        {
            return Input.GetKey(KeyCode.LeftShift);
        }
        return false;
    }

    /// <summary>
    /// 是否按下Space跳躍
    /// </summary>
    /// <returns></returns>
    public bool GetJumpInputDown()
    {
        if(CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
        return false;
    }

    #region Mouse

    //取得Mouse X 的 Axis
    public float GetMouseXAxis()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse X");
        }
        return 0;
    }

    //取得Mouse Y 的 Axis
    public float GetMouseYAxis()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse Y");
        }
        return 0;
    }

    //取得Mouse ScrollWheel 的 Axis
    public float GetMouseScrollWheelAxis()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse ScrollWheel");
        }
        return 0;
    }
    #endregion

    /// <summary>
        /// 鼠標設定
        /// </summary>
    private void CheckCursorState()
    {
        //如果按下ESC鍵
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //如果鼠標沒有隱藏(自由)
            if (Cursor.lockState == CursorLockMode.None)
            {
                //如果鼠標沒有隱藏(自由)
                if (Cursor.lockState == CursorLockMode.None)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
            else
            {
                //設定游標狀態(自由)
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    /// <summary>
    /// 是否可以處理Input
    /// </summary>
    /// <returns></returns>
    private bool CanProcessInput()
    {
        // 如果Cursor狀態不在鎖定中就不能處理Input
        return Cursor.lockState == CursorLockMode.Locked && canInput;
    }
}
