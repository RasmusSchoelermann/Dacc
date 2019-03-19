using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BoardLocation : NetworkBehaviour
{
    [SyncVar]public int Bx = 0;
    [SyncVar] public int By = 0;
}
   
