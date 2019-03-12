using UnityEngine;
using UnityEditor;

public class Unit : ScriptableObject
{
    public int Team;
    public int ArrayX;
    public int ArrayY;
    [MenuItem("Tools/MyTool/Do It in C#")]
    static void DoIt()
    {
        EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
    }
}