// A script used to control camera movement

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShipTutorial : MonoBehaviour
{
    public GameObject tutorialship;

    void LateUpdate()
    {
        transform.position = tutorialship.transform.position + new Vector3(0, 25, 0);
    }
}
