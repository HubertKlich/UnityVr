using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class oknoPostaci : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] PasekDoswiadczenia;
    private TMP_Text Poziom;
    private Camera Kamera;
    int exp = 0,level=0;
    public RectTransform m_parent;
    public Camera m_uiCamera;
    public RectTransform m_image;
    public List<Image> Images;                                       
    void Start()
    {
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_parent, new Vector3(Screen.width/ 15, Screen.height-Screen.width / 15, 1.0f), m_uiCamera, out anchoredPos);
        m_image.anchoredPosition = anchoredPos;
        Kamera = GameObject.Find("Kamera").GetComponent<Camera>();
        Poziom = GameObject.Find("Poziom").GetComponent<TMP_Text>();


        for (int i = 0; i < 36; i++)
        {

            Images[i].enabled = false;
        }


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.P) && level<100)
        {

            Images[exp].enabled = true;
            exp++;
            if (exp > 35)
            {
                level++;
               
                
                if (level<100)
                {
                    for (int i = 0; i < 36; i++)
                    {

                        Images[i].enabled = false;

                    }
                    exp = 0;
                    Poziom.text = level.ToString();
                }
            }
        }
        

    }
    
}
