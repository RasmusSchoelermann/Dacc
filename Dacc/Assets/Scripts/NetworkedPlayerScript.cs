using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedPlayerScript : NetworkBehaviour
{
    public TopDownController controller;
    public Camera fpsCamera;
    //public AudioListener audioListener;

    [SyncVar] public int Team = 0;

    int milis = 2000;

    public override void OnStartLocalPlayer()
    {
        controller.enabled = true;
        fpsCamera.enabled = true;
        //audioListener.enabled = true;


        gameObject.name = "LOCAL Player";
        StartCoroutine(ExecuteAfterTime(3));
        base.OnStartLocalPlayer();
        
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

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
                //print(dis);
            }

        }

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
