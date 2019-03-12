using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TopDownController : MonoBehaviour
{

    public Animator anim;
    private NavMeshAgent navMeshAgent;
    public bool walking;
    public NavMeshAgent Agent;
    public Camera pcam;

    public float pCamSpeed = 20f;
    public float pBorder = 10f;
    public float sSpeed = 2;

    public float minY;
    public float maxY;

    public Vector2 panLimit;

    void Awake()
    {
        navMeshAgent = Agent.GetComponent<NavMeshAgent>();
        pcam = GameObject.Find("PlayerCamera").GetComponent<Camera>();
    }

    void Update()
    {
        Ray ray = pcam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        anim.SetBool("IsWalking", walking);

        if (Input.GetButtonDown("Fire2"))
        {
            if(Physics.Raycast(ray, out hit, 1000))
            {
                navMeshAgent.destination = hit.point;
                navMeshAgent.Resume();
            }
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (!navMeshAgent.hasPath || Mathf.Abs (navMeshAgent.velocity.sqrMagnitude) < float.Epsilon)
            {
                anim.StopPlayback();
                walking = false;
            }
        }
        else
        {
            anim.StopPlayback();
            walking = true;
        }

        Vector3 pos = pcam.transform.position;
        if(Input.GetKey("right") || Input.mousePosition.x >= Screen.width - pBorder)
        {
            pos.z += pCamSpeed * Time.deltaTime;
        }
        else if(Input.GetKey("left") || Input.mousePosition.x <= pBorder)
        {
            pos.z -= pCamSpeed * Time.deltaTime;
        }
        else if (Input.GetKey("up") || Input.mousePosition.y >= Screen.height - pBorder)
        {
            pos.x -= pCamSpeed * Time.deltaTime;
        }
        else if (Input.GetKey("down") || Input.mousePosition.y <= pBorder)
        {
            pos.x += pCamSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * sSpeed * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        pcam.transform.position = pos;
        //Ray ray = Camera.pCam.ScreenPoint
    }
}