using UnityEngine;

public class GridManager : MonoBehaviour {
    [SerializeField] private Grid<GameObject> _grid;
    [SerializeField] private int height;
    [SerializeField] private int width;
    [SerializeField] private float androidCoeff;

    [SerializeField] private Sprite contourLine, contourAngle;
    [SerializeField] private Material contourMaterial;
    // Start is called before the first frame update
    private void Awake () {
        // becarefull, width and height are redefine with screen size
        _grid = new Grid<GameObject> (width, height, 1.0f, new Vector3 (0, 0, 0), true, androidCoeff);

        DrawOutLines(_grid.GetWidth(), _grid.GetHeight());
    }

    public Grid<GameObject> GetGrid () {
        return _grid;
    }

    private void DrawOutLines(int width, int height)
    {
        Transform borders = transform.GetChild(0);

        SetBorder(borders, -1, -1, contourAngle);
        // bottom border
        for (int x = 0; x < width; x++)
        {
            int y = -1;
            SetBorder(borders, x, y, contourLine);
        }
        SetBorder(borders, -1, height, contourAngle, -90);
        // top border
        for (int x = 0; x < width; x++)
        {
            int y = height;
            SetBorder(borders, x, y, contourLine);
        }
        SetBorder(borders, width, height, contourAngle, 180);
        // left border
        for (int y = 0; y < height; y++)
        {
            int x = -1;
            SetBorder(borders, x, y, contourLine, 90);
        }
        SetBorder(borders, width, -1, contourAngle, 90);
        // right border
        for (int y = 0; y < height; y++)
        {
            int x = width;
            SetBorder(borders, x, y, contourLine, 90);
        }
    }

    private void SetBorder(Transform parent, int x, int y, Sprite sprite, float angle = 0)
    {
        GameObject border = new GameObject("Border " + x + "." + y);
        border.AddComponent<SpriteRenderer>().sprite = sprite;
        border.GetComponent<SpriteRenderer>().material = contourMaterial;
        border.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("UI");
        border.GetComponent<SpriteRenderer>().sortingOrder = 0;
        border.transform.parent = parent;
        border.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        border.transform.position= _grid.GetWorldPosition(x, y) + new Vector3(_grid.GetCellSize() / 2f, _grid.GetCellSize() / 2f,1);
        border.transform.localScale = new Vector3(_grid.GetCellSize(), _grid.GetCellSize(), 1);
    }
}