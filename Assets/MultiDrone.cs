using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

public class MultiDrone : Agent
{
    Rigidbody rigidBody;
    public float inputDrag, drag;
    public Camera followCam;
    public float speed;
    public float rawYaw, rawElv, rawRoll, rawPitch;
    public float yaw, elv, roll, pitch;
    //public Transform target;
    private float orgdistance;
    private float currentdistance;
    private float currentdistance1;
    private float improvement;
    private float distmultiplier = 10f;
    private Vector3 directiontotarget;
    private Vector3 initialspawn;
    private Quaternion initialrot;
    public targetscript tgt;
    public agentmanager manager;

    int m_PlayerIndex;
    BehaviorParameters m_BehaviorParameters;
    [HideInInspector]
    public Team team;
    private float rw;
    private float maxstep;

    public enum Team
    {
        Chaser = 0,
        Runner = 1
    }

    public void Start()
    {
        initialspawn = this.transform.localPosition;
        initialrot = this.transform.localRotation;
    }


    public override void Initialize()
    {
        m_BehaviorParameters = gameObject.GetComponent<BehaviorParameters>();
        if (m_BehaviorParameters.TeamId == (int)Team.Chaser)
        {
            team = Team.Chaser;
            initialspawn = this.transform.localPosition;
            initialrot = this.transform.localRotation;
        }
        else
        {
            team = Team.Runner;
            initialspawn = this.transform.localPosition;
            initialrot = this.transform.localRotation;
        }
        var dronestate = new Dronestate
        {
            startingPos = transform.position,
            multidronescript = this,
        };
        manager.dronestates.Add(dronestate);
        m_PlayerIndex = manager.dronestates.IndexOf(dronestate);
        dronestate.playerIndex = m_PlayerIndex;

    }

    public void TakeOff()
    {
        this.transform.localRotation = initialrot;
        //single drone
        this.transform.localPosition = initialspawn + Random.insideUnitSphere * 0.0f;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        //sceneManager.flightStatus = SceneManager.FlightStatus.Flying;
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
        
        if (team == Team.Chaser)
        {
            AddReward(rewards());
            AddReward(-1.0f / this.MaxStep); //max step is 10k 
        }
        else
        {
            AddReward(+1.0f / this.MaxStep);

        }

    }

    public override void OnEpisodeBegin()
    {
        rigidBody = GetComponent<Rigidbody>();
        TakeOff();
        rigidBody.inertiaTensor = new Vector3(1, 1, 1);
        this.rigidBody.angularVelocity = Vector3.zero;
        this.rigidBody.velocity = Vector3.zero;
        //orgdistance = Vector3.Distance(this.transform.localPosition, target.localPosition);
        //tgt.spawn();
        rw = 0;

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
        //currentdistance = Vector3.Distance(this.transform.localPosition, target.localPosition);
        //directiontotarget = target.transform.localPosition - this.transform.localPosition;

    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[3] = Input.GetAxis("Keyboard Yaw");
        actionsOut[0] = Input.GetAxis("Keyboard Elv");
        actionsOut[1] = Input.GetAxis("Keyboard Roll");
        actionsOut[2] = Input.GetAxis("Keyboard Pitch"); 

    }

    private void OnTriggerEnter(Collider other)
    {   
        if (team == Team.Chaser)
        {
            if (other.gameObject.CompareTag("wall") || other.gameObject.CompareTag("agent"))
            {
                manager.crash1();
            }
            else if (other.gameObject.CompareTag("target"))
            {

                manager.hit1();

            }
        }
        else if (team == Team.Runner)
        {
            if (other.gameObject.CompareTag("wall") || other.gameObject.CompareTag("target"))
            {
                manager.crash2();
            }
            else if (other.gameObject.CompareTag("exit"))
            {
                manager.escaped();
            }
        }

    }

    private float rewards()
    {
        float totalreward = 0f;
        Vector3 heading;
        Vector3 correctheading;
        float headingdiff;
        heading = this.transform.forward;
        correctheading = Vector3.ProjectOnPlane(directiontotarget, new Vector3(0f, 1f, 0f));
        headingdiff = Vector3.Angle(new Vector3(heading.x, 0f, heading.z), new Vector3(correctheading.x, 0f, correctheading.z)) / 180f;
        totalreward = -headingdiff * 0.03f;

        return totalreward;
    }
}


