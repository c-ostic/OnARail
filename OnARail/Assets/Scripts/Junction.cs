using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junction : MonoBehaviour
{
    public string id;

    private List<Railway> connections = new List<Railway>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<TrainController>().SetJunction(true, this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<TrainController>().SetJunction(false);
    }

    public void AddConnection(Railway rail)
    {
        connections.Add(rail);
    }

    public IEnumerable<Railway> GetConnections()
    {
        return connections;
    }
}
