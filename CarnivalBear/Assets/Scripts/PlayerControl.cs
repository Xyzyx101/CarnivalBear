using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerCharacter))]
public class PlayerControl : MonoBehaviour
{
    // Cam Override will make controls relative to the camera.  If left blank then main camera will be used.
    public Camera CamOverride;
    private Transform Cam;
    private PlayerCharacter Character;
    private Vector3 Move;
    private bool Jump;

    // Use this for initialization
    void Start()
    {
        if (CamOverride != null)
        {
            Cam = CamOverride.transform;
        }
        else if (Camera.main != null)
        {
            Cam = Camera.main.transform;
        }
        else
        {
            Debug.Log("Warning no Main Camera or Camera Override.  Cam is required for Cameraspace controls. Using Worldspace.");
        }
        Character = GetComponent<PlayerCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Jump)
        {
            Jump = Input.GetButtonDown("Jump");
        }
        bool attack = Input.GetButtonDown("Attack");
        Character.Attack(attack);
    }

    private void FixedUpdate()
    {
        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool crouch = Input.GetButton("Crouch");
        
        Vector3 move = Vector3.zero;
        if (Cam != null)
        {
            Vector3 camforward = Vector3.Scale(Cam.forward, new Vector3(1, 0, 1)).normalized;
            move = v * camforward + h * Cam.right;
            move.y = 0.0f;
            //move = Vector3.ClampMagnitude(move, 1.0f);
        }

        Character.Move(move, crouch, Jump);
        Jump = false;
    }
}
