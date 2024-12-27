using System.Collections;
using UnityEngine;

public class Training : MonoBehaviour {
    public bool training;

    public bool isTraining() {
        return training;
    }

    public IEnumerator StartTraining(float cooldown) {
        training = true;
        yield return new WaitForSecondsRealtime(cooldown);
        training = false;
    }
}
