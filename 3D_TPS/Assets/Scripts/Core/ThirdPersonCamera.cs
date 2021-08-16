using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    #region 欄位
    //[SerializeField] 本質是Private透過序列化
    [Header("Camera跟隨目標")]
    [SerializeField] Transform target;
    [Header("水平軸靈敏度")]
    [SerializeField] float sensitivity_x = 2;
    [Header("垂直軸靈敏度")]
    [SerializeField] float sensitivity_y = 2;
    [Header("滾輪靈敏度")]
    [SerializeField] float sensitivity_z = 5;

    [Header("最小垂直角度")]
    [SerializeField] float minVerticalAngle = -10;
    [Header("最大垂直角度")]
    [SerializeField] float maxVerticalAngle = 85;

    [Header("相機跟目標的距離")]
    [SerializeField] float cameraToargetDistance = 10;
    [Header("最小相機跟目標的距離")]
    [SerializeField] float minDistance = 2;
    [Header("最大相機跟目標的距離")]
    [SerializeField] float maxDistance = 25;
    [Header("Offset")]
    [SerializeField] Vector3 offset;

    InputController input;

    float mouse_x = 0;
    float mouse_y = 30;
    #endregion

    private void Awake()
    {
        input = GameManagerSingleton.Instance.InputController;
    }

    private void LateUpdate()
    {
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            mouse_x += input.GetMouseXAxis() * sensitivity_x;
            mouse_y += input.GetMouseYAxis() * sensitivity_y;

            //限制垂直角度
            mouse_y = Mathf.Clamp(mouse_y, minVerticalAngle, maxVerticalAngle);

            //Quaternion.Euler歐拉角 使相機旋轉
            transform.rotation = Quaternion.Euler(mouse_y, mouse_x, 0);
            //旋轉完並乘上相機跟目標的距離
            transform.position = Quaternion.Euler(mouse_y, mouse_x, 0) * new Vector3(0, 0, -cameraToargetDistance) + target.position + Vector3.up * offset.y;

            cameraToargetDistance += input.GetMouseScrollWheelAxis() * sensitivity_z;
            cameraToargetDistance = Mathf.Clamp(cameraToargetDistance, minDistance, maxDistance);
        }
    }
}
