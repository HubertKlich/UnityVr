using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObslugaAudio : MonoBehaviour
{
    static float AudioTime=0;
    // Start is called before the first frame update
    void Start()
    {
       Cursor.lockState = CursorLockMode.Locked;
        GameObject.Find("Audio Source").GetComponent<AudioSource>().time= AudioTime;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("Audio Source").GetComponent<AudioSource>().volume = MenuWzrok.selectedGlosnosc / 100;
        AudioTime=GameObject.Find("Audio Source").GetComponent<AudioSource>().time;
    }
}
