using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadWarlockUlti : Ulti
{
    //Unit unit;

    public override void testulti(int lvl, Unit target)
    {
        // base.testulti(Unitid);
        //print("Warlock ulti called");
        if(lvl == 1)
        {
            lvl1ult(target);
        }
    }

    void lvl1ult(Unit target)
    {
        print("Warlock ulti lvl1 called");
        base.Unit.Hp = base.Unit.Hp + ((500 / 100) * 25); // Lifesteal
        target.HandleDamage(250,base.Unit,"Magic");
        done();
    }
   
}
