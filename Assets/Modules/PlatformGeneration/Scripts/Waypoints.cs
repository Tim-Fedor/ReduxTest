using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [Range(0f, 2f)]
    [SerializeField] private float waypointSize = 1f;
    
    private void OnDrawGizmos()
    {
        foreach(Transform t in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, waypointSize);
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }
    }

    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        if (currentWaypoint == null)
        {
            return transform.GetChild(0);
        }

        if (currentWaypoint.GetSiblingIndex() < transform.childCount - 1)
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex() + 1);
        }
        else
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex());
        }
    }

    public bool IsWaypointFinal(Transform currentWaypoint)
    {
        return currentWaypoint.GetSiblingIndex() == transform.childCount - 1;
    }
    
    public int GetWaypointIndex(Transform currentWaypoint)
    {
        return currentWaypoint.GetSiblingIndex();
    }
}