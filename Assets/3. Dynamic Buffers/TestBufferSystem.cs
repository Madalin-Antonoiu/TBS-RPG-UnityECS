using Unity.Entities;

public class TestBufferSystem : ComponentSystem {

    protected override void OnUpdate() {
        Entities.ForEach((DynamicBuffer<IntBufferElement> dynamicBuffer) => {

            // Cycling through our normal buffer, not even reinterpreting
            for (int i=0; i < dynamicBuffer.Length; i++) {

                //increasing the values
                IntBufferElement element = dynamicBuffer[i];
                element.Value++;
                dynamicBuffer[i] = element;

            }// minutul 7:50 https://www.youtube.com/watch?v=XC84bc95heI

        });

    }

}