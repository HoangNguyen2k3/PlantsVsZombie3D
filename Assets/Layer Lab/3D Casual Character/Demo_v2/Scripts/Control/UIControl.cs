using UnityEngine;
using UnityEngine.SceneManagement;

namespace Layer_lab._3D_Casual_Character.Demo2
{
    public class UIControl : MonoBehaviour
    {
        public static UIControl Instance { get; private set; }
        public PartType FocusPartTYpe { get; private set; }

        [field: SerializeField] public PanelItem PanelItem { get; set; }
        [field: SerializeField] public PanelPreset PanelPreset { get; set; }
        [field: SerializeField] public PanelAnimation PanelAnimation { get; set; }
        [field: SerializeField] private AnimationBar AnimationBar { get; set; }
        private ItemSlot[] ItemSlot { get; set; }
        [field: SerializeField] public ItemFocusSlot ItemFocusSlot { get; set; }

        [SerializeField] protected ChangeScene changeScene;

        public Sprite[] spriteActiveIcons;
        public Sprite[] spriteBgs;

        public GameObject buttonExport;

        private void Awake()
        {
            Instance = this;

            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                buttonExport.SetActive(false);
            }
        }

        public void SetFocusPart(ItemSlot itemSlot)
        {
            if (itemSlot == null)
            {
                ItemFocusSlot.Hide();
                return;
            }

            FocusPartTYpe = itemSlot.PartType;
            ItemFocusSlot.transform.SetParent(itemSlot.transform, false);
            ItemFocusSlot.transform.position = itemSlot.transform.position;
            ItemFocusSlot.Show();
        }

        public void Init()
        {
            ItemSlot = transform.GetComponentsInChildren<ItemSlot>();

            AnimationBar.Init();
            PanelItem.Init();
            PanelPreset.Init();
            PanelAnimation.Init();

            foreach (var t in ItemSlot)
            {
                t.SetSlot();
            }
        }

        /// <summary>
        /// 랜덤
        /// </summary>
        public void OnClick_Random()
        {
            Demo2Character.Instance.OnRandomChanged.Invoke();
        }

        public void OnClick_Export()
        {
#if UNITY_EDITOR
            Demo2Character.Instance.CharacterPrefabSaver.SaveAsPrefab();
#endif
        }

        public void CloseItemPanel()
        {
            PanelItem.Hide();
        }

        public static Sprite GetSprite(string spriteName)
        {
            return Resources.Load<Sprite>(spriteName);
        }
        //Update
        public virtual void OnClick_Play()
        {
            Debug.Log("👉 OnClick_Play được gọi");

            // Kiểm tra Demo2Character.Instance
            if (Demo2Character.Instance == null)
            {
                Debug.LogError("❌ Demo2Character.Instance = null");
                return;
            }
            else
            {
                Debug.Log("✅ Demo2Character.Instance OK");
            }

            // Kiểm tra CharacterDataHolder.Instance
            if (CharacterDataHolder.Instance == null)
            {
                Debug.LogError("❌ CharacterDataHolder.Instance = null");
                return;
            }
            else
            {
                Debug.Log("✅ CharacterDataHolder.Instance OK");
            }

            // Lấy dữ liệu part
            var partsData = Demo2Character.Instance.CurrentPartsTypeAndNameList();
            CharacterDataHolder.Instance.SelectedCharacterData = partsData;
            Debug.Log($"📦 Đã lưu {partsData.Count} parts vào CharacterDataHolder");

            changeScene.ChangeNewScene("Thong");
        }
    }
}