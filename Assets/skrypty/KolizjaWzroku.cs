using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KolizjaWzroku : MonoBehaviour
{
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("PodpisyObiektow").GetComponent<Renderer>().enabled = false;
        GameObject.Find("WyswietlanieNapisow").GetComponent<TMP_Text>().enabled = false;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("ObiektyPodswietlane"))
        {
            go.AddComponent<Outline>();
            go.GetComponent<Outline>().enabled = false;
            go.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;
            go.GetComponent<Outline>().OutlineColor = Color.red;
            go.GetComponent<Outline>().OutlineWidth = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            GameObject.Find("PodpisyObiektow").transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 1.5f, target.transform.position.z);
            GameObject.Find("PodpisyObiektow").transform.LookAt(GameObject.Find("Kamera").GetComponent<Transform>());
            GameObject.Find("WyswietlanieNapisow").transform.position = new Vector3(GameObject.Find("PodpisyObiektow").transform.position.x, GameObject.Find("PodpisyObiektow").transform.position.y, GameObject.Find("PodpisyObiektow").transform.position.z);
            GameObject.Find("WyswietlanieNapisow").transform.rotation = Quaternion.Euler(new Vector3(GameObject.Find("PodpisyObiektow").transform.eulerAngles.x, GameObject.Find("PodpisyObiektow").transform.eulerAngles.y + 180, GameObject.Find("PodpisyObiektow").transform.eulerAngles.z));
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "ObiektyPodswietlane")
        {
            target = collision.gameObject;


            GameObject.Find("WyswietlanieNapisow").GetComponent<TMP_Text>().text= target.name;
            GameObject.Find("PodpisyObiektow").GetComponent<Renderer>().enabled = true;
            GameObject.Find("WyswietlanieNapisow").GetComponent<TMP_Text>().enabled = true;
            target.GetComponent<Outline>().enabled = true;
        }

        
        
    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "ObiektyPodswietlane")
        {
            GameObject.Find("PodpisyObiektow").GetComponent<Renderer>().enabled = false;
            GameObject.Find("WyswietlanieNapisow").GetComponent<TMP_Text>().enabled = false;
            target.GetComponent<Outline>().enabled = false;
        }



    }
}
