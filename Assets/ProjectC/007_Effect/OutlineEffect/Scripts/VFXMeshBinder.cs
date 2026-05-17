
namespace UnityEngine.VFX.Utility
{
    [AddComponentMenu("VFX/Property Binders/Mesh Binder")]
    [VFXBinder("GameObject/Mesh")]
    class VFXMeshBinder : VFXBinderBase
    {
        public string Property { get { return (string)m_Property; } set { m_Property = value; } }
        [VFXPropertyBinding("UnityEngine.Mesh"), SerializeField, UnityEngine.Serialization.FormerlySerializedAs("m_Parameter")]
        protected ExposedProperty m_Property = "Mesh";
        public GameObject Target = null;

        public override bool IsValid(VisualEffect component)
        {
            return Target != null && component.HasMesh(m_Property);
        }

        public override void UpdateBinding(VisualEffect component)
        {
            if(Target.GetComponentInChildren<MeshFilter>())
            {
                component.SetMesh(m_Property, Target.GetComponentInChildren<MeshFilter>().sharedMesh);
            }
        }

        // リストに表示される形式
        public override string ToString()
        {
            return string.Format("Mesh : '{0}' -> {1}", m_Property, Target == null ? "(null)" : Target.name);
        }
    }
}
