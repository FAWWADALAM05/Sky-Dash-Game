using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFlight : MonoBehaviour
{
    public float flapForce = 5f;

    [Header("Flight Boundary")]
    public float topLimit = 4.2f;
    public float bottomLimit = -4.2f;

    private Rigidbody2D rb;

    // Stores the original gravity so it can be restored after Continue.
    private float originalGravityScale;

    // Prevents flight input after Game Over.
    private bool flightStopped = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Remember the original gravity setting.
        originalGravityScale = rb.gravityScale;
    }

    private void Update()
    {
        // Do not accept flight input after Game Over.
        if (!flightStopped)
        {
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
        }

        // Keep player inside screen boundary.
        Vector3 position = transform.position;

        position.y = Mathf.Clamp(
            position.y,
            bottomLimit,
            topLimit
        );

        transform.position = position;
    }

    private void FlyUp()
    {
        // Extra safety: do not fly while stopped.
        if (flightStopped)
        {
            return;
        }

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            flapForce
        );

        // Play Fly/Tap sound only if Sound is ON.
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

    // Called when Game Over happens.
    public void StopFlight()
    {
        flightStopped = true;

        // Stop all current movement.
        rb.linearVelocity = Vector2.zero;

        // Stop gravity while Game Over is displayed.
        rb.gravityScale = 0f;

        Debug.Log("PLAYER FLIGHT STOPPED.");
    }

    // Called when the player successfully continues after the rewarded ad.
    public void ResumeFlight()
    {
        flightStopped = false;

        // Restore the original gravity.
        rb.gravityScale = originalGravityScale;

        Debug.Log("PLAYER FLIGHT RESUMED.");
    }
}