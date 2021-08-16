using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSmooth : MonoBehaviour
{
    #region 欄位
    [Tooltip("跟隨的player目標")]
    [SerializeField] Transform player;
    [Tooltip("跟目標最大距離")]
    [SerializeField] float distanceToTarget;
    [Tooltip("起始高度")]
    [SerializeField] float startHeight;
    [Tooltip("平滑移動的時間")]
    [SerializeField] float smoothTime;
    [Tooltip("滑鼠滾輪靈敏度")]
    [SerializeField] float sensitivityOffset_z;
    [Tooltip("最小垂直 Y Offset")]
    [SerializeField] float minOffset_Y;
    [Tooltip("最大垂直 Y Offset")]
    [SerializeField] float maxOffset_Y;

    [SerializeField] float Offset_Y;
    InputController input;
    Vector3 smoothPosition = Vector3.zero;
    Vector3 currentVelocity;
    #endregion

    private void Awake()
    {
        input = GameManagerSingleton.Instance.InputController;
        transform.position = player.position + Vector3.up * startHeight;
        Offset_Y = startHeight;
    }

    private void LateUpdate()
    {
        if(input.GetMouseScrollWheelAxis() != 0)
        {
            Offset_Y -= input.GetMouseScrollWheelAxis() * sensitivityOffset_z;
            Offset_Y = Mathf.Clamp(Offset_Y, minOffset_Y, maxOffset_Y);
            Vector3 offsetTarget = player.position + player.up * Offset_Y;
            transform.position = Vector3.Lerp(transform.position, offsetTarget, smoothTime);
        }
        if(CheckDistance())
        {
            smoothPosition = Vector3.SmoothDamp(transform.position, player.position + Vector3.up * Offset_Y, ref currentVelocity, smoothTime);
            transform.position = smoothPosition;
        }
    }

    /// <summary>
    /// 檢查與目標的距離
    /// </summary>
    /// <returns></returns>
    private bool CheckDistance()
    {
        return Vector3.Distance(transform.position, player.position) > distanceToTarget;
    }
}
