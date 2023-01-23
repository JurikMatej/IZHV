using UnityEngine;

/// <summary>
/// TODO Player1 Script is assigned to P2 GameObject - FIX
/// </summary>
public class Player1 : BasePlayer
{
    public Color pColorRespawned = Color.HSVToRGB(0.33f, 0.5f, 0.5f);

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
    /// <param name="duration"></param>
    public override void EnterRespawnedState(float duration)
    {
        // Give invulnerability
        this.gameObject.layer = LayerMask.NameToLayer("P1Invulnerable");
        
        // Fade out the player's color to signify respawned state
        this._sr.color = this.pColorRespawned;
        
        // Register callback for leaving the respawned state in 3s
        Invoke(nameof(LeaveRespawnedState), duration);
    }

    protected override void LeaveRespawnedState()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Player1");
        this._sr.color = this.pColor;
    }
}
