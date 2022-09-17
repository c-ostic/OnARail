using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railway : MonoBehaviour
{
    private Vector2 direction, fromStation, toStation;
    private LineRenderer line;
    private float maxX, maxY, minX, minY;

    public float positionThreshold = 0.005f;

    // Initializes the Railway from the two endpoint stations
    public void Init(Vector2 fromStation, Vector2 toStation)
    {
        this.fromStation = fromStation;
        this.toStation = toStation;
        direction = toStation - fromStation;

        maxX = Mathf.Max(fromStation.x, toStation.x);
        maxY = Mathf.Max(fromStation.y, toStation.y);
        minX = Mathf.Min(fromStation.x, toStation.x);
        minY = Mathf.Min(fromStation.y, toStation.y);

        line = GetComponent<LineRenderer>();
        line.SetPositions(new Vector3[] {fromStation, toStation});
    }

    // Clamps the velocity to match the rail
    public Vector2 ClampVelocity(Transform transform, Vector2 velocity)
    {
        if (Vector2.Dot(velocity, direction) > 0 && (toStation - (Vector2)transform.position).sqrMagnitude < positionThreshold)
        {
            return Vector2.zero;
        }
        else if(Vector2.Dot(velocity, direction) < 0 && (fromStation - (Vector2)transform.position).sqrMagnitude < positionThreshold)
        {
            return Vector2.zero;
        }
        else
        {
            return Vector3.Project(velocity, direction);
        }
    }

    // Makes sure the transform is directly on the line
    public void ClampPosition(Transform transform)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // x is closer to being correct, so correct y (using direction as slope and fromStation as the point)
            float newY = (direction.y / direction.x) * (transform.position.x - fromStation.x) + fromStation.y;

            transform.position = new Vector2(
                Mathf.Clamp(transform.position.x, minX, maxX),
                Mathf.Clamp(newY, minY, maxY));
        }
        else
        {
            // y is closer to being correct, so correct x
            float newX = (direction.x / direction.y) * (transform.position.y - fromStation.y) + fromStation.x;

            transform.position = new Vector2(
                Mathf.Clamp(newX, minX, maxX),
                Mathf.Clamp(transform.position.y, minY, maxY));
        }
    }

    // Returns the direction vector of the railway (for projection of the train's velocity)
    public Vector2 GetDirectionVector()
    {
        return direction;
    }
}
