using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// kelframework的组件,有很多默认方法
/// </summary>
public class KelComponent : MonoBehaviour
{
    protected KelMovement kelMovement;
    protected KelObject kelObject;
    protected GameMode gameMode;
    protected Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    private void Awake()
    {
        gameMode = GameMode.Instanse;
        kelObject = GetComponent<KelObject>();
        kelMovement = GetComponent<KelMovement>();
    }
}
