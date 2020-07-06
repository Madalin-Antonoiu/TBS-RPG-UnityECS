// Testing.cs

using UnityEngine; // import Engine ( for MonoBehavior and Random)
using Unity.Entities; // import Entities
using Unity.Transforms; // import Transforms (for Translation component)
using Unity.Collections; // import Collections (for NativeArray)

public class Testing : MonoBehaviour {

    private void Start() {

        // 1. Define "manager" 
        EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;

        //2. Create an "archetype" with the manager.
        EntityArchetype archetype = manager.CreateArchetype( // 
            typeof(LevelComponent),
            typeof(Translation) // premade component, found in Unity.Transforms.
        );

        // 3. Create "entities array" with the manager and archetype.
        /* a. Create a Native Array to hold Entities and name it entArray - "NativeArray<Entity> entArray" .
        /* b. Instantiate the array with a size and a temporary allocation, 
        /* we will destroy it after intatiating the entities .
        */
        NativeArray<Entity> entArray = new NativeArray<Entity>(5000, Allocator.Temp);
        manager.CreateEntity(archetype, entArray); // All the created entities are passed into entArray here

        // All is left is to cycle through entArray
        for (int i = 0; i < entArray.Length; i++) {
            // + Now moved up inside the cycle 
            Entity entity = entArray[i]; // Instantiate entity to be each element of the array
            manager.SetComponentData(entity, new LevelComponent { level = Random.Range(10, 20) }); // set each level to Random between 10 and 20
        }

        // SUPER IMPORTANT. Dispose of our  temporary entArray (Allocator.Temp, remember?)
        entArray.Dispose();


        // That`s it. We should now be creating 2 Entities with the 2 component archetype, each of them with random level.

    }

}