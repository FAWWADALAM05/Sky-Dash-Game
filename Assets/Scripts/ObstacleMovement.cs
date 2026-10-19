using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [Header("Obstacle Movement")]
    public float moveSpeed = 3f;

    [Header("Obstacle Position")]
    public float leftLimit = -8f;

    [Header("Y Position")]
    public float minY = -3.2f;
    public float maxY = 3.2f;

    [Header("Minimum Y Change")]
    public float minimumYChange = 3f;

    private void Update()
    {
        transform.Translate(
            Vector3.left * moveSpeed * Time.deltaTime
        );

        if (transform.position.x <= leftLimit)
        {
            RepeatObstacle();
        }
    }

    private void RepeatObstacle()
    {
        ObstacleMovement[] obstacles =
            FindObjectsByType<ObstacleMovement>(
                FindObjectsSortMode.None
            );

        float rightmostX = float.MinValue;
        float rightmostY = 0f;

        foreach (ObstacleMovement obstacle in obstacles)
        {
            if (obstacle == this)
            {
                continue;
            }

            if (obstacle.transform.position.x > rightmostX)
            {
                rightmostX = obstacle.transform.position.x;
                rightmostY = obstacle.transform.position.y;
            }
        }

        // Keep X spacing exactly 4 units
        float newX = rightmostX + 4f;

        // Random Y between -3.2 and +3.2
        float newY = Random.Range(minY, maxY);

        int safetyCount = 0;

        // Make sure the new Y is at least 3 units
        // away from the previous obstacle
        while (
            Mathf.Abs(newY - rightmostY) < minimumYChange &&
            safetyCount < 50
        )
        {
            newY = Random.Range(minY, maxY);
            safetyCount++;
        }

        transform.position = new Vector3(
            newX,
            newY,
            transform.position.z
        );
    }
}