using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;
using TMPro;

public class RuchyReki : MonoBehaviour
{
    // Start is called before the first frame update
    float[,] zgieciaPalcow = new float[5, 3];
    float offsetX = 0, offsetY = 0, offsetZ=0;
    public GameObject target; 
    public float factor = 7;
    public bool enableRotation;
    public bool enableTranslation;
    string indata;
    string[] ports = SerialPort.GetPortNames();
    string dataString;
    float[] wartosci = new float[4];
    float[] hq = null;
    float[] Euler = new float[3];
    float f;
    float[] zgiecia = new float[5];
    float[] zgieciaMIN = new float[5];
    float[] zgieciaMAX = new float[5];
    string[] hex = new string[9];
    GameObject kalibracja,kalibText;
    SerialPort arduino;
    void Start()
    {
        kalibracja = GameObject.Find("kalibracja");
        kalibText = GameObject.Find("KalibracjaTekst");
        try
        {
            ports = SerialPort.GetPortNames();
            arduino = new SerialPort(ports[0], 115200);
            
            arduino.Open();
            Debug.Log("Polaczone");

        }
        catch (System.Exception)
        {

        }


    }
  

        // Update is called once per frame
        void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && kalibracja.activeSelf)
        {

            kalibracja.SetActive(false);
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && !kalibracja.activeSelf)
        {

            kalibracja.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        kalibText.GetComponent<TextMeshProUGUI>().text="Wcisnij \"P\"\n"+ (-Euler[2] * (float)(180 / Math.PI) - offsetX )+ " " +(-Euler[1] * (float)(180 / Math.PI) - offsetY )+ " "+(-Euler[0] * (float)(180 / Math.PI) - offsetZ);
        if (arduino.IsOpen)
          {
                                                      
              try
              {

                indata = arduino.ReadLine();
               // Debug.Log(indata);
                if (indata.Contains("!"))
                {
                    hex[0]=indata.Substring(1, 8);
                    hex[1] = indata.Substring(9, 8);
                }
                else if (indata.Contains("@"))
                {
                    hex[2] = indata.Substring(1, 8);
                    hex[3] = indata.Substring(9, 8);
                }
                else if (indata.Contains("#"))
                {
                    hex[4] = indata.Substring(1, 2);
                    hex[5] = indata.Substring(3, 2);
                    hex[6] = indata.Substring(5, 2);
                    hex[7] = indata.Substring(7, 2);
                    hex[8] = indata.Substring(9, 2);
                }

                    wartosci[0] = ZamianaZHex(hex[0]);
                    wartosci[1] = ZamianaZHex(hex[1]);
                    wartosci[2] = ZamianaZHex(hex[2]);
                    wartosci[3] = ZamianaZHex(hex[3]);
                zgiecia[0] = Convert.ToInt32(hex[4], 16);
                zgiecia[1] = Convert.ToInt32(hex[5], 16);
                zgiecia[2] = Convert.ToInt32(hex[6], 16);
                zgiecia[3] = Convert.ToInt32(hex[7], 16);
                zgiecia[4] = Convert.ToInt32(hex[8], 16);
                //Debug.Log(zgiecia[0]+" "+zgiecia[1]+" "+zgiecia[2]+" "+zgiecia[3]+" "+zgiecia[4]);
                //   Debug.Log(wartosci[0] + " " + wartosci[1] + " " + wartosci[2] + " " + wartosci[3]);

                if (hq != null)
                      {
                          quaternionToEuler(quatProd(hq, wartosci), Euler);
                      }
                      else
                      {
                          quaternionToEuler(wartosci, Euler);
                      }
                //-Euler[1] * (float)(180 / Math.PI), -Euler[2] * (float)(180 / Math.PI), 0

               

                if (Input.GetKey(KeyCode.U))
                {
                  
                        offsetX = -Euler[2] * (float)(180 / Math.PI);
                  
                        offsetY = -Euler[1] * (float)(180 / Math.PI);
                 
                        offsetZ = -Euler[0] * (float)(180 / Math.PI);
                    
                }
                if (Input.GetKey(KeyCode.T))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        zgieciaMIN[i] = zgiecia[i];
                    }

                }
                if (Input.GetKey(KeyCode.Y))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        zgieciaMAX[i] = zgiecia[i];
                    }

                }
           //     Debug.Log(zgieciaMIN[0] + " " + zgieciaMAX[0] + " " + zgieciaMIN[1] + " " + zgieciaMAX[1] + " " + zgieciaMIN[2] + " " + zgieciaMAX[2] + " " + zgieciaMIN[3] + " " + zgieciaMAX[3] + " " + zgieciaMIN[4] + " " + zgieciaMAX[4]);
                //GameObject.Find("RightHand").transform.localRotation = Quaternion.Euler(-Euler[2] * (float)(180 / Math.PI)-offsetX, -Euler[1] * (float)(180 / Math.PI)-offsetY, -Euler[0] * (float)(180 / Math.PI)-offsetZ);
                GameObject.Find("RightHand").transform.localRotation = Quaternion.Euler(Euler[1] * (float)(180 / Math.PI) - offsetX, -Euler[2] * (float)(180 / Math.PI) - offsetY+60, 0);

                //  Debug.Log(Euler[0]+" "+ Euler[1]+" "+Euler[1] * (float)(180 / Math.PI) + " "+ Euler[2] * (float)(180 / Math.PI));


            }
              catch (System.IO.IOException ioe)
              {
                  Debug.Log("IOException: " + ioe.Message);
              }
          }
          else
              dataString = "NOT OPEN";    
       //  Debug.Log(zgiecia[0] + " " + zgiecia[1] + " " + zgiecia[2] + " " + zgiecia[3] + " " + zgiecia[4]);

                                                 //wskazujacy
        zgieciaPalcow[0,0] = (20 / (zgieciaMIN[0] - zgieciaMAX[0]) )*( zgieciaMIN[0] - zgiecia[0]);
        zgieciaPalcow[0,1] = (70 / (zgieciaMIN[0] - zgieciaMAX[0])) * (zgieciaMIN[0] - zgiecia[0]);
        zgieciaPalcow[0,2] = (50 / (zgieciaMIN[0] - zgieciaMAX[0])) * (zgieciaMIN[0] - zgiecia[0]);
                                                              //kciuk
        zgieciaPalcow[1,0] = 31-(41 / (zgieciaMIN[1] - zgieciaMAX[1])) * (zgieciaMIN[1] - zgiecia[1]);
        zgieciaPalcow[1,1] = (-20 / (zgieciaMIN[1] - zgieciaMAX[1])) * (zgieciaMIN[1] - zgiecia[1]);
        zgieciaPalcow[1,2] = (-20 / (zgieciaMIN[1] - zgieciaMAX[1])) * (zgieciaMIN[1] - zgiecia[1]);
        //srodkowy
        zgieciaPalcow[2, 0] = (20 / (zgieciaMIN[2] - zgieciaMAX[2])) * (zgieciaMIN[2] - zgiecia[2]);
        zgieciaPalcow[2, 1] = (70 / (zgieciaMIN[2] - zgieciaMAX[2])) * (zgieciaMIN[2] - zgiecia[2]);
        zgieciaPalcow[2, 2] = (50 / (zgieciaMIN[2] - zgieciaMAX[2])) * (zgieciaMIN[2] - zgiecia[2]);
        //serdeczny
        zgieciaPalcow[3, 0] = (20 / (zgieciaMIN[3] - zgieciaMAX[3])) * (zgieciaMIN[3] - zgiecia[3]);
        zgieciaPalcow[3, 1] = (70 / (zgieciaMIN[3] - zgieciaMAX[3])) * (zgieciaMIN[3] - zgiecia[3]);
        zgieciaPalcow[3, 2] = (50 / (zgieciaMIN[3] - zgieciaMAX[3])) * (zgieciaMIN[3] - zgiecia[3]);
        //maly
        zgieciaPalcow[4, 0] = (10 / (zgieciaMIN[4] - zgieciaMAX[4])) * (zgieciaMIN[4] - zgiecia[4]);
        zgieciaPalcow[4, 1] = (50 / (zgieciaMIN[4] - zgieciaMAX[4])) * (zgieciaMIN[4] - zgiecia[4]);
        zgieciaPalcow[4, 2] = (40 / (zgieciaMIN[4] - zgieciaMAX[4])) * (zgieciaMIN[4] - zgiecia[4]);

        GameObject.Find("RightHandIndex1").transform.localRotation = Quaternion.Euler(zgieciaPalcow[0,0], GameObject.Find("RightHandIndex1").transform.localRotation.eulerAngles.y, GameObject.Find("RightHandIndex1").transform.localRotation.eulerAngles.z);
        GameObject.Find("RightHandIndex2").transform.localRotation = Quaternion.Euler(zgieciaPalcow[0,1], GameObject.Find("RightHandIndex2").transform.localRotation.eulerAngles.y, GameObject.Find("RightHandIndex2").transform.localRotation.eulerAngles.z);
        GameObject.Find("RightHandIndex3").transform.localRotation = Quaternion.Euler(zgieciaPalcow[0,2], GameObject.Find("RightHandIndex3").transform.localRotation.eulerAngles.y, GameObject.Find("RightHandIndex3").transform.localRotation.eulerAngles.z);

        GameObject.Find("RightHandThumb1").transform.localRotation = Quaternion.Euler(GameObject.Find("RightHandThumb1").transform.localRotation.eulerAngles.x, GameObject.Find("RightHandThumb1").transform.localRotation.eulerAngles.y, zgieciaPalcow[1, 0]);
        GameObject.Find("RightHandThumb2").transform.localRotation = Quaternion.Euler(GameObject.Find("RightHandThumb2").transform.localRotation.eulerAngles.x, GameObject.Find("RightHandThumb2").transform.localRotation.eulerAngles.y, zgieciaPalcow[1, 1]);
        GameObject.Find("RightHandThumb3").transform.localRotation = Quaternion.Euler(GameObject.Find("RightHandThumb3").transform.localRotation.eulerAngles.x, GameObject.Find("RightHandThumb3").transform.localRotation.eulerAngles.y, zgieciaPalcow[1, 2]);

        GameObject.Find("RightHandMiddle1").transform.localRotation = Quaternion.Euler(zgieciaPalcow[2, 0], GameObject.Find("RightHandMiddle1").transform.localRotation.eulerAngles.y, GameObject.Find("RightHandMiddle1").transform.localRotation.eulerAngles.z);
        GameObject.Find("RightHandMiddle2").transform.localRotation = Quaternion.Euler(zgieciaPalcow[2, 1], GameObject.Find("RightHandMiddle2").transform.localRotation.eulerAngles.y, GameObject.Find("RightHandMiddle2").transform.localRotation.eulerAngles.z);
        GameObject.Find("RightHandMiddle3").transform.localRotation = Quaternion.Euler(zgieciaPalcow[2, 2], GameObject.Find("RightHandMiddle3").transform.localRotation.eulerAngles.y, GameObject.Find("RightHandMiddle3").transform.localRotation.eulerAngles.z);

        GameObject.Find("RightHandRing1").transform.localRotation = Quaternion.Euler(zgieciaPalcow[3, 0], GameObject.Find("RightHandRing1").transform.localRotation.eulerAngles.y, GameObject.Find("RightHandRing1").transform.localRotation.eulerAngles.z);
        GameObject.Find("RightHandRing2").transform.localRotation = Quaternion.Euler(zgieciaPalcow[3, 1], GameObject.Find("RightHandRing2").transform.localRotation.eulerAngles.y, GameObject.Find("RightHandRing2").transform.localRotation.eulerAngles.z);
        GameObject.Find("RightHandRing3").transform.localRotation = Quaternion.Euler(zgieciaPalcow[3, 2], GameObject.Find("RightHandRing3").transform.localRotation.eulerAngles.y, GameObject.Find("RightHandRing3").transform.localRotation.eulerAngles.z);

        GameObject.Find("RightHandPinky1").transform.localRotation = Quaternion.Euler(zgieciaPalcow[4, 0], GameObject.Find("RightHandPinky1").transform.localRotation.eulerAngles.y, GameObject.Find("RightHandPinky1").transform.localRotation.eulerAngles.z);
        GameObject.Find("RightHandPinky2").transform.localRotation = Quaternion.Euler(zgieciaPalcow[4, 1], GameObject.Find("RightHandPinky2").transform.localRotation.eulerAngles.y, GameObject.Find("RightHandPinky2").transform.localRotation.eulerAngles.z);
        GameObject.Find("RightHandPinky3").transform.localRotation = Quaternion.Euler(zgieciaPalcow[4, 2], GameObject.Find("RightHandPinky3").transform.localRotation.eulerAngles.y, GameObject.Find("RightHandPinky3").transform.localRotation.eulerAngles.z);
    }
    public float ZamianaZHex(string value)
    {
        if (value.Length == 8)
        {
            int intbits = (Convert.ToInt32(value.Substring(6, 2), 16) << 24) | ((Convert.ToInt32(value.Substring(4, 2), 16) & 0xff) << 16) | ((Convert.ToInt32(value.Substring(2, 2), 16) & 0xff) << 8) | (Convert.ToInt32(value.Substring(0, 2), 16) & 0xff);
            byte[] bytes = BitConverter.GetBytes(intbits);
            f = BitConverter.ToSingle(bytes, 0);
        }
        return f;
    }


    void quaternionToEuler(float[] q, float[] euler)
    {
        //  Debug.Log(q[0] + " " + q[1] + " " + q[2]+" "+q[3]);
        euler[0] = Convert.ToSingle(Math.Atan2(2 * q[1] * q[2] - 2 * q[0] * q[3], 2 * q[0] * q[0] + 2 * q[1] * q[1] - 1)); // psi
        euler[1] = Convert.ToSingle(-Math.Asin(2 * q[1] * q[3] + 2 * q[0] * q[2])); // theta
        euler[2] = Convert.ToSingle(Math.Atan2(2 * q[2] * q[3] - 2 * q[0] * q[1], 2 * q[0] * q[0] + 2 * q[3] * q[3] - 1)); // phi
     //   Debug.Log((180 / Math.PI) * euler[0] + " " + (180 / Math.PI) * euler[1] + " " + (180 / Math.PI) * euler[2]);
    }
    float[] quatProd(float[] a, float[] b)
    {
        float[] q = new float[4];

        q[0] = a[0] * b[0] - a[1] * b[1] - a[2] * b[2] - a[3] * b[3];
        q[1] = a[0] * b[1] + a[1] * b[0] + a[2] * b[3] - a[3] * b[2];
        q[2] = a[0] * b[2] - a[1] * b[3] + a[2] * b[0] + a[3] * b[1];
        q[3] = a[0] * b[3] + a[1] * b[2] - a[2] * b[1] + a[3] * b[0];

        return q;
    }
}
