using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CrowOffensive : BirdAbility {
    public float cooldown = 10f;
    public float timeEnemiesAreImpacted = 3f;

    private bool onCooldown = false;
    private PlayerInput input; // Input for the player

    void Start()
    {
        input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (!onCooldown && input.actions.FindAction("Offensive Ability").WasPressedThisFrame()
            && CanUseAbilities() && PointInProgress())
        {
            CrowAbility();
        }
    }

    public void CrowAbility() 
    {
        if (onCooldown)
        {
            Debug.Log("The crow is on cooldown and cannot activate its ability");
        }

        StartCoroutine(DisableEnemies());
        StartCoroutine(Cooldown());
    }

    IEnumerator DisableEnemies()
    {
        // Determine which birds are on other team
        List<BirdAbility> enemyAbilities = new List<BirdAbility>();
        GameManager gameManager = GameManager.Instance;
        if (gameManager.leftPlayer1 == this || gameManager.leftPlayer2 == this)
        {
            enemyAbilities.AddRange(gameManager.rightPlayer1.GetComponents<BirdAbility>());
            enemyAbilities.AddRange(gameManager.rightPlayer2.GetComponents<BirdAbility>());
        }
        else
        {
            enemyAbilities.AddRange(gameManager.leftPlayer1.GetComponents<BirdAbility>());
            enemyAbilities.AddRange(gameManager.leftPlayer2.GetComponents<BirdAbility>());
        }

        // Disable all the enemies abilities
        foreach (BirdAbility enemy in enemyAbilities) 
        {
            enemy.DisableAbilities(true);
            Debug.Log(enemy);
        }

        // Wait for ability to end
        yield return new WaitForSeconds(timeEnemiesAreImpacted);

        // Enable all the enemies abilities
        foreach (BirdAbility enemy in enemyAbilities) 
        {
            enemy.DisableAbilities(false);
        }
    }

    private IEnumerator Cooldown() 
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }
}
