using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int Team;

    public int ArrayX;
    public int ArrayY;
    public int maxHp = 100;
    public int Hp = 10;
    public int Damage = 10;
    public int Range = 1;
    public float Attackspeed = 1;
    public bool BoardTeam = false;
    bool activeai;
    public bool targetinrange;
    Battle test;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startAi(Battle Battleboard)
    {
        //Unit nextUnit = null;
        test = Battleboard;
        //gameObject.transform.position;
        //test.MoveUnit(this, 3, 3);
        //nextUnit = test.RangeCheck(this,BoardTeam);
        //test.MoveUnit(this, nextUnit.ArrayX + 1, nextUnit.ArrayY);
        StartCoroutine(AI());
    }

    IEnumerator AI()
    {
        yield return new WaitForSeconds(1f);
        Unit nextUnit = null;
        nextUnit = test.RangeCheck(this, BoardTeam);
        if(targetinrange)
        {
            StartCoroutine(Attack(nextUnit));
        }
        else
        {
            if(nextUnit != null)
            {
                test.MoveUnit(this, nextUnit.ArrayX + 1, nextUnit.ArrayY);
            }
           
        }
        StartCoroutine(AI());
    }

    void stopAi()
    {

    }

    void BattleKI()
    {

    }

    IEnumerator Attack(Unit Target)
    {
        
        Target.HandleDamage(Damage);
        if(Target == null)
        {
            targetinrange = false;
        }
        yield return new WaitForSeconds(Attackspeed);

        float Udis = (Target.ArrayX - ArrayX) + (Target.ArrayY - ArrayY);
        if (Udis < 0)
        {
            Udis = Udis * -1;
        }
        if (Udis >= Range)
        {
            targetinrange = false;
        }
        if(targetinrange == true)
        {
            StartCoroutine(Attack(Target));
        }
    }

    void HandleDamage(int Damage)
    {
        Hp = Hp - Damage;
        if(Hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
       
       
       
}
