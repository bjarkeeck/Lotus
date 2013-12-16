using System;

namespace LotusEngine
{
    /// <summary>
    /// Marks this field so it is serialized when the gameobject is saved.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class SerializeAttribute : Attribute { }
}