using UnityEngine;

/*
     * ComboWatcher.cs
     * Created by: Sylvia Yi
     * Date Created: 2025-03-24
     * 
     * Description: This script is in charge of the special VFX when COMBO score was received.
     * 
     * Last Changed by: Sylvia Yi
     * Last Date Changed: 2025-03-24
     * 
     *   -> 1.0 - Created ComboWatcher.cs and its' related particle system object, to display a larger frame when there is a combo hit.
     *
     */

public class ComboWatcher : MonoBehaviour
{
    public ScoreManager scoreManager;  
    public ParticleSystem comboParticles; 

    private float lastMultiplier = 1f; 

    void Update()
    {
        if (scoreManager != null && comboParticles != null)
        {
            if (scoreManager.CurrentMultiplier > 1f && scoreManager.CurrentMultiplier != lastMultiplier)
            {
                
                comboParticles.Play();
            }

            
            lastMultiplier = scoreManager.CurrentMultiplier;
        }
    }
}
