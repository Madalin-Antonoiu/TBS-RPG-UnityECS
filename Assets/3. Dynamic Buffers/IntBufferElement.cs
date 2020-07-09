using Unity.Entities;

/* ###
 * IBufferElementData is the Unity interface that handles buffer elements 
 */
[InternalBufferCapacity(5)]
public struct IntBufferElement : IBufferElementData {
    public int Value;
}