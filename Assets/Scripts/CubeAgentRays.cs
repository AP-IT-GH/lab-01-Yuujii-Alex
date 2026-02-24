using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class CubeAgentRays : Agent
{
    public Transform Target;
    public float speedMultiplier = 0.1f;
    public float rotationMultiplier = 5f;

    public override void OnEpisodeBegin()
    {
        // Reset de positie en orientatie van de agent altijd aan het begin van een episode
        this.transform.localPosition = new Vector3(0, 0.5f, 0);
        this.transform.localRotation = Quaternion.identity;

        // Verplaats de target naar een nieuwe willekeurige locatie
        Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition); // positie van de agent
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Acties, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.z = actionBuffers.ContinuousActions[0]; // x beweging
        transform.Translate(controlSignal * speedMultiplier);

        transform.Rotate(0.0f, actionBuffers.ContinuousActions[1] * rotationMultiplier, 0.0f);

        // Beloningen
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        // target bereikt
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // Van het platform gevallen?
        else if (this.transform.localPosition.y < 0)
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Target"))
    //     {
    //         SetReward(1.0f); // beloning voor het bereiken van de target
    //         EndEpisode(); // einde van het episode
    //     }
    // }
}
