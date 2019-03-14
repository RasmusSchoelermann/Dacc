using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int Team;
    public int ArrayX;
    public int ArrayY;
    public int Hp = 100;
    public int Damage = 10;
    public int Range = 1;
    public int Attackspeed = 1;
    Battle test;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startAi(Battle Battleboard ,GameObject[,] BattleBoardPlanes)
    {
        test = Battleboard;
        //gameObject.transform.position;
        test.MoveUnit(this, 3, 3);
    }

    void stopAi()
    {

    }

    void BattleKI()
    {

    }
        
}
