using UnityEngine;

public class GridManager : MonoBehaviour {
    [SerializeField] private Grid<GameObject> _grid;
    [SerializeField] private int height;
    [SerializeField] private int width;
    [SerializeField] private float androidCoeff;
    // Start is called before the first frame update
    private void Awake () {
        _grid = new Grid<GameObject> (width, height, 1.0f, new Vector3 (0, 0, 0), true, androidCoeff);
    }

    public Grid<GameObject> GetGrid () {
        return _grid;
    }
}