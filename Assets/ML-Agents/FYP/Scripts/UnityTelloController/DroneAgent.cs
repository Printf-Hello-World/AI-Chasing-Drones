using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

public class DroneAgent : Agent
{
    Rigidbody rigidBody;
    public float inputDrag, drag;
    public Camera followCam;
    public float speed;
    public float rawYaw, rawElv, rawRoll, rawPitch;
    private float yaw, elv, roll, pitch;
    public Transform target;
    private float orgdistance;
    private float currentdistance;
    private float currentdistance1;
    private float improvement;
    private float distmultiplier = 10f;
    private Vector3 directiontotarget;
    private Vector3 initialspawn;
    public targetsingledrone tgt;
    public bool hit;
    public bool end;
    


    public void Start()
    {
        initialspawn = this.transform.localPosition;
        rigidBody = GetComponent<Rigidbody>();
    }

    public void TakeOff()
    {
        this.transform.localRotation = Quaternion.identity;
        //single drone
        this.transform.localPosition = initialspawn; //+ Random.insideUnitSphere * 0.25f;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
    }


    public override void OnActionReceived(float[] vectorAction)
    {

        rawElv = vectorAction[0];
        rawRoll = vectorAction[1];
        rawPitch = vectorAction[2];
        rawYaw = vectorAction[3];

        if (speed == 0)
        {
            speed = .5f;
        }
        else if (speed < 0)
        {
            speed = 1 + speed;
            speed /= 2;
        }
        else
        {
            speed /= 2;
            speed += .5f;
        }
        elv = rawElv * speed;
        roll = rawRoll * speed;
        pitch = rawPitch * speed;
        yaw = rawYaw * speed;
        //Debug.Log("Up/Down inputs: " + elv);
        //Debug.Log("Yaw inputs: " + yaw);
        //Debug.Log("Forward/Backward inputs: " + pitch);
        //Debug.Log("Left/Right inputs: " + roll);
        //Debug.Log(roll);

        //if (sceneManager.flightStatus == SceneManager.FlightStatus.Flying)
        //{
        //    rigidBody.AddForce(transform.up * 9.81f);
        //    bool receivingInput = false;
        //    var pitchInput = pitch;
        //    rigidBody.AddForce(transform.forward * pitchInput);
        //    if (System.Math.Abs(pitchInput) > 0)
        //    {
        //        receivingInput = true;
        //    }
        //    var elvInput = elv;
        //    rigidBody.AddForce(transform.up * elvInput);
        //    if (System.Math.Abs(elvInput) > 0)
        //    {
        //        receivingInput = true;

        //    }
        //    var rollInput = roll;
        //    rigidBody.AddForce(transform.right * rollInput);
        //    if (System.Math.Abs(rollInput) > 0)
        //    {

        //        receivingInput = true;
        //    }

        //    var yawInput = yaw;
        //    rigidBody.AddTorque(transform.up * yawInput);
        //    if (System.Math.Abs(yawInput) > 0)
        //    {

        //        receivingInput = true;
        //    }

        //    if (receivingInput & rigidBody.drag != inputDrag)
        //    {
        //        rigidBody.drag = inputDrag;
        //        rigidBody.angularDrag = inputDrag;
        //    }
        //    else if (!receivingInput & rigidBody.drag != drag)
        //    {
        //        rigidBody.drag = drag;
        //        rigidBody.angularDrag = drag * .9f;
        //    }
        //    AddReward(-1 / this.MaxStep);

        //}

        rigidBody.AddForce(transform.up * 9.81f);
        bool receivingInput = false;
        var pitchInput = pitch;
        rigidBody.AddForce(transform.forward * pitchInput);
        if (System.Math.Abs(pitchInput) > 0)
        {
            receivingInput = true;
        }
        var elvInput = elv;
        rigidBody.AddForce(transform.up * elvInput);
        if (System.Math.Abs(elvInput) > 0)
        {
            receivingInput = true;

        }
        var rollInput = roll;
        rigidBody.AddForce(transform.right * rollInput);
        if (System.Math.Abs(rollInput) > 0)
        {

            receivingInput = true;
        }

        var yawInput = yaw;
        rigidBody.AddTorque(transform.up * yawInput);
        if (System.Math.Abs(yawInput) > 0)
        {

            receivingInput = true;
        }

        if (receivingInput & rigidBody.drag != inputDrag)
        {
            rigidBody.drag = inputDrag;
            rigidBody.angularDrag = inputDrag;
        }
        else if (!receivingInput & rigidBody.drag != drag)
        {
            rigidBody.drag = drag;
            rigidBody.angularDrag = drag * .9f;
        }


        //AddReward(-3 / this.MaxStep);
        AddReward(rewards());

        if (this.StepCount == this.MaxStep - 50)
        {
            //end = true;
        }

        
    }

    public override void OnEpisodeBegin()
    {
        //TakeOff();
        rigidBody.inertiaTensor = new Vector3(1, 1, 1);
        this.rigidBody.angularVelocity = Vector3.zero;
        this.rigidBody.velocity = Vector3.zero;
        orgdistance = Vector3.Distance(transform.localPosition, target.localPosition);
        tgt.spawn();

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition); //3
        sensor.AddObservation(this.transform.localRotation); //4
        sensor.AddObservation(this.transform.forward.normalized); //3
        sensor.AddObservation(elv);
        sensor.AddObservation(roll);
        sensor.AddObservation(yaw);
        sensor.AddObservation(pitch);
        sensor.AddObservation(this.rigidBody.velocity.x);
        sensor.AddObservation(this.rigidBody.velocity.y);
        sensor.AddObservation(this.rigidBody.velocity.z);
        Debug.DrawRay(this.transform.position, this.transform.forward, Color.green);
        currentdistance = Vector3.Distance(this.transform.localPosition, target.localPosition);
        directiontotarget = target.transform.localPosition - this.transform.localPosition;
        //Debug.Log(this.rigidBody.velocity);

    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[3] = Input.GetAxis("Keyboard Yaw");
        actionsOut[0] = Input.GetAxis("Keyboard Elv");
        actionsOut[1] = Input.GetAxis("Keyboard Roll");
        actionsOut[2] = Input.GetAxis("Keyboard Pitch");
        //Debug.Log(actionsOut[1]);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("wall"))
        {
            AddReward(-1.0f);
            EndEpisode();
        }
        if (other.gameObject.CompareTag("target"))
        {

            AddReward(1.0f);
            //hit = true;
            EndEpisode();
            //other.gameObject.SetActive(false);

        }
        if (other.gameObject.CompareTag("agent"))
        {

            AddReward(-2.0f);
            EndEpisode();

        }
    }

    private float rewards()
    {
        float totalreward = 0f;
        Vector3 heading;
        Vector3 correctheading;
        float headingdiff;
        heading = this.transform.forward;
        currentdistance1 = currentdistance / orgdistance;
        //orgdistance = currentdistance;
        correctheading = Vector3.ProjectOnPlane(directiontotarget, new Vector3(0f, 1f, 0f));
        headingdiff = Vector3.Angle(new Vector3(heading.x, 0f, heading.z), new Vector3(correctheading.x, 0f, correctheading.z)) / 180f;
        //Debug.Log(headingdiff);
        totalreward = -headingdiff * 0.01f - currentdistance1*0.1f;
        return totalreward;
    }
}


