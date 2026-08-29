using UnityEngine;

namespace Arbor
{
	[System.Serializable]
    internal struct ObjectId : System.IEquatable<ObjectId>
    {
		public static ObjectId None => default;

#if UNITY_6000_3_OR_NEWER
		[SerializeField]
		internal EntityId _EntityId;

		public ObjectId(Object obj)
		{
			_EntityId = obj.GetEntityId();
		}

		public bool Equals(ObjectId other)
		{
			return _EntityId == other._EntityId;
		}

		public override int GetHashCode()
		{
			return _EntityId.GetHashCode();
		}

		public bool IsValid()
		{
			return _EntityId.IsValid();
		}

		public static ObjectId From(EntityId entityId)
		{
			return new ObjectId()
			{
				_EntityId = entityId,
			};
		}
#else
		[SerializeField]
		internal int _InstanceId;

		public ObjectId(Object obj)
		{
			_InstanceId = obj.GetInstanceID();
		}

		public bool Equals(ObjectId other)
		{
			return _InstanceId == other._InstanceId;
		}

		public override int GetHashCode()
		{
			return _InstanceId;
		}

		public bool IsValid()
		{
			return _InstanceId != 0;
		}

		public static ObjectId From(int instanceId)
		{
			return new ObjectId()
			{
				_InstanceId = instanceId,
			};
		}
#endif
		public override bool Equals(object obj)
		{
			return obj is ObjectId other
				&& Equals(other);
		}

		public static bool operator ==(ObjectId lhs, ObjectId rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(ObjectId lhs, ObjectId rhs)
		{
			return !(lhs == rhs);
		}
    }
}
