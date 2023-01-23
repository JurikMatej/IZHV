using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Play screen left border (leftmost visible point)
    private float cameraViewBorderLeft;
    
    // Determines after how much reserve on the X axis the object has till eventually despawning
    public float despawnReserve = 5f;
    
    private void Start()
    {
        // <b>IMPORTANT:</b>
        // The split screen cameras must be aligned to each other at the X axis
        // In order for despawning to work properly
        cameraViewBorderLeft = SceneProperties.LeftBorder;
    }
    
    private void Update()
    {
        // Move the Obstacle to he left on each Update based on the Game Speed from Game State instance
        this.transform.position += Vector3.left * (GameState.Singleton.GameSpeed * Time.deltaTime);
        float despawnPoint = cameraViewBorderLeft - despawnReserve;
        
        if (this.transform.position.x < despawnPoint) 
        {
            // Despawn this obstacle if it is positioned outside of the camera view
            Destroy(gameObject);
        }
    }
}
