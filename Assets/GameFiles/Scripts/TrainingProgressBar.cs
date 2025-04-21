using UnityEngine;
using UnityEngine.UI;

public class TrainingProgressBar : MonoBehaviour {

    public Training training;
    public Texture unitHeadshot;

    [SerializeField]
    public Slider progressBar;

    void FixedUpdate() {
        if (training != null) {
            // Update Gold Bar to show remaining percentage
            progressBar.value = training.GetProgress();
        }
    }
}