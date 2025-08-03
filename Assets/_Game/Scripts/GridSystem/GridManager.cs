using UnityEngine;

public class GridManager : MonoBehaviour {
    public int rows = 5;
    public int columns = 9;
    public float cellSize = 1.5f;
    private GridCell[,] gridCells;
    public GridCell gridCellPrefab;
    public Material[] cellMaterial;

    void Start() {
        GenerateGrid();
    }

    void GenerateGrid() {
        gridCells = new GridCell[rows, columns];
        Vector3 startPosition = transform.position;
        int temp = 0;
        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < columns; col++) {
                Vector3 worldPos = startPosition + new Vector3(col * cellSize, 0, -row * cellSize);

                GameObject cellObject = Instantiate(gridCellPrefab.gameObject, transform);
                cellObject.transform.position = worldPos;
                GridCell cell = cellObject.GetComponent<GridCell>();
                cell.worldPosition = worldPos;
                cell.gridPosition = new Vector2Int(row, col);
                cell.InitMaterial(cellMaterial[temp++ % 2]);
                gridCells[row, col] = cell;
            }
        }
    }

    public GridCell GetCellFromWorldPosition(Vector3 worldPos) {
        int col = Mathf.FloorToInt((worldPos.x - transform.position.x) / cellSize);
        int row = Mathf.FloorToInt(-(worldPos.z - transform.position.z) / cellSize);
        if (row >= 0 && row < rows && col >= 0 && col < columns)
            return gridCells[row, col];
        return null;
    }
}