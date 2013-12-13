using System;

namespace LotusEngine
{
    /// <summary>
    /// Marks this component as a unique component; there can only be one on a given GameObject.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UniqueComponentAttribute : Attribute { }
}