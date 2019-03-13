using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;


public class TopDownController : NetworkBehaviour
{

    public Animator anim;
    public GameObject Board;
    public NavMeshAgent navMeshAgent;
    public bool walking;
    public NavMeshAgent Agent;
    public Camera pcam;
    public int PlayerTeam = 0;

    public float pCamSpeed = 20f;
    public float pBorder = 10f;
    public float sSpeed = 2;

    public float minY;
    public float maxY;

    public Vector2 panLimit;

    public bool UnitHit = false;

    Unit[,] Feld = new Unit[8,8];
    Unit[] Bank = new Unit[8];
   
        
    GameObject currentUnit;
    Unit selectedUnit;
    int Px;
    int Py;

    void Awake()
    {
        Feld[0, 6] = new Unit();
        Feld[3, 3] = new Unit();

        //navMeshAgent = Agent.GetComponent<NavMeshAgent>();
        //pcam = GameObject.Find("PlayerCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }
        Ray ray = pcam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        anim.SetBool("IsWalking", walking);

        if (Input.GetKey("y"))
        {
            Board.GetComponent<Battle>().startbattle(Feld);
        }

            if (Input.GetKey("q"))
        {
            if(Physics.Raycast(ray,out hit))
            {
                if(hit.transform.gameObject.tag == "Unit")
                {
                    if (hit.transform.gameObject.GetComponent<Unit>().Team == PlayerTeam)
                    {
                        currentUnit = hit.transform.gameObject;
                        BoardLocation test = hit.transform.gameObject.GetComponent<BoardLocation>();
                        selectedUnit = Feld[test.Bx, test.By];
                        Px = test.Bx;
                        Py = test.By;
                        UnitHit = true;
                    }
                    else
                        return;
                }
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (UnitHit == false)
            {
                if (Physics.Raycast(ray, out hit, 1000))
                {
                    CmdScrPlayerSetDestination(hit.point);
                }
            }
            else
            {
                if(Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.tag == "Feld")
                    {
                        CmdScrFigureSetDestination(hit.transform.gameObject.transform.position,currentUnit);
                        UnitHit = false;
                        BoardLocation test = hit.transform.gameObject.GetComponent<BoardLocation>();
                        if (Feld[test.Bx,test.By] == null)
                        {
                            Feld[test.Bx, test.By] = selectedUnit;
                            Feld[Px, Py] = null;
                            currentUnit.GetComponent<BoardLocation>().Bx = test.Bx;
                            currentUnit.GetComponent<BoardLocation>().By = test.By;
                        }

                    }
                    else if(hit.transform.gameObject.tag == "Bank")
                    {
                        CmdScrFigureSetDestination(hit.transform.gameObject.transform.position, currentUnit);
                        UnitHit = false;
                        BoardLocation test = hit.transform.gameObject.GetComponent<BoardLocation>();
                        if(Bank[test.Bx] == null)
                        {
                            Bank[test.Bx] = selectedUnit;
                            Feld[Px, Py] = null;
                            currentUnit.GetComponent<BoardLocation>().Bx = test.Bx;
                            currentUnit.GetComponent<BoardLocation>().By = test.By;
                        }
                        
                       
                    }
                }
            }
        }
        if (navMeshAgent != null)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || Mathf.Abs(navMeshAgent.velocity.sqrMagnitude) < float.Epsilon)
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

    [Command]
    public void CmdScrFigureSetDestination(Vector3 argPosition, GameObject Unit)
    {//Step B, I do simple work, I not verifi a valid position in server, I only send to all clients
        Vector3 temp = argPosition;
        temp.y = Unit.transform.position.y;
        Unit.transform.position = temp;
    }

    [ClientRpc]
    public void RpcScrFigureSetDestination(Vector3 argPosition, GameObject Unit)
    {//Step C, only the clients move
        Vector3 temp = argPosition;
        temp.y = Unit.transform.position.y;
        Unit.transform.position = temp;
    }

}