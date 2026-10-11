using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float resetX = 8f;
    public float leftLimit = -8f;

    [Header("Random Y Position")]
    public float minY = -3f;
    public float maxY = 3f;

    [Header("Random X Spacing")]
    public float minGapX = 4f;
    public float maxGapX = 7f;

    [Header("Camera Spacing")]
    public float cameraRightOffset = 2f;

    void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        if (transform.position.x <= leftLimit)
        {
            RecycleObstacle();
        }
    }

    private void RecycleObstacle()
    {
        ObstacleMovement[] allObstacles =
            FindObjectsByType<ObstacleMovement>(FindObjectsSortMode.None);

        float rightmostX = GetCameraRightEdge();

        foreach (ObstacleMovement obstacle in allObstacles)
        {
            if (obstacle != this)
            {
                if (obstacle.transform.position.x > rightmostX)
                {
                    rightmostX = obstacle.transform.position.x;
                }
            }
        }

        float randomGap = Random.Range(minGapX, maxGapX);

        Vector3 newPosition = transform.position;

        newPosition.x = rightmostX + randomGap;
        newPosition.y = Random.Range(minY, maxY);

        transform.position = newPosition;
    }

    private float GetCameraRightEdge()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            return resetX;
        }

        float cameraRightEdge =
            mainCamera.transform.position.x +
            mainCamera.orthographicSize * mainCamera.aspect;

        return cameraRightEdge + cameraRightOffset;
    }
}