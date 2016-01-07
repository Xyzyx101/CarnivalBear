using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UnityStandardAssets.Effects.ParticleSystemMultiplier))]
public class RollerBombExplosion : MonoBehaviour
{
    public float ExplosionForce = 4f;
    public float ExplosionRadius = 10f;
    public float Damage = 100f;
    public float DamageRadius = 2f;
    public LayerMask DamageMask;
    public float DestroyTime = 3f;
    private bool Active = true;

    private IEnumerator Start()
    {
        // wait one frame because some explosions instantiate debris which should then
        // be pushed by physics force
        yield return null;

        float multiplier = GetComponent<UnityStandardAssets.Effects.ParticleSystemMultiplier>().multiplier;

        float r = ExplosionRadius * multiplier;
        var cols = Physics.OverlapSphere(transform.position, r);
        var rigidbodies = new List<Rigidbody>();
        foreach (var col in cols)
        {
            if (col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody))
            {
                rigidbodies.Add(col.attachedRigidbody);
            }
        }
        foreach (var rb in rigidbodies)
        {
            rb.AddExplosionForce(ExplosionForce * multiplier, transform.position, r, 1 * multiplier, ForceMode.Impulse);
        }
    }

    void Update()
    {
        if (Active)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, DamageRadius, DamageMask, QueryTriggerInteraction.Ignore);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].tag == "Player")
                {
                    PlayerCharacter player = hitColliders[i].GetComponent<PlayerCharacter>();
                    player.Hurt(Damage);
                }
                i++;
            }
        }
        Active = false;
        DestroyTime -= Time.deltaTime;
        if (DestroyTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

}
