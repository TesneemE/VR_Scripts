using UnityEngine;
using UnityEngine.SceneManagement;

public class RunesCollider : MonoBehaviour
{
    // public GameObject rune;
    public Material material;
    // private bool glowing = false;

    void EnableRuneEmission()
    {  
        material.EnableKeyword("_DECALEMISSIONONOFF");
    }
}
