using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    void Start() {
        AudioManager.Instance.Play(AudioManager.SoundType.Music_Menu);
    }

    public void PlayGame() {
        SceneManager.LoadScene("DemoLevel");
    }

    public void QuitGame() {
        Application.Quit();
    }

}
