using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities; // Use Entities package

public class Testing : MonoBehaviour {

    private void Start() {

        EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager; // 1. Define "manager" variable of type Entity Manager to be a new Entity Manager for this script.

        /* 2. a. Create a new Entity. (manager.CreateEntity();)
         * +  b. Create the Entity with a component of type LevelComponent (.CreateEntity(typeof(LevelComponent))
         * +  c. Create a reference to the entity ( Entity entity = ... )
        */
        Entity entity = manager.CreateEntity(typeof(LevelComponent)); 

        /*Use the manager to set the component data of LevelComponent to be level 10 for the entity declared above*/
        manager.SetComponentData(entity, new LevelComponent { level = 10 });

    }

}