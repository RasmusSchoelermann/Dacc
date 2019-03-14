using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [System.Serializable]
    public class PoolRefs
    {
        public GameObject[] Units;
    }
    public PoolRefs[] Seltenheit;


    [System.Serializable]
    public class Zahl
    {
        public int[] Units = new int[2];
    }
    public Zahl[] UnitQuantity;

    // Start is called before the first frame update
    void Start()
    {
       // int Axeanzahl = UnitQuantity[Seltenheit[0].Units[0].gameObject.GetComponent<Unit>().id].Units[0];

        /*if(Playerlevel == 1 && UnitQuantititySeltenheit[randomselt].Units[randomunit].gameObject.GetComponent<Unit>().id].Units[unitanzahl] != 0)
        {
                mach dies und das
                
        }*/
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    /*// Start is called before the first frame update
    void Start()
    {
        //int test = UnitQuantity[id].Units[0];
        // int Axeanzahl = UnitQuantity[Seltenheit[0].Units[0].gameObject.GetComponent<Unit>().id].Units[0]; // Komplizierte version von unten :(


        int randomselt = 0; // Random bestimmen
        int randomunit = 0; //Random bestimmen
        int id = Seltenheit[randomselt].Units[randomunit].gameObject.GetComponent<Unit>().id; // gibt dir die id der Unit
        int wieviele = UnitQuantity[id].Units[0]; // Gibt Aus wie viele noch da sind


        //  var Unit = (GameObject)Instantiate(Seltenheit[randomselt].Units[randomunit].gameObject,position, spawnRotation); Spawnt Einheit
        //   NetworkServer.Spawn(Creep); Spawn Einheit auf dem Server/Client
        // 


    }
    GameObject ziehen()
    {
        if(level)
        {
            // Random chance
            // Random int test von 0-100 if(test <= 7) -> stufe 1 if(test >70 && test <= 80) if(test >80 )
        }
        else
        {

        }


        // Einheit ziehen
    }*/
   
}
