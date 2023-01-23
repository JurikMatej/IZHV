using UnityEngine;

/// <summary>
/// TODO Player2 Script is assigned to P1 GameObject - FIX
/// </summary>
public class Player2 : BasePlayer
{
    public Color pColorRespawned = Color.HSVToRGB(0f, 0.5f, 0.5f);
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            this.Death();
        }

        if (other.CompareTag("Powerup2xScore"))
        {
            Destroy(other.gameObject);
            this.Activate2xScore();
        }
    }
    
    /// <summary>
    /// Switch player1's collision layer to invulnerable collision layer for 3s
    /// </summary>
    public override void EnterRespawnedState(float duration)
    {
        // Give invulnerability
        this.gameObject.layer = LayerMask.NameToLayer("P2Invulnerable");
        
        // Fade out the player's color to signify respawned state
        this._sr.color = this.pColorRespawned;
        
        // Register callback for leaving the respawned state in 3s
        Invoke(nameof(LeaveRespawnedState), duration);
    }

    protected override void LeaveRespawnedState()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Player2");
        this._sr.color = this.pColor;
    }
}
