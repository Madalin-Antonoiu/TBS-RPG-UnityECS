using Unity.Entities; // import Entities

public struct LevelComponent : IComponentData { // We extend IComponentData whenever we want to use this as a Component. Always struct, not class.
    
    public float level;
    // That's it. Components hold only data, no behavior. We define its behavior in Testing.cs

}
