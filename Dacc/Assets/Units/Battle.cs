﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Battle : NetworkBehaviour
{
    public GameObject UnitsPrefab;
    public Vector3 enemyspawnposition;
    public Vector3 spawnposition;
    public Quaternion spawnRotation;
    int round = 0;
    bool inbattle = false;

    int creeppower = 0;

    public bool alive;
    [System.Serializable]
    public class Planes2d
    {
        public GameObject[] Planes = new GameObject[8];
    }
    public Planes2d[] testArray = new Planes2d[8];

    public GameObject[,] PlanesArray;


    bool spawn = false;
    public Unit[,] BattleBoard = new Unit[8,8];
    public Unit[] OwnUnits = new Unit[10];
    public Unit[] EnemyUnits = new Unit[10];
    Unit[,] BoardSave = new Unit[8, 8];
    int C = 0;

    public RoundManager roundmanager;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       /* if(!isServer)
        {
            return;
        }


        if(Input.GetKeyDown("b"))
        {
            endbattle();
        }

        if (Input.GetKeyDown("f"))
        {
            
            var unit = (GameObject)Instantiate(UnitsPrefab, spawnposition, spawnRotation);
            unit.tag = "Unit";
            unit.GetComponent<Unit>().Team = 0;
            unit.GetComponent<BoardLocation>().Bx = 0;
            unit.GetComponent<BoardLocation>().By = 6;
            NetworkServer.Spawn(unit);
            BattleBoard[0, 6] = unit.GetComponent<Unit>();

        }
        if (Input.GetKeyDown("e"))
        {
           
            var unit = (GameObject)Instantiate(UnitsPrefab, enemyspawnposition, spawnRotation);
            unit.tag = "Unit";
            unit.GetComponent<BoardLocation>().Bx = 3;
            unit.GetComponent<BoardLocation>().By = 3;
            NetworkServer.Spawn(unit);
            BattleBoard[3, 3] = unit.GetComponent<Unit>();
        }*/

        
    }

    public void startbattle()
    {
        
        C = 0;

        if(inbattle)
        {
            return;
        }
        inbattle = true;
        // round++;
        
        int c =0;
        foreach (Unit U in BoardSave)
        {
            if (U != null)
            {
                GameObject temp = (GameObject)Instantiate(U.gameObject, U.gameObject.transform.position, spawnRotation);
                NetworkServer.Spawn(temp);
                U.ArrayX = U.GetComponent<BoardLocation>().Bx;
                U.ArrayY = U.GetComponent<BoardLocation>().By;
                BattleBoard[U.ArrayX, U.ArrayY] = temp.GetComponent<Unit>();
                OwnUnits[c] = temp.GetComponent<Unit>();
                U.gameObject.SetActive(false);
                c++;
            }

        }
       //roundmanager = GameObject.FindGameObjectWithTag("manager").GetComponent<RoundManager>();
       roundmanager.readycheck();

        /*if (round < 4 || round > 5 && round % 5 == 0)
        {
            spawncreeps();
        }
        else
        {
            // FindEnemy
        }
    
         foreach (Unit U in BattleBoard)
        {
            if (U != null)
            {
                U.ArrayX = U.GetComponent<BoardLocation>().Bx;
                U.ArrayY = U.GetComponent<BoardLocation>().By;
                //U.startAi(this, PlanesArray);
            }

        }*/

        //battle();
    }

   

    [Command]
    public void CmdScraddUnit(GameObject item,int X,int Y)
    {
       BoardSave[X, Y] = item.GetComponent<Unit>();
    
    }

    public void battle()
    {
        
        foreach  (Unit u in OwnUnits)
        {
            if(u != null)
            {
                u.BoardTeam = true;
                u.startAi(this);
            }
           
        }
        foreach (Unit u in EnemyUnits)
        {
            if (u != null)
            {
                u.BoardTeam = false;
                u.startAi(this);
            }

        }
         //StartCoroutine(Example(C));
         //C++;
    }

    IEnumerator Example(int c)
    {
       
        yield return new WaitForSeconds(0.3f);
        if(OwnUnits[c] != null)
        {
            OwnUnits[c].BoardTeam = true;
            OwnUnits[c].startAi(this);
            battle();
        }
       
       
    }

    public void MoveUnit(Unit Unittomove, int X,int Y)
    {
        if(X < 0 || X > 7)
        {
            return;
        }
        if (Y < 0 || Y > 7)
        {
            return;
        }
        if(BattleBoard[X,Y] != null)
        {
            return;
        }
        Vector3 newposition = testArray[Y].Planes[X].gameObject.transform.position;
        newposition.y = Unittomove.gameObject.transform.position.y;
        Unittomove.gameObject.transform.position = newposition;
        BattleBoard[X, Y] = Unittomove;
        BattleBoard[Unittomove.ArrayX, Unittomove.ArrayY] = null;
        Unittomove.ArrayX = X;
        Unittomove.ArrayY = Y;

    }
   //public void 

    public Unit RangeCheck(Unit Unit,bool boardteam)
    {
        Unit temp = null;
        float dis = 1000;
        if (boardteam == true)
        {
            foreach  (Unit u in EnemyUnits)
            {
                if(u != null)
                {
                    float Udis =  Mathf.Sqrt(Mathf.Pow(u.ArrayX - Unit.ArrayX, 2) + Mathf.Pow(u.ArrayY - Unit.ArrayY, 2));
                    if (Udis < 0)
                    {
                        Udis = Udis * -1;
                    }
                    if (Mathf.Floor(Udis) <= Unit.Range)
                    {
                        Unit.targetinrange = true;
                        return u;
                    }
                    else
                    {
                        float newdis = Udis;
                        if (newdis < dis)
                        {
                            temp = u;
                            dis = newdis;
                        }

                    }
                }
               
            }
            Unit.taargetdis = dis;
            return temp;
        }
        else
        {
            foreach (Unit u in OwnUnits)
            {
                if (u != null)
                {
                    float Udis = Mathf.Sqrt(Mathf.Pow(u.ArrayX - Unit.ArrayX, 2) + Mathf.Pow(u.ArrayY - Unit.ArrayY, 2));
                    if (Udis < 0)
                    {
                        Udis = Udis * -1;
                    }
                    if (Mathf.Floor(Udis) <= Unit.Range)
                    {
                        Unit.targetinrange = true;
                        return u;
                    }
                    else
                    {
                        float newdis = Udis;
                        if (newdis < dis)
                        {
                            temp = u;
                            dis = newdis;
                        }

                    }
                }
            }
            Unit.taargetdis = dis;
            return temp;
        }
        return null;
    }

    public void endbattle()
    {
        inbattle = false;
        foreach (Unit U in BattleBoard)
        {
            if (U != null)
            {
                Destroy(U.gameObject);
            }

        }

        foreach (Unit u in BoardSave)
        {
            if(u != null)
            {
                u.gameObject.SetActive(true);
            }
            
        }
    }

    void spawncreeps()
    {
        if(creeppower == 0)
        {
            Vector3 newposition = testArray[0].Planes[3].gameObject.transform.position;
            newposition.y = 4.7f;
            var Creep = (GameObject)Instantiate(UnitsPrefab, newposition, spawnRotation);
            Creep.tag = "Unit";
            Creep.GetComponent<BoardLocation>().Bx = 3;
            Creep.GetComponent<BoardLocation>().By = 0;
            Creep.GetComponent<Unit>().Team = -1;
            NetworkServer.Spawn(Creep);
            EnemyUnits[0] = Creep.GetComponent<Unit>();
            BattleBoard[3, 0] = Creep.GetComponent<Unit>();

            newposition = testArray[0].Planes[4].gameObject.transform.position;
            newposition.y = 4.7f;
            Creep = (GameObject)Instantiate(UnitsPrefab, newposition, spawnRotation);
            Creep.tag = "Unit";
            Creep.GetComponent<BoardLocation>().Bx = 4;
            Creep.GetComponent<BoardLocation>().By = 0;
            Creep.GetComponent<Unit>().Team = -1;
            NetworkServer.Spawn(Creep);
            EnemyUnits[1] = Creep.GetComponent<Unit>();
            BattleBoard[4, 0] = Creep.GetComponent<Unit>();

            /*newposition = testArray[0].Planes[2].gameObject.transform.position;
            newposition.y = 4.7f;
            Creep = (GameObject)Instantiate(UnitsPrefab, newposition, spawnRotation);
            Creep.tag = "Unit";
            Creep.GetComponent<BoardLocation>().Bx = 2;
            Creep.GetComponent<BoardLocation>().By = 0;
            Creep.GetComponent<Unit>().Team = -1;
            NetworkServer.Spawn(Creep);
            EnemyUnits[2] = Creep.GetComponent<Unit>();
            BattleBoard[2, 0] = Creep.GetComponent<Unit>();*/

        }
        //creeppower++;
    }
}
