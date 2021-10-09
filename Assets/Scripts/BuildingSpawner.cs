using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingSpawner : MonoBehaviour
{
    [SerializeField] private Transform dataCenters;
    [SerializeField] private GameObject grid;
    [SerializeField] private int minDistanceBetweenDataCenters;
    

    void spawnDatacenter()
    {
        for (int i = 0; i < 1000; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(0, grid.getWidth() - 1),
                Random.Range(0, grid.getHeight() - 1),
                0);
            if (grid.GetValue(pos) == null && !isTooCloseFromDatacenters(pos))
            {
                //instantiate new datacenter at pos
            }
        }
        Debug.LogError("can't find available datacenter position");
    }

    private bool isTooCloseFromDatacenters(Vector3 pos)
    {
        
    }
}
