using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class MoverSystem : ComponentSystem { 

    protected override void OnUpdate() {

        Entities.ForEach((ref Translation moveMe, ref MoveSpeedComponent moveSpeedComponent) => {  
            moveMe.Value.y += moveSpeedComponent.moveSpeed * Time.DeltaTime; // moving up

            // Bounce when hit top and bottom of the screen
            if(moveMe.Value.y > 5f) {
                moveSpeedComponent.moveSpeed = -math.abs(moveSpeedComponent.moveSpeed);
            }

            if (moveMe.Value.y < -5f) {
                moveSpeedComponent.moveSpeed = +math.abs(moveSpeedComponent.moveSpeed);
            }

        });

    }
}