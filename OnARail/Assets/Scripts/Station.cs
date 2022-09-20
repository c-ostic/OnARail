using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    public float spawnChancePerSecond = 0.1f;

    private Color stationColor;
    private List<Color> passengers = new List<Color>();
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
        if (Random.Range(0, 1) <= spawnChancePerSecond * Time.deltaTime)
        {
            //passengers.Add(colorManager.GetRandomColor(stationColor));
        }

        // Remove same color passengers from the list
        passengers.RemoveAll(color => color == stationColor);

        DrawPassengers();
    }

    public void SetColor(Color color)
    {
        stationColor = color;
        sprite.color = color;
    }

    private void DrawPassengers()
    {
        //TODO
    }
}
