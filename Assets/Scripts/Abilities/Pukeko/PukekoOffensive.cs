using System.Collections;
using UnityEngine;

/// <summary>
/// Sonic Squawk- sound wave that goes in has a cone effect that silences birds 
/// (making them unable to use abilities for 3 seconds) and pushes them back roughly 2m 
/// (40s cooldown)
/// </summary>
public class PukekoOffensiveAbility : BirdAbility
{
    [Header("Pukeko Offensive Settings")]
    [SerializeField] private float cooldown = 40f;
    [SerializeField] private float silenceDuration = 3f;
    [SerializeField] private float pushBackForce = 2f;

    [Header("Cone Settings")]
    [SerializeField] private float coneAngle = 45f;
    [SerializeField] private float coneRange = 5f;
    [SerializeField] private int coneRayCount = 10; // Number of rays to cast within the cone (adjust for performance and feel)

    private bool onCooldown = false;
    private RaycastHit[] hits; // Pre-allocate to avoid garbage collection as long as possible

    void Awake()
    {
        hits = new RaycastHit[coneRayCount];
    }

    public void OnOffensiveAbility()
    {
        if (!onCooldown)
        {
            onCooldown = true;
            StartCoroutine(SonicSquawk());
        }
    }

    private IEnumerator SonicSquawk()
    {
        // Find all birds in the cone area with raycast
        for (int i = 0; i < coneRayCount; i++)
        {
            float angle = -coneAngle / 2 + coneAngle / (coneRayCount - 1) * i; // Split into equal segments
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward; // Offset from where bird is facing
            int hitCount = Physics.RaycastNonAlloc(transform.position, direction, hits, coneRange);
            for (int j = 0; j < hitCount; j++)
            {
                if (hits[j].collider.CompareTag("Bird"))
                {
                    // Apply silence effect to the bird
                    if (hits[j].collider.TryGetComponent<BirdAbility>(out var birdAbility))
                        StartCoroutine(ApplySilence(silenceDuration, birdAbility));

                    // Apply push back force to the bird
                    if (hits[j].collider.TryGetComponent<Rigidbody>(out var rb))
                    {
                        Vector3 pushDirection = (hits[j].collider.transform.position - transform.position).normalized;
                        rb.AddForce(pushDirection * pushBackForce, ForceMode.Impulse);
                    }
                }
            }
        }

        yield return new WaitForSeconds(cooldown);
        onCooldown = false;   
    }

    public IEnumerator ApplySilence(float duration, BirdAbility bird)
    {
        bird.DisableAbilities(true);
        yield return new WaitForSeconds(duration);
        bird.DisableAbilities(false);
    }
}
