using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    void Start() {
        AudioManager.Instance.Play(AudioManager.SoundType.Music_Menu);
    }

    public void EscapeMenu() {
        AudioManager.Instance.Play(AudioManager.SoundType.PurchaseSuccess);
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        FindAnyObjectByType<GameManager>().SetStateGameplay();
    }

    public void OpenSettings() {
        AudioManager.Instance.Play(AudioManager.SoundType.PurchaseSuccess);
    }

    public void QuitGame() {
        AudioManager.Instance.Play(AudioManager.SoundType.PurchaseSuccess);
        FindAnyObjectByType<GameManager>().SetStateGameplay();
        SceneManager.LoadScene("MainMenu");
    }
}
