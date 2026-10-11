using UnityEngine;

public class CoinMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float leftLimit = -8f;

    [Header("Random Y Position")]
    public float minY = -3.5f;
    public float maxY = 3.5f;

    [Header("Coin X Spacing")]
    public float minGapX = 3f;
    public float maxGapX = 5f;

    [Header("Camera Spacing")]
    public float cameraRightOffset = 2f;

    private void Start()
    {
        // Randomize starting Y position
        Vector3 position = transform.position;

        position.y = Random.Range(minY, maxY);

        transform.position = position;
    }

    private void Update()
    {
        // Move coin from right to left
        transform.Translate(
            Vector3.left * moveSpeed * Time.deltaTime
        );

        // When coin leaves the camera
        if (transform.position.x <= leftLimit)
        {
            RecycleCoin();
        }
    }

    // Recycle the coin after it leaves the screen
    // OR after the player collects it
    public void RecycleCoin()
    {
        MoveToRightOfOtherCoins();
    }

    private void MoveToRightOfOtherCoins()
    {
        CoinMovement[] allCoins =
            FindObjectsByType<CoinMovement>(
                FindObjectsSortMode.None
            );

        float rightmostX = GetCameraRightEdge();

        foreach (CoinMovement coin in allCoins)
        {
            if (coin != this)
            {
                if (coin.transform.position.x > rightmostX)
                {
                    rightmostX = coin.transform.position.x;
                }
            }
        }

        // Random gap between coins
        float randomGap = Random.Range(
            minGapX,
            maxGapX
        );

        Vector3 newPosition = transform.position;

        // New X position after the rightmost coin
        newPosition.x = rightmostX + randomGap;

        // New random Y position
        newPosition.y = Random.Range(
            minY,
            maxY
        );

        transform.position = newPosition;
    }

    private float GetCameraRightEdge()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            return 8f;
        }

        float cameraRightEdge =
            mainCamera.transform.position.x +
            mainCamera.orthographicSize *
            mainCamera.aspect;

        return cameraRightEdge + cameraRightOffset;
    }
}