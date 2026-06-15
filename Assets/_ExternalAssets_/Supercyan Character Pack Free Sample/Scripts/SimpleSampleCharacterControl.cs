using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Supercyan.FreeSample
{
    public class SimpleSampleCharacterControl : MonoBehaviour
    {
        private enum ControlMode
        {
            Tank,
            Direct
        }

        [Header("Movement")]
        [SerializeField] private float m_moveSpeed = 2f;
        [SerializeField] private float m_turnSpeed = 200f;
        [SerializeField] private float m_jumpForce = 4f;

        [Header("References")]
        [SerializeField] private Animator m_animator;
        [SerializeField] private Rigidbody m_rigidBody;

        [Header("Input")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference jumpAction;
        [SerializeField] private InputActionReference walkAction;

        [SerializeField] private ControlMode m_controlMode = ControlMode.Direct;

        private float m_currentV = 0;
        private float m_currentH = 0;

        private readonly float m_interpolation = 10;
        private readonly float m_walkScale = 0.33f;
        private readonly float m_backwardsWalkScale = 0.16f;
        private readonly float m_backwardRunScale = 0.66f;

        private bool m_wasGrounded;
        private Vector3 m_currentDirection = Vector3.zero;

        private float m_jumpTimeStamp = 0;
        private readonly float m_minJumpInterval = 0.25f;
        private bool m_jumpInput = false;

        private bool m_isGrounded;

        private readonly List<Collider> m_collisions = new();

        private void Awake()
        {
            if (!m_animator)
                m_animator = GetComponent<Animator>();

            if (!m_rigidBody)
                m_rigidBody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            moveAction?.action.Enable();
            jumpAction?.action.Enable();
            walkAction?.action.Enable();

            if (jumpAction != null)
                jumpAction.action.performed += OnJump;
        }

        private void OnDisable()
        {
            if (jumpAction != null)
                jumpAction.action.performed -= OnJump;

            moveAction?.action.Disable();
            jumpAction?.action.Disable();
            walkAction?.action.Disable();
        }

        private void OnJump(InputAction.CallbackContext ctx)
        {
            m_jumpInput = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
                {
                    if (!m_collisions.Contains(collision.collider))
                        m_collisions.Add(collision.collider);

                    m_isGrounded = true;
                }
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            bool validSurface = false;

            foreach (ContactPoint contact in collision.contacts)
            {
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
                {
                    validSurface = true;
                    break;
                }
            }

            if (validSurface)
            {
                m_isGrounded = true;

                if (!m_collisions.Contains(collision.collider))
                    m_collisions.Add(collision.collider);
            }
            else
            {
                m_collisions.Remove(collision.collider);

                if (m_collisions.Count == 0)
                    m_isGrounded = false;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            m_collisions.Remove(collision.collider);

            if (m_collisions.Count == 0)
                m_isGrounded = false;
        }

        private void FixedUpdate()
        {
            m_animator.SetBool("Grounded", m_isGrounded);

            switch (m_controlMode)
            {
                case ControlMode.Direct:
                    DirectUpdate();
                    break;

                case ControlMode.Tank:
                    TankUpdate();
                    break;
            }

            m_wasGrounded = m_isGrounded;
            m_jumpInput = false;
        }

        private void TankUpdate()
        {
            Vector2 input = moveAction.action.ReadValue<Vector2>();

            float h = input.x;
            float v = input.y;

            bool walk = walkAction.action.IsPressed();

            if (v < 0)
            {
                if (walk)
                    v *= m_backwardsWalkScale;
                else
                    v *= m_backwardRunScale;
            }
            else if (walk)
            {
                v *= m_walkScale;
            }

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
            transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

            m_animator.SetFloat("MoveSpeed", m_currentV);

            JumpingAndLanding();
        }

        private void DirectUpdate()
        {
            Vector2 input = moveAction.action.ReadValue<Vector2>();

            float h = input.x;
            float v = input.y;

            Transform camera = Camera.main.transform;

            if (walkAction.action.IsPressed())
            {
                v *= m_walkScale;
                h *= m_walkScale;
            }

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

            float directionLength = direction.magnitude;
            direction.y = 0;
            direction = direction.normalized * directionLength;

            if (direction != Vector3.zero)
            {
                m_currentDirection = Vector3.Slerp(
                    m_currentDirection,
                    direction,
                    Time.deltaTime * m_interpolation);

                transform.rotation = Quaternion.LookRotation(m_currentDirection);
                transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

                m_animator.SetFloat("MoveSpeed", direction.magnitude);
            }
            else
            {
                m_animator.SetFloat("MoveSpeed", 0f);
            }

            JumpingAndLanding();
        }

        private void JumpingAndLanding()
        {
            bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

            if (jumpCooldownOver && m_isGrounded && m_jumpInput)
            {
                m_jumpTimeStamp = Time.time;

                m_rigidBody.AddForce(
                    Vector3.up * m_jumpForce,
                    ForceMode.Impulse);
            }
        }
    }
}