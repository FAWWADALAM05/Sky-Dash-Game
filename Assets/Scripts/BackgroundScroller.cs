using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    public float repeatWidth = 17.3f;

    void Update()
    {
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

        if (transform.position.x <= -repeatWidth)
        {
            transform.position += new Vector3(repeatWidth * 2f, 0f, 0f);
        }
    }
}