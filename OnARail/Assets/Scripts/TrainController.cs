using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrainController : MonoBehaviour
{
    private Railway currRailway;
    private Junction currJunction;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool atJunction;

    public float speed = 100f;
    public float speedSmoothing = 6f;
    public float positionSmoothing = 10f;
    public float rotationSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Junction[] allJunctions = FindObjectsOfType<Junction>();
        currJunction = allJunctions[Random.Range(0, allJunctions.Length)];

        transform.position = currJunction.transform.position;
        currRailway = currJunction.GetConnections().First();
    }

    private void Update()
    {
        // Get input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // rotate the train to match the rail
        Quaternion rotationTarget = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, rb.velocity));
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationTarget, rotationSpeed * Time.deltaTime);   
    }

    void FixedUpdate()
    {        
        // move the train
        rb.velocity = Vector2.Lerp(rb.velocity, movement * speed, speedSmoothing * Time.fixedDeltaTime);

        // adjust the velocity
        if (atJunction)
        {
            // if at a junction, choose the railway with the highest projection magnitude from the current junction's railways
            Vector2 bestMove = Vector2.zero;
            Railway bestRail = currRailway;
            foreach (Railway railway in currJunction.GetConnections())
            {
                Vector2 tempMove = railway.ClampVelocity(transform, movement * speed);

                if (tempMove.sqrMagnitude > bestMove.sqrMagnitude)
                {
                    bestMove = tempMove;
                    bestRail = railway;
                }
            }
            rb.velocity = bestMove;
            currRailway = bestRail;
            rb.position = Vector2.Lerp(rb.position, currJunction.transform.position, positionSmoothing * Time.fixedDeltaTime);
        }
        else
        {
            rb.velocity = currRailway.ClampVelocity(transform, rb.velocity);
            rb.position = Vector2.Lerp(rb.position, currRailway.ClampPosition(transform), positionSmoothing * Time.fixedDeltaTime);
        }
    }

    public void SetJunction(bool isAtJunction, Junction newJunction = null)
    {
        atJunction = isAtJunction;
        currJunction = newJunction;
    }
}
