using UnityEngine;
using UnityEngine.UI;

public class TrainingProgressBar : MonoBehaviour {

    public Training training;
    public Texture unitHeadshot;

    [SerializeField]
    private Slider progressBar;

    void FixedUpdate() {
        if (this.gameObject.activeSelf && training != null) {
            // Update Gold Bar to show remaining percentage
            progressBar.value = training.GetProgress();
        }
    }
}