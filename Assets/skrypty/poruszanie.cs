using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine.Networking;

class Postac
{
    public String Nick, Haslo, Danex, Danez, y;
    public float dist = 0;
   
}
class InniGracze
{
    public GameObject NazwaGracza, Gracz, Kanwa;
    public string Online = "1";

}
class Zmienne
{
    public float moveSpeed = 1f;
    public bool Polaczenie = false;
    public string dane = null;
    public string[] PodzielenieTablicy;

}
public class poruszanie : MonoBehaviour
{

    
    
    
   
    Postac KlasaGracza = new Postac();
    InniGracze KlasaInnychGraczy = new InniGracze();
    Zmienne KlasaZmiennych = new Zmienne();
    public struct data
    {
        public string NickDane, xDane, yDane, zDane;
        public int numer;
        public bool Online;

    }


    List<data> Gracze;




    void Start()
    {

        KlasaGracza.Nick= nowy.Nick;
        KlasaGracza.Danex = nowy.x;
        KlasaGracza.Danez = nowy.z;
        KlasaInnychGraczy.NazwaGracza = GameObject.Find("Nazwa");
        KlasaInnychGraczy.Kanwa = GameObject.Find("Canvas");
        TMP_Text Nazwa = KlasaInnychGraczy.NazwaGracza.GetComponent<TMP_Text>();

        if (KlasaGracza.Nick != null)
        {
            Nazwa.text = KlasaGracza.Nick;
            Nazwa.name = KlasaGracza.Nick + "Tag";
            KlasaInnychGraczy.Gracz = GameObject.Find("Gracz");
            KlasaInnychGraczy.Gracz.name = KlasaGracza.Nick;
            KlasaInnychGraczy.Gracz.transform.position = new Vector3(float.Parse(KlasaGracza.Danex), 0, float.Parse(KlasaGracza.Danez));
            
        }
      
       // GameObject.Find("Kamera").transform.SetParent(Gracz.GetComponent<Transform>());
    }


    public void Update()
    {

        
        Ruch();

        GameObject.Find("Kamera").transform.position = new Vector3(KlasaInnychGraczy.Gracz.transform.position.x, GameObject.Find("Kamera").transform.position.y, KlasaInnychGraczy.Gracz.transform.position.z);
        KlasaInnychGraczy.NazwaGracza.transform.position = new Vector3(KlasaInnychGraczy.Gracz.transform.position.x, KlasaInnychGraczy.NazwaGracza.transform.position.y, KlasaInnychGraczy.NazwaGracza.transform.position.z);
        if (!KlasaZmiennych.Polaczenie)
        {
            StartCoroutine(GetDate());
        }
       
        

        UstawianieInnychGraczy();
                      


            }




            void Ruch()
    {
       
            if (Input.GetKey(KeyCode.A))
            {
            GameObject.Find(KlasaGracza.Nick).transform.position -= GameObject.Find(KlasaGracza.Nick).transform.right * Time.deltaTime * 5;
        }
            if (Input.GetKey(KeyCode.D))
            {
            GameObject.Find(KlasaGracza.Nick).transform.position += GameObject.Find(KlasaGracza.Nick).transform.right * Time.deltaTime * 5;
        }
        if (Input.GetKey(KeyCode.S))
        {
            GameObject.Find(KlasaGracza.Nick).transform.position -= GameObject.Find(KlasaGracza.Nick).transform.forward * Time.deltaTime * 5;
        }
        if (Input.GetKey(KeyCode.W))
        {
            // KlasaGracza.Danez = (KlasaInnychGraczy.Gracz.transform.position.z + 5).ToString();
            GameObject.Find(KlasaGracza.Nick).transform.position += GameObject.Find(KlasaGracza.Nick).transform.forward * Time.deltaTime * 5;
        }

        KlasaGracza.Danez = GameObject.Find(KlasaGracza.Nick).transform.position.z.ToString();
        KlasaGracza.Danex = GameObject.Find(KlasaGracza.Nick).transform.position.x.ToString();

    }


    IEnumerator GetDate()
    {
        KlasaZmiennych.Polaczenie = true;
        WWWForm form = new WWWForm();
        form.AddField("Komenda", "UPDATE pozycje SET x ='"+ KlasaGracza.Danex + "', z ='" + KlasaGracza.Danez + "', Online='" + KlasaInnychGraczy.Online + "' WHERE Nick='"+ KlasaGracza.Nick + "'");
        using (UnityWebRequest www = UnityWebRequest.Post("https://serwer2131273.home.pl/Calosc.php",form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                KlasaZmiennych.dane = www.downloadHandler.text;
            }
        }
        KlasaZmiennych.Polaczenie = false;
    }

    void UstawianieInnychGraczy()
    {
        

        if (KlasaZmiennych.dane != null)
        {
            if (Gracze == null)
                Gracze = new List<data>();
            if (Gracze.Count > 0)
                Gracze.Clear();

            string[] podzielone = KlasaZmiennych.dane.Replace("</br>", ";").Split(char.Parse(";"));

            for (int k = 0; k < Convert.ToInt32(podzielone.Count() - 1); k++)
            {
                KlasaZmiennych.PodzielenieTablicy = podzielone[k].Split(char.Parse(" "));
                if (GameObject.Find(KlasaZmiennych.PodzielenieTablicy[1]) != null){
                    KlasaGracza.dist = Vector3.Distance(GameObject.Find(KlasaZmiennych.PodzielenieTablicy[1]).transform.position, GameObject.Find(KlasaGracza.Nick).transform.position);
                }
                else
                {
                    KlasaGracza.dist = Math.Abs(float.Parse(KlasaZmiennych.PodzielenieTablicy[2])- KlasaInnychGraczy.Gracz.transform.position.x);
                }
                if (KlasaGracza.dist < 5 && KlasaZmiennych.PodzielenieTablicy[5] == "1")
                {

                    Debug.Log(KlasaZmiennych.PodzielenieTablicy[1]);

                    data itm = new data();
                    itm.numer = int.Parse(KlasaZmiennych.PodzielenieTablicy[0]);
                    itm.NickDane = KlasaZmiennych.PodzielenieTablicy[1];
                    itm.xDane = KlasaZmiennych.PodzielenieTablicy[2];
                    itm.yDane = KlasaZmiennych.PodzielenieTablicy[3];
                    itm.zDane = KlasaZmiennych.PodzielenieTablicy[4];
                    itm.Online = true;
                    Gracze.Add(itm);
                    if (GameObject.Find(itm.NickDane) != null && GameObject.Find(itm.NickDane + "Tag") != null && !GameObject.Find(KlasaGracza.Nick))
                    {
                        GameObject.Find(itm.NickDane).transform.position = Vector3.Lerp(GameObject.Find(itm.NickDane).transform.position, new Vector3(float.Parse(itm.xDane), 0, float.Parse(itm.zDane)), Time.deltaTime);
                        GameObject.Find(itm.NickDane + "Tag").transform.position = new Vector3(GameObject.Find(itm.NickDane).transform.position.x, GameObject.Find(itm.NickDane).transform.position.y+5, GameObject.Find(itm.NickDane).transform.position.z);

                        
                    }
                    Gracze.Add(itm);
                   

                }
                else
                {
                    UsuwanieGracza();
                }

            



                }

            }
        
        if (Gracze != null)
        {
            if (Gracze.Count > 0)
            {

                foreach (data itm in Gracze)
                {
                    if (itm.NickDane!= KlasaGracza.Nick)
                    {

                        if (GameObject.Find(itm.NickDane) == null && GameObject.Find(itm.NickDane + "Tag") == null)
                        {

                            if (GameObject.Find(KlasaGracza.Nick + "(Clone)") == null)
                            {
                                Instantiate(GameObject.Find(KlasaGracza.Nick), new Vector3(float.Parse(itm.xDane), float.Parse(itm.yDane), float.Parse(itm.zDane)), Quaternion.identity);
                                GameObject.Find(KlasaGracza.Nick + "(Clone)").name = itm.NickDane;
                                Instantiate(GameObject.Find(KlasaGracza.Nick +"Tag"), new Vector3(GameObject.Find(itm.NickDane).transform.position.x, 1.5f, 0), Quaternion.identity);
                                GameObject.Find(KlasaGracza.Nick +"Tag(Clone)").name = itm.NickDane + "Tag";
                                GameObject.Find(itm.NickDane + "Tag").GetComponent<TMP_Text>().text = itm.NickDane;
                                GameObject.Find(itm.NickDane + "Tag").transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>());

                            }
                        }

                    }
                }
            }
        }
    }
           void UsuwanieGracza()
    {
        if (GameObject.Find(KlasaZmiennych.PodzielenieTablicy[1]) && KlasaZmiennych.PodzielenieTablicy[1] != KlasaGracza.Nick)
        {
            Destroy(GameObject.Find(KlasaZmiennych.PodzielenieTablicy[1]));
            Destroy(GameObject.Find(KlasaZmiennych.PodzielenieTablicy[1] + "Tag"));
            if (Gracze != null)
            {
                if (Gracze.Count > 0)
                {
                    foreach (data itm1 in Gracze)
                    {
                        if (itm1.NickDane == KlasaZmiennych.PodzielenieTablicy[1])
                        {

                            Gracze.Remove(itm1);

                        }
                    }
                }
            }
        }
    }
    IEnumerator ExampleCoroutine(int czas)
    {
       
        yield return new WaitForSeconds(czas);


    }

    void OnApplicationQuit()
    {
        KlasaInnychGraczy.Online = "0";
        StartCoroutine(GetDate());
        StartCoroutine(ExampleCoroutine(5));
    }
}
