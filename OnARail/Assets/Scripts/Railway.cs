using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railway : MonoBehaviour
{
    private Vector2 direction;
    private LineRenderer line;
    private float maxX, maxY, minX, minY;

    // Initializes the Railway from the two endpoint stations
    public void Init(Vector2 fromStation, Vector2 toStation)
    {
        direction = toStation - fromStation;

        maxX = Mathf.Max(fromStation.x, toStation.x);
        maxY = Mathf.Max(fromStation.y, toStation.y);
        minX = Mathf.Min(fromStation.x, toStation.x);
        minY = Mathf.Min(fromStation.y, toStation.y);

        line = GetComponent<LineRenderer>();
        line.SetPositions(new Vector3[] {fromStation, toStation});
    }

    // Returns the direction vector of the railway (for projection of the train's velocity)
    public Vector2 GetDirectionVector()
    {
        return direction;
    }

    // Clamps a position between the two endpoints of the railway
    public void ClampPosition(Transform transform)
    {
        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x, minX, maxX), 
            Mathf.Clamp(transform.position.y, minY, maxY));
    }
}
