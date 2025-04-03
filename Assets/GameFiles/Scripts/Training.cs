using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Training : MonoBehaviour {
    public bool training;

    private float progressTime;
    private float progressTimeTotal;

    void FixedUpdate() {
        if (training) {
            if (progressTime < progressTimeTotal) {
                progressTime +=   Time.deltaTime;
            } else {
                training = false;
                progressTime = progressTimeTotal;
            }
        }
    }

    public bool isTraining() {
        return training;
    }

    public float GetProgress() {
        return progressTime / progressTimeTotal;
    }

    public IEnumerator StartTraining(float cooldown) {
        progressTime = 0;
        progressTimeTotal = cooldown;
        training = true;
        yield return new WaitForSeconds(cooldown);
    }
}
