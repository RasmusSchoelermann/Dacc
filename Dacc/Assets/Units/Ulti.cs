using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ulti : MonoBehaviour
{
   public Unit Unit;
  public virtual void testulti(int lvl,Unit target)
    {
        print("Debug ulti called");

        done();
    }

    public virtual void setup(Unit unit)
    {
        Unit = unit;

    }

    public void done()
    {
        Unit.StartCoroutine(Unit.AI());
        print("Start ai again");

    }
}
