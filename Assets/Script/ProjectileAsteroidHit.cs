
using UnityEngine;

public class ProjectileAsteroidHit : MonoBehaviour
{
    [SerializeField] private float asteroidDamage = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        AsteroidObstacle asteroid =
            other.GetComponent<AsteroidObstacle>();

        if (asteroid == null)
            asteroid = other.GetComponentInParent<AsteroidObstacle>();

        if (asteroid != null)
        {
            asteroid.TakeDamage(asteroidDamage);
            Destroy(gameObject);
        }
    }
}
