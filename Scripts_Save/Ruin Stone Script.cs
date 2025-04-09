using UnityEngine;

public class RuinStoneScript : MonoBehaviour
{
    public string interactionKey = "e";
    private bool isInteracted = false;
    private Renderer stoneRenderer;
    private Material stoneMaterial; // Store the material directly
    

    void Start()
    {
        stoneRenderer = GetComponent<Renderer>();
        if (stoneRenderer == null)
        {
            Debug.LogError("Renderer is null on " + gameObject.name);
            return;
        }

        stoneMaterial = stoneRenderer.sharedMaterial; // Get the material directly
        stoneMaterial.EnableKeyword("_DECALEMISSIONONOFF");
        stoneMaterial.SetColor("_DecakEmissionColor", Color.blue);

        Debug.Log("Start: Initial Emission Color: " + stoneMaterial.GetColor("_DecakEmissionColor"));
    }

    void Update()
    {
        if (isInteracted == false && Input.GetKeyDown(interactionKey))
        {
            Interact();
        }
    }

    private void Interact()
    {
        Debug.Log("Interact() function called.");
        Debug.Log("Interacting with " + gameObject.name);

        stoneMaterial.EnableKeyword("_DECALEMISSIONONOFF");
        stoneMaterial.SetColor("_DecakEmissionColor", Color.green);
        Debug.Log("Interact: Color set to: " + stoneMaterial.GetColor("_DecakEmissionColor"));

        Debug.Log("Emission should now be green for " + gameObject.name);

        isInteracted = true;
        Debug.Log("interacted bool = " + isInteracted);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Press 'E' to interact with " + gameObject.name);
        }
    }
}