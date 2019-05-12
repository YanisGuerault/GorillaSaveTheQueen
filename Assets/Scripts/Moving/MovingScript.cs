using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingScript : MonoBehaviour
{
    [SerializeField]
    float m_TranslationSpeed;
    [SerializeField]
    float m_RotationSpeed;
    [SerializeField]
    float m_JumpVerticalVelocity;

    Rigidbody m_Rigidbody;

    bool m_isGrownded = false;
    [SerializeField]
    LayerMask m_GroundedLayerMask;

    float m_NextJumpTime;
    [SerializeField]
    float m_JumpCoolDownDuration;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_NextJumpTime = Time.time;
    }

    private void FixedUpdate()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        bool jump = Input.GetButton("Jump");

        m_Rigidbody.MoveRotation(Quaternion.AngleAxis(hInput * m_RotationSpeed * Time.fixedDeltaTime, Vector3.up) * m_Rigidbody.rotation);

        if (m_isGrownded)
        {
            if (jump && Time.time > m_NextJumpTime)
            {
                m_Rigidbody.velocity += Vector3.up * m_JumpVerticalVelocity;
                m_isGrownded = false;
                m_NextJumpTime = Time.time + m_JumpCoolDownDuration;
            }
            else
            {
                m_Rigidbody.velocity = vInput * transform.forward * m_TranslationSpeed;
            }
            m_Rigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((m_GroundedLayerMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || Vector3.Distance(collision.contacts[0].point, transform.position) < .05f)
                m_isGrownded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if ((m_GroundedLayerMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            m_isGrownded = false;
        }
    }
}
