using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouterController : MonoBehaviour
{
    public struct Route
    {
        public Route(GameObject port, float cout)
        {
            Port = port;
            Cout = cout;
        }
        public GameObject Port { get; }
        public float Cout { get; }
    }
    private List<GameObject> _ports = new List<GameObject>(4);
    private List<Route> _routingTable;
    private GameObject _datacenters;
    // Start is called before the first frame update
    void Awake()
    {
        _datacenters = GameObject.Find("DataCenters");
        _routingTable = new List<Route>(_datacenters.transform.childCount);
    }
    

    public void UpdateTable()
    {
        foreach (GameObject cable in _ports)
        {
            RouterController routerController = null;
            GameObject datacenter = null;
            string portTargetTag;
            CableController cableController = cable.GetComponent<CableController>();
            if (cableController.GetBegin() == gameObject)
            {
                portTargetTag = cableController.GetEnd().tag;
                if (portTargetTag.Equals("Router"))
                {
                    routerController = cableController.GetEnd().GetComponent<RouterController>();
                }
                else if (portTargetTag.Equals("DataCenter"))
                {
                    datacenter = cableController.GetEnd();
                }
            }
            else
            {
                portTargetTag = cableController.GetBegin().tag;
                if (portTargetTag.Equals("Router"))
                {
                    routerController = cableController.GetBegin().GetComponent<RouterController>();
                }
                else if (portTargetTag.Equals("DataCenter"))
                {
                    datacenter = cableController.GetBegin();
                }
            }
            
            if (portTargetTag.Equals("DataCenter") && datacenter != null)
            {
                int datacenterID = GetDataCenterIdFromGameObject(datacenter);
                if (datacenterID == -1)
                    continue;
                if (cableController.GetWeight() < _routingTable[datacenterID].Cout)
                {
                    _routingTable[datacenterID] = new Route(datacenter, cableController.GetWeight());
                }
            }
            else if (portTargetTag.Equals("Router") && routerController != null)
            {
                List<Route> routerPath = routerController.GetTable();
                for (int j = 0; j < _routingTable.Capacity; j++)
                {
                    if (routerPath[j].Cout + cableController.GetWeight() < _routingTable[j].Cout)
                    {
                        _routingTable[j] = routerPath[j];
                    }
                }
            }
        }
    }

    public List<Route> GetTable()
    {
        return _routingTable;
    }
    public GameObject GetShortestPath(int datacenterID)
    {
        return _routingTable[datacenterID].Port;
    }
    public GameObject GetShortestPath(GameObject datacenter)
    {
        int id = GetDataCenterIdFromGameObject(datacenter);
        if (id == -1)
        {
            return null;
        }
        else return _routingTable[id].Port;
    }
    static int GetDataCenterIdFromGameObject(GameObject datacenter)
    {
        int i = 0;
        int datacenterID = -1;
        Transform allDatacenters = GameObject.Find("DataCenters").transform;
        foreach (GameObject oneDatacenter in allDatacenters)
        {
            if (oneDatacenter == datacenter)
            {
                datacenterID = i;
                break;
            }
            i++;
        }

        return datacenterID;
    }
}
