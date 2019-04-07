using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotateSpeed = 3.0f;
    public float moveSpeed = 30.0f;
    public float upDownSpeed = 10.0f;

    public float mouseX, mouseY;

    public Transform CameraTrans;

    // Use this for initialization

    void Start()

    {

    }

    // Update is called once per frame

    void Update()

    {
        if(Input.GetMouseButton(1))
        {
            // 获得鼠标当前位置的X和Y

            mouseX = Input.GetAxis("Mouse X") * 1;

            mouseY = Input.GetAxis("Mouse Y") * 1;

            //父节点沿y旋转
            transform.localRotation = transform.localRotation * Quaternion.Euler(0, mouseX, 0);
            //子节点沿x旋转
            CameraTrans.localRotation = CameraTrans.localRotation * Quaternion.Euler(-mouseY, 0, 0);

        }

        int upDown = 0;
        if(Input.GetKey( KeyCode.LeftShift))
        {
            upDown--;
        }
        if(Input.GetKey( KeyCode.Space))
        {
            upDown++;
        }

        //Vector3 forward = Camera.main.transform.forward;
        //forward.y = 0;
        //Vector3 right = Camera.main.transform.right;
        //right.y = 0;

        //进行移动 wsad平移 shift上 space下
        transform.position += 
        (transform.forward * Input.GetAxis("Vertical") + 
            transform.right * Input.GetAxis("Horizontal") + 
        new Vector3(0,upDown,0))
            * moveSpeed * Time.deltaTime;
    }
}
