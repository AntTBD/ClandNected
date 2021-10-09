using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingSpawner : MonoBehaviour
{
    [SerializeField] private Transform dataCenters;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private int minDistanceBetweenDataCenters;
    

    void SpawnDatacenter()
    {
        Grid<GameObject> grid = gridManager.GetGrid();
        for (int i = 0; i < 1000; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(0, grid.GetWidth() - 1),
                Random.Range(0, grid.GetHeight() - 1),
                0);
            if (grid.GetValue(pos) == null && !IsTooCloseFromDatacenters(pos))
            {
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
}
