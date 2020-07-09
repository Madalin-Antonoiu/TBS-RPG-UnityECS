using UnityEngine;
using Unity.Entities;

public class Testing1 : MonoBehaviour {

    private void Start() {

        // Create a simple entity
        EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entity entity = manager.CreateEntity();

        // Add our Buffer element 
        manager.AddBuffer<IntBufferElement>(entity);
        DynamicBuffer<IntBufferElement> dynamicBuffer = manager.GetBuffer<IntBufferElement>(entity);

        //Add elements to the dynamic buffer
        dynamicBuffer.Add(new IntBufferElement { Value = 1 });
        dynamicBuffer.Add(new IntBufferElement { Value = 2 });
        dynamicBuffer.Add(new IntBufferElement { Value = 3 });

        // Reinterpret it so you can directly work with int ( only if you have a single Value)
        DynamicBuffer<int> intDynamicBuffer = dynamicBuffer.Reinterpret<int>();
        intDynamicBuffer[1] = 5;


    }

}