using UnityEngine;

public class FollowWayPoint : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float waypointReachedDistance = 2.5f;

    private int currentWaypointIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		if (waypoints.Length == 0)
		{
			enabled = false;
		}
	}

    // Update is called once per frame
    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        
        float distance = Vector3.Distance(transform.position, targetWaypoint.position);
        
        if (distance < waypointReachedDistance)
        {
            currentWaypointIndex++;
            
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        else
        {
			Vector3 direction = waypoints[currentWaypointIndex].transform.position - transform.position;
			Quaternion lookRotation = Quaternion.LookRotation(direction);
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime
			* rotationSpeed);

			transform.Translate(0, 0, moveSpeed * Time.deltaTime);
        }
    }
}
