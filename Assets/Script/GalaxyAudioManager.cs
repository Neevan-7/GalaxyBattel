
using UnityEngine;

public class GalaxyAudioManager : MonoBehaviour
{
    public static GalaxyAudioManager Instance { get; private set; }

    [Header("Gameplay SFX Source")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Player Sounds")]
    [SerializeField] private AudioClip playerShoot;
    [SerializeField] private AudioClip playerDamaged;

    [Header("Enemy Sounds")]
    [SerializeField] private AudioClip enemyShoot;
    [SerializeField] private AudioClip enemyExplosion;

    [Header("Skills and Pickups")]
    [SerializeField] private AudioClip crystalPickup;
    [SerializeField] private AudioClip shieldActivate;
    [SerializeField] private AudioClip multiBulletActivate;
    [SerializeField] private AudioClip missileLaunch;

    [Header("Other Sounds")]
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip buttonClick;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Duplicate GalaxyAudioManager found!");
            enabled = false;
            return;
        }

        Instance = this;

        if (sfxSource == null)
            sfxSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("An audio clip has not been assigned.");
            return;
        }

        if (sfxSource == null)
        {
            Debug.LogWarning("Assign an AudioSource for gameplay SFX.");
            return;
        }

        sfxSource.PlayOneShot(clip, Mathf.Clamp01(volume));
    }

    public void PlayerShoot()
    {
        PlaySound(playerShoot, 0.7f);
    }

    public void PlayerDamaged()
    {
        PlaySound(playerDamaged);
    }

    public void EnemyShoot()
    {
        PlaySound(enemyShoot, 0.6f);
    }

    public void EnemyExplosion()
    {
        PlaySound(enemyExplosion);
    }

    public void CrystalPickup()
    {
        PlaySound(crystalPickup);
    }

    public void ShieldActivate()
    {
        PlaySound(shieldActivate);
    }

    public void MultiBulletActivate()
    {
        PlaySound(multiBulletActivate);
    }

    public void MissileLaunch()
    {
        PlaySound(missileLaunch);
    }

    public void GameOver()
    {
        PlaySound(gameOver);
    }

    public void ButtonClick()
    {
        PlaySound(buttonClick, 0.5f);
    }

    // Useful for additional sound clips.
    public void PlayCustomSound(AudioClip clip, float volume = 1f)
    {
        PlaySound(clip, volume);
    }
}
