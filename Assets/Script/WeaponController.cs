
using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Normal Weapon")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.25f;

    [Header("Mobile Controls")]
    [SerializeField] private MobileFireButton mobileFireButton;
    [SerializeField] private VirtualJoystick aimJoystick;

    [Header("Multi-Bullet Upgrade")]
    [SerializeField] private GameObject multiBulletPrefab;
    [SerializeField] private int bulletCount = 3;
    [SerializeField] private float bulletSpreadAngle = 15f;
    [SerializeField] private float multiBulletDuration = 8f;

    [Header("Big Missile Upgrade")]
    [SerializeField] private GameObject bigMissilePrefab;
    [SerializeField] private float bigMissileDuration = 8f;

    private Camera mainCamera;
    private float nextFireTime;
    private bool multiBulletActive;
    private bool bigMissileActive;

    private Coroutine multiBulletCoroutine;
    private Coroutine bigMissileCoroutine;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.IsGameOver)
        {
            return;
        }

        if (firePoint == null)
            return;

        HandleAiming();
        HandleShooting();
    }

    private void HandleAiming()
    {
        if (Application.isMobilePlatform)
        {
            if (aimJoystick == null)
                return;

            Vector2 aimDirection = aimJoystick.Direction;

            if (aimDirection.sqrMagnitude > 0.01f)
            {
                float angle =
                    Mathf.Atan2(aimDirection.y, aimDirection.x)
                    * Mathf.Rad2Deg - 90f;

                firePoint.rotation =
                    Quaternion.Euler(0f, 0f, angle);
            }

            return;
        }

        if (mainCamera == null)
            return;

        Vector3 mousePosition =
            mainCamera.ScreenToWorldPoint(Input.mousePosition);

        mousePosition.z = 0f;

        Vector2 direction =
            (Vector2)(mousePosition - firePoint.position);

        if (direction.sqrMagnitude > 0.001f)
        {
            float angle =
                Mathf.Atan2(direction.y, direction.x)
                * Mathf.Rad2Deg - 90f;

            firePoint.rotation =
                Quaternion.Euler(0f, 0f, angle);
        }
    }

    private void HandleShooting()
    {
        bool wantsToShoot;

        if (Application.isMobilePlatform)
        {
            wantsToShoot =
                mobileFireButton != null &&
                mobileFireButton.IsHeld;
        }
        else
        {
            wantsToShoot =
                Input.GetMouseButton(0) ||
                Input.GetKey(KeyCode.Space);
        }

        if (wantsToShoot && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        if (firePoint == null)
            return;

        // Big missile takes priority.
        if (bigMissileActive && bigMissilePrefab != null)
        {
            Instantiate(
                bigMissilePrefab,
                firePoint.position,
                firePoint.rotation
            );

            GalaxyAudioManager.Instance?.MissileLaunch();
            return;
        }

        if (multiBulletActive)
        {
            ShootMultipleBullets();
            return;
        }

        if (projectilePrefab != null)
        {
            Instantiate(
                projectilePrefab,
                firePoint.position,
                firePoint.rotation
            );

            GalaxyAudioManager.Instance?.PlayerShoot();
        }
    }

    private void ShootMultipleBullets()
    {
        GameObject bulletPrefab =
            multiBulletPrefab != null
                ? multiBulletPrefab
                : projectilePrefab;

        if (bulletPrefab == null)
            return;

        int count = Mathf.Max(2, bulletCount);
        float totalSpread = bulletSpreadAngle * (count - 1);
        float startAngle = -totalSpread / 2f;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + bulletSpreadAngle * i;

            Quaternion rotation =
                firePoint.rotation *
                Quaternion.Euler(0f, 0f, angle);

            Instantiate(
                bulletPrefab,
                firePoint.position,
                rotation
            );
        }

        GalaxyAudioManager.Instance?.PlayerShoot();
    }

    public void ActivateMultiBullet()
    {
        if (multiBulletCoroutine != null)
            StopCoroutine(multiBulletCoroutine);

        multiBulletCoroutine =
            StartCoroutine(MultiBulletRoutine());
    }

    private IEnumerator MultiBulletRoutine()
    {
        multiBulletActive = true;

        GalaxyAudioManager.Instance?.MultiBulletActivate();

        Debug.Log("Multi-bullet activated!");

        yield return new WaitForSeconds(multiBulletDuration);

        multiBulletActive = false;
        multiBulletCoroutine = null;

        Debug.Log("Multi-bullet ended.");
    }

    public void ActivateBigMissile()
    {
        if (bigMissilePrefab == null)
        {
            Debug.LogError(
                "Assign Big Missile Prefab in WeaponController!"
            );
            return;
        }

        if (bigMissileCoroutine != null)
            StopCoroutine(bigMissileCoroutine);

        bigMissileCoroutine =
            StartCoroutine(BigMissileRoutine());
    }

    private IEnumerator BigMissileRoutine()
    {
        bigMissileActive = true;

        Debug.Log("Big missile activated!");

        yield return new WaitForSeconds(bigMissileDuration);

        bigMissileActive = false;
        bigMissileCoroutine = null;

        Debug.Log("Big missile ended.");
    }
}
