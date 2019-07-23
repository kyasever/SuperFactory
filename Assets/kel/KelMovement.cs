using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移动组件,提供MoveTo接口
/// </summary>
public class KelMovement : KelComponent
{
    /// <summary>
    /// 移动速度
    /// </summary>
    public float MoveSpeed = 10f;

    private Vector3 followPos;
    /// <summary>
    /// 导航的目标地点,更改目标之后重新移动
    /// </summary>
    public Vector3 FollowPos
    {
        get { return followPos; }
        set { followPos = value; IsArrived = false; }
    }

    private bool isArrived = false;
    /// <summary>
    /// 判断是否接近了设定的目标点
    /// </summary>
    public bool IsArrived
    {
        get
        {
            return isArrived;
        }
        set { isArrived = value; }
    }

    private float sqrMagnitude;
    /// <summary>
    /// 距离终点还有多远
    /// </summary>
    public float Distance { get { return Mathf.Sqrt(sqrMagnitude); } }

    // Update is called once per frame
    void Update()
    {
        //如果已经到达目标地点了,那么不进行运算
        if(isArrived)
        {
            return;
        }

        Vector3 d = followPos - Position;
        d.y = 0;
        Vector3 dis = d.normalized * MoveSpeed * Time.deltaTime;
        sqrMagnitude = dis.sqrMagnitude;
        if (d.sqrMagnitude > sqrMagnitude)
        {
            Position = Position + dis;
        }
        else
        {
            IsArrived = true;
            Position = followPos;
        }
    }

    /// <summary>
    /// 使目标移动
    /// </summary>
    public void MoveTo(Vector3 v)
    {
        isArrived = false;
        followPos = v;
        //直接看向目标地点
        Vector3 newv = v;
        newv.y = Position.y;
        transform.LookAt(newv);
    }
}
