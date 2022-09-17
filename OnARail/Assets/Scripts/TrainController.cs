using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrainController : MonoBehaviour
{
    private Railway currRailway;
    private Station currStation;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool atJunction;

    public float speed = 100f;
    public float smoothing = 0.05f;
    public float rotationSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currStation = FindObjectOfType<Station>();
        transform.position = currStation.transform.position;
        currRailway = currStation.GetConnections().First();
    }

    private void Update()
    {
        // Get input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // rotate the train to match the rail
        Quaternion rotationTarget = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, rb.velocity));
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationTarget, rotationSpeed * Time.deltaTime);

        // clamp the position to be directly on the rail at all times
        currRailway.ClampPosition(transform);
    }

    void FixedUpdate()
    {        
        // move the train
        rb.velocity = Vector2.Lerp(rb.velocity, movement * speed, smoothing * Time.fixedDeltaTime);

        // adjust the velocity
        if (atJunction)
        {
            // if at a junction, choose the railway with the highest projection magnitude from the current station's railways
            Vector2 bestMove = Vector2.zero;
            Railway bestRail = currRailway;
            foreach (Railway railway in currStation.GetConnections())
            {
                Vector2 tempMove = railway.ClampVelocity(transform, rb.velocity);
                if (tempMove.sqrMagnitude > bestMove.sqrMagnitude)
                {
                    bestMove = tempMove;
                    bestRail = railway;
                }
            }
            rb.velocity = bestMove;
            currRailway = bestRail;
        }
        else
        {
            rb.velocity = currRailway.ClampVelocity(transform, rb.velocity);
        }
    }

    public void SetJunction(bool isAtJunction, Station newStation = null)
    {
        atJunction = isAtJunction;
        currStation = newStation;
    }
}
