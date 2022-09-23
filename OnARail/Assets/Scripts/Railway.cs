using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railway : MonoBehaviour
{
    private Vector2 direction, fromJunction, toJunction;
    private LineRenderer line;
    private float maxX, maxY, minX, minY;

    // Initializes the Railway from the two endpoint Junctions
    public void Init(Vector2 fromJunction, Vector2 toJunction)
    {
        this.fromJunction = fromJunction;
        this.toJunction = toJunction;
        direction = toJunction - fromJunction;

        maxX = Mathf.Max(fromJunction.x, toJunction.x);
        maxY = Mathf.Max(fromJunction.y, toJunction.y);
        minX = Mathf.Min(fromJunction.x, toJunction.x);
        minY = Mathf.Min(fromJunction.y, toJunction.y);

        line = GetComponent<LineRenderer>();
        line.SetPositions(new Vector3[] {fromJunction, toJunction});
    }

    // Clamps the velocity to match the rail
    public Vector2 ClampVelocity(Transform transform, Vector2 velocity, bool isAtJunction)
    {
        if (isAtJunction && Vector2.Dot(velocity, direction) > 0 && 
            Vector2.Distance(toJunction, transform.position) < Vector2.Distance(fromJunction, transform.position))
        {
            return Vector2.zero;
        }
        else if(isAtJunction && Vector2.Dot(velocity, direction) < 0 && 
            Vector2.Distance(toJunction, transform.position) > Vector2.Distance(fromJunction, transform.position))
        {
            return Vector2.zero;
        }
        else
        {
            return Vector3.Project(velocity, direction);
        }
    }

    // Makes sure the transform is directly on the line
    public Vector2 ClampPosition(Transform transform)
    {
        Vector2 newPos;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // x is closer to being correct, so correct y (using direction as slope and fromJunction as the point)
            float newY = (direction.y / direction.x) * (transform.position.x - fromJunction.x) + fromJunction.y;

            newPos = new Vector2(
                Mathf.Clamp(transform.position.x, minX, maxX),
                Mathf.Clamp(newY, minY, maxY));
        }
        else
        {
            // y is closer to being correct, so correct x
            float newX = (direction.x / direction.y) * (transform.position.y - fromJunction.y) + fromJunction.x;

            newPos = new Vector2(
                Mathf.Clamp(newX, minX, maxX),
                Mathf.Clamp(transform.position.y, minY, maxY));
        }

        return newPos;
    }

    // Returns the direction vector of the railway (for projection of the train's velocity)
    public Vector2 GetDirectionVector()
    {
        return direction;
    }
}
