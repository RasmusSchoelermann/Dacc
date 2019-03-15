using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int Team;
    public int id;

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
    public float taargetdis;
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
        if (targetinrange)
        {
            StartCoroutine(Attack(nextUnit));
        }
        else
        {
            if (nextUnit != null)
            {
                if(taargetdis <=4)
                {
                    Vector2 temp = getfreespotstounit(nextUnit, true);

                    test.MoveUnit(this, Mathf.RoundToInt(temp.x), Mathf.RoundToInt(temp.y));
                }
                else
                {
                    Vector2 temp = movetowardstarget(nextUnit);
                    test.MoveUnit(this, Mathf.RoundToInt(temp.x), Mathf.RoundToInt(temp.y));
                }
               
            }

        }
        StartCoroutine(AI());
    }

    Vector2 getfreespotstounit(Unit Target,bool getnearest)
    {
        Vector2 freespot = new Vector2(-1,-1);
        float dis = 100;

        
        for (int ix = -1; ix < 2; ix++)
        {
            for (int iy = -1; iy < 2; iy++)
            {
                int testx = Target.ArrayX + ix;
                int testy = Target.ArrayY + iy;
                if (ix == 0 && iy == 0)
                {
                    
                }
                   
                else if (testx < 0 || testx > 7)
                {

                }
                else if (testy < 0 || testy > 7)
                {

                }
                else
                {
                    if (test.BattleBoard[testx,testy] == null)
                    {
                        float newdis = Mathf.Sqrt(Mathf.Pow(testx - ArrayX,2) + Mathf.Pow(testy - ArrayY,2));
                        if (newdis < 0)
                        {
                                newdis = newdis * -1;
                        }
                        if(getnearest)
                        {
                            if (newdis < dis)
                            {
                                freespot = new Vector2(testx, testy);
                                dis = newdis;
                            }
                        }
                        else
                        {
                            if (newdis > dis)
                            {
                                freespot = new Vector2(testx, testy);
                                dis = newdis;
                            }
                        }
                           
                           
                    }
                }

            }
        }
        return freespot;
    
    }

    Vector2 movetowardstarget(Unit Target)
    {
        Vector2 feld = new Vector2();
        int testx = ArrayX;
        int testy = ArrayY;

        if (ArrayX > Target.ArrayX)
        {
            testx = ArrayX - 3;
            if (testx < Target.ArrayX)
            {
                testx = Target.ArrayX;
            }
        }
        else if (ArrayX < Target.ArrayX)
        {
            testx = ArrayX + 3;
            if (testx > Target.ArrayX)
            {
                testx = Target.ArrayX;
            }
        }

        if (ArrayY > Target.ArrayY)
        {
            testy = ArrayY - 3;
            if (testy < Target.ArrayY)
            {
                testy = Target.ArrayY;
            }
        }
        else if (ArrayY < Target.ArrayY)
        {
            testy = ArrayY + 3;
            if (testy > Target.ArrayY)
            {
                testy = Target.ArrayY;
            }
        }

      
        feld.x = testx;
        feld.y = testy;

        if (test.BattleBoard[testx, testy] == null)
        {
            return feld;
        }
        else
        {
            for (int ix = -1; ix < 3; ix++)
            {
                for (int iy = -1; iy < 3; iy++)
                {
                    int testxb = testx + ix;
                    int testyb = testy + iy;
                
                   
                    if (testxb < 0 || testxb > 7)
                    {

                    }
                    else if (testyb < 0 || testyb > 7)
                    {

                    }
                    else
                    {
                        if (test.BattleBoard[testxb, testyb] == null)
                        {
                            feld.x = testxb;
                            feld.y = testyb;
                            return feld;

                        }
                    }

                }
            }

            return feld;
        }
    }

    void stopAi()
    {

    }

    void BattleKI()
    {

    }

    IEnumerator Attack(Unit Target)
    {
        
       
        if(Target == null)
        {
            targetinrange = false;
        }
        else
        {
            Target.HandleDamage(Damage);
        }
        yield return new WaitForSeconds(Attackspeed);

        float Udis = Mathf.Sqrt(Mathf.Pow(Target.ArrayX - ArrayX, 2) + Mathf.Pow(Target.ArrayY - ArrayY, 2));
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
