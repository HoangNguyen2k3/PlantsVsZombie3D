using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {
    public List<Button> list_button_level = new();
    public string STRING_NAME_MAIN = "Hoang";
    private void Awake() {
        if (PlayerPrefs.HasKey("CurrentLevel")) {
            for (int i = 0; i < list_button_level.Count; i++) {
                if (i <= (PlayerPrefs.GetInt("CurrentLevel") - 1)) {
                    list_button_level[i].interactable = true;
                }
                else {
                    list_button_level[i].interactable = false;
                }
            }
        }
        else {
            PlayerPrefs.SetInt("CurrentLevel", 1);
            list_button_level[0].interactable = true;
            for (int i = 1; i < list_button_level.Count; i++) {
                list_button_level[i].interactable = false;
            }
        }
    }
    public void OnClickSelectLevel(int level) {
        PlayerPrefs.SetInt("CurrentLevelHere", level);
        SceneManager.LoadScene(STRING_NAME_MAIN);
    }
}
