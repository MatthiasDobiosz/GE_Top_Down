using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class GraphController : MonoBehaviour
{
    void Start()
    {
        EventManager.StartListening("updateGrid", UpdateGridHandler);
    }

    void UpdateGridHandler(Dictionary<string, object> message)
    {
        StartCoroutine(UpdateGrid((Bounds)message["bounds"]));
    }

    private IEnumerator UpdateGrid(Bounds bounds)
    {
        yield return new WaitForSeconds(1f);
        AstarPath.active.UpdateGraphs(new GraphUpdateObject(bounds));
    }
}
