using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {
    public void ChangeNewScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
    public void ExitGame() {
        Application.Quit();
    }
}
