using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour {
    public static GameMode Instanse { get; private set; }

    public Transform HUDParentTrans;

    void Awake()
    {
        Instanse = this;

        for (int i = 0; i < mineCount; i++)
        {
            Vector3 p = new Vector3(-13, 0, -30) + new Vector3(Random.Range(-100f, 100f), 0, Random.Range(-100f, 100f));
            Mine m = Instantiate(minePrefeb, p, Quaternion.identity).GetComponent<Mine>();
            mineList.Add(m);
        }
        for (int i = 0; i < robotCount; i++)
        {
            Vector3 p = new Vector3(10, 0, 10) + new Vector3(Random.Range(-300f, 300f), 0, Random.Range(-300f, 300f));
            Robot m = Instantiate(robotPrefeb, p, Quaternion.identity).GetComponent<Robot>();
        }

    }

    public List<Box>  boxList = new List<Box>(); 
    public List<Mine> mineList = new List<Mine>();

    public GameObject minePrefeb;
    public GameObject robotPrefeb;

    public int mineCount = 10;
    public int robotCount = 10;

    public Box GetBox()
    {
        return boxList[0];
    }
    //获取一个矿点 遍历.如果没有排队 直接返回,如果排队中,那么返回队伍最短的那个...
    //考虑直接排序然后返回0 这个其实挺稳妥的
    public Mine GetMine()
    {
        mineList.Sort((Mine a, Mine b) =>
        {
            if (a.waitList.Count < b.waitList.Count)
            {
                return -1;
            }
            else if (a.waitList.Count == b.waitList.Count)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        });
        return mineList[0];
    }

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
