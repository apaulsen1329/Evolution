using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour {
    public int population = 10;
    public int pantry = 20;
    public Transform agentFab;
    public GameObject foodFab;
    public float speedDelta = 1f; // the amount of change in speed each generation can mutate by
    public float senseDelta = 1f; // the amount of change in sense distance each generation can mutate by
    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < population; i++) {
            var creature = Instantiate(agentFab);
            var axis = Random.Range(1, 100);
            if (axis >= 50) { 
                var j = Random.Range(1, 100);
                var x1 = 0;
                if (j >=50) {
                    x1 = 20;
                } else {
                    x1 = -20;
                }
                var z1 = Random.Range(-19, 19);
                Vector3 spawn = new Vector3(x1, 1, z1);
                creature.position = spawn;
            } else { 
                var k = Random.Range(1, 100);
                var z2 = 0;
                if (k >= 50) {
                    z2 = 20;
                } else {
                    z2 = -20;
                }
                var x2 = Random.Range(19, -19);
                Vector3 spawn = new Vector3(x2, 1, z2);
                creature.position = spawn;
            }
        }

        dropFood(pantry);
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            // Debug.Log("Space Pressed");
            //Find all agents with 2 or more meals
            var allAgents = FindObjectsOfType<AgentControl>();

            foreach (var agent in allAgents) {
                // Debug.Log(agent);
                agent.stamina = 100f;
                agent.wandering = true;

                if (agent.meals >= 2) {
                    var newAgent = Instantiate(agent);
                    newAgent.meals = 0;
                    //update speed
                    var newSpeed = agent.agent.speed + Random.Range(-speedDelta, speedDelta);
                    //update sense
                    var newSense = agent.senseDistance + Random.Range(-senseDelta, senseDelta);
                    if (newSpeed < 0) newSpeed = 0;
                    if (newSense < 0) newSense = 0;
                    newAgent.agent.speed = newSpeed;
                    newAgent.senseDistance = newSense;
                    // update scale
                    //newAgent.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
                    // update color
                }
                agent.meals = 0;
            }

            dropFood(pantry);
        }
    }

    void dropFood(int amount) {
        for (int i = 0; i < amount; i++) {
            var food = Instantiate(foodFab);
            var x = Random.Range(-19, 19);
            var z = Random.Range(-19, 19);
            
            Vector3 spawn = new Vector3(x, 0.25f, z);
            food.transform.position = spawn;
        }
    }
}