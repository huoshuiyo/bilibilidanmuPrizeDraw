using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CreateGuid : MonoBehaviour
{
   public int guidCount;
    public int c1 = 0;
    public int c2 = 0;
    public int c3 = 0;
    public int c4 = 0;
    public int c5 = 0;
    public int c6 = 0;
    public int c7 = 0;
    public int c8 = 0;
    public int c9 = 0;
    public int c0 = 0;

    public int cA = 0;
    public int cB = 0;
    public int cC = 0;
    public int cD = 0;
    public int cE = 0;
    public int cF = 0; 
    public int cG = 0;
    public int cH = 0;
    public int cI = 0;
    public int cJ = 0;
    public int cK = 0;
    public int cL = 0;
    public int cM = 0;
    public int cN = 0;
    public int cO = 0;
    public int cP = 0;
    public int cQ = 0;
    public int cR = 0;
    public int cS = 0;
    public int cT = 0;
    public int cU = 0;
    public int cV = 0;
    public int cW = 0;
    public int cX = 0;
    public int cY = 0;
    public int cZ = 0;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < guidCount; i++)
        {
            var uuid = Guid.NewGuid().ToString();
            string first = uuid[0].ToString();
            Debug.Log(first);
            switch (first)
            {
                case "1":
                    c1++;
                    break;
                case "2":
                    c2++;
                    break;
                case "3":
                    c3++;
                    break;
                case "4":
                    c4++;
                    break;
                case "5":
                    c5++;
                    break;
                case "6":
                    c6++;
                    break;
                case "7":
                    c7++;
                    break;
                case "8":
                    c8++;
                    break;
                case "9":
                    c9++;
                    break;
                case "0":
                    c0++;
                    break;
                case "a":
                    cA++;
                    break;
                case "b":
                    cB++;
                    break;
                case "c":
                    cC++;
                    break;
                case "d":
                    cD++;
                    break;
                case "e":
                    cE++;
                    break;
                case "f":
                    cF++;
                    break;
                case "g":
                    cG++;
                    break;
                case "h":
                    cH++;
                    break;
                case "i":
                    cI++;
                    break;
                case "j":
                    cJ++;
                    break;
                case "k":
                    cK++;
                    break;
                case "l":
                    cL++;
                    break;
                case "m":
                    cM++;
                    break;
                case "n":
                    cN++;
                    break;
                case "o":
                    cO++;
                    break;
                case "p":
                    cP++;
                    break;
                case "q":
                    cQ++;
                    break;
                case "r":
                    cR++;
                    break;
                case "s":
                    cS++;
                    break;
                case "t":
                    cT++;
                    break;
                case "u":
                    cU++;
                    break;
                case "v":
                    cV++;
                    break;
                case "w":
                    cW++;
                    break;
                case "x":
                    cX++;
                    break;
                case "y":
                    cY++;
                    break;
                case "z":
                    cZ++;
                    break;
                default:
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
