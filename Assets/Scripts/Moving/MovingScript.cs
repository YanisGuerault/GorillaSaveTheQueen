using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class MovingScript : SimpleGameStateObserver, IEventHandler
{

    #region Physics gravity
    [SerializeField]
    Vector3 m_LowGravity;
    [SerializeField]
    Vector3 m_HighGravity;
    //Vector3 m_Gravity;
    #endregion


    private Rigidbody m_Rigidbody;
    private Transform m_Transform;

    [SerializeField]
    private float m_TranslationSpeed;
    [SerializeField]
    private float m_JumpImpulsionMagnitude;

    private bool m_IsGrounded;


    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Transform = GetComponent<Transform>();
    }

    private void Reset()
    {
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
    }

    // Use this for initialization
    void Start()
    {
        m_IsGrounded = false;
        //m_Gravity = m_HighGravity;
    }

    private void Update()
    {
        //bool fire = Input.GetAxis("Fire1") > 0;
        ////Debug.Log("Fire = " + fire);
        //m_Gravity = fire ? m_LowGravity : m_HighGravity;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        bool jump = Input.GetAxis("Jump") > 0 || Input.GetKeyDown(KeyCode.Space);
        bool fire = Input.GetAxis("Fire1") > 0;

        //m_Rigidbody.rotation = Quaternion.AngleAxis(90 * Mathf.Sign(hInput), Vector3.up);

        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Transform.forward * m_TranslationSpeed * hInput * Time.fixedDeltaTime);

        if (jump && m_IsGrounded)
        {
            Vector3 jumpForce = Vector3.up * m_JumpImpulsionMagnitude;
            m_Rigidbody.AddForce(jumpForce, ForceMode.Impulse);
        }

        if (m_IsGrounded)
        {
            m_Rigidbody.velocity = Vector3.zero;
        }

        m_Rigidbody.angularVelocity = Vector3.zero;

        Vector3 gravity = m_HighGravity;
        if (fire && m_Rigidbody.velocity.y < 0) gravity = m_LowGravity;

        m_Rigidbody.AddForce(gravity * m_Rigidbody.mass);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")
            || collision.gameObject.CompareTag("Platform"))
        {
            Vector3 colLocalPt = m_Transform.InverseTransformPoint(collision.contacts[0].point);

            //Debug.LogError(Time.frameCount+" colLocalPt = " + colLocalPt+ "   colLocalPt.magnitude = "+ colLocalPt.magnitude);

            if (colLocalPt.magnitude < .5f)
                m_IsGrounded = true;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")
            || collision.gameObject.CompareTag("Platform"))
        {
            m_IsGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
    }

    //Game state events
    protected override void GameMenu(GameMenuEvent e)
    {
        Reset();
    }
}
