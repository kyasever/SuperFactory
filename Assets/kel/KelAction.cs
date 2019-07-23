using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 行动队列指示器,用于指示这个机器人目前应该去哪
/// </summary>
public class KelAction : KelComponent
{
    /// <summary>
    /// 当前这个机器人处于谁的控制之下,大部分时候都不归自己控制,只有在移动的时候是自己
    /// </summary>
    public KelObject CurrertStation;

    //机器人将自身的控制权限交给其他物体,只有当其他物体把权限还给自己的时候,才能进行下一步操作,不然一律不归自己控制
    //对于一个采矿机器人来说,它只有两个节点,矿点和箱子,中间一律无关

    public List<RobotAction> ActionList = new List<RobotAction>();

    private void Start()
    {
        //ActionList.Add(new RobotAction())
    }

}

public class RobotAction
{
    public KelObject TargetObj;
    public RobotAction(KelObject kelObject)
    {
        TargetObj = kelObject;
    }

    public virtual void Action()
    {

    }
}