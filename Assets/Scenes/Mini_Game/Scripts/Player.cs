using UnityEngine;
using UnityEngine.InputSystem;

namespace Mini_Game.Scripts 
{
    public class Player : MonoBehaviour
    {
        [Header("Attribute")]
        [SerializeField] Rigidbody2D rb;

        [Header("Properties")]
        [SerializeField] float moveSpeed = 1;
        [SerializeField] float jumpHeight = 3;

        [Header("Control")]
        [SerializeField] InputAction moveAction;
        [SerializeField] InputAction jumpAction;

        bool isGround;
        float move_axis;

        public void Move(float axis)
        {
            var speed = moveSpeed * axis;
            rb.linearVelocityX = speed; 
        }

        public void Jump()
        {
            if (!isGround) return;

            var velocity = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * jumpHeight);
            rb.linearVelocityY = velocity;
        }

        void Awake()
        {
            moveAction.performed += callback => { move_axis = callback.ReadValue<float>(); };
            moveAction.canceled += callback => { move_axis = 0; };
            jumpAction.performed += callback => { Jump(); };
        }

        void OnEnable()
        {
            moveAction.Enable();
            jumpAction.Enable();
        }

        void OnDisable()
        {
            moveAction.Disable();
            jumpAction.Disable();
        }
        
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("GameOver");
                Manager.instance.GameOver();
            }
        }

        void Update()
        {
            isGround = Physics2D.Raycast(transform.position, Vector2.down, 0.6f);

            Move(move_axis);
        }
    }
}