using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;
using UnityControllerForTello;

public class telloagent : Agent
{
    // Start is called before the first frame update
    private Vector3 initialspawn;
    public SceneManager scenemanger;
    public DroneSimulator simulator;
    Rigidbody rb;
    public float rawElv;
    public float rawRoll;
    public float rawPitch;
    public float rawYaw;
    Collider collide;
    public float inputDrag, drag;

    void Start()
    {
        initialspawn = this.transform.localPosition;
        rb = this.GetComponent<Rigidbody>();
        collide = this.GetComponent<Collider>();
    }

    public override void OnEpisodeBegin()
    {
        this.transform.position = initialspawn;
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        simulator.TakeOff();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition); //3
        sensor.AddObservation(this.transform.localRotation); //4
        sensor.AddObservation(this.transform.forward.normalized); //3
        sensor.AddObservation(scenemanger.elv);
        sensor.AddObservation(scenemanger.roll);
        sensor.AddObservation(scenemanger.yaw);
        sensor.AddObservation(scenemanger.pitch);
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.y);
        sensor.AddObservation(rb.velocity.z);
        Debug.DrawRay(this.transform.position, this.transform.forward, Color.green);
        //Debug.Log(this.rigidBody.velocity);

    }
    public override void OnActionReceived(float[] vectorAction)
    {

        rawElv = vectorAction[0];
        rawRoll = vectorAction[1];
        rawPitch = vectorAction[2];
        rawYaw = vectorAction[3];
        scenemanger.pitch = rawPitch;
        scenemanger.elv = rawElv;
        scenemanger.yaw = rawYaw;
        scenemanger.roll = rawRoll;

        rb.AddForce(transform.up * 9.81f);
        bool receivingInput = false;
        var pitchInput = scenemanger.pitch;
        rb.AddForce(transform.forward * pitchInput);
        if (System.Math.Abs(pitchInput) > 0)
        {
            receivingInput = true;
        }
        var elvInput = scenemanger.elv;
        rb.AddForce(transform.up * elvInput);
        if (System.Math.Abs(elvInput) > 0)
        {
            receivingInput = true;
        }
        var rollInput = scenemanger.roll;
        rb.AddForce(transform.right * rollInput);
        if (System.Math.Abs(rollInput) > 0)
        {

            receivingInput = true;
        }

        var yawInput = scenemanger.yaw;
        rb.AddTorque(transform.up * yawInput);
        if (System.Math.Abs(yawInput) > 0)
        {

            receivingInput = true;
        }

        if (receivingInput & rb.drag != inputDrag)
        {
            rb.drag = inputDrag;
            rb.angularDrag = inputDrag;
        }
        else if (!receivingInput & rb.drag != drag)
        {
            rb.drag = drag;
            rb.angularDrag = drag * .9f;
        }

        //AddReward(rewards())

    }
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[3] = Input.GetAxis("Keyboard Yaw");
        actionsOut[0] = Input.GetAxis("Keyboard Elv");
        actionsOut[1] = Input.GetAxis("Keyboard Roll");
        actionsOut[2] = Input.GetAxis("Keyboard Pitch");
        Debug.Log(actionsOut[1]);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("target"))
        {

            EndEpisode();

        }

    }
}
