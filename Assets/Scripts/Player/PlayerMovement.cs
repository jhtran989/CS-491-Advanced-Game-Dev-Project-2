using System;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float movementSpeed;

        public Transform orientation;

        float horizontalInput, verticalInput;

        Vector3 moveDirection;

        Rigidbody _player_rb;

        private void OnEnable()
        {
            // TODO: maybe create a file with all delegates and move the declarations there (instead of having a delegate in different scripts)
            TerminalTrigger.TerminalTriggerPlayerEnter += StopPlayerMovement;
            TerminalController.TerminalControllerPlayerLeave += ResumePlayerMovement;
        }
        
        private void OnDisable()
        {
            TerminalTrigger.TerminalTriggerPlayerEnter -= StopPlayerMovement;
            TerminalController.TerminalControllerPlayerLeave -= ResumePlayerMovement;
        }

        private void Start() 
        {
            _player_rb = GetComponent<Rigidbody>();
            _player_rb.freezeRotation = true;
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
            _player_rb.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);        
        }

        private void StopPlayerMovement()
        {
            _player_rb.velocity = Vector3.zero;
            
            // also freeze player position
            // constraints is just a bit mask -- to freeze, BITWISE AND with the Freeze Position constraint
            _player_rb.constraints |= RigidbodyConstraints.FreezePosition;
        } 

        private void ResumePlayerMovement()
        {
            // no unfreeze option...
            // constraints is just a bit mask -- to unfreeze, BITWISE AND with the negation of the Freeze Position constraint
            _player_rb.constraints &= ~RigidbodyConstraints.FreezePosition;
        }
    }
}
