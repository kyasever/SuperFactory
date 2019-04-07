using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDTips : MonoBehaviour
{
    public float ShowDistanse = 60f;
    public float xOffset;
    public float yOffset;

    private RectTransform rectTransform;
    // Use this for initialization
    private GameObject UIobj;
    private Text textCom;

    public GameObject UIPrefeb;

    void Start()
    {
        UIobj = Instantiate(UIPrefeb, GameMode.Instanse.HUDParentTrans);

        rectTransform = UIobj.GetComponent<RectTransform>();

        textCom = UIobj.GetComponent<Text>();
    }

    public void SetText(string text)
    {
        textCom.text = text;
        }


    // Update is called once per frame
    void Update()
    {
        if ((transform.position - Camera.main.transform.position).sqrMagnitude > ShowDistanse * ShowDistanse)
        {
            rectTransform.gameObject.SetActive(false);
            return;
        }
        Vector2 player2DPosition = Camera.main.WorldToScreenPoint(transform.position);
        rectTransform.position = player2DPosition + new Vector2(xOffset, yOffset);

        //血条超出屏幕就不显示
        if (player2DPosition.x > Screen.width || player2DPosition.x < 0 || player2DPosition.y > Screen.height || player2DPosition.y < 0)
        {
            rectTransform.gameObject.SetActive(false);
        }
        else
        {
            rectTransform.gameObject.SetActive(true);
        }

    }
}
