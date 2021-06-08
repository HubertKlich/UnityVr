using UnityEngine;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.Networking;

public class nowy : MonoBehaviour
{
    private GameObject PoleNick,PoleHaslo,PanelLogowania, PoleNickRej, PoleHasloRej, PolePowtorzHaslo;
    private GameObject Rej, Log,Menu;
    static public String Nick,Haslo,Online,x,y,z;
    string myLog;
    Queue myLogQueue = new Queue();

    void Start()
    {
        

        PanelLogowania = GameObject.Find("Calosc");
        PanelLogowania.transform.position = new Vector3(0, 0, 0);
        Menu = GameObject.Find("Menu");
        Rej = GameObject.Find("Rejestracja");
        Log = GameObject.Find("Logowanie");

        GameObject.Find("PrzyciskLogowania").GetComponent<Button>().onClick.AddListener(Logowanie);
        GameObject.Find("Zarejestruj").GetComponent<Button>().onClick.AddListener(Rejestracja);
        GameObject.Find("WROCzRej").GetComponent<Button>().onClick.AddListener(PrzejdzDoMenu);
        GameObject.Find("WROCzLog").GetComponent<Button>().onClick.AddListener(PrzejdzDoMenu);                  
        GameObject.Find("START").GetComponent<Button>().onClick.AddListener(PrzejdzDoLogowania);
        GameObject.Find("ZAREJESTRUJ").GetComponent<Button>().onClick.AddListener(PrzejdzDoRejestracji);
         GameObject.Find("WYJDZ").GetComponent<Button>().onClick.AddListener(OnApplicationQuit);

        Menu.SetActive(true);
        Log.SetActive(false);
        Rej.SetActive(false);

       
    }

    public void Logowanie()
    {
        PoleNick = GameObject.Find("Nick");
        TMP_InputField poleNicku = PoleNick.GetComponent<TMP_InputField>();
        PoleHaslo = GameObject.Find("Haslo");
        TMP_InputField poleHasla = PoleHaslo.GetComponent<TMP_InputField>();
        StartCoroutine(GetDate("UPDATE pozycje SET Online =1 WHERE Nick='" + poleNicku.text + "' AND Haslo='" + poleHasla.text + "'",false, "serwer2131273.home.pl/Logowanie.php"));
        StartCoroutine(GetDate("SELECT * FROM pozycje WHERE Nick='" + poleNicku.text + "' AND Haslo='" + poleHasla.text + "'", true, "serwer2131273.home.pl/Czytanie.php"));
    }
    public void Rejestracja()
    {
        PoleNickRej = GameObject.Find("NickRej");
        TMP_InputField poleNicku = PoleNickRej.GetComponent<TMP_InputField>();
        PoleHasloRej = GameObject.Find("HasloRej");
        TMP_InputField poleHasla = PoleHasloRej.GetComponent<TMP_InputField>();
        PolePowtorzHaslo = GameObject.Find("Powtorzenie");
        TMP_InputField polePotorzenia = PolePowtorzHaslo.GetComponent<TMP_InputField>();
        if (poleHasla.text == polePotorzenia.text)
        {

            StartCoroutine(GetDate("insert into pozycje(ID) select(max(ID) + 1) from pozycje",false, "serwer2131273.home.pl/Logowanie.php"));
            StartCoroutine(GetDate("UPDATE pozycje SET Nick='" + poleNicku.text + "', Haslo='" + poleHasla.text + "', Online='0', x='0', y='0', z='0' WHERE Nick=''",false, "serwer2131273.home.pl/Logowanie.php"));

        }
    }
    public void PrzejdzDoMenu()
    {
        Menu.SetActive(true);
        Log.SetActive(false);
        Rej.SetActive(false);

    }
    public void PrzejdzDoLogowania()
    {
       
        Menu.SetActive(false);
        Log.SetActive(true);
        GameObject.Find("Nick").GetComponent<TMP_InputField>().text = "Rupim";
        GameObject.Find("Haslo").GetComponent<TMP_InputField>().text = "Szpagaciara1";
    }
    public void PrzejdzDoRejestracji()
    {
        Menu.SetActive(false);
        Rej.SetActive(true);

    }

    private void Update()
    {
        
    }
    public void OnApplicationQuit()
    {
        Application.Quit();
    }
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        myLog = logString;
        string newString = "\n [" + type + "] : " + myLog;
        myLogQueue.Enqueue(newString);
        if (type == LogType.Exception)
        {
            newString = "\n" + stackTrace;
            myLogQueue.Enqueue(newString);
        }
        myLog = string.Empty;
        foreach (string mylog in myLogQueue)
        {
            myLog += mylog;
        }
    }
    IEnumerator GetDate(string Komenda,bool CzyLogowanie,string Serwer)
    {

        WWWForm form = new WWWForm();
        form.AddField("Komenda", Komenda);
        using (UnityWebRequest www = UnityWebRequest.Post(Serwer, form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
               
                }
            else
            {
                if (CzyLogowanie)
                {
                    string dane = www.downloadHandler.text;
                    string[] PodzielenieTablicy = dane.Split(char.Parse(" "));
                    Nick = PodzielenieTablicy[0];
                    x = PodzielenieTablicy[1];
                    z = PodzielenieTablicy[3];
                    SceneManager.LoadScene(1);

                }
            }
        }

    }
    void OnGUI()
    {
        GUILayout.Label(myLog);
    }

}