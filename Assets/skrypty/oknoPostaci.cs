using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class oknoPostaci : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] PasekDoswiadczenia;
    private TMP_Text Poziom;
    private Camera Kamera;
    int exp = 0,level=0;
    void Start()
    {
        Kamera = GameObject.Find("Kamera").GetComponent<Camera>();
        Poziom = GameObject.Find("Poziom").GetComponent<TMP_Text>();
        


        for (int i = 0; i < 32; i++)
        {
            
            GameObject.Find("Doswiadczenie (" + i + ")").GetComponent<Renderer>().enabled = false;
        }          


    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.P) && level<100)
        {

            GameObject.Find("Doswiadczenie (" + exp + ")").GetComponent<Renderer>().enabled = true;
            exp++;
            if (exp > 31)
            {
                level++;
               
                
                if (level<100)
                {
                    for (int i = 0; i < 32; i++)
                    {

                        GameObject.Find("Doswiadczenie (" + i + ")").GetComponent<Renderer>().enabled = false;

                    }
                    exp = 0;
                    Poziom.text = level.ToString();
                }
            }
        }
        

    }
    
}
