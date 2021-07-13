using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {
    public FlockManager manager;
    float speed;
    bool turning = false;

    // Start is called before the first frame update
    void Start() {
        speed = Random.Range(manager.minSpeed, manager.maxSpeed);
    }

    // Update is called once per frame
    void Update() {
        // bounding box of the manager cube
        Bounds boundingBox = new Bounds(manager.transform.position, manager.swimLimits * 2);
        RaycastHit hit = new RaycastHit();
        Vector3 direction = Vector3.zero;

        // if fish is outside the cube or about to hit something then start turning around
        if (!boundingBox.Contains(transform.position)) {
            turning = true;
            direction = manager.transform.position - transform.position;
        } else if (Physics.Raycast(transform.position, transform.forward * 50, out hit)) {
            turning = true;
            direction = Vector3.Reflect(transform.forward, hit.normal);
            Debug.DrawRay(transform.position, transform.forward * 50, Color.red);
        } else turning = false;

        if (turning) {
            // turn towards the center of manager cube or reflect
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(direction),
                                                  manager.rotationSpeed * Time.deltaTime);
        } else {
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(manager.minSpeed, manager.maxSpeed);
            if (Random.Range(0, 100) < 20)
                ApplyRules(); // random applies for optimization
        }

        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules() {
        GameObject[] cluster = manager.fishes;

        Vector3 vCenter = Vector3.zero; // average center vector
        Vector3 vAvoid = Vector3.zero; // average avoid vector
        float globalSpeed = 0.01f;
        float nDistance; // neighbour distance
        int groupSize = 0; // size of a current sub-cluster

        foreach (GameObject fish in cluster) {
            if (fish != this.gameObject) {
                nDistance = Vector3.Distance(fish.transform.position, this.transform.position);
                if (nDistance <= manager.neighbourDistance) {
                    vCenter += fish.transform.position;
                    groupSize++;
                    if (nDistance < manager.thresholdDistance) {
                        vAvoid += this.transform.position - fish.transform.position;
                    }
                    globalSpeed += fish.GetComponent<Flock>().speed;
                }
            }
        }

        // update group properties
        if (groupSize > 0) {
            vCenter = vCenter / groupSize + manager.goalPos - this.transform.position;
            speed = globalSpeed / groupSize;

            Vector3 direction = vCenter + vAvoid - transform.position;
            if (direction != Vector3.zero) {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(direction),
                                                      manager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
