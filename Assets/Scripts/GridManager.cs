using UnityEngine;

public class GridManager : MonoBehaviour {
    private Grid<GameObject> _grid;
    // Start is called before the first frame update
    private void Start () {
        _grid = new Grid<GameObject> (4, 2, 2f, new Vector3 (0, 0, 0));
    }
    
    public Grid<GameObject> GetGrid()
    {
        return _grid;
    }

}