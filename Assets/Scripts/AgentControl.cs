using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentControl : MonoBehaviour {
    public NavMeshAgent agent;
    public GameObject nearestFood;
    public float stamina = 100f;
    public bool wandering = true;
    public int meals = 0;
    public float senseDistance = 3f;
    

    // Start is called before the first frame update
    void Start() {
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(randPosition(1));
    }

    public Vector3 randPosition (float height) {
        var x = Random.Range(-19, 19);
        var z = Random.Range(-19, 19);
            
        Vector3 position = new Vector3(x, height, z);

        return position;
    }

    // Update is called once per frame
    void Update() {
        if (wandering) {
            nearestFood = findNearestFood(agent.transform.position, senseDistance);
            if (nearestFood) {
                agent.SetDestination(nearestFood.transform.position);
                wandering = false;
            }
        }

        if (stamina <= 0 && meals < 1) {
            //kill them
            agent.gameObject.SetActive(false);
        } else {
             if (wandering == true) {
                //They have a chance to change direction
                var chance = Random.Range(1.0f, 100.0f);
                if (chance < 1.5f) {
                    agent.SetDestination(randPosition(1));
                }
            }

            if (agent.pathStatus == NavMeshPathStatus.PathComplete) {
                wandering = true;
            }

            //Update Stamina
            stamina -= 0.1f;
        }
        if (meals >= 1 && stamina < 50f) {
            goHome();
        }
    }

    GameObject findNearestFood (Vector3 center, float radius) {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        GameObject nearestFood = null;
        float shortestDist = 9999.9f;
        float dist;
        foreach (var hitCollider in hitColliders) {
            if (hitCollider.gameObject.CompareTag("Food")) {
                dist = Vector3.Distance(center, hitCollider.transform.position);
                if (dist < shortestDist) {
                    shortestDist = dist;
                    nearestFood = hitCollider.gameObject;
                }
            }
        }
        return nearestFood;
    }

    void goHome() {
        wandering = false;
        //find closest edge
        var absx = Mathf.Abs(agent.transform.position.x);
        var absz = Mathf.Abs(agent.transform.position.z);
        if (absx > absz) {
            if (agent.transform.position.x > 0) {
                agent.SetDestination(new Vector3(20, 1, agent.transform.position.z));
            } else {
               agent.SetDestination(new Vector3(-20, 1, agent.transform.position.z));
            }
        } else {
            if (agent.transform.position.z > 0) {
                agent.SetDestination(new Vector3(agent.transform.position.x, 1, 20));
            } else {
               agent.SetDestination(new Vector3(agent.transform.position.x, 1, -20));
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Food")) {
            Destroy(other.gameObject);
            meals++;
            wandering = true;
        }
    }
}