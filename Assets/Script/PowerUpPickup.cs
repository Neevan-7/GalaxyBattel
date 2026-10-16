
using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    public enum PowerUpType
    {
        Health,
        MultiBullet,
        Shield,
        BigMissile
    }

    [Header("Pickup Settings")]
    [SerializeField] private PowerUpType powerUpType;
    [SerializeField] private int healAmount = 2;
    [SerializeField] private float effectDuration = 8f;

    private bool collected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
            return;

        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();

        if (health == null)
            return;

        WeaponController weapon =
            other.GetComponentInParent<WeaponController>();

        switch (powerUpType)
        {
            case PowerUpType.Health:
                if (health.CurrentHealth >= health.MaxHealth)
                {
                    Debug.Log("Health is already full.");
                    return;
                }

                health.Heal(healAmount);
                break;

            case PowerUpType.MultiBullet:
                if (weapon == null)
                    return;

                weapon.ActivateMultiBullet();
                break;

            case PowerUpType.Shield:
                health.ActivateShield(effectDuration);
                break;

            case PowerUpType.BigMissile:
                if (weapon == null)
                    return;

                weapon.ActivateBigMissile();
                break;
        }

        collected = true;
        Destroy(gameObject);
    }
}
