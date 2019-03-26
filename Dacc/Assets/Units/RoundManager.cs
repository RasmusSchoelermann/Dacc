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
    bool battle = true;
    float preparetime = 30;
    float battletime = 60;
    int battleready = 0;
    int round = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        if (isServer == false)
        {
            return;
        }
        //StartCoroutine(ExecuteAfterTime(35));
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

            StopCoroutine(ExecuteAfterTime(1));
            StartCoroutine(ExecuteAfterTime(2));

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
                i--;
                temp = Boards.Count;
            }
        }

        if (ready >= Boards.Count)
        {
            battleready = 0;
            if (round < 4 || round > 5 && round % 5 == 0)
            {
                foreach (GameObject c in Boards)
                {
                   
                    c.GetComponent<Battle>().battle();
                }
            }
            else
            {
                
                matchmaking.updatelist();
                matchmaking.StartMatchmaking();
            }
           
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        

        if (battle)
        {

            foreach (GameObject c in Boards)
            {
                c.GetComponent<Battle>().endbattle();
            }
            battle = false;
            round++;
            //NEW ROUND
            foreach (TopDownController item in controllers)
            {
                if (item.name.StartsWith("LOCAL"))
                {
                    item.CmdNewRound(round);
                }
            }
            yield return new WaitForSeconds(time);
            StartCoroutine(ExecuteAfterTime(battletime));
        }
        else
        {
            foreach (TopDownController item in controllers)
            {
                if (item.name.StartsWith("LOCAL"))
                {
                    item.CmdLockinput();
                }
            }
            yield return new WaitForSeconds(5);
            battle = true;
            ready = 0;
            setupready = 0;

            int temp = Boards.Count;
            for (int i = 0; i < temp; i++)
            {
                if (Boards[i].GetComponent<Battle>().alive == false)
                {
                    Boards.RemoveAt(i);
                    i--;
                    temp = Boards.Count;
                }
            }

            foreach (GameObject c in Boards)
            {
               
                if (round < 4 || round > 5 && round % 5 == 0)
                {
                   
                    c.GetComponent<Battle>().spawncreeps();
                 
                }
                else
                {
                    c.GetComponent<Battle>().startbattle();
                }

                


            }
          
            yield return new WaitForSeconds(time);
            StartCoroutine(ExecuteAfterTime(preparetime));
        }
       
        
    }

    public void battlecheck()
    {
        battleready++;
        int temp = Boards.Count;
        for (int i = 0; i < temp; i++)
        {
            if (Boards[i].GetComponent<Battle>().alive == false)
            {
                Boards.RemoveAt(i);
                i--;
                temp = Boards.Count;
            }
        }

        if (battleready >= Boards.Count)
        {
            StopCoroutine(ExecuteAfterTime(1));
            StartCoroutine(ExecuteAfterTime(preparetime));

        }
    }
   
}
