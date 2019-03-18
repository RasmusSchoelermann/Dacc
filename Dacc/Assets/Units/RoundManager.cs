using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RoundManager : NetworkBehaviour
{
    List<Battle> Boards;
    int ready = 0;
    Matchmaking matchmaking;
    List<TopDownController> controllers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown("y"))
        {
            if (isServer == false)
            {
                return;
            }
            foreach (TopDownController c in controllers)
            {
                c.Board.GetComponent<Battle>().startbattle(c.Feld);
            }
            
        }
    }

    public void addcontroller(GameObject c)
    {
        controllers.Add(c.GetComponent<TopDownController>());
    }

    public void getrefs()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Feld");
        int c = 0;
        foreach (GameObject item in temp)
        {
            if (item.name.StartsWith("Board"))
            {
                Boards[c] = item.GetComponent<Battle>();
                c++;
            }
        }
        matchmaking = GameObject.FindGameObjectWithTag("Matchmaking").GetComponent<Matchmaking>();
    }

    public void readycheck()
    {
        ready++;
        int temp = Boards.Count;
        for (int i = 0; i < temp; i++)
        {
            if (Boards[i].alive == false)
            {
                Boards.RemoveAt(i);
            }
        }

        if (ready >= Boards.Count)
        {
            matchmaking.updatelist();
            matchmaking.StartMatchmaking();
        }
    }
}
