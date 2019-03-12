using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class NewController : NetworkBehaviour
{
    public Camera pCam;
    public NavMeshAgent navMeshAgent;
    //public bool walking;

    void Awake()
    {

    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        Ray ray = pCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetButtonDown("Fire2"))
        {
            if (Physics.Raycast(ray, out hit, 1000))
            {
                //this.transform.position = hit.transform.position;
                CmdScrPlayerSetDestination(hit.point);
            }
        }
    }
    [Command]
    public void CmdScrPlayerSetDestination(Vector3 argPosition)
    {//Step B, I do simple work, I not verifi a valid position in server, I only send to all clients
        RpcScrPlayerSetDestination(argPosition);
        navMeshAgent.SetDestination(argPosition);
    }

    [ClientRpc]
    public void RpcScrPlayerSetDestination(Vector3 argPosition)
    {//Step C, only the clients move
        navMeshAgent.SetDestination(argPosition);
    }
}