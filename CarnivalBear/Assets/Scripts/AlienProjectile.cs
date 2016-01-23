using UnityEngine;
using System.Collections;

public class AlienProjectile : MonoBehaviour
{

    [SerializeField]
    float Force = 100f;
    [SerializeField]
    float Damage = 100f;
    [SerializeField]
    float DeathTime = 4f;
    float DeathTimer;

    Rigidbody RB;
    F3DParticleScale ParticleScale;

    AudioSource BounceSound;
    
    void Awake()
    {
        RB = GetComponent<Rigidbody>();
        ParticleScale = GetComponent<F3DParticleScale>();
        BounceSound = GetComponent<AudioSource>();
        DeathTimer = DeathTime;
    }
    
    void Update()
    {
        DeathTimer -= Time.deltaTime;
        if (DeathTimer < 0f)
        {
            Destroy(gameObject);
        }
    }

    public void Shoot(Vector3 targetPosition)
    {
        Vector3 shootDir = (targetPosition - transform.position).normalized;
        RB.AddForce(shootDir * Force, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        BounceSound.Play();
        if (collision.gameObject.tag == "Player")
        {
            PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();
            player.Hurt(Damage);
            DeathTimer = 0.5f;
            ParticleScale.ParticleScale *= 2f;
            RB.velocity = Vector3.zero;
        }
    }
}
