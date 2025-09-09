using UnityEditor.U2D;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Vector3 direccion;
    float moveY = 0;
    [SerializeField] float speed = 5;
    [SerializeField] float maxFallingSpeed = 25;
    [SerializeField] float gravity = 9.81f;
    CharacterController selfController;
    public Transform target;
   
    void Start()
    {
        selfController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        direccion = (target.position - transform.position);
        direccion.y = 0;
         direccion.Normalize();
        if (selfController.isGrounded)
        {
            moveY = 0;
        }
        else
        {
            moveY = gravity;
            if (moveY < -maxFallingSpeed)
            {
                moveY = -maxFallingSpeed;
            }
        }
        
        direccion.y = -gravity;
        selfController.Move(direccion * speed * Time.fixedDeltaTime);
    }
}
