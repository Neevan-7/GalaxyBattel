
//using UnityEngine;

//public class PlayerHealth : MonoBehaviour
//{
//    [Header("Health")]
//    [SerializeField] private int maxHealth = 5;

//    private int currentHealth;
//    public int MaxHealth => maxHealth;

//    public int CurrentHealth => currentHealth;

//    private void Awake()
//    {
//        currentHealth = maxHealth;
//    }

//    public void TakeDamage(int damage)
//    {
//        if (currentHealth <= 0)
//            return;

//        currentHealth = Mathf.Max(0, currentHealth - damage);

//        Debug.Log(
//            "Player health: " + currentHealth + "/" + maxHealth
//        );

//        if (currentHealth <= 0)
//        {
//            Die();
//        }

//        if (currentHealth <= 0)
//        {
//            currentHealth = 0;

//            if (GameManager.Instance != null)
//            {
//                GameManager.Instance.EndGame();
//            }

//            return;
//        }

//    }

//    private void Die()
//    {
//        if (GameManager.Instance != null)
//        {
//            GameManager.Instance.EndGame();
//        }
//        if (BlastSpawner.Instance != null)
//            BlastSpawner.Instance.SpawnBlast(transform.position);
//        Debug.Log("Player destroyed! Game Over.");
//        gameObject.SetActive(false);
//    }
//}


using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 5;

    [Header("Protection Shield")]
    [SerializeField] private GameObject shieldPrefab;

    private GameObject activeShield;

    private int currentHealth;
    private bool isDead;
    private Coroutine shieldCoroutine;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public bool IsShieldActive { get; private set; }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead || damage <= 0 || IsShieldActive)
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        Debug.Log("Player health: " + currentHealth + "/" + maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (isDead || amount <= 0)
            return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);

        Debug.Log("Player healed: " + currentHealth + "/" + maxHealth);
    }

    //public void ActivateShield(float duration)
    //{
    //    if (isDead || duration <= 0)
    //        return;

    //    if (shieldCoroutine != null)
    //        StopCoroutine(shieldCoroutine);

    //    shieldCoroutine = StartCoroutine(ShieldRoutine(duration));
    //}

    public void ActivateShield(float duration)
    {
        if (shieldPrefab == null)
        {
            Debug.LogError("Assign Shield Prefab in PlayerHealth!");
            return;
        }

        if (shieldCoroutine != null)
            StopCoroutine(shieldCoroutine);

        if (activeShield == null)
        {
            activeShield = Instantiate(
                shieldPrefab,
                transform.position,
                Quaternion.identity,
                transform
            );

            activeShield.transform.localPosition = Vector3.zero;
            activeShield.transform.localRotation = Quaternion.identity;
        }

        shieldCoroutine = StartCoroutine(ShieldRoutine(duration));
    }

    private IEnumerator ShieldRoutine(float duration)
    {
        IsShieldActive = true;

        yield return new WaitForSeconds(duration);

        IsShieldActive = false;

        if (activeShield != null)
            Destroy(activeShield);

        activeShield = null;
        shieldCoroutine = null;
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        if (BlastSpawner.Instance != null)
            BlastSpawner.Instance.SpawnBlast(transform.position);

        if (GameManager.Instance != null)
            GameManager.Instance.EndGame();

        Debug.Log("Player destroyed! Game Over.");
        gameObject.SetActive(false);
    }
}
