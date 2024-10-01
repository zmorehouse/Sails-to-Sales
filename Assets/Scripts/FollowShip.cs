// A script used to control camera movement

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShip : MonoBehaviour
{
    public GameObject ship;

    void LateUpdate()
    {
        transform.position = ship.transform.position + new Vector3(0, 25, 0);
    }
}
