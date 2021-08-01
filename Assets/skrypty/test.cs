using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    { int wartosc = 1035419724;
        Debug.Log("10328851");
     //   uint fb = Convert.ToUInt32(f);

         BitConverter.ToSingle(BitConverter.GetBytes((int)wartosc), 0);
        Debug.Log(BitConverter.ToSingle(BitConverter.GetBytes((int)wartosc), 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public double FromFloatSafe(object f)
    {
        uint fb = Convert.ToUInt32(f);


        uint sign, exponent = 0, mantessa = 0;
        uint bias = 127;

        sign = (fb >> 31) & 1;
        exponent = (fb >> 23) & 0xFF;
        mantessa = (fb & 0x7FFFFF);
        double fSign = Math.Pow((-1), sign);
        double fMantessa = 1 + (1 / mantessa);
        double fExponent = Math.Pow(2, (exponent - bias));
        double ret = fSign * fMantessa * fExponent;
        return ret;
    }
}

