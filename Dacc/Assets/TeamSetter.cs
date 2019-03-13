using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TeamSetter : NetworkBehaviour
{

   [SyncVar] public int Team = 0;
    // Start is called before the first frame update
    void Awake()
    {

         GameObject spawn1;
         float dis;
         spawn1 = GameObject.Find("pSpawn");
         dis = Vector3.Distance(spawn1.transform.position, gameObject.transform.position);
         if (dis <= 10)
         {
             CmdScrTeamSetDestination(1);
             print(dis);
         }
         else
         {
             spawn1 = GameObject.Find("pSpawn(1)");
             dis = Vector3.Distance(spawn1.transform.position, gameObject.transform.position);
             if (dis <= 10)
             {
                 CmdScrTeamSetDestination(0);
                 print(dis);
             }

         }
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Command]
    public void CmdScrTeamSetDestination(int team)
    {
        Team = team;
        gameObject.GetComponent<TopDownController>().PlayerTeam = Team;
        RpcScrTeamSetDestination(team);
    }

    [ClientRpc]
    public void RpcScrTeamSetDestination(int team)
    {
        Team = team;
        gameObject.GetComponent<TopDownController>().PlayerTeam = Team;
    }
    [Command]
    public void CmdScrTeamPrintSetDestination(int team)
    {
        print(team);
    }

}
