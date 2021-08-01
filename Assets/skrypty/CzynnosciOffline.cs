using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzynnosciOffline : MonoBehaviour
{
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float pitchMax = 75f,pitchMin=-75f;

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
        if (pitch < pitchMin)
            pitch = pitchMin;
        if (pitch > pitchMax)
            pitch = pitchMax;                                                     

        transform.eulerAngles = new Vector3(pitch, GameObject.Find("CameraPivot").transform.eulerAngles.y, 0.0f);
        GameObject.Find("CameraPivot").transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
        GameObject.Find(nowy.Nick).transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);

        //GameObject.Find("PatrzenieNaObiekt").GetComponent<Renderer>().enabled = false;

    }

    
}
