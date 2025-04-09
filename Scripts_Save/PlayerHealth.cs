using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthSlider; // UI health slider
    public Image fillImage; // Reference to the Fill image of the slider

    public GameObject jumpscareCam;
    public GameObject blackscreen;
    public AudioSource staticSound;
    public AudioSource jumpscareSound;
    public bool enableCursorAfterDeath;
    public string sceneName;  

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensures health doesn't go below 0
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void Die()
    {
        StartCoroutine(KillPlayer());
    }

    private IEnumerator KillPlayer()
    {
        yield return new WaitForSeconds(3.5f);
     if (blackscreen != null)
    {
        blackscreen.SetActive(true); // Show black screen
    }
        AudioListener.pause = true;
        yield return new WaitForSeconds(6f);

        if (enableCursorAfterDeath)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        SceneManager.LoadScene(sceneName);
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;

            // Change the health bar color based on health
            if (fillImage != null)
            {
                fillImage.color = Color.Lerp(Color.red, Color.yellow, healthSlider.value);
            }
        }
    }
}
