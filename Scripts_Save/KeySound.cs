using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class KeySound : MonoBehaviour
{
    public GameObject player; // The player object - assigned in inspector
    public float nearDistance = 5.0f; // Maximum distance for checking proximity
    public float minDistance = 1.0f; // Minimum distance to control the audio volume
    public AudioSource audioSource; // Audio source for playing the pickup sound
    public AudioClip pickupAudio; // The audio clip to play when near
    public Material defaultMaterial; // Default material for key
    public Material highlightedMaterial; // Highlighted material for key when near
    private bool isPickedUp = false; // Flag to check if key has been picked up
    private Renderer keyRenderer; // To change the material (outline)
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable; // Reference to XRGrabInteractable component
    private Coroutine audioCoroutine; // To track the audio playback coroutine

    void Start()
    {
        keyRenderer = GetComponent<Renderer>(); // Get the key's renderer
        keyRenderer.material = defaultMaterial; // Set the default material at the start

        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>(); // Get the XRGrabInteractable component
        grabInteractable.selectEntered.AddListener(OnKeyPickup); // Register the pick-up event
    }

    void Update()
    {
        if (!isPickedUp)
        {
            // Check if player is near the key
            bool checkNear = IsNearTarget(player);

            if (checkNear)
            {
                if (audioCoroutine == null)
                {
                    audioCoroutine = StartCoroutine(PlayAudioRandomly()); // Start random audio playback
                }
                HighlightKey(true); // Change the outline to yellow
            }
            else
            {
                if (audioCoroutine != null)
                {
                    StopCoroutine(audioCoroutine); // Stop audio playback
                    audioCoroutine = null;
                }
                HighlightKey(false); // Revert the outline to normal
            }
        }
    }

    bool IsNearTarget(GameObject target)
    {
        if (target == null) return false;

        // Check if the distance is within nearDistance
        float distance = Vector3.Distance(transform.position, target.transform.position);
        return distance <= nearDistance; // Returns true if player is close enough
    }

    IEnumerator PlayAudioRandomly()
    {
        // Continue playing audio at random intervals while the player is near
        while (!isPickedUp && IsNearTarget(player))
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            // Calculate volume based on distance: closer = louder
            float volume = Mathf.Lerp(0f, 1f, (nearDistance - distance) / nearDistance); // Volume between 0 and 1

            // Adjust audio source volume based on proximity
            audioSource.volume = volume;

            // Play the audio clip
            audioSource.clip = pickupAudio;
            audioSource.Play();

            // Random interval between 1 and 5 seconds
            float randomDelay = Random.Range(1f, 5f);
            yield return new WaitForSeconds(randomDelay);
        }
    }

    void HighlightKey(bool highlight)
    {
        // Change material based on proximity
        if (highlight)
        {
            keyRenderer.material = highlightedMaterial; // Change outline to yellow
        }
        else
        {
            keyRenderer.material = defaultMaterial; // Revert to default
        }
    }

    // This method will be called when the key is picked up
    private void OnKeyPickup(SelectEnterEventArgs args)
    {
        isPickedUp = true;
        if (audioCoroutine != null)
        {
            StopCoroutine(audioCoroutine); // Stop audio playback when picked up
            audioCoroutine = null;
        }
        audioSource.Stop(); // Stop the audio immediately
        keyRenderer.material = defaultMaterial; // Revert material after pickup
    }
}


