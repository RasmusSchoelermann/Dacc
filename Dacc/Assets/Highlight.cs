using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Highlight : MonoBehaviour
{
    public float width = 1.0f;
    public Material mat;

    void OnMouseEnter()
    {
        width = 1.1f;
        mat.SetFloat("_OutlineWidth", width);
    }

    private void OnMouseExit()
    {
        width = 1.0f;
        mat.SetFloat("_OutlineWidth", width);
    }
}
