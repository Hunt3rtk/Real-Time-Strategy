using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour {

    private Material flashMaterial;

    [SerializeField]
    private Color flashColor = new Color(1f, 1f, 1f, 1f); // White color for flash

    [SerializeField]
    private float flashTime = 0.15f; // Duration of the flash

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

        if (!GetComponentInChildren<SkinnedMeshRenderer>()) {
            flashMaterial = GetComponent<MeshRenderer>().materials[1];
        } else {
            flashMaterial = GetComponentInChildren<SkinnedMeshRenderer>().materials[1]; // Get the material of the object
        }

        flashMaterial.SetColor("_Color", flashColor); // Set the color of the material to the flash color
    }


    public IEnumerator Flash(float delay = 0f) {

        if (flashMaterial == null) yield break;

        yield return new WaitForSecondsRealtime(delay); // Wait for the specified delay

        float currentTime = 0f;
        float transperancy = 1f;

        while (currentTime < flashTime) {

            currentTime += Time.deltaTime;

            transperancy = Mathf.Lerp(1f, 0f, currentTime / flashTime);

            flashMaterial.SetFloat("_Transparency", transperancy);

            yield return null;
        }
    }
}
