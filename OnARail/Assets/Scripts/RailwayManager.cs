using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RailwayManager : MonoBehaviour
{
    public GameObject railwayPrefab;
    public TextAsset connectionData;

    void Awake()
    {
        // Create a dictionary of junction based on their ids
        Dictionary<string, Junction> junctions = FindObjectsOfType<Junction>().ToDictionary(junction => junction.id);

        // Create a list of connections from the data (each connection is a string array)
        IEnumerable<string[]> allConnections = connectionData.text.Split("\n").Select(connectionLine => connectionLine.Split("//"));

        int railId = 1;
        foreach(string[] con in allConnections)
        {
            Junction from = junctions[con[0].Trim()];
            Junction to = junctions[con[1].Trim()];

            // create the railway object from prefab
            GameObject railwayObject = Instantiate(railwayPrefab);
            railwayObject.name = "R" + railId++;
            Railway railway = railwayObject.GetComponent<Railway>();

            
            // initialize the values for the railway script component
            railway.Init(from.transform.position, to.transform.position);

            // add the railway to the connections for each junction
            from.AddConnection(railway);
            to.AddConnection(railway);
        }
    }
}
