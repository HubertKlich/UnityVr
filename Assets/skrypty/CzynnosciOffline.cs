using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzynnosciOffline : MonoBehaviour
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
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        GameObject.Find(nowy.Nick).transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);

        //GameObject.Find("PatrzenieNaObiekt").GetComponent<Renderer>().enabled = false;

    }
    
}
