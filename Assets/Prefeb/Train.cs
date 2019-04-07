using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/*
 * 火车系统
 * 火车遵循列车时刻表运动.(暂时)
 * A站停留30s 运动1min到达B站 B站停留30s 运动1min回来 先这样
 * 每一个行动都是作为一个Action 开始执行时,执行中 执行结束之后都有相应的回调函数
 * 火车速度 10/s 中间每一个行为都有一个ActionQuene
 * 标签组件 挂载标签组件的对象可以创建标签
*/


public class Train : MonoBehaviour
{
    private HUDTips tips;

    [HideInInspector]
    public ActionController controller;

    // Start is called before the first frame update
    void Start()
    {
        tips = GetComponent<HUDTips>();

        controller = new ActionController
        {
            tips = tips,
            transform = this.transform
        };
        controller.Init();
    }

    // Update is called once per frame
    void Update()
    {
        controller.Update();
    }
}

public class ActionController
{
    public List<FAction> actionList = new List<FAction>();
    public HUDTips tips;
    public int count = 0;
    public Transform transform;

    public void Init()
    {
        actionList.Add(new StopAction(this) { TotalTime = 3f});
        actionList.Add(new MoveAction(this) { Destination = 100f, MoveSpeed = 10f, transform = this.transform });
        actionList.Add(new StopAction(this) { TotalTime = 6f });
        actionList.Add(new MoveAction(this) { Destination = -100f, MoveSpeed = -10f, transform = this.transform });
        actionList[0].OnStart();
    }

    /// <summary>
    /// 供外部调用的Update
    /// </summary>
    public void Update()
    {
        if (!actionList[count].OnMove())
        {
            actionList[count].OnEnd();
            count++;
            if (count == actionList.Count)
            {
                count = 0;
            }
            actionList[count].OnStart();
        }
    }

    public void OutMessage(string str)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Action");
        sb.Append((count + 1).ToString());
        sb.Append("of");
        sb.AppendLine(actionList.Count.ToString());
        sb.AppendLine(str);
        tips.SetText(sb.ToString());
    }

}


public class FAction
{
    protected ActionController controller;
    public FAction(ActionController actionController)
    {
        this.controller = actionController;
     }

    public virtual void OnStart() { }
    public virtual bool OnMove() { return true; }
    public virtual void OnEnd() { }
}

//等待操作 
public class StopAction : FAction
{
    public float TotalTime = 50f;
    private float releaseTime = 0f;
    public StopAction(ActionController controller):base(controller)
    {
    }

    public override void OnStart()
    {
        releaseTime = TotalTime;
    }

    public override bool OnMove()
    {
        releaseTime -= Time.deltaTime;
        controller.OutMessage("Release Time:" + releaseTime.ToString("F2"));
        return releaseTime > 0f;
    }


}

//移动操作 日后要换成dotween和动画插值轨道移动
public class MoveAction : FAction
{
    public float Destination;
    public float MoveSpeed = 10f;
    public Transform transform;

    public MoveAction(ActionController controller) : base(controller)
    {

    }

    public override bool OnMove()
    {
        Vector3 vector3 = transform.position;
        vector3.z = vector3.z + MoveSpeed * Time.deltaTime;
        controller.OutMessage("Moving");

        if (MoveSpeed > 0 && vector3.z > Destination || (MoveSpeed < 0 && vector3.z < Destination))
        {
            vector3.z = Destination;
            transform.position = vector3;
            return false;
        }
        transform.position = vector3;
        return true;
    }
}
