using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class CubeAgent : Agent
{
    public Transform Target;
    public float speedMultiplier = 0.1f;

    public override void OnEpisodeBegin()
    {
        // this = de agent zelf, we geven het script aan de agent zelf, dus we kunnen met this de agent aanspreken
        // reset de positie en orientatie als de agent gevallen is
        if (this.transform.localPosition.y < 0) // localPosition omdat we de agent in een parent object hebben zitten
        {

            // reset de positie en orientatie van de agent
            this.transform.localPosition = new Vector3(0, 0.5f, 0);

            // reset de rotatie van de agent
            this.transform.localRotation = Quaternion.identity;
        }

        // verplaats de target (sphere) naar een nieuwe willekeurige locatie 
        // geen transform.localPosition omdat we Target als Transform hebben gedefinieerd, dus we kunnen direct de positie aanpassen
        Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Target.localPosition); // positie van de target
        sensor.AddObservation(this.transform.localPosition); // positie van de agent
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Acties, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        transform.Translate(controlSignal * speedMultiplier);

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
