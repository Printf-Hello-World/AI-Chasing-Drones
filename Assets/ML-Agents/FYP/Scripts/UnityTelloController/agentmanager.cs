using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.Serialization;


[System.Serializable]
public class Dronestate
{
    public int playerIndex;
    public Vector3 startingPos;
    public MultiDrone multidronescript;
    
}

public class agentmanager : MonoBehaviour
{
 
    //public GameObject exit;
    public targetscript tgt;
    public List<Dronestate> dronestates = new List<Dronestate>();
    private Vector3 initialexit;
    //public void Start()
    //{
    //    initialexit = exit.transform.localPosition;
    //}
    private void reset()
    {
        //exit.transform.localPosition = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.5f, 1f), initialexit.z);\
        //tgt.spawn();
        
    }
    public void crash1()
    {
        foreach (var ps in dronestates)
        {
            if (ps.multidronescript.team == MultiDrone.Team.Chaser)
            {
                ps.multidronescript.AddReward(-3f);
            }   
            ps.multidronescript.EndEpisode();
            reset();
            //Debug.Log("chaser crashed!");
        }
    }
    public void hit1()
    {
        foreach (var ps in dronestates)
        {
            if (ps.multidronescript.team == MultiDrone.Team.Chaser)
            {
                ps.multidronescript.AddReward(+5f);
            }
            if (ps.multidronescript.team == MultiDrone.Team.Runner)
            {
                ps.multidronescript.AddReward(-1f);
            }
            ps.multidronescript.EndEpisode();
            //Debug.Log("gotcha");
            reset();

        }
    }
    public void escaped()
    {
        foreach (var ps in dronestates)
        {
            if (ps.multidronescript.team == MultiDrone.Team.Chaser)
            {
                ps.multidronescript.AddReward(-2f);
            }
            if (ps.multidronescript.team == MultiDrone.Team.Runner)
            {
                ps.multidronescript.AddReward(+2f);
            }
            ps.multidronescript.EndEpisode();
            //Debug.Log("escaped!");
            reset();

        }
    }
    public void crash2()
    {
        foreach (var ps in dronestates)
        {
            if (ps.multidronescript.team == MultiDrone.Team.Runner)
            {
                ps.multidronescript.AddReward(-1f);
            }
            ps.multidronescript.EndEpisode();
            //Debug.Log("target crashed!");
            reset();
        }
    }
}
