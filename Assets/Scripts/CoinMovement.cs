using UnityEngine;

public class CoinMovement : MonoBehaviour
{
    [Header("Coin Movement")]
    public float moveSpeed = 3f;

    [Header("Coin X Position")]
    public float xSpacing = 10f;

    [Header("Random Y Position")]
    public float minY = -3.2f;
    public float maxY = 3.2f;

    [Header("Starting X")]
    public float startingX = 1f;

    [Header("Screen")]
    public float leftLimit = -8f;

    [Header("Obstacle Safety")]
    public float obstacleSafeDistance = 1.5f;

    [Header("Coin Safety")]
    public float coinSafeDistance = 2f;

    [Header("Search")]
    public int maxAttempts = 500;

    private void Start()
    {
        SetStartingPosition();
    }

    private void Update()
    {
        // Move coin left
        transform.Translate(
            Vector3.left * moveSpeed * Time.deltaTime
        );

        // Continuously check obstacle safety
        if (IsTooCloseToObstacle())
        {
            MoveToSafeY();
        }

        // Continuously check other coins
        if (IsTooCloseToCoin())
        {
            MoveToSafeY();
        }

        // Recycle
        if (transform.position.x <= leftLimit)
        {
            RecycleCoin();
        }
    }

    private void SetStartingPosition()
    {
        CoinMovement[] coins =
            FindObjectsByType<CoinMovement>(
                FindObjectsSortMode.None
            );

        System.Array.Sort(
            coins,
            (a, b) =>
                a.transform.position.x.CompareTo(
                    b.transform.position.x
                )
        );

        int index =
            System.Array.IndexOf(
                coins,
                this
            );

        float newX =
            startingX + (index * xSpacing);

        float newY =
            FindSafeY(newX);

        transform.position = new Vector3(
            newX,
            newY,
            transform.position.z
        );
    }

    public void RecycleCoin()
    {
        CoinMovement[] coins =
            FindObjectsByType<CoinMovement>(
                FindObjectsSortMode.None
            );

        float rightmostX = float.MinValue;

        foreach (CoinMovement coin in coins)
        {
            if (coin == this)
            {
                continue;
            }

            if (coin.transform.position.x > rightmostX)
            {
                rightmostX =
                    coin.transform.position.x;
            }
        }

        float newX;

        if (rightmostX == float.MinValue)
        {
            newX = startingX;
        }
        else
        {
            newX =
                rightmostX + xSpacing;
        }

        float newY =
            FindSafeY(newX);

        transform.position = new Vector3(
            newX,
            newY,
            transform.position.z
        );
    }

    // --------------------------------------------------
    // FIND SAFE Y
    // --------------------------------------------------

    private float FindSafeY(float x)
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            float randomY =
                Random.Range(
                    minY,
                    maxY
                );

            Vector2 position =
                new Vector2(
                    x,
                    randomY
                );

            if (IsPositionSafe(position))
            {
                return randomY;
            }
        }

        // Systematic search
        for (int i = 0; i <= 100; i++)
        {
            float testY =
                Mathf.Lerp(
                    minY,
                    maxY,
                    i / 100f
                );

            Vector2 position =
                new Vector2(
                    x,
                    testY
                );

            if (IsPositionSafe(position))
            {
                return testY;
            }
        }

        // Last resort
        return 0f;
    }

    // --------------------------------------------------
    // POSITION SAFETY
    // --------------------------------------------------

    private bool IsPositionSafe(
        Vector2 position
    )
    {
        if (IsObstacleAtPosition(position))
        {
            return false;
        }

        if (IsCoinAtPosition(position))
        {
            return false;
        }

        return true;
    }

    // --------------------------------------------------
    // REAL OBSTACLE COLLIDER CHECK
    // --------------------------------------------------

    private bool IsObstacleAtPosition(
        Vector2 position
    )
    {
        ObstacleMovement[] obstacles =
            FindObjectsByType<ObstacleMovement>(
                FindObjectsSortMode.None
            );

        foreach (
            ObstacleMovement obstacle
            in obstacles
        )
        {
            Collider2D[] colliders =
                obstacle.GetComponentsInChildren<
                    Collider2D
                >();

            foreach (
                Collider2D obstacleCollider
                in colliders
            )
            {
                if (obstacleCollider == null ||
                    !obstacleCollider.enabled)
                {
                    continue;
                }

                Vector2 closestPoint =
                    obstacleCollider.ClosestPoint(
                        position
                    );

                float distance =
                    Vector2.Distance(
                        position,
                        closestPoint
                    );

                if (distance <
                    obstacleSafeDistance)
                {
                    return true;
                }
            }
        }

        return false;
    }

    // --------------------------------------------------
    // COIN CHECK
    // --------------------------------------------------

    private bool IsCoinAtPosition(
        Vector2 position
    )
    {
        CoinMovement[] coins =
            FindObjectsByType<CoinMovement>(
                FindObjectsSortMode.None
            );

        foreach (
            CoinMovement coin
            in coins
        )
        {
            if (coin == this)
            {
                continue;
            }

            float distance =
                Vector2.Distance(
                    position,
                    coin.transform.position
                );

            if (distance <
                coinSafeDistance)
            {
                return true;
            }
        }

        return false;
    }

    // --------------------------------------------------
    // CONTINUOUS OBSTACLE CHECK
    // --------------------------------------------------

    private bool IsTooCloseToObstacle()
    {
        return IsObstacleAtPosition(
            transform.position
        );
    }

    private bool IsTooCloseToCoin()
    {
        return IsCoinAtPosition(
            transform.position
        );
    }

    // --------------------------------------------------
    // MOVE COIN TO SAFE Y
    // --------------------------------------------------

    private void MoveToSafeY()
    {
        float safeY =
            FindSafeY(
                transform.position.x
            );

        transform.position =
            new Vector3(
                transform.position.x,
                safeY,
                transform.position.z
            );
    }
}