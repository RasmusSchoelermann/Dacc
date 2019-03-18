using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Highlight : MonoBehaviour
{
    public float width = 1.0f;
    public Material mat;
    public Texture tex;

    void Start()
    {
        mat.mainTexture = tex;
        tex = Instantiate(mat.mainTexture);
        mat.mainTexture = tex;
    }

    void OnMouseEnter()
    {
        width = 1.1f;
        gameObject.GetComponentInChildren<Renderer>().material.SetFloat("_OutlineWidth", width);
    }

    private void OnMouseExit()
    {
        width = 1.0f;
        gameObject.GetComponentInChildren<Renderer>().material.SetFloat("_OutlineWidth", width);
    }
}
