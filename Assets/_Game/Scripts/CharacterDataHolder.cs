using System.Collections.Generic;
using UnityEngine;
using Layer_lab._3D_Casual_Character.Demo2;

public class CharacterDataHolder : MonoBehaviour
{
    public static CharacterDataHolder Instance { get; private set; }
    public Dictionary<PartType, int> SelectedCharacterData = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
