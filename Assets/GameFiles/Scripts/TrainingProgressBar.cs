using UnityEngine;
using UnityEngine.UI;

public class TrainingProgressBar : MonoBehaviour {

    public Training training;
    public Texture unitHeadshot;

    [SerializeField]
    private Slider progressBar;

    void Update() {
        if (this.gameObject.activeSelf && training != null) {
            Debug.Log(training.GetProgress());
            // Update Gold Bar to show remaining percentage
            progressBar.value = training.GetProgress();
        }
    }
}