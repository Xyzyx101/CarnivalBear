using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrashCan : Enemy
{
    [SerializeField]
    GameObject TurkeyLegPrefab;
    [SerializeField]
    GameObject GarbageExplosion;

    override protected void Die()
    {
        int legs = Random.Range(1, 2);
        for (int i = 0; i < legs; ++i)
        {
            Instantiate(TurkeyLegPrefab, transform.position + 3f * Vector3.up, transform.rotation);
        }
        var colliders = Physics.OverlapSphere(transform.position, 4f);
        var rigidbodies = new List<Rigidbody>();
        foreach (var col in colliders)
        {
            if (col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody))
            {
                rigidbodies.Add(col.attachedRigidbody);
            }
        }
        foreach (var rb in rigidbodies)
        {
            rb.AddExplosionForce(200f, transform.position, 3f, 1f, ForceMode.Impulse);
        }
        Instantiate(GarbageExplosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
