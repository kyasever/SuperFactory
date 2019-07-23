using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//这个东西是Unity的Com,但是相当于kelframework的Object
//提供其他组件的中介者
public class KelObject : MonoBehaviour
{
    public KelMovement KelMovement { get; private set; }
    public GameMode GameMode;

    public void MoveTo(Vector3 to)
    {
        KelMovement.MoveTo(to);
    }

    private Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    private void Awake()
    {
        KelMovement = GetComponent<KelMovement>();
        GameMode = GameMode.Instanse;
    }




}

