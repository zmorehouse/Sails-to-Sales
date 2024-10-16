using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class MonkeyController : MonoBehaviour
{
    private bool hasFood = false;
    private bool tick = false;
    private GameObject food;
    public SnakeController player;
    private Vector3 centre = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {   
        if (player.onBeat)
        {
            if (hasFood)
            {
                // pathfind to centre of island and drop off food
                float dist = Vector3.Distance(centre, transform.position);
                if (dist < 2)
                {
                    hasFood = false;
                }
                else
                {
                    Vector3 dir = (Vector3)Vector3Int.RoundToInt((centre - transform.position) / dist);
                    move(dir);
                }
            }
            else
            {
                // determine closest item from player inventory
                if (player.inventory.Count > 0)
                {
                    GameObject closest = player.inventory[0];
                    float closestDist = Vector3.Distance(closest.transform.position, transform.position);
                    foreach (GameObject food in player.inventory)
                    {
                        Vector3 pos = food.transform.position;
                        float dist = Vector3.Distance(pos, transform.position);
                        if (dist < closestDist)
                        {
                            closestDist = dist;
                            closest = food;
                        }
                    }
                    // rotate monkey towards closest food
                    Vector3 dir = (Vector3) Vector3Int.RoundToInt((closest.transform.position - transform.position) / closestDist);
                    move(dir);

                }
            }
        }
    }

    void move(Vector3 movement)
    {
        Vector3 prev = transform.position;
        transform.position += movement;
        if (hasFood)
        {
            food.transform.position = prev;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!hasFood && player.inventory.Contains(other.gameObject) && other.tag == "Food")
        {
            //remove food from player inv
            bool beginShift = false;
            Vector3 temp;
            Vector3 prev = other.gameObject.transform.position;
            // shift all items folowing stolen item
            foreach (GameObject item in player.inventory) 
            {
                if (beginShift)
                {
                    temp = item.transform.position;
                    item.transform.position = prev;
                    prev = temp;
                }
                if (item == other.gameObject)
                {
                    beginShift = true;
                }
            }
            // remove item from inventory
            player.inventory.Remove(other.gameObject);

            food = other.gameObject;
            hasFood = true;
        }
        // what other collisions are we concerned with?
    }

}
