using System.Collections;
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

    //Synergien:
    int Warlockbuff = 0;
    int Warriorbuff = 0;
    int Assasinbuff = 0;
    int Demonhunterbuff = 0;
    int Druidbuff = 0;
    int Hunterbuff = 0;
    int Knightbuff = 0;
    int Magebuff = 0;
    int Mechbuff = 0;
    int Shamanbuff = 0;
    int Beastbuff = 0;
    int Demonbuff = 0;
    int Dwarfbuff = 0;
    int Dragonbuff = 0;
    int Elementbuff = 0;
    int Elfbuff = 0;
    int Goblinbuff = 0;
    int Humanbuff = 0;
    int Nagabuff = 0;
    int Ogrebuff = 0;
    int Orcbuff = 0;
    int Trollbuff = 0;
    int Undeadbuff = 0;

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

         Warlockbuff = 0;
         Warriorbuff = 0;
         Assasinbuff = 0;
        Demonhunterbuff = 0;
         Druidbuff = 0;
         Hunterbuff = 0;
         Knightbuff = 0;
         Magebuff = 0;
         Mechbuff = 0;
        Shamanbuff = 0;
         Beastbuff = 0;
         Demonbuff = 0;
         Dwarfbuff = 0;
         Dragonbuff = 0;
         Elementbuff = 0;
         Elfbuff = 0;
         Goblinbuff = 0;
         Humanbuff = 0;
         Nagabuff = 0;
         Ogrebuff = 0;
         Orcbuff = 0;
         Trollbuff = 0;
         Undeadbuff = 0;

        if (inbattle)
        {
            return;
        }
        inbattle = true;
        // round++;
        
        int c =0;
        List<int> ids = new List<int>();
        foreach (Unit U in BoardSave)
        {
            if (U != null)
            {
                if(c >= 10)
                {
                    U.gameObject.SetActive(false);
                    //STOP(Move unit to bank)
                }
                else
                {
                    GameObject temp = (GameObject)Instantiate(U.gameObject, U.gameObject.transform.position, spawnRotation);
                    NetworkServer.Spawn(temp);
                    U.ArrayX = U.GetComponent<BoardLocation>().Bx;
                    U.ArrayY = U.GetComponent<BoardLocation>().By;
                    BattleBoard[U.ArrayX, U.ArrayY] = temp.GetComponent<Unit>();
                    OwnUnits[c] = temp.GetComponent<Unit>();
                    U.gameObject.SetActive(false);
                    if(ids.Contains(temp.GetComponent<Unit>().id) == false)
                    {
                        CheckclassandSpecies(temp.GetComponent<Unit>());
                        ids.Add(temp.GetComponent<Unit>().id);
                    }
                    
                    c++;
                }
                
            }

        }
        
        foreach (Unit U in OwnUnits)
        {
            if(U != null)
            {
                ApplyBuffs(U);
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

    void CheckclassandSpecies(Unit u)
    {
        if (u.Class == "Warlock")
        {
            Warlockbuff++;

        }
        else if (u.Class == "Warrior")
        {
            Warriorbuff++;

        }
        else if (u.Class == "Shaman")
        {
           Shamanbuff++;

        }
        else if (u.Class == "Mech")
        {
            Mechbuff++;

        }
        else if (u.Class == "Mage")
        {
           Magebuff++;

        }
        else if (u.Class == "Knight")
        {
            Knightbuff++;

        }
        else if (u.Class == "Hunter")
        {
           Hunterbuff++;

        }
        else if (u.Class == "Druide")
        {
            Druidbuff++;

        }
        else if (u.Class == "Demon Hunter")
        {
            Demonhunterbuff++;

        }
        else if (u.Class == "Assassin")
        {
            Assasinbuff++;

        }

        if (u.Species == "Beast")
        {
            Beastbuff++;

        }
        else if (u.Species == "Demon")
        {
            Demonbuff++;

        }
        else if (u.Species == "Dragon")
        {
           Dragonbuff++;

        }
        else if (u.Species == "Dwarf")
        {
           Dwarfbuff++;

        }
        else if (u.Species == "Elemental")
        {
            Elementbuff++;

        }
        else if (u.Species == "Elf")
        {
            Elfbuff++;

        }
        else if (u.Species == "Goblin")
        {
            Goblinbuff++;

        }
        else if (u.Species == "Human")
        {
            Humanbuff++;

        }
        else if (u.Species == "Naga")
        {
            Nagabuff++;

        }
        else if (u.Species == "Ogre")
        {
            Ogrebuff++;

        }
        else if (u.Species == "Orc")
        {
           Orcbuff++;

        }
        else if (u.Species == "Troll")
        {
            Trollbuff++;

        }
        else if (u.Species == "Undead")
        {
            Undeadbuff++;

        }

    }

    void ApplyBuffs(Unit u)
    {
        if (Warlockbuff >= 1)
        {
            u.Lifesteal += 10;
        }
        if(Warriorbuff >= 1)
        {
            if(u.Class == "Warrior")
            {
                u.Armor += 5;
            }
        }
    }
   

    [Command]
    public void CmdScraddUnit(GameObject item,int X,int Y,int px,int py)
    {
        if(Y == -1 && py != -1)
        {
           
            BoardSave[px, py] = null;
            item.GetComponent<BoardLocation>().Bx = X;
            item.GetComponent<BoardLocation>().By = Y;
        }
        else if(py == -1 && Y != -1)
        {

            BoardSave[X, Y] = item.GetComponent<Unit>();
            item.GetComponent<BoardLocation>().Bx = X;
            item.GetComponent<BoardLocation>().By = Y;
            //selectedUnit.ArrayX = test.Bx;
            //selectedUnit.ArrayY = test.By;
        }
        else if(py != -1 && Y != -1)
        {
            BoardSave[X, Y] = item.GetComponent<Unit>();
            BoardSave[px, py] = null;
            item.GetComponent<BoardLocation>().Bx = X;
            item.GetComponent<BoardLocation>().By = Y;
        }

        item.GetComponent<Unit>().ArrayX = X;
        item.GetComponent<Unit>().ArrayY = Y;
       

    }
    [Command]
    public void CmdScrRemoveUnit(int px, int py)
    {
            BoardSave[px, py] = null;
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

        int aliveunits = 0;
        foreach (Unit item in EnemyUnits)
        {
            if (item != null)
            {
                aliveunits++;
            }
        }
        if (aliveunits <= 0)
        {
            roundmanager.battlecheck();
        }
        else
        {
            aliveunits = 0;
            foreach (Unit item in OwnUnits)
            {
                if (item != null)
                {
                    aliveunits++;
                }
            }
            if (aliveunits <= 0)
            {
                roundmanager.battlecheck();
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

    public void checkboard()
    {
        int aliveunits = -1;
        foreach (Unit item in EnemyUnits)
        {
            if(item != null)
            {
                aliveunits++;
            }
        }
        if(aliveunits <= 0)
        {
            roundmanager.battlecheck();
        }
        else
        {
            aliveunits = -1;
            foreach (Unit item in OwnUnits)
            {
                if (item != null)
                {
                    aliveunits++;
                }
            }
            if (aliveunits <= 0)
            {
                roundmanager.battlecheck();
            }
        }
       
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

        BattleBoard = new Unit[8, 8];
        OwnUnits = new Unit[10];
        EnemyUnits = new Unit[10];
}

    public void spawncreeps()
    {
        if(creeppower == 0)
        {
            Vector3 newposition = testArray[0].Planes[3].gameObject.transform.position;
            newposition.y = 4.7f;
            var Creep = (GameObject)Instantiate(UnitsPrefab, newposition, spawnRotation);
            Creep.tag = "Unit";
            Creep.GetComponent<BoardLocation>().Bx = 3;
            Creep.GetComponent<BoardLocation>().By = 0;
            Creep.GetComponent<Unit>().ArrayX = 3;
            Creep.GetComponent<Unit>().ArrayY = 0;
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
            Creep.GetComponent<Unit>().ArrayX = 4;
            Creep.GetComponent<Unit>().ArrayY = 0;
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
        startbattle();
    }
}
