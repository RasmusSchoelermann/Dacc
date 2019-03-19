using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RoundManager : NetworkBehaviour
{
    public List<GameObject> Boards;
    int ready = 0;
    int setupready = 0;
    Matchmaking matchmaking;
    public List<TopDownController> controllers;
    

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
            ready = 0;
            setupready = 0;
           
            foreach (GameObject c in Boards)
            {
                c.GetComponent<Battle>().startbattle();
                
               
               
            }
            
        }
        if (Input.GetKeyDown("b"))
        {
            if (isServer == false)
            {
                return;
            }
            foreach (TopDownController c in controllers)
            {
                c.Board.GetComponent<Battle>().endbattle();
            }
        }
       

        }

    public void addcontroller(GameObject c)
    {
        if (controllers.Contains(c.GetComponent<TopDownController>()))
        {

        }
        else
        {
            controllers.Add(c.GetComponent<TopDownController>());
        }
       
    }

    public void getrefs()
    {
      
        matchmaking = GameObject.FindGameObjectWithTag("Matchmaking").GetComponent<Matchmaking>();
    }

    public void readycheck()
    {
        ready++;
        int temp = Boards.Count;
        for (int i = 0; i < temp; i++)
        {
            if (Boards[i].GetComponent<Battle>().alive == false)
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
