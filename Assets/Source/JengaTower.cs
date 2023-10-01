using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

// Enum representing grade levels
public enum Grade
{
    sixth = 6,
    seventh = 7,
    eighth = 8
}

// A serializable list class
[System.Serializable]
public class SerializableList<T>
{
    public List<T> list;
}

// Main class for the JengaTower
public class JengaTower : MonoBehaviour
{
    [Header("API DATA")]
    // API URL for data retrieval
    public string URL;

    [Header("General")]
    // Grade of the Jenga tower (enum)
    public Grade grade;
    // Reference to the prefab for bricks
    public GameObject brickGO = null;

    // Offset configuration for positioning bricks
    [Header("Offsets")]
    public int bricksPerLevel = 3;
    public Vector3 heightOffset = new Vector3(0, 0.5f, 0);
    public List<Vector3> positionOffsets;
    public List<Vector3> rotationOffsets;

    // Private list to keep track of bricks in the tower
    private List<JengaBrickComponent> _bricks;

    // Initialization at the start of the game
    void Start()
    {
        _bricks = new List<JengaBrickComponent>();

        // Disable the MeshRenderer of the tower at the beginning
        GetComponent<MeshRenderer>().enabled = false;

        // Get data from the API
        StartCoroutine(FetchData());
    }

    // Update logic per frame
    void Update()
    {
        // Enable physics for bricks when 'T' key is pressed
        if (Input.GetKeyDown(KeyCode.T))
        {
            for (int i = 0; i < _bricks.Count; i++)
            {
                if (_bricks[i]._brickData.mastery == 0)
                    Destroy(_bricks[i].gameObject);
                else
                    _bricks[i].rb.isKinematic = false;
            }
        }
    }

    // Method to spawn a brick in the tower
    void SpawnBrick(BrickData brickData, int brickIndex)
    {
        GameObject brick = Instantiate(brickGO, transform);

        int level = brickIndex / bricksPerLevel;
        int offset = brickIndex % positionOffsets.Count;

        brick.transform.localPosition = positionOffsets[offset] + heightOffset * level;
        brick.transform.rotation = Quaternion.Euler(rotationOffsets[offset]);

        JengaBrickComponent brickComponent = brick.GetComponent<JengaBrickComponent>();
        brickComponent._brickData = brickData;

        brickComponent.UpdateMastery();

        _bricks.Add(brickComponent);
    }

    // Method to make an HTTP request to the API and process the data
    public IEnumerator FetchData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                // Call the FillData method to process and fill the data
                FillData(request.downloadHandler.text);
            }
        }
    }

    // Method to fill data after fetching from the API
    private void FillData(string jsonString)
    {
        // Parse JSON data and sort it
        SerializableList<BrickData> bricksData = JsonUtility.FromJson<SerializableList<BrickData>>(
            "{\"list\": " + jsonString + "}");

        bricksData.list.Sort();

        int j = 0;
        for (int i = 0; i < bricksData.list.Count; i++)
        {
            // Filter bricks based on the specified grade and spawn them in the tower
            if ((int)grade == (int)Char.GetNumericValue(bricksData.list[i].grade[0]))
            {
                SpawnBrick(bricksData.list[i], j);
                j++;
            }
        }
    }
}