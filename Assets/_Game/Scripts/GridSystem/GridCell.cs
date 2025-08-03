using UnityEngine;

public class GridCell : MonoBehaviour {
    public Vector3 worldPosition;
    public Vector2Int gridPosition; // Vị trí dạng hàng/cột
    public Plant currentPlant;
    public bool isOccupied => currentPlant != null;
    public MeshRenderer meshRenderer;
    public void InitMaterial(Material mat) {
        meshRenderer.material = mat;
    }
}
