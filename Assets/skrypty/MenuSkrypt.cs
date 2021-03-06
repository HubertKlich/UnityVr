using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuSkrypt : MonoBehaviour
{
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
   
    // Start is called before the first frame update
    void Start()
    {

       
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        yaw += speedH * Input.GetAxis("Mouse X") * Time.timeScale;
        pitch -= speedV * Input.GetAxis("Mouse Y") * Time.timeScale;
        if (pitch > 90)
        {
            pitch = 90;
        }
        if (pitch < -90)
        {
            pitch = -90;
        }
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);


    }


}
