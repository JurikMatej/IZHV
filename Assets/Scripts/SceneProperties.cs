using System;
using UnityEngine;

public class SceneProperties : MonoBehaviour
{
    public static float LeftBorder { get; private set; }
    public static float RightBorder { get; private set; }
    public static float TopBorder { get; private set; }

    private void Start()
    {
        LeftBorder = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        RightBorder = LeftBorder + 20;
        TopBorder = Camera.main.ScreenToWorldPoint(Vector3.up).y + 5;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        
        Gizmos.DrawLine(new (LeftBorder, 0), new(LeftBorder, 5));
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new (RightBorder, 0), new (RightBorder, 5));
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new(-10, TopBorder), new(10, TopBorder));
    }
}
