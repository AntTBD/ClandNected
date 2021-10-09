using UnityEngine;

public class GridManager : MonoBehaviour {
    private Grid<GameObject> _grid;
    [SerializeField] private int height;
    [SerializeField] private int width;
    // Start is called before the first frame update
    private void Awake () {
        _grid = new Grid<GameObject> (width, height, 1.0f, new Vector3 (0, 0, 0), true);
    }

    public Grid<GameObject> GetGrid () {
        return _grid;
    }
}