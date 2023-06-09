using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    List<GameObject> checkpoints;
    int currIndex;

    // Start is called before the first frame update
    void Start()
    {
        checkpoints = new List<GameObject>();

        for (int i = 0; i < transform.childCount; ++i)
        {
            checkpoints.Add(transform.GetChild(i).gameObject);
        }

        if (checkpoints.Count != 0)
            LevelManager.current.lastCheckpoint = 0;
    }

    public void UpdateCheckpoint(GameObject newCP) {
        try
        {
            LevelManager.current.lastCheckpoint = checkpoints.IndexOf(newCP);
            for (int i = 0; i < newCP.transform.childCount; i++) {
                newCP.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        catch
        {
            return;
        }
    }

    public Vector3 GetLastCheckpointPos() {
        return checkpoints[LevelManager.current.lastCheckpoint].transform.position;
    }

    public GameObject GetLastCheckpoint() {
        return checkpoints[LevelManager.current.lastCheckpoint];
    }

    public GameObject GetIndexedCheckpoint(int index) {
        return checkpoints[index];
    }
}
