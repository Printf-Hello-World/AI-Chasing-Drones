using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetsingledrone : MonoBehaviour
{
    private Vector3 underdesk;
    private Vector3 behindcouch;
    private Vector3 belowlamp;
    private Vector3 belowcupboard;
    private Vector3 everywhereelse;
    private Vector3 centerpoint;
    private Vector3 spawnposition;
    private Vector3 abovecupboard;
    private int number;
    private float random;

    private void Start()
    {
        underdesk = new Vector3(1.421f, -0.5f, 2.808f);
        behindcouch = new Vector3(-1.04f, 0f, 2.926f);
        belowcupboard = new Vector3(3.279f, 0.091f, 3.059f);
        belowlamp = new Vector3(2.822f, 0.5f, 3.85f);
        centerpoint = this.transform.localPosition; //(1, 0.8, 0.5) is the forward working on set this in inspector
        abovecupboard = new Vector3(3.279f, 1f, 3.059f);
    }

    public void spawn()
    {
        random = Random.Range(0f, 1f);

        if (random < 0.99999f)
        {
            spawnposition = centerpoint + Random.insideUnitSphere * 0.5f;//use 0.1f for working model , was 0.5f for tello3 hover
            if (Physics.CheckSphere(spawnposition, 0.3f))
            {

                spawn();
            }
            else
            {
                this.transform.localPosition = spawnposition;
            }
        }
        else
        {
            number = Random.Range(1, 6);
            switch (number)
            {
                case 1:
                    this.transform.localPosition = underdesk;
                    break;
                case 2:
                    this.transform.localPosition = behindcouch;
                    break;
                case 3:
                    this.transform.localPosition = belowcupboard;
                    break;
                case 4:
                    this.transform.localPosition = belowlamp;
                    break;
                case 5:
                    this.transform.localPosition = abovecupboard;
                    break;
            }

        }

    }
    //void OnDrawGizmosSelected()
    //{
    //    // Draw a yellow sphere at the transform's position
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(transform.position, 1.5f);
    //}

}
