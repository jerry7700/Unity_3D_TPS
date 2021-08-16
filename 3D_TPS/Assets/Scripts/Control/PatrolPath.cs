using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    [Tooltip("WayPoint的半徑")]
    [SerializeField] float WayPointDrawsRadius = 1f;


    /// <summary>
    /// 取得下一個WayPoint的編號
    /// </summary>
    /// <param name="wayPointNumber"></param>
    /// <returns></returns>
    public int GetNextWayPointNumder(int wayPointNumber)
    {
        if (wayPointNumber + 1 > transform.childCount - 1)
        {
            return 0;
        }
        return wayPointNumber + 1;
    }

    /// <summary>
    /// 取得WayPoint的位置
    /// </summary>
    /// <param name="wayPointNumber"></param>
    /// <returns></returns>
    public Vector3 GetWayPointPosition(int wayPointNumber)
    {
        return transform.GetChild(wayPointNumber).position;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            int j = GetNextWayPointNumder(i);
            //畫往下一個巡邏點的線
            Gizmos.DrawLine(GetWayPointPosition(i), GetWayPointPosition(j));
            Gizmos.DrawSphere(GetWayPointPosition(i), WayPointDrawsRadius);
        }
    }
}
