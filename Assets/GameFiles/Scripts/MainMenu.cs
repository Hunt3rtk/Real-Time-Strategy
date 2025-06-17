using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    void Start() {
        AudioManager.Instance.Play(AudioManager.SoundType.Music_Menu);
    }

    public void PlayGame() {
        AudioManager.Instance.Play(AudioManager.SoundType.PurchaseSuccess);
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame() {
        AudioManager.Instance.Play(AudioManager.SoundType.PurchaseSuccess);
        Application.Quit();
    }

}
