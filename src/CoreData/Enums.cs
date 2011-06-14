using System;

namespace MonoMac.CoreData {

        public enum NSEntityMappingType : uint {
                Undefined = 0x00,
                Custom = 0x01,
                Add = 0x02,
		Remove = 0x03,
		Copy = 0x05,
		Transform = 0x06
        }

	public enum NSAttributeType : uint {
		Undefined = 0,
		Integer16 = 100,
		Integer32 = 200,
		Integer64 = 300,
		Decimal = 400,
		Double = 500,
		Float = 600,
		String = 700,
		Boolean = 800,
		Date = 900,
		Binary = 1000,
		Transformable = 1800    
	}

	[Flags]
	public enum NSFetchRequestResultType : uint {
		ManagedObject = 0x00,
		ManagedObjectID = 0x01,
		DictionaryResultType = 0x02,
		NSCountResultType = 0x04
	}

	public enum NSKeyValueSetMutationKind : uint {
		Union = 1,
		Minus = 2,
		Intersect = 3,
		NSKeyValueSet = 4
	}

	public enum NSDeleteRule : uint {
		NoAction,
		Nullify,
		Cascade,
		Deny
	}

	public enum NSPersistentStoreRequestType {
		Fetch = 1,
		Save
	}

	public enum NSManagedObjectContextConcurrencyType {
		Confinement, PrivateQueue, MainQueue
	}

	public enum NSMergePolicyType {
		Error, PropertyStoreTrump, PropertyObjectTrump, Overwrite, RollbackMerge
	}
}
