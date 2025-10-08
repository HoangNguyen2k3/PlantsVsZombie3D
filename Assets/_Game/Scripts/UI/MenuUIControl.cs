using UnityEngine;

public class MenuUIControl : MonoBehaviour
{
    [SerializeField] protected ChangeScene changeScene;

    public void OnClick_Play()
    {
        if (CharacterDataHolder.Instance != null
            && CharacterDataHolder.Instance.SelectedCharacterData != null)
        {
            // Có data → load gameplay
            changeScene.ChangeNewScene("Thong");
        }
        else
        {
            // Chưa chọn nhân vật → chuyển qua SelectCharacter trước
            changeScene.ChangeNewScene("SelectCharacter");
        }
    }

    public void OnClick_SelectCharacter()
    {
        Debug.Log("👉 OnClick_SelectCharacter được gọi");
        
        // Kiểm tra có data đã lưu không
        if (HasCharacterData())
        {
            Debug.Log("✅ Có data nhân vật, chuyển vào SelectCharacter để load");
        }
        else
        {
            Debug.Log("❌ Chưa có data nhân vật, chuyển vào SelectCharacter để random");
        }
        
        changeScene.ChangeNewScene("SelectCharacter");
    }

    public void OnClick_Exit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Kiểm tra có data nhân vật đã lưu chưa
    /// </summary>
    private bool HasCharacterData()
    {
        if (CharacterDataHolder.Instance == null || CharacterDataHolder.Instance.SelectedCharacterData == null)
            return false;
            
        return CharacterDataHolder.Instance.SelectedCharacterData.Count > 0;
    }

    public void OnClick_SelectLevel(string name)
    {
        changeScene.ChangeNewScene(name);
    }
}
