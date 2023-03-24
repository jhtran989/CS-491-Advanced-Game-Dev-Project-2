using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float movementSpeed;

        public Transform orientation;

        float horizontalInput, verticalInput;

        Vector3 moveDirection;

        Rigidbody rb;

        private void Start() 
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void Update() 
        {
            PlayerInput();
        }

        private void PlayerInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }

        private void MovePlayer()
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);        
        }
    }
}
