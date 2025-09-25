using UnityEngine;
using System.Collections.Generic;
using Layer_lab._3D_Casual_Character.Demo2;

/// <summary>
/// Áp dụng dữ liệu nhân vật (CharacterDataHolder) vào model hiện tại.
/// - Lấy index part đã chọn ở màn SelectCharacter
/// - Bật/tắt các GameObject tương ứng trong prefab Gameplay
/// - Đặc biệt: Body sẽ bật cả nhánh màu đã chọn
/// </summary>
public class CharacterDataApply : MonoBehaviour
{
    [Header("Parents trong prefab")]
    [SerializeField] private Transform bodyParent;   // Characters/Body
    [SerializeField] private Transform partAParent;  // Characters/Parts/PartA
    [SerializeField] private Transform partBParent;  // Characters/Parts/PartB

    private void Start()
    {
        var holder = CharacterDataHolder.Instance;
        if (holder == null || holder.SelectedCharacterData == null)
            return;

        ApplyCharacter(holder.SelectedCharacterData);
    }

    /// <summary>
    /// Áp dụng dữ liệu (index các part) vào nhân vật
    /// </summary>
    public void ApplyCharacter(Dictionary<PartType, int> data)
    {
        foreach (var kvp in data)
        {
            var type = kvp.Key;
            var index = kvp.Value;

            Transform group = FindPartGroup(type);
            if (group == null) continue;

            if (type == PartType.Body)
            {
                ApplyBody(group, index);
            }
            else
            {
                ApplyPart(group, index);
            }
        }
    }

    /// <summary>
    /// Xử lý riêng cho Body: 
    /// Bật 1 nhánh màu (Body_Black, Body_White, …) và toàn bộ con trong nhánh đó
    /// </summary>
    private void ApplyBody(Transform group, int index)
    {
        // Tắt toàn bộ nhánh màu trước
        foreach (Transform child in group)
            child.gameObject.SetActive(false);

        // Nếu index hợp lệ → bật nhánh đã chọn
        if (index >= 0 && index < group.childCount)
        {
            var selectedBranch = group.GetChild(index);
            selectedBranch.gameObject.SetActive(true);

            var allParts = GetAllChildrenRecursive(selectedBranch);
            foreach (var go in allParts)
                go.SetActive(true);
        }
    }

    /// <summary>
    /// Áp dụng cho các part thường (Hair, Eye, Head, …)
    /// Bật đúng item theo index, tắt các item khác
    /// </summary>
    private void ApplyPart(Transform group, int index)
    {
        var options = GetSelectableItems(group);

        // Tắt tất cả items trước
        foreach (var go in options) go.SetActive(false);

        // Nếu index hợp lệ → bật item tương ứng
        if (index >= 0 && index < options.Count)
        {
            ActivateWithAncestors(options[index], group);
        }
    }

    /// <summary>
    /// Tìm group tương ứng với PartType trong prefab
    /// </summary>
    private Transform FindPartGroup(PartType type)
    {
        switch (type)
        {
            // Part A
            case PartType.Beard: return partAParent.Find("Beard");
            case PartType.Brow: return partAParent.Find("Brow");
            case PartType.Earring: return partAParent.Find("Earring");
            case PartType.Eye: return partAParent.Find("Eye");
            case PartType.Eyewear: return partAParent.Find("Eyewear");
            case PartType.Hair: return partAParent.Find("Hair");
            case PartType.Mouth: return partAParent.Find("Mouth");

            // Part B
            case PartType.Head: return partBParent.Find("Head");
            case PartType.Back: return partBParent.Find("Back");
            case PartType.Chest: return partBParent.Find("Chest");
            case PartType.Foot: return partBParent.Find("Feet");
            case PartType.Hand: return partBParent.Find("Hands");
            case PartType.Leg: return partBParent.Find("Legs");

            // Vũ khí
            case PartType.Wield_Gear_Left:
                return partBParent.Find("Wield_Gear/Wield_Gear_Left");
            case PartType.Wield_Gear_Right:
                return partBParent.Find("Wield_Gear/Wield_Gear_Right");

            // Body
            case PartType.Body: return bodyParent;
        }
        return null;
    }

    /// <summary>
    /// Lấy danh sách các item selectable trong 1 group
    /// </summary>
    private List<GameObject> GetSelectableItems(Transform group)
    {
        var result = new List<GameObject>();

        // Nếu có CharacterPartItem → ưu tiên
        var markers = group.GetComponentsInChildren<CharacterPartItem>(true);
        foreach (var m in markers)
            result.Add(m.gameObject);

        // Nếu không có, lấy toàn bộ leaf nodes
        if (result.Count == 0)
            CollectLeafGameObjects(group, result);

        return result;
    }

    /// <summary>
    /// Thu thập toàn bộ GameObject con không có child (leaf node)
    /// </summary>
    private void CollectLeafGameObjects(Transform parent, List<GameObject> list)
    {
        foreach (Transform child in parent)
        {
            if (child.childCount == 0)
                list.Add(child.gameObject);
            else
                CollectLeafGameObjects(child, list);
        }
    }

    /// <summary>
    /// Thu thập toàn bộ descendants (dùng cho Body branch)
    /// </summary>
    private List<GameObject> GetAllChildrenRecursive(Transform parent)
    {
        var result = new List<GameObject>();
        foreach (Transform child in parent)
        {
            result.Add(child.gameObject);
            if (child.childCount > 0)
                result.AddRange(GetAllChildrenRecursive(child));
        }
        return result;
    }

    /// <summary>
    /// Bật 1 item kèm toàn bộ cha của nó (đảm bảo visible)
    /// </summary>
    private void ActivateWithAncestors(GameObject item, Transform groupRoot)
    {
        var t = item.transform;
        while (t != null)
        {
            t.gameObject.SetActive(true);
            if (t == groupRoot) break;
            t = t.parent;
        }
    }
}
