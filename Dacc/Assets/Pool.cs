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
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
