using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuWzrok : MonoBehaviour
{
    private GameObject target, select, Menu, Rozdzielczosc, Wymiary;
    public Shader gradient;
    float x = 1f;
    List<GameObject> grafikaKolorowe = new List<GameObject>();
    List<GameObject> dzwiekKolorowe = new List<GameObject>();
    List<string> grafika = new List<string>();
    List<string> dzwiek = new List<string>();
    List<string> rozdzielczosci = new List<string>();
    int selected = 0, selectedRozdzielczosc = 0;
    float selectedGlosnosc = 100;
    bool kolizjaWyswietlenie = false;
    string nazwaObiektuZaznaczonego="";
    public Texture2D teksturaGrafiki;
    // Start is called before the first frame update
    void Start()
    {
        Menu = GameObject.Find("Menu");
        Rozdzielczosc = GameObject.Find("Rozdzielczosc");
        Wymiary = GameObject.Find("Wymiary");
        grafikaKolorowe.Add(Wymiary);
        dzwiekKolorowe.Add(Wymiary);
        grafika.Add("GRAFIKA");
        grafika.Add("Rozdzielczosc");
        grafika.Add("1920x1080");
        dzwiek.Add("DZWIEK");
        dzwiek.Add("Glosnosc");
        dzwiek.Add(selectedGlosnosc.ToString()+"%");
        Menu.SetActive(false);
        rozdzielczosci.Add("1920x1080");
        rozdzielczosci.Add("800x600");
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("ObiektyPodswietlane"))
        {
            go.GetComponent<Renderer>().material.shader = gradient;
            go.GetComponent<Renderer>().material.SetFloat("_DissolveAmount", 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //GameObject.Find("Dzwiek").transform.LookAt(GameObject.Find("Main Camera").GetComponent<Transform>());
        if (kolizjaWyswietlenie)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                selected++;
                if (selected > grafikaKolorowe.Count - 1)
                {
                    selected = 0;
                }
                select.GetComponent<TMP_Text>().faceColor = Color.white;
                select = grafikaKolorowe[selected];
                select.GetComponent<TMP_Text>().faceColor = Color.red;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                selected--;
                if (selected < 0)
                {
                    selected = grafikaKolorowe.Count - 1;
                }
                select.GetComponent<TMP_Text>().faceColor = Color.white;
                select = grafikaKolorowe[selected];
                select.GetComponent<TMP_Text>().faceColor = Color.red;
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (nazwaObiektuZaznaczonego == "Grafika")
                {
                    selectedRozdzielczosc++;
                    if (selectedRozdzielczosc > rozdzielczosci.Count - 1)
                    {
                        selectedRozdzielczosc = 0;
                    }
                    grafika[2] = rozdzielczosci[selectedRozdzielczosc];
                    grafikaKolorowe[selected].GetComponent<TMP_Text>().text = rozdzielczosci[selectedRozdzielczosc];
                }
                if (nazwaObiektuZaznaczonego == "Dzwiek")
                {
                    selectedGlosnosc++;
                    if (selectedGlosnosc > 100)
                    {
                        selectedGlosnosc = 0;
                    }
                    dzwiek[2] = selectedGlosnosc.ToString()+"%";
                    grafikaKolorowe[selected].GetComponent<TMP_Text>().text = selectedGlosnosc.ToString() + "%";
                    GameObject.Find("Audio Source").GetComponent<AudioSource>().volume = selectedGlosnosc / 100;
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (nazwaObiektuZaznaczonego == "Grafika")
                {
                    selectedRozdzielczosc--;
                    if (selectedRozdzielczosc < 0)
                    {
                        selectedRozdzielczosc = rozdzielczosci.Count - 1;
                    }
                    grafika[2] = rozdzielczosci[selectedRozdzielczosc];
                    grafikaKolorowe[selected].GetComponent<TMP_Text>().text = rozdzielczosci[selectedRozdzielczosc];
                }
                if (nazwaObiektuZaznaczonego == "Dzwiek")
                {
                    selectedGlosnosc--;
                    if (selectedGlosnosc <0)
                    {
                        selectedGlosnosc = 100;
                    }
                    dzwiek[2] = selectedGlosnosc.ToString() + "%";
                    grafikaKolorowe[selected].GetComponent<TMP_Text>().text = selectedGlosnosc.ToString() + "%";
                    GameObject.Find("Audio Source").GetComponent<AudioSource>().volume = selectedGlosnosc / 100;
                }
            }

        }
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "ObiektyPodswietlane")
        {

            target = collision.gameObject;
            nazwaObiektuZaznaczonego = target.name;
            select = grafikaKolorowe[selected];
            select.GetComponent<TMP_Text>().faceColor = Color.red;
            if (target.name == "Grafika")
            {
                Menu.GetComponent<TMP_Text>().text = grafika[0];
                Rozdzielczosc.GetComponent<TMP_Text>().text = grafika[1];
                Wymiary.GetComponent<TMP_Text>().text = grafika[2];
            }
            if (target.name == "Dzwiek")
            {
                Menu.GetComponent<TMP_Text>().text = dzwiek[0];
                Rozdzielczosc.GetComponent<TMP_Text>().text = dzwiek[1];
                Wymiary.GetComponent<TMP_Text>().text = dzwiek[2];
            }

        }



    }
    void OnTriggerStay(Collider collision)
    {
        target = collision.gameObject;
        Menu.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 2.5f, target.transform.position.z);
        Rozdzielczosc.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 2.25f, target.transform.position.z);
        Wymiary.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 2f, target.transform.position.z);
        Menu.transform.LookAt(GameObject.Find("Main Camera").GetComponent<Transform>());
        if (collision.gameObject.tag == "ObiektyPodswietlane")
        {

            
            if (target.GetComponent<Renderer>().material.GetFloat("_DissolveAmount") > -1f)
            {
                x = x - 0.01f;
                target.GetComponent<Renderer>().material.SetFloat("_DissolveAmount", x); ;
            }
        }

        if (target.GetComponent<Renderer>().material.GetFloat("_DissolveAmount") < .5f)
        {
            kolizjaWyswietlenie = true;
            Menu.SetActive(true);

           
        }

    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "ObiektyPodswietlane")
        {
            kolizjaWyswietlenie = false;
            x = 1f;
            Menu.SetActive(false);
            target.GetComponent<Renderer>().material.SetFloat("_DissolveAmount", x);

        }



    }

}
