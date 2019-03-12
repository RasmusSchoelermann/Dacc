using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TopDownController : MonoBehaviour
{

    private Animator anim;
    private NavMeshAgent navMeshAgent;
    private bool walking;
    public NavMeshAgent Agent;
    Camera pcam;


    void Awake()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = Agent.GetComponent<NavMeshAgent>();
        pcam = GameObject.Find("PlayerCamera").GetComponent<Camera>();
    }

    void Update()
    {
        Ray ray = pcam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Input.GetButtonDown("Fire2"))
        {
            if(Physics.Raycast(ray, out hit, 1000))
            {
                walking = true;
                navMeshAgent.destination = hit.point;
                navMeshAgent.Resume();
            }
        }
        //Ray ray = Camera.pCam.ScreenPoint
    }
}