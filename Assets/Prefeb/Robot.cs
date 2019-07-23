using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * 机器人系统
 * 机器人遵循初始化队列移动
 * 暂定一个采矿机器人的目标队列
 * 移动到矿点 - 采矿 - 移动到箱子 - 移动到矿点 - - - 
 * 移动路径搜寻器 己方点距离最近的车站的位置 
 * 移动到矿点的细化流程 移动到车站后被车站获取控制权限.车站将权限交给火车.火车将权限交给机器人,机器人移动到矿点.
 * 管道移动方式 
 * 备注 要将所有的移动到某点全换成dotween来移动
*/
public class Robot : MonoBehaviour
{
    public float moveSpeed = 10f;

    //这个要重写了. 换成队列式操作
    public enum State
    {
        前往, 排队, 采集, 送货, 移动
    }
    public State state = State.前往;

    private GameMode gameMode;

    // Use this for initialization
    private void Start()
    {
        gameMode = GameMode.Instanse;
        FindMine();
    }

    public int mineCount = 0;
    public float miningInterval = 1f;
    private float miningCD = 1f;
    private Mine mine;

    public List<int> list = new List<int>();

    private bool isArrived = false;
    /// <summary>
    /// 判断是否接近了设定的目标点
    /// </summary>
    public bool IsArrived
    {
        get { return isArrived; }
        set { isArrived = value; }
    }

    /// <summary>
    /// 由外部调用,获取当前的arrived状态.避免多次调用
    /// </summary>
    public bool GetIsArrived()
    {
        Vector3 d = followPos - transform.position;
        d.y = 0;
        Vector3 dis = d.normalized * moveSpeed * Time.deltaTime;
        if (d.sqrMagnitude < dis.sqrMagnitude)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private Vector3 followPos;
    /// <summary>
    /// 导航的目标地点,更改目标之后重新移动
    /// </summary>
    public Vector3 FollowPos
    {
        get { return followPos; }
        set { followPos = value;IsArrived = false;}
    }

    /// <summary>
    /// 向gamemode请求一个移动目标,将自己加入目标的排队队列中,并向这个目标移动
    /// </summary>
    private void FindMine()
    {
        mine = gameMode.GetMine();
        FollowPos = mine.transform.position;
        mine.waitList.Add(this);
    }
    /// <summary>
    /// 向gamemode请求一个箱子目标,箱子不需要排队
    /// </summary>
    private void FindBox()
    {
        FollowPos = gameMode.GetBox().transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        switch (state)
        {
            //直接把自己加进某一个排队的队列.但是要等到了才能真正进入排队状态
            case State.前往:
                //在抵达之前虽然已经进入了排队队列,但是不受排队管理器控制.
                if (IsArrived)
                {
                    state = State.排队;
                }
                break;
            //检测到到了目标点,将自己加入排队管理器,等待排队完成,不做操作,这时候的目标完全由排队管理器控制...
            case State.排队:
                break;
            //采集中... 自己是不能进入采集模式的
            case State.采集:
                miningCD -= Time.deltaTime;
                if (miningCD < 0)
                {
                    mineCount++;
                    mine.mineCount--;
                    miningCD = miningInterval;
                    if (mineCount == 10)
                    {
                        mine.EndCollect();
                        //切换到送货状态,目标设定为箱子;
                        state = State.送货;
                        FindBox();
                    }
                }
                break;
            //前往送货箱
            case State.送货:
                if (IsArrived)
                {
                    //卸货 然后返回流程第一步
                    gameMode.GetBox().mineCount += mineCount;
                    mineCount = 0;
                    FindMine();
                    state = State.前往;
                }
                break;

        }
        if(!IsArrived)
        {
            MoveTo(FollowPos);
        }
    }
    //向目标地点移动,如果只差一步就停在目标地点 日后可以加一个isarriveed变量优化运算
    public void MoveTo(Vector3 v)
    {
        Vector3 d = v - transform.position;
        d.y = 0;
        Vector3 dis = d.normalized * moveSpeed * Time.deltaTime;
        if (d.sqrMagnitude > dis.sqrMagnitude)
        {
            transform.position = transform.position + dis;
        }
        else
        {
            IsArrived = true;
            transform.position = v;
        }
    }
}

/// <summary>
/// 行为类,存储机器人的行为列表
/// </summary>
public class Action
{
    //目标地点
    public Vector3 pos;
    //当到达目标地点后执行的操作
    public void OnArrive()
    {

    }
}