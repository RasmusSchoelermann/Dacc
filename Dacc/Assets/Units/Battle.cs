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

    [System.Serializable]
    public class Planes2d
    {
        public GameObject[] Planes = new GameObject[8];
    }
    public Planes2d[] testArray = new Planes2d[8];

    public GameObject[,] PlanesArray;

  
    bool spawn = false;
    Unit[,] BattleBoard = new Unit[8,8];

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(!isServer)
        {
            return;
        }


        if(Input.GetKeyDown("b"))
        {
            startbattle(BattleBoard);
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
        }

        
    }

    public void startbattle(Unit[,] boardref)
    {
        //BattleBoard = boardref;
        foreach (Unit U in BattleBoard)
        {
            if (U != null)
            {
                U.ArrayX = U.GetComponent<BoardLocation>().Bx;
                U.ArrayY = U.GetComponent<BoardLocation>().By;
                U.startAi(this, PlanesArray);
            }

        }
    }

    public void MoveUnit(Unit Unittomove, int X,int Y)
    {
        Vector3 newposition = testArray[X].Planes[Y].gameObject.transform.position;
        newposition.y = Unittomove.gameObject.transform.position.y;
        Unittomove.gameObject.transform.position = newposition;
        BattleBoard[X, Y] = Unittomove;
        BattleBoard[Unittomove.ArrayX, Unittomove.ArrayX] = null;
        Unittomove.ArrayX = X;
        Unittomove.ArrayY = Y;

    }

    public void RangeCheck(Unit Unit)
    {

    }
}
