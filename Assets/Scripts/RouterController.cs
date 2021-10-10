using System;
using System.Collections.Generic;
using UnityEngine;

public class RouterController : MonoBehaviour
{
    
    [Serializable] public struct Route
    {
        public Route(GameObject port, float cout)
        {
            Port = port;
            Cout = cout;
        }
        public GameObject Port { get; }
        public float Cout { get; }
    }
    [SerializeField] private List<GameObject> _ports = new List<GameObject>(4);
    [SerializeField] private List<Route> _routingTable;
    private GameObject _datacenters;
    // Start is called before the first frame update
    void Awake()
    {
        _datacenters = GameObject.Find("DataCenters");
        _routingTable = new List<Route>(_datacenters.transform.childCount);
        foreach (Transform unused in _datacenters.transform)
        {
            _routingTable.Add(new Route(null, 0));
        }
    }

    void Start()
    {
        //UpdateTable();
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
            Debug.LogWarning("datacenter on cable : " + datacenter.name);
            
            if (portTargetTag.Equals("DataCenter") && datacenter != null)
            {
                int datacenterID = GetDataCenterIdFromGameObject(datacenter);
                Debug.LogWarning("Datacenter ID : " + datacenterID);
                if (datacenterID == -1)
                    continue;
                if (_routingTable[datacenterID].Port == null || cableController.GetWeight() < _routingTable[datacenterID].Cout)
                {
                    _routingTable[datacenterID] = new Route(cable, cableController.GetWeight());
                    Debug.LogWarning("Added to route");
                }
            }
            else if (portTargetTag.Equals("Router") && routerController != null)
            {
                List<Route> routerPath = routerController.GetTable();
                for (int j = 0; j < _routingTable.Count; j++)
                {
                    if (_routingTable[j].Port == null || routerPath[j].Cout + cableController.GetWeight() < _routingTable[j].Cout)
                    {
                        _routingTable[j] = routerPath[j];
                    }
                }
            }
        }

        int i = 0;
        foreach (Route route in _routingTable)
        {
            if (route.Port != null)
                Debug.LogWarning("Route ["+i+"]: " + route.Port.name + " Cout : " + route.Cout);
            else Debug.LogWarning("Route Cout : " + route.Cout);
            i++;
        }
    }

    public List<Route> GetTable()
    {
        return _routingTable;
    }
    public GameObject GetShortestPath(int datacenterID)
    {
        Debug.LogWarning("Datacenter search id :" + datacenterID);
        GameObject cable = _routingTable[datacenterID].Port;
        if (cable == null)
            return null;
        CableController destinationCable = cable.GetComponent<CableController>();
        if (destinationCable.GetBegin() == gameObject)
        {
            return cable.transform.GetChild(0).gameObject;
        }
        return cable.transform.GetChild(cable.transform.childCount-1).gameObject;
    }
    public GameObject GetShortestPath(GameObject datacenter)
    {
        int id = GetDataCenterIdFromGameObject(datacenter);
        if (id == -1)
        {
            Debug.LogError("Can't find datacenter in datacenters");
            return null;
        }
        return GetShortestPath(id);
    }
    static int GetDataCenterIdFromGameObject(GameObject datacenter)
    {
        int i = 0;
        int datacenterID = -1;
        Transform allDatacenters = GameObject.Find("DataCenters").transform;
        //Debug.LogError("Datacenters : " + allDatacenters.name);
        foreach (Transform oneDatacenter in allDatacenters)
        {
            //Debug.LogError("i : " + i);
            if (oneDatacenter.gameObject == datacenter)
            {
                datacenterID = i;
                break;
            }
            i++;
        }

        return datacenterID;
    }

    public void addPort(GameObject port)
    {
        _ports.Add(port);
        UpdateTable();
    }
}
