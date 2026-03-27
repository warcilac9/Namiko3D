using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyDestSingleton
{
    private static EnemyDestSingleton instance;
    private List<GameObject> checkpoints = new List<GameObject>();
    public List<GameObject> Checkpoints
    {
        get
        {
            EnsureCheckpoints();
            return checkpoints;
        }
    }

    public static EnemyDestSingleton Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyDestSingleton();
            }

            instance.EnsureCheckpoints();
            return instance;
        }
    }

    private void EnsureCheckpoints()
    {
        checkpoints.RemoveAll(checkpoint => checkpoint == null);

        if (checkpoints.Count > 0)
        {
            return;
        }

        checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
    }
}
