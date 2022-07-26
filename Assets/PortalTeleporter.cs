using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PortalTeleporter : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private GameObject player;
    public GameObject receiver;

    private bool playerIsOverlapping = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("MainPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("MainPlayer");
        }
        else
        {
            Debug.Log(player.transform);
            if(playerIsOverlapping)
            {
                Vector3 portalToPlayer = player.transform.position - transform.position;
                float dotProduct = Vector3.Dot(transform.up, portalToPlayer);
                //Player moved across the portal

                    float rotationDifference = -Quaternion.Angle(transform.rotation, receiver.transform.rotation);
                    rotationDifference += 180;
                    player.transform.Rotate(Vector3.up, rotationDifference);

                    Vector3 positionOffset = Quaternion.Euler(0f, rotationDifference, 0f) * portalToPlayer;
                    Vector3 pos = receiver.transform.Find("Spawn").position;
                    player.GetComponent<CharacterController>().enabled = false;
                    player.transform.position = pos;
                    player.GetComponent<CharacterController>().enabled = true;
                    Debug.Log("New Position:" + player.transform.position);
                    playerIsOverlapping = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("MainPlayer"))
        {
            Debug.Log("Old Position: " + player.transform.position);
            playerIsOverlapping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainPlayer"))
        {
            playerIsOverlapping = false;
        }
    }
}
