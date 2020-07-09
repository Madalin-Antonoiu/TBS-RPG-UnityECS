// Testing.cs

using UnityEngine; // import Engine ( for MonoBehavior and Random)
using Unity.Entities; // import Entities
using Unity.Transforms; // import Transforms (for Translation component)
using Unity.Collections; // import Collections (for NativeArray)
using Unity.Rendering; // import Rendering (for RenderMesh) +new
using Unity.Mathematics;

public class Testing : MonoBehaviour {

    // + new define a Unity editable field mesh to add a mesh (Drag them in the editor)
    [SerializeField] private Mesh unitMesh;
    [SerializeField] private Material unitMaterial;

    private void Start() {

        // 1. Define "manager" 
        EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;

        //2. Create an "archetype" with the manager.
        EntityArchetype archetype = manager.CreateArchetype( 
            typeof(LevelComponent),
            typeof(Translation),

            typeof(RenderBounds), // + absolute must, without this no rendering happens
            typeof(RenderMesh), // render mesh +new
            typeof(LocalToWorld), // view it +new

            typeof(MoveSpeedComponent) // ++
        );

        // 3. Create "entities array" with the manager and archetype.
        NativeArray<Entity> entArray = new NativeArray<Entity>(10000, Allocator.Temp);
        manager.CreateEntity(archetype, entArray); 

        // 4. Set each component data
        for (int i = 0; i < entArray.Length; i++) {
            Entity entity = entArray[i]; 

            // Setting data for each component
            manager.SetComponentData(entity, new LevelComponent { 
                level = UnityEngine.Random.Range(10, 20) 
            });

            manager.SetComponentData(entity, new MoveSpeedComponent { 
                moveSpeed = UnityEngine.Random.Range(2f, 3f)
            });

            manager.SetComponentData(entity, new Translation { 
                Value = new float3(UnityEngine.Random.Range(-8, 8f), UnityEngine.Random.Range(-5, 5f), 0)
            });

            // +new Set the Mesh for each entity
            manager.SetSharedComponentData(entity, new RenderMesh {
                mesh = unitMesh,
                material = unitMaterial,
            });

        }

        entArray.Dispose();

    }

}