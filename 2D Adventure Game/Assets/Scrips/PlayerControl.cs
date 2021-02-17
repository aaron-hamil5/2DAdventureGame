using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControl : MonoBehaviour
{
    #region Variables
    public enum FaceDirection {FaceLeft = -1, FaceRight = 1};
    public FaceDirection Facing = FaceDirection.FaceRight;
    public LayerMask GroundLayer;
    private Rigidbody2D body;
    private Transform transformX;
    public CircleCollider2D circleCollider2D;
    public bool isGrounded = false;
    public string horrizontal = "Horizontal";
    public string jump = "Jump";
    public float maxSpeed = 50f;
    public float jumpPower = 600;
    public float jumpTimeOut = 1f;
    private bool canJump = true;
    private bool canControl = true;
    public static PlayerControl playerControl;
    private static float _Health = 100f;
    #endregion

    #region Health
    public static float Health
    {
        get
        {
            return _Health;
        }
        set
        {
            _Health = value;
            if (_Health <= 0)
            {
                Die();
            }
        }
    }
    #endregion

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        transformX = GetComponent<Transform>();
        playerControl = this;
    }

    private bool GetGrounded() 
    { 
        //Check ground
        Vector2 CircleCenter = new Vector2(transformX.position.x, transformX.position.y) + circleCollider2D.offset; Collider2D[] HitColliders = Physics2D.OverlapCircleAll(CircleCenter, circleCollider2D.radius, GroundLayer);
        if (HitColliders.Length > 0) return true;
        return false;
    }

    private void FlipDirection()
    {
        Facing = (FaceDirection)((int)Facing * -1f);
        Vector3 LocalScale = transformX.localScale;
        LocalScale.x = -1f;
        transformX.localScale = LocalScale;
    }

    private void Jump()
    {
        //If we are grounded, then jump
        if (!isGrounded || !canJump) return;
        body.AddForce(Vector2.up * jumpPower);
        canJump = false;
        Invoke("ActivateJump", jumpTimeOut);
    }

    private void ActivateJump()
    {
        canJump = true;
    }

    private void FixedUpdate()
    {
        if(!canControl || Health <= 0)
        {
            return;
        }

        isGrounded = GetGrounded();
        float Horiz = CrossPlatformInputManager.GetAxis(horrizontal);

        if (CrossPlatformInputManager.GetButton(jump))
        {
            Jump();
        }

        body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(body.velocity.y, - Mathf.Infinity, jumpPower));

        if ((Horiz <0f && Facing !=FaceDirection.FaceLeft)|| (Horiz >0f && Facing != FaceDirection.FaceRight))
        {
            FlipDirection();
        }
    }

    private void OnDestroy()
    {
        playerControl = null;
    }

    static void Die()
    {
        Destroy(PlayerControl.playerControl.gameObject);
    }

    public static void Reset()
    {
        Health = 100f;
    }
}
