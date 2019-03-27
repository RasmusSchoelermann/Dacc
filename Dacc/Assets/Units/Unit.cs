using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Unit : NetworkBehaviour
{
    [SyncVar]public int Team;
    public int id;
    public int Pcost = 1;

    public int ArrayX;
    public int ArrayY;
    public int maxHp = 100;
    public int Hp = 10;
    public int Damage = 10;
    public int Range = 1;
    public float Attackspeed = 1;
    public int mana = 0;
    public int manaattackgain = 10;
    public int manadamagegain = 15;
    public bool BoardTeam = false;
    bool activeai;
    public bool targetinrange;
    public float taargetdis;
    public int movedistance = 3;
    public Sprite UIimg;
    bool dead = false;
    public Ulti Ultiscript;
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
                if (taargetdis <= movedistance)
                {
                    Vector2 temp = getfreespotstounit(nextUnit, true);
                    if(temp.x == -1 || temp.y == -1)
                    {

                    }
                    else
                    {
                        List<Vector2> testpath;
                        testpath = getpath(temp);
                        if (validpath(temp) == false)
                        {

                            if (testpath.Count > 0)
                            {
                                if (testpath.Count > movedistance)
                                {
                                    if (testpath[movedistance] != null)
                                    {
                                        temp = testpath[movedistance];
                                        test.MoveUnit(this, Mathf.RoundToInt(temp.x), Mathf.RoundToInt(temp.y));
                                    }
                                }
                                else
                                {
                                    if (testpath[testpath.Count] != null)
                                    {
                                        temp = testpath[movedistance];
                                        test.MoveUnit(this, Mathf.RoundToInt(temp.x), Mathf.RoundToInt(temp.y));
                                    }
                                }
                            }
                        }
                        else
                        {
                            test.MoveUnit(this, Mathf.RoundToInt(temp.x), Mathf.RoundToInt(temp.y));
                        }
                    }
                  


                }
                else
                {
                    Vector2 temp = movetowardstarget(nextUnit);
                    if (temp.x == -1 || temp.y == -1)
                    {

                    }
                    else
                    {
                        List<Vector2> testpath;
                        testpath = getpath(temp);
                        if (validpath(temp) == false)
                        {

                            if (testpath.Count > 0)
                            {
                                if (testpath.Count > movedistance)
                                {
                                    if (testpath[movedistance] != null)
                                    {
                                        temp = testpath[movedistance];
                                        test.MoveUnit(this, Mathf.RoundToInt(temp.x), Mathf.RoundToInt(temp.y));
                                    }
                                }
                                else
                                {
                                    if (testpath[testpath.Count] != null)
                                    {
                                        temp = testpath[movedistance];
                                        test.MoveUnit(this, Mathf.RoundToInt(temp.x), Mathf.RoundToInt(temp.y));
                                    }
                                }
                            }
                        }
                        else
                        {
                            test.MoveUnit(this, Mathf.RoundToInt(temp.x), Mathf.RoundToInt(temp.y));
                        }
                    }
                }
               
            } // Handle move unit

        }
        StartCoroutine(AI());
    }

    Vector2 getinrange(Unit Target)
    {
        Vector2 freespot = new Vector2(-1, -1);

        return freespot;

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
    
    } //get free spot near target

    Vector2 movetowardstarget(Unit Target)
    {
        Vector2 feld = new Vector2();
        int testx = ArrayX;
        int testy = ArrayY;

        if (ArrayX > Target.ArrayX)
        {
            testx = ArrayX - movedistance;
            if (testx < Target.ArrayX)
            {
                testx = Target.ArrayX;
            }
        }
        else if (ArrayX < Target.ArrayX)
        {
            testx = ArrayX + movedistance;
            if (testx > Target.ArrayX)
            {
                testx = Target.ArrayX;
            }
        }

        if (ArrayY > Target.ArrayY)
        {
            testy = ArrayY - movedistance;
            if (testy < Target.ArrayY)
            {
                testy = Target.ArrayY;
            }
        }
        else if (ArrayY < Target.ArrayY)
        {
            testy = ArrayY + movedistance;
            if (testy > Target.ArrayY)
            {
                testy = Target.ArrayY;
            }
        }

      
       

        float newdis = Mathf.Sqrt(Mathf.Pow(testx - Target.ArrayX, 2) + Mathf.Pow(testy - Target.ArrayY, 2));
        if(Mathf.Floor(newdis) < Range)
        {
            float dif = Range - Mathf.Floor(newdis);
            
            if (testx > Target.ArrayX)
            {
                testx += Mathf.RoundToInt(dif);
                if (testx < Target.ArrayX)
                {
                    testx = Target.ArrayX;
                }
            }
            else if  (testx < Target.ArrayX)
            {
                testx -= Mathf.RoundToInt(dif);
                if (testx > Target.ArrayX)
                {
                    testx = Target.ArrayX;
                }
            }

            if (testy > Target.ArrayY)
            {
                testy += Mathf.RoundToInt(dif);
                if (testy < Target.ArrayY)
                {
                    testy = Target.ArrayY;
                }
            }
            else if (testy < Target.ArrayY)
            {
                testy -= Mathf.RoundToInt(dif);
                if (testy > Target.ArrayY)
                {
                    testy = Target.ArrayY;
                }
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
            float dis = 100;

            for (int ix = -1; ix < 2; ix++)
            {
                for (int iy = -1; iy < 2; iy++)
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
                            newdis = Mathf.Sqrt(Mathf.Pow(testx - ArrayX, 2) + Mathf.Pow(testy - ArrayY, 2));
                            if (newdis < 0)
                            {
                                newdis = newdis * -1;
                            }
                            
                            if (newdis < dis)
                            {
                                feld.x = testxb;
                                feld.y = testyb;
                                dis = newdis;
                            }
                          
                            

                            feld.x = testxb;
                            feld.y = testyb;

                        }
                    }

                }
            }


            return feld;
        }
    } // move unit closer to target(Kepps Range in mind)

    Vector2 checkmovepoint(Vector2 moveposition)// Checks Move position end returns correctet path if obstructed
    {
        if(validpath(moveposition) == true)
        {
            return moveposition;
        }
        else
        {
            // return valid path
        }
        return moveposition;
    }

    bool validpath(Vector2 movepoint)// check if path is valid
    {
        int X = Mathf.RoundToInt(movepoint.x);
        int Y = Mathf.RoundToInt(movepoint.y);

        bool valid = true;

        while(X != ArrayX && Y != ArrayY)
        {


            if(test.BattleBoard[X,Y] != null)
            {
                valid = false;
            }

            if (X > ArrayX)
            {
                X--;
                if (X < ArrayX)
                {
                    X = ArrayX;
                }
            }
            else if (X < ArrayX)
            {
                X++;
                if (X > ArrayX)
                {
                    X = ArrayX;
                }
            }

            if (Y > ArrayY)
            {
                Y--;
                if (Y < ArrayY)
                {
                    Y = ArrayY;
                }
            }
            else if (Y < ArrayY)
            {
                Y++;
                if (Y > ArrayY)
                {
                    Y = ArrayY;
                }
            }
        }

        return valid;
    }

    List<Vector2> getpath(Vector2 goal)
    {
        List<Cell> OpenList = new List<Cell>();
        List<Cell> ClosedList = new List<Cell>();
        List<Vector2> FinalPath = new List<Vector2>();

        Cell root = new Cell();
        root.waypoint.x = ArrayX;
        root.waypoint.y = ArrayY;
        root.parent = null;
        bool check = false;
        // Start->Parent = nullptr;
        root.Heuristiccost = Mathf.Sqrt(Mathf.Pow(goal.x - root.waypoint.x, 2) + Mathf.Pow(goal.y - root.waypoint.y, 2));
        //UE_LOG(LogClass, Display, TEXT("Costs: %f"), root.Heuristiccost);
        //UE_LOG(LogClass, Display, TEXT("Start CenterX: %f"), root.Waypoint->GetActorLocation().X);
        //UE_LOG(LogClass, Display, TEXT("Goal CenterX: %f"), GetActorLocation().X);
        //UE_LOG(LogClass, Display, TEXT("Start CenterY: %f"), root.Waypoint->GetActorLocation().Y);
        //UE_LOG(LogClass, Display, TEXT("Goal CenterY: %f"), GetActorLocation().Y);
        OpenList.Add(root);

        while (OpenList.Count > 0)
        {
            if (test.BattleBoard[Mathf.RoundToInt(goal.x), Mathf.RoundToInt(goal.y)] == null)
            {
                return FinalPath;
            }
            else
            {
                for (int ix = -1; ix < 2; ix++)
                {
                    for (int iy = -1; iy < 2; iy++)
                    {
                        int testx = Mathf.RoundToInt(goal.x) + ix;
                        int testy = Mathf.RoundToInt(goal.y) + iy;

                        if (ix == 0 && iy == 0)
                        {

                        }

                        else if (testx < 0 || testx > 7)
                        {

                        }
                        else if (testy < 0 || testy > 7)
                        {

                        }

                        else if (test.BattleBoard[Mathf.RoundToInt(goal.x + ix), Mathf.RoundToInt(goal.y + iy)] == null)
                        {
                            check = true;
                        }
                    }
                }
                if(check)
                {
                    return FinalPath;
                }
            }

            Cell currentNode = OpenList[0];

            if (currentNode.waypoint.x == goal.x && currentNode.waypoint.y == goal.y)
            {
                Cell checknode = currentNode;
                while (checknode.parent != null)
                {
                    FinalPath.Insert(0,checknode.waypoint);
                    checknode = checknode.parent;
                }
                FinalPath.Insert(0,checknode.waypoint);
                //UE_LOG(LogClass, Display, TEXT("GOAL Return Final Path"));
                return FinalPath;
            }

            for (int ix = -1; ix < 2; ix++)
            {
                for (int iy = -1; iy < 2; iy++)
                {
                    Cell nachbar = new Cell();
                    nachbar.waypoint.x = currentNode.waypoint.x + ix;
                    nachbar.waypoint.y = currentNode.waypoint.y + iy;

                    if (ix == 0 && iy == 0)
                    {
                       
                    }

                    else if (nachbar.waypoint.x < 0 || nachbar.waypoint.x > 7)
                    {
                       
                    }
                    else if (nachbar.waypoint.y < 0 || nachbar.waypoint.y > 7)
                    {
                       
                    }
                    else
                    {
                        if (test.BattleBoard[Mathf.RoundToInt(nachbar.waypoint.x), Mathf.RoundToInt(nachbar.waypoint.y)] != null)
                        {

                        }
                        else
                        {
                            Cell testNode = new Cell();
                            testNode.Heuristiccost = Mathf.Sqrt(Mathf.Pow(goal.x - nachbar.waypoint.x, 2) + Mathf.Pow(goal.y - nachbar.waypoint.y, 2));
                            testNode.waypoint = nachbar.waypoint;
                            if (OpenList.Contains(testNode) == true || ClosedList.Contains(testNode) == true) //currentNode.CheckContains(OpenList, currentNode.Waypoint->Nachbarn[i]) == false && currentNode.CheckContains(ClosedList, currentNode.Waypoint->Nachbarn[i]) == false
                            {

                            }
                            else
                            {
                                // TODO
                                float cost = Mathf.Sqrt(Mathf.Pow(goal.x - nachbar.waypoint.x, 2) + Mathf.Pow(goal.y - nachbar.waypoint.y, 2));
                                float test = OpenList[0].Heuristiccost;
                                //UE_LOG(LogClass, Display, TEXT("Costs: %f"), cost);
                                //UE_LOG(LogClass, Display, TEXT("Tests: %f"), test);

                                if (cost < test && OpenList.Contains(testNode) == false && ClosedList.Contains(testNode) == false)
                                {
                                    OpenList.Insert(0, new Cell(nachbar.waypoint, currentNode, cost));
                                    //UE_LOG(LogClass, Display, TEXT("Insert"));
                                    nachbar.parent = currentNode;
                                }
                                else if (OpenList.Contains(testNode) == false && ClosedList.Contains(testNode) == false)
                                {
                                    int length = OpenList.Count;
                                    for (int ib = 0; ib < length; ib++)
                                    {
                                        if (cost < OpenList[ib].Heuristiccost && OpenList.Contains(testNode) == false && ClosedList.Contains(testNode) == false)
                                        {
                                            OpenList.Insert(ib, new Cell(nachbar.waypoint, currentNode, cost));
                                            //UE_LOG(LogClass, Display, TEXT("Sorted in Place"));
                                            nachbar.parent = currentNode;
                                            break;
                                        }
                                        else if (ib == length - 1)
                                        {
                                            OpenList.Add(new Cell(nachbar.waypoint, currentNode, cost));
                                            //UE_LOG(LogClass, Display, TEXT("Sort to last"));
                                            nachbar.parent = currentNode;
                                        }
                                        else
                                        {

                                        }

                                    }

                                }
                                else
                                {

                                }
                            }
                        }
                    }

                    
                    
                }

            }
            ClosedList.Add(currentNode);
            //int a = 
            OpenList.Remove(currentNode);
            //UE_LOG(LogClass, Display, TEXT("Removed %d"), a);
        }

       // UE_LOG(LogClass, Display, TEXT("NO PATH"));
        return FinalPath;
    } // get path to goal

    void stopAi()
    {

    }

    void BattleKI()
    {

    }

    IEnumerator Attack(Unit Target) // Attack Target
    {
        
       
        if(Target == null)
        {
            targetinrange = false;
           
        }
        else
        {
            Target.HandleDamage(Damage,this);
            mana += manaattackgain;
            if (mana >= 100)
            {
                Ulti();
            }
        }
        yield return new WaitForSeconds(Attackspeed);

        if (Target == null)
        {
        }
        else
        {
            float Udis = Mathf.Sqrt(Mathf.Pow(Target.ArrayX - ArrayX, 2) + Mathf.Pow(Target.ArrayY - ArrayY, 2));
            if (Udis < 0)
            {
                Udis = Udis * -1;
            }
            if (Udis >= Range)
            {
                targetinrange = false;
            }
            if (targetinrange == true)
            {
                StartCoroutine(Attack(Target));
            }
        }
          
    }

    void HandleDamage(int Damage,Unit Causer) // Handle Damage adn detroy self if 0 hp
    {
        Hp = Hp - Damage;
        mana += manadamagegain;
        if (Hp <= 0)
        {

            if (dead == false)
            {
                dead = true;
                test.checkboard();
                Destroy(this.gameObject);
            }


        }
        else
        {
            if(mana >= 100)
            {
                Ulti();
            }
        }
    }
       
    void Ulti()
    {
        Ultiscript.testulti(id);
    }
}

public class Cell
{
   public Vector2 waypoint;
   public Cell parent;
   public float Heuristiccost;

    public Cell()
    {
      
    }

    public Cell(Vector2 way,Cell par,float cost)
    {
        waypoint = way;
        parent =  par;
        Heuristiccost = cost;
    }
}
