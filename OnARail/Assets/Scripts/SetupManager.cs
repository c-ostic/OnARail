using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SetupManager : MonoBehaviour
{
    public int numberStations = 10;
    public GameObject stationPrefab;

    private ColorManager colorManager;

    // Start is called before the first frame update
    void Start()
    {
        colorManager = FindObjectOfType<ColorManager>();

        // Generate stations on junctions
        List<Junction> junctions = FindObjectsOfType<Junction>().ToList();

        int stationId = 1;
        foreach(Color color in colorManager.GetSemiRandomDistribution(numberStations))
        {
            // Get a random junction and remove it from the list
            int randJunction = Random.Range(0, junctions.Count);
            Junction junction = junctions[randJunction];
            junctions.RemoveAt(randJunction);

            // Instantiate a station and set it to the position of the junction
            GameObject stationObject = Instantiate(stationPrefab);
            stationObject.name = "S" + (stationId++);
            stationObject.transform.position = junction.transform.position;

            // Set the color of the station
            Station station = stationObject.GetComponent<Station>();
            station.SetColor(color);
        }
    }
}
