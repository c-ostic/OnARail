using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Station : MonoBehaviour
{
    public float spawnChancePerSecond = 0.1f;
    public int maxPassengers = 10;
    public float spacingRadius = 1.5f;
    public float spacingAngle = 20;
    public float startingAngle = 90;
    public int direction = -1;
    public GameObject passengerPrefab;

    private Color stationColor;
    private List<Passenger> passengers = new List<Passenger>();
    private ColorManager colorManager;
    private SpriteRenderer sprite;

    private void Awake()
    {
        colorManager = FindObjectOfType<ColorManager>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Try to spawn a passenger
        if (passengers.Count < maxPassengers && Random.Range(0f, 1f) <= spawnChancePerSecond * Time.deltaTime)
        {
            // If the chance succeeded, create a passenger with a random destination color and add it to the list
            GameObject passengerObject = Instantiate(passengerPrefab);
            passengerObject.transform.parent = transform.parent;
            Passenger passenger = passengerObject.GetComponent<Passenger>();
            passenger.SetDestination(colorManager.GetRandomColor(stationColor));
            passengers.Add(passenger);
        }

        // Remove same color passengers from the list
        passengers.RemoveAll(passenger => passenger.GetDestination() == stationColor);

        DrawPassengers();
    }

    private void DrawPassengers()
    {
        for(int i = 0;i < passengers.Count;i++)
        {
            float angle = startingAngle + i * spacingAngle * direction;
            float xPos = transform.position.x + spacingRadius * Mathf.Cos(angle);
            float yPos = transform.position.y + spacingRadius * Mathf.Sin(angle);
            passengers[i].transform.position = new Vector3(xPos, yPos, 0);
        }
    }

    public void SetColor(Color color)
    {
        stationColor = color;
        sprite.color = color;
    }
}
