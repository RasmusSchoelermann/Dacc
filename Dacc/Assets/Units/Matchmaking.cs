using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Matchmaking : NetworkBehaviour
{

    public List<Battle> Boards;
    public List<Battle> AvaibleEnemy;

    // Start is called before the first frame update
    void Start()
    {
        //getrefs();
    }

    // Update is called once per frame
    void Update()
    {

    }

 
    public void updatelist()
    {
        int temp = Boards.Count;
        for (int i = 0; i < temp; i++)
        {
            if (Boards[i].alive == false)
            {
                Boards.RemoveAt(i);
                i--;
                temp = Boards.Count;
            }
        }
        temp = AvaibleEnemy.Count;
        for (int i = 0; i < temp; i++)
        {
            if (AvaibleEnemy[i].alive == false)
            {
                AvaibleEnemy.RemoveAt(i);
                i--;
                temp = AvaibleEnemy.Count;
            }
        }

    }

    public void StartMatchmaking()// index wir d von länge abgezogen und ergebinis wird drauf addiert
    {
        Quaternion spawnRotation = new Quaternion();
        Random.InitState(Random.Range(0, 1000));
        List<Battle> tempAvaibleEnemy = new List<Battle>(AvaibleEnemy); 
        //randUID = Random.Range(0, 4);
        foreach (Battle item in Boards)
        {
            if(tempAvaibleEnemy.Contains(item))
            {
                tempAvaibleEnemy.Remove(item);

                int random = Random.Range(0, tempAvaibleEnemy.Count);
                Battle enemy = tempAvaibleEnemy[random];
                tempAvaibleEnemy.Remove(enemy);
                Unit[] tempEnemyUnits = new Unit[10];
                int c = 0;
                foreach (Unit U in enemy.OwnUnits)
                {
                    if (U != null)
                    {
                        GameObject temp = (GameObject)Instantiate(U.gameObject, U.gameObject.transform.position, spawnRotation);
                        NetworkServer.Spawn(temp);
                        tempEnemyUnits[c] = temp.GetComponent<Unit>();
                        tempEnemyUnits[c].ArrayX = U.GetComponent<BoardLocation>().Bx;
                        tempEnemyUnits[c].ArrayY = U.GetComponent<BoardLocation>().By;
                        //BattleBoard[U.ArrayX, U.ArrayY] = temp.GetComponent<Unit>();
                        temp.GetComponent<Unit>().Team = U.Team;
                        temp.GetComponent<Unit>().BoardTeam = false;
                        //tempEnemyUnits[c] = temp.GetComponent<Unit>();
                        c++;
                    }
                }

                foreach (Unit U in tempEnemyUnits)
                {
                    if (U != null)
                    {
                        U.ArrayX = 7 - U.ArrayX;
                        U.ArrayY = 7 - U.ArrayY;
                    Vector3 newposition = item.testArray[U.ArrayY].Planes[U.ArrayX].gameObject.transform.position;
                    newposition.y = 4.4f;
                    U.transform.position = newposition;
                        item.BattleBoard[U.ArrayX, U.ArrayY] = U;
                    }

                }

                item.EnemyUnits = tempEnemyUnits;

                tempAvaibleEnemy.Add(item);
            }
            else
            {
                int random = Random.Range(0, tempAvaibleEnemy.Count);
                Battle enemy = tempAvaibleEnemy[random];
                tempAvaibleEnemy.Remove(enemy);
                Unit[] tempEnemyUnits = new Unit[10];
                int c = 0;
                foreach  (Unit U in enemy.OwnUnits)
                {
                    if (U != null)
                    {
                        GameObject temp = (GameObject)Instantiate(U.gameObject, U.gameObject.transform.position, spawnRotation);
                        NetworkServer.Spawn(temp);
                        tempEnemyUnits[c] = temp.GetComponent<Unit>();
                        tempEnemyUnits[c].ArrayX = U.GetComponent<BoardLocation>().Bx;
                        tempEnemyUnits[c].ArrayY = U.GetComponent<BoardLocation>().By;
                        //BattleBoard[U.ArrayX, U.ArrayY] = temp.GetComponent<Unit>();
                        temp.GetComponent<Unit>().Team = U.Team;
                        temp.GetComponent<Unit>().BoardTeam = false;
                        //tempEnemyUnits[c] = temp.GetComponent<Unit>();
                        c++;
                    }
                }

                foreach  (Unit U in tempEnemyUnits)
                {
                    if (U != null)
                    {
                        U.ArrayX = 7 - U.ArrayX;
                        U.ArrayY = 7 - U.ArrayY;
                        Vector3 newposition = item.testArray[U.ArrayY].Planes[U.ArrayX].gameObject.transform.position;
                        newposition.y = 4.4f;
                        U.transform.position = newposition;
                        item.BattleBoard[U.ArrayX, U.ArrayY] = U;

                    }

                }

                item.EnemyUnits = tempEnemyUnits;
            }
           
        }

        foreach (Battle item in Boards)
        {
           item.battle();
        }
    }
    
}
