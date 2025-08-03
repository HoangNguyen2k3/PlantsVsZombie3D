using UnityEngine;

public class GamePlayManager : MonoBehaviour {
    public GridManager gridManager;
    public Plant selectedPlantPrefab;
    public LayerMask gridCellLayer; //Layer của grid cell
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) {
                GridCell gridcell = hit.collider.GetComponentInParent<GridCell>();
                if (gridcell != null) {
                    if (!gridcell.isOccupied) {
                        Instantiate(selectedPlantPrefab, gridcell.transform.position, selectedPlantPrefab.transform.rotation);
                    }
                }
            }
        }
    }
    public void ChangeCurrentPlant(Plant plant) {
        selectedPlantPrefab = plant;
    }
}
