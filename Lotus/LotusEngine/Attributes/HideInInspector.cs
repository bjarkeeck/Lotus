using System;

namespace LotusEngine
{
    /// <summary>
    /// Marks this field so it is not shown in the AWESUHMEditor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class HideInInspectorAttribute : Attribute { }
}