using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailwayManager : MonoBehaviour
{
    [System.Serializable]
    public struct Connection
    {
        public Station fromStation;
        public Station toStation;
    }

    public GameObject railwayPrefab;
    public List<Connection> allConnections;    

    void Awake()
    {
        int railId = 1;
        foreach(Connection con in allConnections)
        {
            // create the railway object from prefab
            GameObject railwayObject = Instantiate(railwayPrefab);
            railwayObject.name = "R" + railId++;
            Railway railway = railwayObject.GetComponent<Railway>();

            // initialize the values for the railway script component
            railway.Init(con.fromStation.transform.position, con.toStation.transform.position);

            // add the railway to the connections for each station
            con.fromStation.AddConnection(railway);
            con.toStation.AddConnection(railway);
        }
    }
}
