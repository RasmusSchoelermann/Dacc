using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ulti : MonoBehaviour
{
    public Unit Unit;
  public virtual void testulti(int Unitid,Unit unit)
    {
        print("Debug ulti called");
        Unit = unit;
    }
}
