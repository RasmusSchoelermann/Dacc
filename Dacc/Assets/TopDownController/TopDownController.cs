using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Events;


public class TopDownController : NetworkBehaviour
{

    public Animator anim;
    public GameObject Board;
    public GameObject Bankfrech;
    public NavMeshAgent navMeshAgent;
    public bool walking;
    public NavMeshAgent Agent;
    public Camera pcam;
    [SyncVar] public int PlayerTeam = 0;

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

    //UI
    public Image pLock;
    public Image pBuyUi;
   // public Image pUnit;
    public Button pUnitButton;
    bool UIActive = false;

    //Battle
    public GameObject UnitsPrefab;
    public Vector3 enemyspawnposition;
    public Vector3 spawnposition;
    public Quaternion spawnRotation;

    //Unit


    [System.Serializable]
    public class MyEventType : UnityEvent { }

    public MyEventType OnEvent;

    void Awake()
    {
        pBuyUi.enabled = false;
        pLock.enabled = false;
        //pUnit.enabled = false;

        pUnitButton.onClick.AddListener(BuyUnit);
        StartCoroutine(ExecuteAfterTime(4));
    }

    

    public void BuyUnit()
    {
        
        for (int Bankpos = 0; Bankpos < 8;)
        {
            if (Bank[Bankpos] == null)
            {
                print("oh boy it begins again");
                spawnposition = Bankfrech.GetComponent<Bank>().Slots[Bankpos].gameObject.transform.position;
                spawnposition.y = 4.7f;
                var unit = (GameObject)Instantiate(UnitsPrefab, spawnposition, spawnRotation);
                unit.tag = "Unit";
                unit.GetComponent<Unit>().Team = PlayerTeam;
                unit.GetComponent<BoardLocation>().Bx = Bankpos;
                unit.GetComponent<BoardLocation>().By = -1;
                NetworkServer.Spawn(unit);
                Bank[Bankpos] = unit.GetComponent<Unit>();
                return;
            }
            else
            {
                Bankpos++;
                if (Bankpos == 8)
                {
                    print("Kein Platz");
                }
            }
            
        }
       
       
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

        
        if(Input.GetKeyDown("space"))
        {
            switch(UIActive)
            {
                case false:
                    pBuyUi.enabled = true;
                    pLock.enabled = true;
                    //pUnit.enabled = true;
                    UIActive = true;
                    break;
                case true:
                    pBuyUi.enabled = false;
                    pLock.enabled = false;
                   // pUnit.enabled = false;
                    UIActive = false;
                    break;
                default:
                    break;
            }
        }

        if (Input.GetKeyDown("f"))
        {

            var unit = (GameObject)Instantiate(UnitsPrefab, spawnposition, spawnRotation);
            unit.tag = "Unit";
            unit.GetComponent<Unit>().Team = 0;
            unit.GetComponent<BoardLocation>().Bx = 0;
            unit.GetComponent<BoardLocation>().By = 6;
            NetworkServer.Spawn(unit);
            Feld[0, 6] = unit.GetComponent<Unit>();

        }
        if (Input.GetKeyDown("e"))
        {

            var unit = (GameObject)Instantiate(UnitsPrefab, enemyspawnposition, spawnRotation);
            unit.tag = "Unit";
            unit.GetComponent<BoardLocation>().Bx = 3;
            unit.GetComponent<BoardLocation>().By = 3;
            NetworkServer.Spawn(unit);
            Feld[3, 3] = unit.GetComponent<Unit>();
        }

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
                        if (test.By == -1)
                        {
                            selectedUnit = Bank[test.Bx];
                        }
                        else
                        {
                            selectedUnit = Feld[test.Bx, test.By];

                        }
                        
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
                    if (hit.transform.tag == "Feld")
                    {
                        BoardLocation test = hit.transform.gameObject.GetComponent<BoardLocation>();

                        if (Feld[test.Bx,test.By] == null)
                        {
                            CmdScrFigureSetDestination(hit.transform.gameObject.transform.position, currentUnit);
                            UnitHit = false;

                            Feld[test.Bx, test.By] = selectedUnit;
                            if (Py == -1)
                            {
                                Bank[Px] = null;
                            }
                            else
                            {
                                Feld[Px, Py] = null;

                            }
                            
                            currentUnit.GetComponent<BoardLocation>().Bx = test.Bx;
                            currentUnit.GetComponent<BoardLocation>().By = test.By;
                        }
                        else
                            UnitHit = false;

                    }
                    else if(hit.transform.gameObject.tag == "Bank")
                    {
                        
                        BoardLocation test = hit.transform.gameObject.GetComponent<BoardLocation>();
                        if (Bank[test.Bx] == null)
                        {
                            CmdScrFigureSetDestination(hit.transform.gameObject.transform.position, currentUnit);
                            UnitHit = false;
                            Bank[test.Bx] = selectedUnit;
                            Feld[Px, Py] = null;
                            currentUnit.GetComponent<BoardLocation>().Bx = test.Bx;
                            currentUnit.GetComponent<BoardLocation>().By = -1;
                        }
                        else
                            UnitHit = false;
                        
                       
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

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Feld");
        foreach (GameObject g in temp)
        {
            if (g.name.StartsWith("Board"))
            {
                TBB tempi = g.GetComponent<TBB>();
                int test = tempi.team;
                if (test == PlayerTeam)
                {
                    Board = g;
                }
            }
        }
        temp = GameObject.FindGameObjectsWithTag("Bank");
        foreach (GameObject g in temp)
        {
            if (g.name.StartsWith("Bank"))
            {
                if (g.GetComponent<TBB>().team == PlayerTeam)
                {
                    Bankfrech = g;
                }
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

    [Command]
    public void CmdScrFigureSetDestination(Vector3 argPosition, GameObject Unit)
    {
        Vector3 temp = argPosition;
        temp.y = Unit.transform.position.y;
        Unit.transform.position = temp;
    }

    [ClientRpc]
    public void RpcScrFigureSetDestination(Vector3 argPosition, GameObject Unit)
    {
        Vector3 temp = argPosition;
        temp.y = Unit.transform.position.y;
        Unit.transform.position = temp;
    }

    [Command]
    public void CmdScrTeamSetDestination(int team)
    {
        PlayerTeam = team;
        RpcScrTeamSetDestination(team);
    }

    [ClientRpc]
    public void RpcScrTeamSetDestination(int team)
    {
        PlayerTeam = team;
    }
    [Command]
    public void CmdScrTeamPrintSetDestination(int team)
    {
        print(team);
    }
    

}