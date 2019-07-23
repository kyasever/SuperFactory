using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : KelObject {
    public bool isUsing = false;
    public int mineCount = 10000;
    public List<Robot> waitList = new List<Robot>();
    public Robot isMining;


    public void EndCollect()
    {
        isMining = null;
        if (waitList.Count > 0)
        {
            Robot r = waitList[0];
            if (r.IsArrived)
            {
                isUsing = true;
                r.state = Robot.State.采集;
                isMining = r;
                waitList.Remove(r);
            }
        }
        else
        {
            isUsing = false;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isUsing)
        {
            EndCollect();
        }
    }
}
