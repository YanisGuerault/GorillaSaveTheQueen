﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using SDD.Events;

public class PlayerController : MonoBehaviour
{
    #region Variables
    private enum ControlMode
    {
        Tank,
        Direct,
        TwoDUp,
        TwoDLeft
    }

    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private float m_turnSpeed = 200;
    [SerializeField] private float m_jumpForce = 4;
    [SerializeField] private bool m_doubleJumpActive = false;

    public float Speed { get { return m_moveSpeed; } set { m_moveSpeed = value; }  }
    public float Jump { get { return m_jumpForce; } set { m_jumpForce = value; } }

    [SerializeField] private Animator m_animator;
    [SerializeField] private Rigidbody m_rigidBody;

    [SerializeField] private ControlMode m_controlMode = ControlMode.TwoDLeft;

    private CameraController m_Camera ;

    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 10;
    private readonly float m_walkScale = 0.33f;
    private readonly float m_backwardsWalkScale = 0.16f;
    private readonly float m_backwardRunScale = 0.66f;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;

    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.25f;
    private bool jumping = false;
    private bool double_jumping = false;

    private bool m_isGrounded;
    private Transform m_Ground;

    [SerializeField] private bool canMove = false;

    private List<Collider> m_collisions = new List<Collider>();
    #endregion

    #region Events Subscriptions
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<ActiveMovingEvent>(ActiveMoving);
        EventManager.Instance.AddListener<BonusToBePlacedEvent>(PlaceBonus);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<BonusToBePlacedEvent>(PlaceBonus);
        EventManager.Instance.RemoveListener<ActiveMovingEvent>(ActiveMoving);
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void Awake()
    {
        canMove = false;
        SubscribeEvents();
    }
    #endregion

    #region Bonus

    private void PlaceBonus(BonusToBePlacedEvent e)
    {
        m_animator.SetTrigger("Pickup");
        StartCoroutine(PlaceObjectCoroutine(TrapBonus.Prefab));
        SfxManager.Instance.PlaySfx2D(Constants.PLAYER_TRAP_SFX);
    }

    private IEnumerator PlaceObjectCoroutine(GameObject other)
    {
        yield return new WaitForSeconds(1.5f);
        Instantiate(other, this.transform.position + new Vector3(0,0.5f,0), Quaternion.Euler(0, 0, 0));
    }
    #endregion

    #region OnCollision & OnTrigger Methods

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            m_isGrounded = true;

            if (collision.gameObject.CompareTag("Ground"))
            {
                m_Ground = collision.transform;
                m_Camera.setGround(collision.transform);
            }
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }

        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerStay(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Water"))
        {
            EventManager.Instance.Raise(new PlayerHasBeenHitEvent());
        }
    }

    #endregion

    #region OnMoving Methods
    void Update()
    {
        m_animator.SetBool("Grounded", m_isGrounded);

        if (canMove)
        {
            switch (m_controlMode)
            {
                case ControlMode.Direct:
                    DirectUpdate();
                    break;

                case ControlMode.Tank:
                    TankUpdate();
                    break;

                case ControlMode.TwoDUp:
                    TwoDUpdate(Vector3.forward);
                    break;

                case ControlMode.TwoDLeft:
                    TwoDUpdate(Vector3.left);
                    break;

                default:
                    Debug.LogError("Unsupported state");
                    break;
            }
        }

        m_wasGrounded = m_isGrounded;
    }

    private void TankUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        bool walk = Input.GetKey(KeyCode.LeftShift);

        if (v < 0)
        {
            if (walk) { v *= m_backwardsWalkScale; }
            else { v *= m_backwardRunScale; }
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
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Transform camera = Camera.main.transform;

        if (Input.GetKey(KeyCode.LeftShift))
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
            m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

            transform.rotation = Quaternion.LookRotation(m_currentDirection);
            transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

            m_animator.SetFloat("MoveSpeed", direction.magnitude);
        }

        JumpingAndLanding();
    }

    private void TwoDUpdate(Vector3 vector)
    {
        float h = Input.GetAxis("Horizontal");

        Transform camera = Camera.main.transform;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            h *= m_walkScale;
        }

        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        Vector3 direction = vector * m_currentH;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if (direction != Vector3.zero)
        {

            m_currentDirection = direction;//Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

            transform.rotation = Quaternion.LookRotation(m_currentDirection);
            transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

            m_animator.SetFloat("MoveSpeed", direction.magnitude);
        }

        JumpingAndLanding();
    }

    #endregion

    #region Jumping Methods
    private void JumpingAndLanding()
    {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

        if (jumpCooldownOver && m_isGrounded && (Input.GetKey(KeyCode.Space) || Input.GetAxis("Jump") > 0))
        {
            m_jumpTimeStamp = Time.time;
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
            jumping = true;
            double_jumping = false;
            SfxManager.Instance.PlaySfx2D(Constants.PLAYER_JUMP_SFX);
        }

        if (m_doubleJumpActive && !m_isGrounded && (Input.GetKey(KeyCode.Space) || Input.GetAxis("Jump") > 0) && jumping == true && double_jumping == false && jumpCooldownOver)
        {
            m_jumpTimeStamp = Time.time;
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
            double_jumping = true;
        }

        if (!m_wasGrounded && m_isGrounded)
        {
            m_animator.SetTrigger("Land");
            jumping = false;
            double_jumping = false;
        }

        if (!m_isGrounded && m_wasGrounded)
        {
            m_animator.SetTrigger("Jump");
        }

        if (m_Ground != null && transform.position.y < m_Ground.position.y-20)
        {
            EventManager.Instance.Raise(new PlayerHasBeenHitEvent());
        }
    }
    #endregion

    #region Others Methods

    private IEnumerator CoroutinePickupObject(float x, GameObject other)
    {
        yield return new WaitForSeconds(x);
        EventManager.Instance.Raise(new PlayerGetABonus() { bonus = other.GetType() });
        Destroy(other);
    }

    private void PickupObject(GameObject gameobject)
    {
        m_animator.SetTrigger("Pickup");
        StartCoroutine(CoroutinePickupObject(1.5f, gameobject));
    }

    private void ActiveMoving(ActiveMovingEvent e)
    {
        canMove = e.Active;
    }

    private void Start()
    {
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }

    #endregion

}
