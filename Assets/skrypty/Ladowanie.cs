using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Ports;
using TMPro;
using System.Linq;
using UnityEngine.Networking;

public class Ladowanie : MonoBehaviour
{
    private GameObject pasek,napisy;
    float wartoscPaskaMax;
    string[] ports = SerialPort.GetPortNames();
    SerialPort arduino;
    bool Polaczenie = false,Czekanie=false, Test1=false,Test2=false;
    float IloscProcesow=2;
    int IloscWykonanychProcesow = 0;
    // Start is called before the first frame update
    void Start()
    {
        

        pasek = GameObject.Find("PasekLadowania");
        napisy = GameObject.Find("napis");
        napisy.GetComponent<TMP_Text>().text="Ładowanie";
        wartoscPaskaMax = pasek.transform.localScale.x;
        pasek.transform.localScale = new Vector3(0f, pasek.transform.localScale.y, pasek.transform.localScale.z);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Test1 == false)
        {
            napisy.GetComponent<TMP_Text>().text = "Sprawdzanie Arduino...";
            
            try
            {
                ports = SerialPort.GetPortNames();
                arduino = new SerialPort(ports[0], 115200);
                arduino.Open();
                Debug.Log(arduino.ReadLine());
         
                if (arduino.ReadLine() == "test")
                {
                    DodanieDoPaska();
                    Test1 = true;
                }
                
            }
            catch (System.Exception)
            {
                napisy.GetComponent<TMP_Text>().text = "Arduino nie podłączone";
            }
        }
        if (Test2 == false && Test1==true)
        {
            napisy.GetComponent<TMP_Text>().text = "Połączenie z serwerem...";
            try
            {
                if (!Polaczenie && !Czekanie)
                {
                    Czekanie = true;
                    StartCoroutine(GetDate("SELECT * FROM pozycje WHERE Nick='" + nowy.Nick + "'"));
                }
                
            }
            catch (System.Exception)
            {
            }
        }

        }
    void DodanieDoPaska()
    {
        if (IloscWykonanychProcesow < IloscProcesow)
        {
            pasek.transform.localScale = new Vector3(pasek.transform.localScale.x + (1 / IloscProcesow) * wartoscPaskaMax, pasek.transform.localScale.y, pasek.transform.localScale.z);
            IloscWykonanychProcesow++;
        }
        if(IloscWykonanychProcesow>=IloscProcesow)
            SceneManager.LoadScene(2);
        

    }

    IEnumerator GetDate(string Komenda)
    {
        
            WWWForm form = new WWWForm();
        form.AddField("Komenda", Komenda);
        using (UnityWebRequest www = UnityWebRequest.Post("https://serwer2131273.home.pl/Czytanie.php", form))
        {
           
                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);

                }
                else
                {
                    string dane = www.downloadHandler.text;
                    string[] PodzielenieTablicy = dane.Split(char.Parse(" "));
                    if (PodzielenieTablicy[4] == "1")
                    {
                   
                    DodanieDoPaska();
                    Test2 = true;   
                    }
                    Debug.Log(dane);
                }
                if(www.isDone)
                Polaczenie = www.isDone;
            Czekanie = false;
        }

    }
  
   
}
