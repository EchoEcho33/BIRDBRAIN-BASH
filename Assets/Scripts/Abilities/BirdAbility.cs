using UnityEngine;

public class BirdAbility : MonoBehaviour {
    private bool abilitiesDisabled = false;

    public void DisableAbilities(bool disabledOrNot)
    {
        abilitiesDisabled = disabledOrNot;
    }

    public bool CanUseAbilities()
    {
        return !abilitiesDisabled;
    }

    public bool PointInProgress()
    {
        // If the point has just started, cannot use ability
        if (GameManager.Instance.gameState == GameManager.GameState.PointStart) return false;

        // If the point has just ended, cannot use ability, else we are good to go
        return GameManager.Instance.gameState != GameManager.GameState.PointEnd;
    }
}