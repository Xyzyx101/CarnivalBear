using UnityEngine;
using System.Collections;

public class Chainsaw : MonoBehaviour
{
    PlayerCharacter Player;

    // Use this for initialization
    void Start()
    {
        Player = FindObjectOfType<PlayerCharacter>();
        if (Player == null)
        {
            Debug.LogError("Player not found");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            float damage = Player.GetDamage();
            enemy.Hurt(damage);
        }
    }
}
