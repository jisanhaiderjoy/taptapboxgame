using UnityEngine;

namespace PlayNoob.ScriptableObjects {
    /// <summary>
    /// Base class for ScriptableObjects that need a public description field.
    /// </summary>
    public class DescriptionBaseSO : ScriptableObject {
        [TextArea] public string description;
    }
}