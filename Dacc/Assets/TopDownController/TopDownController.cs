﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class TopDownController : NetworkBehaviour
{

    public Animator anim;
    public GameObject Board;
    public GameObject Bankfrech;
    public GameObject pooler;
    public NavMeshAgent navMeshAgent;
    public bool walking;
    public Camera pcam;
    [SyncVar] public int PlayerTeam = 0;

    public float pCamSpeed = 20f;
    public float pBorder = 10f;
    public float sSpeed = 2;

    public float minY;
    public float maxY;

    public Vector2 panLimit;

    public bool UnitHit = false;
    public RoundManager roundmanager;

    public Unit[,] Feld = new Unit[8,8];
    Unit[] Bank = new Unit[8];
   
        
    GameObject currentUnit;
    Unit selectedUnit;
    int Px;
    int Py;

    //UI
    //public Image pLock;
    public int[] Units = new int[5];
    public GameObject pBuyUi;
    //public Button pUnit;
    public Button pUnitButton,pUnitButton1,pUnitButton2,pUnitButton3,pUnitButton4,pRollButton,pExitButton;
    public Button pMove, pbBank, pSell, pRoll, pLevel;
    bool UIActive = false;
    bool SellSelected = false;
    //Pool
    public Vector3 enemyspawnposition;
    public Vector3 spawnposition;
    public Quaternion spawnRotation;
    public GameObject UPool;

    //Player
    public int Level = 1;


    [System.Serializable]
    public class MyEventType : UnityEvent { }

    public MyEventType OnEvent;

    void Awake()
    {
        pBuyUi.SetActive(false);
        StartCoroutine(ExecuteAfterTime(4));

    }

    public int PullUnit()
    {
        int randChance;
        GameObject UnitsPrefab;
        randChance = Random.Range(1, 100);
        if (Level == 1)
        {
            int randUID;
            Random.seed = Random.Range(0, 1000);
            randUID = Random.Range(0, 4);
            int uID = UPool.GetComponent<Pool>().Seltenheit[0].Units[randUID].gameObject.GetComponent<Unit>().id;
            /*if(randChance <=50)
            {
                UnitsPrefab = UPool.GetComponent<Pool>().Seltenheit[0].Units[uID];
            }
            else
            {
                UnitsPrefab = UPool.GetComponent<Pool>().Seltenheit[0].Units[uID];
            }*/
            //CmdScrPullUnit(i,PlayerTeam,uID);

            return uID;
        }
        return 0;
        
    }

    void NewRound()
    {
        for (int i = 0; i < 5; i++)
        {
           Units[i] = PullUnit();
        }
        pUnitButton.gameObject.SetActive(true);
        pUnitButton1.gameObject.SetActive(true);
        pUnitButton2.gameObject.SetActive(true);
        pUnitButton3.gameObject.SetActive(true);
        pUnitButton4.gameObject.SetActive(true);
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


            if (Input.GetButtonDown("Fire1"))
            {
                SellUnit();
            }

            if (Input.GetKeyDown("space"))
            {
                NewRound();
                switch (UIActive)
                {
                    case false:
                        pBuyUi.SetActive(true);
                        UIActive = true;
                        break;
                    case true:
                        pBuyUi.SetActive(false);
                        UIActive = false;
                        break;
                    default:
                        break;
                }
            }
            if (Input.GetKeyDown("e"))
            {
                pSell.onClick.Invoke();
            }

            if (Input.GetKey("y"))
            {
                Board.GetComponent<Battle>().startbattle(Feld);
            }
            if (Input.GetKeyDown("q"))
            {
                MoveUnit();
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
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.tag == "Feld")
                        {
                            BoardLocation test = hit.transform.gameObject.GetComponent<BoardLocation>();

                            if (Feld[test.Bx, test.By] == null)
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
                        else if (hit.transform.gameObject.tag == "Bank")
                        {

                            BoardLocation test = hit.transform.gameObject.GetComponent<BoardLocation>();
                            if (Bank[test.Bx] == null)
                            {
                                CmdScrFigureSetDestination(hit.transform.gameObject.transform.position, currentUnit);
                                UnitHit = false;
                                Bank[test.Bx] = selectedUnit;
                                if (Py == -1)
                                {
                                    Bank[Px] = null;
                                }
                                else
                                {
                                    Feld[Px, Py] = null;

                                }
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

    public void B1()
    {
        BuyUnit(0);
    }
    public void B2()
    {
        BuyUnit(1);
    }
    public void B3()
    {
        BuyUnit(2);
    }
    public void B4()
    {
        BuyUnit(3);
    }
    public void B5()
    {
        BuyUnit(4);
    }

    public void BuyUnit(int slot)
    {
        for (int Bankpos = 0; Bankpos < 8;)
        {
            if (Bank[Bankpos] == null)
            {
                print("oh boy it begins again");
                EventSystem.current.currentSelectedGameObject.SetActive(false);
                spawnposition = Bankfrech.GetComponent<Bank>().Slots[Bankpos].gameObject.transform.position;
                spawnposition.y = 4f;
                // Quaternion test = new Quaternion();
                // test.y = -90;
                CmdScrbuyUnit(spawnposition ,Units[slot] ,Bankpos,PlayerTeam);
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

    public void MoveUnit()
    {
        Ray ray = pcam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "Unit")
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

    public void SellSelect()
    {
        SellSelected = true;
    }

    public void SellUnit()
    {
        print("Sold");
        Ray ray = pcam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool UnitSelected = false;
        if (SellSelected == true)
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "Unit")
                {
                    if (hit.transform.gameObject.GetComponent<Unit>().Team == PlayerTeam)
                    {
                        currentUnit = hit.transform.gameObject;
                        Destroy(currentUnit);
                    }
                    else
                        return;
                }
            }
        }
    }

    void bDown()
    {

    }

    void bUp()
    {

    }
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        UPool = GameObject.FindGameObjectWithTag("Pool");
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
        roundmanager = GameObject.FindGameObjectWithTag("Roundmanager").GetComponent<RoundManager>();
        if (isServer == true)
        {
            roundmanager.getrefs();
        }
        CmdScrtest(gameObject);
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
    public void CmdScrsetTeam(int team)
    {
        PlayerTeam = team;
        RpcScrsetTeam(team);
    }

    [ClientRpc]
    public void RpcScrsetTeam(int team)
    {
        PlayerTeam = team;
    }
    [Command]
    public void CmdScrTeamPrint(int team)
    {
        print(team);
    }

    [Command]
    public void CmdScrtest(GameObject c)
    {
        roundmanager.addcontroller(c);
    }

    [Command]
    public void CmdScrbuyUnit(Vector3 spawnpositionb, int uID,int Bankpos,int team)
    {
        
        var unit = (GameObject)Instantiate(UPool.GetComponent<Pool>().Seltenheit[0].Units[uID], spawnpositionb, spawnRotation);
        unit.tag = "Unit";
        unit.GetComponent<Unit>().Team = PlayerTeam;
        unit.GetComponent<BoardLocation>().Bx = Bankpos;
        unit.GetComponent<BoardLocation>().By = -1;

        NetworkServer.Spawn(unit);
        
        var rotationVector = transform.rotation.eulerAngles;
        rotationVector.y = -90;
        unit.transform.rotation = Quaternion.Euler(rotationVector);

        RpcScrBuyUnit(team,unit,Bankpos);


    }
    [ClientRpc]
    public void RpcScrBuyUnit(int team,GameObject unit,int Bankpos)
    {
        if(PlayerTeam == team)
        {
            Bank[Bankpos] = unit.GetComponent<Unit>();
        }
    }

    
}