using UnityEngine;
using System.Collections;
using System;
using System.IO.Ports;

public class testowy : MonoBehaviour
{

    public GameObject target; // is the gameobject to 


    // Increase the speed/influence rotation
    public float factor = 7;

    public bool enableRotation;
    public bool enableTranslation;
    string[] ports = SerialPort.GetPortNames();
    string dataString;
    string[] data;
    float[] wartosci= new float[4];
    float[] hq = null;
    float[] Euler = new float[3];
    float f;
    SerialPort arduino;
    bool Zczytywanie = false;
    Vector3 currentEulerAngles;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            ports = SerialPort.GetPortNames();
            arduino = new SerialPort(ports[0], 115200, Parity.None, 8, StopBits.One);
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
        if (arduino.IsOpen)
        {

            try
            {
                if (Input.GetKeyDown(KeyCode.V))
                {
                    arduino.Write("v");
                    dataString = arduino.ReadLine();
                    Debug.Log(dataString);
                    Zczytywanie = true;
                }
                if (Zczytywanie)
                {
                    arduino.Write("q ");
                    for (int i = 0; i < 13; i++)
                    {
                        dataString = arduino.ReadLine();
                        data = dataString.Split(',');
                        if (data.Length >= 5)
                        {
                            
                            wartosci[0] = ZamianaZHex(data[0]);
                            wartosci[1] = ZamianaZHex(data[1]);
                            wartosci[2] = ZamianaZHex(data[2]);
                            wartosci[3] = ZamianaZHex(data[3]);

                               Debug.Log(wartosci[0] + " " + wartosci[1] + " " + wartosci[2] + " " + wartosci[3]);

                            if (hq != null)
                            {
                                quaternionToEuler(quatProd(hq, wartosci), Euler);
                            }
                            else
                            {
                                quaternionToEuler(wartosci, Euler);
                            }
                                                                              //   = new Vector3(-Euler[1] * (float)(180 / Math.PI), -Euler[2] * (float)(180 / Math.PI), 0)
                               GameObject.Find("Cube").transform.eulerAngles += new Vector3(-Euler[1] * (float)(180 / Math.PI), -Euler[2] * (float)(180 / Math.PI), 0) * Time.deltaTime * 5;

                        }

                    }
                }
            }
            catch (System.IO.IOException ioe)
            {
                Debug.Log("IOException: " + ioe.Message);
            }
        }
        else
            dataString = "NOT OPEN";
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
        euler[1] = Convert.ToSingle (-Math.Asin(2 * q[1] * q[3]+ 2 * q[0] * q[2])); // theta
        euler[2] = Convert.ToSingle(Math.Atan2(2 * q[2] * q[3] - 2 * q[0] * q[1], 2 * q[0] * q[0] + 2 * q[3] * q[3] - 1)); // phi
          Debug.Log((180 / Math.PI) * euler[0] + " " + (180 / Math.PI) * euler[1] + " " + (180 / Math.PI) * euler[2]);
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

