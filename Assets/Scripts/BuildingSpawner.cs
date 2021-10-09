using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingSpawner : MonoBehaviour
{
    [SerializeField] private Transform dataCenters;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private int minDistanceBetweenDataCenters;
    private Grid<GameObject> _grid;

    void Start() 
    {
        _grid = gridManager.GetGrid();
    }
    void SpawnDatacenter()
    {
        
        for (int i = 0; i < 1000; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(0, _grid.GetWidth() - 1),
                Random.Range(0, _grid.GetHeight() - 1),
                0);
            Debug.Log("Random pos : " + pos);
            if (_grid.GetValue(pos) == null && !IsTooCloseFromDatacenters(pos))
            {
                Debug.Log("Pos is valid");
                //instantiate new datacenter at pos
                //grid.SetValue(pos, new DataCenter());
            }
        }
        Debug.LogError("can't find available datacenter position");
    }

    private bool IsTooCloseFromDatacenters(Vector3 pos)
    {
        foreach (Transform dataCenter in dataCenters)
        {
            if (Vector3.Distance(pos,dataCenter.position) < minDistanceBetweenDataCenters)
            {
                return true;
            }
        }
        return false;
    }

    public void SpawnHouse()
    {
        var datacenter = dataCenters.GetChild(Random.Range(0, dataCenters.childCount - 1)).position;
        Vector3 pos = new Vector3(0,0,0);
        pos.x = RandomFromDistribution.RandomNormalDistribution(datacenter.x, 0.8f);
        pos.y = RandomFromDistribution.RandomNormalDistribution(datacenter.y, 0.8f);
        Debug.Log("House pos : " + pos);
        if (_grid.GetValue(pos) == null)
        {
            Debug.Log("Pos is valid");
            //instantiate new house at pos
            //grid.SetValue(pos, new House());
        }
    }
}
