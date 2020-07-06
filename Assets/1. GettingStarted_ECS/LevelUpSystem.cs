using UnityEngine;
using Unity.Entities; // import Entities

public class LevelUpSystem : ComponentSystem { // extend : ComponentSystem from Entities pack

    /* WHAT we try to do? Increase the Level for every Entity that has the Level Component */
    protected override void OnUpdate() {
        
        /* HOW we do it? We use Entities.ForEach() loop.
         * "ref" means this is a refference so we won't be able to modify Level Component data
        */

        Entities.ForEach((ref LevelComponent levelComponent) => { // 
            /* In here the code will run on every Entity with "levelComponent" 
             * component, which is a ref to LevelComponent we created. 
             * The code here run on the Main Thread (more on this later) so
             * we can use Debug.Log to see it.
             + We increase the level by one level per second.
            */

            levelComponent.level += 1f * Time.DeltaTime; 
            //Debug.Log(levelComponent.level);
        });

        /* And this is it. We don't need to add anything to Testing.cs for a System.*/
    }
}