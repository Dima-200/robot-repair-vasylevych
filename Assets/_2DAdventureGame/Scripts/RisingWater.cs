using UnityEngine;

public class RisingWater : MonoBehaviour
{
    public float riseSpeed = 0.2f;
    public float maxHeight = 6.0f;
    public int damage = 1;

    float minHeight;
    int direction = 1;

    void Start()
    {
        minHeight = transform.position.y;
    }

    void Update()
    {
        Vector3 position = transform.position;
        position.y += direction * riseSpeed * Time.deltaTime;

        if (position.y >= maxHeight)
        {
            position.y = maxHeight;
            direction = -1;
        }
        else if (position.y <= minHeight)
        {
            position.y = minHeight;
            direction = 1;
        }

        transform.position = position;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        PlayerController controller =
            other.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.ChangeHealth(-damage);
        }
    }
}
