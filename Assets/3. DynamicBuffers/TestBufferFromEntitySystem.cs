﻿using Unity.Entities;

public class TestBufferFromEntitySystem : ComponentSystem {
    
    protected override void OnUpdate() {
        /*
        Entities.WithAll<Tag_Bob>().ForEach((Entity bobEntity) => {
            BufferFromEntity<IntBufferElement> intBufferFromEntity = GetBufferFromEntity<IntBufferElement>();

            Entity aliceEntity = Entity.Null;

            Entities.WithAll<Tag_Alice>().ForEach((Entity aliceEntityTmp) => {
                aliceEntity = aliceEntityTmp;
            });

            DynamicBuffer<IntBufferElement> aliceDynamicBuffer = intBufferFromEntity[aliceEntity];
            
            for (int i = 0; i < aliceDynamicBuffer.Length; i++) {
                IntBufferElement intBufferElement = aliceDynamicBuffer[i];
                intBufferElement.Value++;
                aliceDynamicBuffer[i] = intBufferElement;
            }
        });
        */
    }

}
