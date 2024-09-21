using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackagePool : MonoBehaviour
{
    public GameObject packagePrefab; // Reference to the package prefab
    public int poolSize = 10; // Number of packages in the pool

    private Queue<GameObject> packageQueue = new Queue<GameObject>();

    void Start()
    {
        // Create and deactivate the initial pool of packages
        for (int i = 0; i < poolSize; i++)
        {
            GameObject package = Instantiate(packagePrefab);
            package.SetActive(false);
            packageQueue.Enqueue(package);
        }
    }

    public GameObject GetPackage()
    {
        if (packageQueue.Count > 0)
        {
            GameObject package = packageQueue.Dequeue();
            package.SetActive(true);
            return package;
        }
        else
        {
            // Optionally handle the case where pool is exhausted
            return null;
        }
    }

    public void ReturnPackage(GameObject package)
    {
        package.SetActive(false);
        packageQueue.Enqueue(package);
    }
}
