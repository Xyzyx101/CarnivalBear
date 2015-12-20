using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour {
    [SerializeField]
    float Health;

    void OnCollisionEnter(Collision collision)
    {
       if(collision.collider.tag=="Player")
        {
            PlayerCharacter player =  collision.collider.gameObject.GetComponent<PlayerCharacter>();
            if(player!=null)
            {
                player.Heal(Health);
                Destroy(gameObject);
            }
        }
    }
}
