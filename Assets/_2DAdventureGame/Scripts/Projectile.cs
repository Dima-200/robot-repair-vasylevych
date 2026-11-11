
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        if (rigidbody2d == null)
            return;

        rigidbody2d.linearVelocity = Vector2.zero;
        rigidbody2d.AddForce(direction * force);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController enemy =
            other.GetComponent<EnemyController>();

        if (enemy != null)
        {
            enemy.Fix();
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyController enemy =
            collision.gameObject.GetComponent<EnemyController>();

        if (enemy != null)
        {
            enemy.Fix();
            Destroy(gameObject);
        }
    }
}
