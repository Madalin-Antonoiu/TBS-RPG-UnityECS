using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities; // Use Entities package

public class Testing : MonoBehaviour {

    
    private void Start() {

        EntityManager manager = new EntityManager(); // 1. Define "manager" variable of type Entity Manager to be a new Entity Manager for this script.
        manager.CreateEntity();                      // 2. Create a new Entity.

    }

}