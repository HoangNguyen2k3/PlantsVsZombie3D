using UnityEngine;

namespace Layer_lab._3D_Casual_Character.Demo2
{
    public class DemoControl : MonoBehaviour
    {
        public static DemoControl Instance { get; set; }
        
        [field: SerializeField] public string ItemImagePath { get; set; }
        [field: SerializeField] public PresetData PresetData { get; set; } // ScriptableObject 참조
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Demo2Character.Instance.Init();
            UIControl.Instance.Init();
            
            // Kiểm tra có data đã lưu không
            if (HasCharacterData())
            {
                Debug.Log("✅ Có data nhân vật, đang load...");
                LoadCharacterData();
            }
            else
            {
                Debug.Log("❌ Chưa có data nhân vật, random character...");
                Demo2Character.Instance.OnRandomChanged.Invoke();
            }
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

        /// <summary>
        /// Load data nhân vật đã lưu vào character hiện tại
        /// </summary>
        private void LoadCharacterData()
        {
            if (CharacterDataHolder.Instance == null || CharacterDataHolder.Instance.SelectedCharacterData == null)
            {
                Debug.LogError("❌ Không có data để load");
                return;
            }

            var savedData = CharacterDataHolder.Instance.SelectedCharacterData;
            Debug.Log($"📦 Đang load {savedData.Count} parts từ CharacterDataHolder");

            // Load từng part
            foreach (var kvp in savedData)
            {
                var partType = kvp.Key;
                var index = kvp.Value;
                
                var characterPart = Demo2Character.Instance.CurrentCharacterPartByType(partType);
                if (characterPart != null)
                {
                    characterPart.SetPartByIndex(index);
                    Debug.Log($"✅ Loaded {partType}: index {index}");
                }
                else
                {
                    Debug.LogWarning($"⚠️ Không tìm thấy part {partType}");
                }
            }
        }
    }
}