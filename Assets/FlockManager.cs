using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour {
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] fishes;
    public Vector3 swimLimits = new Vector3(5, 5, 5); // radius = 5
    public Vector3 goalPos;

    [Header("Fish Settings")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    [Range(0.5f, 1.0f)]
    public float thresholdDistance; // min distance to triger avoid each other
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start() {
        fishes = new GameObject[numFish];

        for (int i = 0; i < numFish; i++) {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                                Random.Range(-swimLimits.y, swimLimits.y),
                                                                Random.Range(-swimLimits.z, swimLimits.z));
            fishes[i] = (GameObject) Instantiate(fishPrefab, pos, Quaternion.identity);
            fishes[i].GetComponent<Flock>().manager = this;
        }

        goalPos = this.transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (Random.Range(0, 100) < 10)
            goalPos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                            Random.Range(-swimLimits.y, swimLimits.y),
                                                            Random.Range(-swimLimits.z, swimLimits.z));
    }
}
