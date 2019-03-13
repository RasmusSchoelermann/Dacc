using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public GameObject[,] PlanesArray;
    Unit[,] BattleBoard;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startbattle(Unit[,] boardref)
    {
        BattleBoard = boardref;
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
        Vector3 newposition = PlanesArray[X, Y].gameObject.transform.position;
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
