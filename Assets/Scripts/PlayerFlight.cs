using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFlight : MonoBehaviour
{
    [Header("Flight Settings")]
    public float flapForce = 5f;

    [Header("Flight Boundary")]
    public float topLimit = 4.2f;
    public float bottomLimit = -4.2f;

    private Rigidbody2D rb;
    private Animator animator;

    private float originalGravityScale;

    private bool flightStopped = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (rb != null)
        {
            originalGravityScale = rb.gravityScale;
        }
    }

    private void Start()
    {
        flightStopped = false;

        // Start normal flying animation
        if (animator != null)
        {
            animator.SetBool("Fly", true);
        }
    }

    private void Update()
    {
        // Do not allow flying after Game Over
        if (flightStopped)
        {
            return;
        }

        // Keyboard — Space
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            FlyUp();
        }

        // Mouse — Left Click
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            FlyUp();
        }

        // Mobile Touch
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            FlyUp();
        }

        // Keep player inside the vertical boundary
        KeepPlayerInsideBoundary();
    }

    private void FlyUp()
    {
        if (flightStopped)
        {
            return;
        }

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                flapForce
            );
        }

        // Play TapFlying animation
        if (animator != null)
        {
            animator.ResetTrigger("Tap");
            animator.SetTrigger("Tap");
        }

        // Play Fly/Tap sound only if Sound is ON
        if (MainMenuSettings.IsSoundEnabled())
        {
            AudioSource[] audioSources =
                GetComponents<AudioSource>();

            if (audioSources.Length > 1 &&
                audioSources[1].clip != null)
            {
                audioSources[1].Play();
            }
        }
    }

    private void KeepPlayerInsideBoundary()
    {
        Vector3 position = transform.position;

        position.y = Mathf.Clamp(
            position.y,
            bottomLimit,
            topLimit
        );

        transform.position = position;
    }

    // =========================================================
    // GAME OVER — STOP PLAYER
    // =========================================================

    public void StopFlight()
    {
        flightStopped = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            rb.gravityScale = 0f;
        }

        Debug.Log("PLAYER FLIGHT STOPPED.");
    }

    // =========================================================
    // CONTINUE AFTER REWARDED AD
    // =========================================================

    public void ResumeFlight()
    {
        flightStopped = false;

        if (rb != null)
        {
            rb.gravityScale = originalGravityScale;
        }

        Debug.Log("PLAYER FLIGHT RESUMED.");
    }
}