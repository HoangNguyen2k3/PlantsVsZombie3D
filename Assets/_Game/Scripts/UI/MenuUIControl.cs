using UnityEngine;
using UnityEngine.UI;

public class MenuUIControl : MonoBehaviour {
    [Header("Scene Control")]
    [SerializeField] protected ChangeScene changeScene;

    [Header("UI Setting")]
    [SerializeField] protected Transform showHide;
    [SerializeField] protected bool isShow = true;

    [Header("Audio Sliders")]
    [SerializeField] private Slider sldMusic;
    [SerializeField] private Slider sldSfx;

    private void Start() {
        // Mặc định ẩn UI Setting
        isShow = false;
        if (showHide != null)
            showHide.gameObject.SetActive(false);

        // Gán giá trị ban đầu từ SoundManager (nếu có)
        if (SoundManager.Instance != null) {
            float musicVol = SoundManager.Instance.GetMusicVolume();
            float sfxVol = SoundManager.Instance.GetSfxVolume();

            if (sldMusic != null) {
                sldMusic.value = musicVol;
                sldMusic.onValueChanged.AddListener(OnMusicSliderChanged);
            }

            if (sldSfx != null) {
                sldSfx.value = sfxVol;
                sldSfx.onValueChanged.AddListener(OnSfxSliderChanged);
            }
        }
    }

    // ================== NÚT BẤM UI ==================
    public void OnClick_Play() {
        changeScene.ChangeNewScene("SelectLevel");
    }

    public void OnClick_SelectCharacter() {
        Debug.Log("👉 OnClick_SelectCharacter được gọi");

        if (HasCharacterData())
            Debug.Log("✅ Có data nhân vật, chuyển vào SelectCharacter để load");
        else
            Debug.Log("❌ Chưa có data nhân vật, chuyển vào SelectCharacter để random");

        changeScene.ChangeNewScene("SelectCharacter");
    }

    public void OnClick_Exit() {
        Application.Quit();
    }

    public void OnClick_Setting() {
        if (isShow) Hide();
        else Show();
    }

    // ================== HIỆN / ẨN UI ==================
    public virtual void Show() {
        isShow = true;
        if (showHide != null)
            showHide.gameObject.SetActive(true);
    }

    public virtual void Hide() {
        isShow = false;
        if (showHide != null)
            showHide.gameObject.SetActive(false);
    }

    // ================== KIỂM TRA DATA ==================
    private bool HasCharacterData() {
        if (CharacterDataHolder.Instance == null || CharacterDataHolder.Instance.SelectedCharacterData == null)
            return false;

        return CharacterDataHolder.Instance.SelectedCharacterData.Count > 0;
    }

    public void OnClick_SelectLevel(string name) {
        if (CharacterDataHolder.Instance != null &&
            CharacterDataHolder.Instance.SelectedCharacterData != null) {
            changeScene.ChangeNewScene("Thong");
        }
        else {
            changeScene.ChangeNewScene("SelectCharacter");
        }
    }

    // ================== ÂM THANH ==================
    private void OnMusicSliderChanged(float value) {
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetMusicVolume(value);
    }

    private void OnSfxSliderChanged(float value) {
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetSfxVolume(value);
    }

    // Tăng giảm thủ công nếu muốn gán vào nút +
    public void IncreaseMusicVolume() => AdjustMusicVolume(0.1f);
    public void DecreaseMusicVolume() => AdjustMusicVolume(-0.1f);
    public void IncreaseSfxVolume() => AdjustSfxVolume(0.1f);
    public void DecreaseSfxVolume() => AdjustSfxVolume(-0.1f);

    private void AdjustMusicVolume(float delta) {
        if (SoundManager.Instance == null || sldMusic == null) return;
        float newValue = Mathf.Clamp01(sldMusic.value + delta);
        sldMusic.value = newValue;
        SoundManager.Instance.SetMusicVolume(newValue);
    }

    private void AdjustSfxVolume(float delta) {
        if (SoundManager.Instance == null || sldSfx == null) return;
        float newValue = Mathf.Clamp01(sldSfx.value + delta);
        sldSfx.value = newValue;
        SoundManager.Instance.SetSfxVolume(newValue);
    }
}
