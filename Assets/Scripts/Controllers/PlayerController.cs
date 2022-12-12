using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Your solution must make use of the following fields. If these values are changed, even at runtime,
    /// the character controller should respect the new values and act as detailed in the Unity inspector.
    /// </summary>

    [SerializeField]
    private float m_jumpApexHeight;

    [SerializeField]
    private float m_jumpApexTime;

    [SerializeField]
    private float m_terminalVelocity;

    [SerializeField]
    private float m_coyoteTime;

    [SerializeField]
    private float m_jumpBufferTime;

    [SerializeField]
    private float m_accelerationTimeFromRest;

    [SerializeField]
    private float m_decelerationTimeToRest;

    [SerializeField]
    private float m_maxHorizontalSpeed;

    [SerializeField]
    private float m_accelerationTimeFromQuickturn;

    [SerializeField]
    private float m_decelerationTimeFromQuickturn;

    public enum FacingDirection { Left, Right }

    private Rigidbody2D rb;


    private float jumpBufferCounter;
    private float apexTimeCounter;

    [Header("Grounded check variables")]
    public Transform groundCheck;
    public LayerMask groundLayers;

    public bool IsWalking()
    {
        if(ShovelKnightInput.GetDirectionalInput().x != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayers);
    }

    public FacingDirection GetFacingDirection()
    {
        if(ShovelKnightInput.GetDirectionalInput().x > 0)
        {
            return FacingDirection.Right;
        }
        else 
        {
            return FacingDirection.Left;
        }
    }

    // Add additional methods such as Start, Update, FixedUpdate, or whatever else you think is necessary, below.

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(IsWalking())
        {
            transform.position += new Vector3(ShovelKnightInput.GetDirectionalInput().x, 0) * Time.deltaTime * m_maxHorizontalSpeed;
        }
        if(jumpBufferCounter > 0f && IsGrounded())//apex jump time with jump buffer added
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity = new Vector2(rb.velocity.x, m_jumpApexHeight);

            jumpBufferCounter = 0f;
        }

        if(ShovelKnightInput.IsJumpPressed() && rb.velocity.y > 0f)//coyote jump time
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - m_coyoteTime);
        }

        if(!IsGrounded())
        {
            apexTimeCounter -= Time.deltaTime;

            if(apexTimeCounter <= 0)
            {
                rb.AddForce(Vector2.down * m_terminalVelocity, ForceMode2D.Impulse);
            }
        }

        if(ShovelKnightInput.WasJumpPressed())//when space is pressed add to jump buffer and apex time
        {
            jumpBufferCounter = m_jumpBufferTime;
            apexTimeCounter = m_jumpApexTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }
}
