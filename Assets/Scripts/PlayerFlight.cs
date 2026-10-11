using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFlight : MonoBehaviour
{
    public float flapForce = 5f;

    [Header("Flight Boundary")]
    public float topLimit = 4.2f;
    public float bottomLimit = -4.2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
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

        // Keep player inside screen boundary
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
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            flapForce
        );

        // Play Fly/Tap sound only if Sound is ON
        if (MainMenuSettings.IsSoundEnabled())
        {
            AudioSource[] audioSources = GetComponents<AudioSource>();

            if (audioSources.Length > 1 &&
                audioSources[1].clip != null)
            {
                audioSources[1].Play();
            }
        }
    }
}