using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 14f;
    private float wallJumpingPower = 10f; // Adjust as needed
    private float fallMultiplier = 2.5f;
    private float lowJumpMultiplier = 2f;
    private float glideMultiplier = -2f; // Adjust as needed
    private int tokenCount = 0; // Total collected tokens
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform tr;
    [SerializeField] private LayerMask gl;
    [SerializeField] private float wallCheckDistance = 0.2f; // Distance to check for walls
    [SerializeField] private LayerMask wallLayer; // Layer for walls
    [SerializeField] private TextMeshProUGUI tokenText; // Reference to the TextMeshPro UI text for displaying the token count

    private const float groundCheckRadius = 0.2f;
    private const float groundCheckOffset = 0.05f;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallJumping;
    private bool canGlide;
    private bool isGliding;
    private bool canJump = true;
    private float coyoteTime = 0.1f;
    private float coyoteTimer;
    private float bonusAirTime = 1f;
    private float bonusAirTimer;
    private int jumpCount = 0;
    private int maxJumps = 2;
    private float jumpCooldown = 0.2f;
    private float jumpCooldownTimer = 0f;
    private bool jumpButtonDown = false;
    private bool glideButtonDown = false;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        isGrounded = IsGrounded();
        isTouchingWall = IsTouchingWall();

        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
            jumpCount = 0;
            isWallJumping = false;
            isGliding = false; // Stop gliding when grounded
            canGlide = false; // Disable glide when grounded
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
            canGlide = true; // Enable glide when not grounded
        }

        if (!isGrounded)
        {
            bonusAirTimer -= Time.deltaTime;
        }

        if (jumpCooldownTimer > 0f)
        {
            jumpCooldownTimer -= Time.deltaTime;
            if (jumpCooldownTimer <= 0f)
            {
                canJump = true;
            }
        }

        Flip();

        if (Input.GetButtonDown("Jump") && (isGrounded || coyoteTimer > 0 || bonusAirTimer > 0) && jumpCount < maxJumps && canJump)
        {
            Jump();
            jumpCount++;
            canJump = false;
            jumpCooldownTimer = jumpCooldown;
        }

        if (Input.GetButtonDown("Jump") && isTouchingWall && !isGrounded)
        {
            WallJump();
        }

        if (Input.GetKeyDown(KeyCode.G) && !isGrounded && canGlide)
        {
            isGliding = true;
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            isGliding = false;
        }

        if (isGliding)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * glideMultiplier * Time.deltaTime;
        }
        else if (rb.velocity.y < 0 && !isGrounded)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        UpdateTokenText();
    }

    private void FixedUpdate()
    {
        Move(horizontal);
    }

    private void Move(float move)
    {
        Vector2 targetVelocity = new Vector2(move * speed, rb.velocity.y);
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, Time.fixedDeltaTime * 10f);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        bonusAirTimer = bonusAirTime;
    }

    private void WallJump()
    {
        isWallJumping = true;
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        rb.AddForce(Vector2.right * (isFacingRight ? -1 : 1) * wallJumpingPower, ForceMode2D.Impulse);
        canJump = false;
    }

    private void Flip()
    {
        if ((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(tr.position - Vector3.up * groundCheckOffset, groundCheckRadius, Vector2.down, 0f, gl);
        return hit.collider != null;
    }

    public int GetTokenCount()
    {
        return tokenCount;
    }

    private bool IsTouchingWall()
    {
        RaycastHit2D hitRight = Physics2D.Raycast(tr.position, Vector2.right * (isFacingRight ? 1 : -1), wallCheckDistance, wallLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(tr.position, Vector2.left * (isFacingRight ? 1 : -1), wallCheckDistance, wallLayer);
        return hitRight.collider != null || hitLeft.collider != null;
    }

    private void UpdateTokenText()
    {
        if (tokenText != null)
        {
            tokenText.text = "Tokens: " + tokenCount.ToString(); // Update the UI text to display the token count
        }
    }

    public void CollectToken(int value)
    {
        tokenCount += value;
    }
}
