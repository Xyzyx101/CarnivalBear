using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    [SerializeField]
    float HP = 100f;

	public void Hurt(float amount)
    {
        HP -= amount;
        if (HP <= 0f)
        {
            Die();
        }
    }

    virtual protected void Die()
    {
        Destroy(gameObject);
    }
}
