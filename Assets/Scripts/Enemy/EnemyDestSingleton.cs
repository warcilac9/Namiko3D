using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyDestSingleton
{
    private static EnemyDestSingleton instance;
    private List<GameObject> checkpoints = new List<GameObject>();
    public List<GameObject> Checkpoints {get {return checkpoints;}}

    public static EnemyDestSingleton Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyDestSingleton();
                instance.Checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
            }
            return instance;
        }
    }
}
