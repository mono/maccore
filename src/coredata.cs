using System;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreData
{
	[BaseType (typeof (NSPersistentStore))]
	interface NSAtomicStore {

		[Export ("initWithPersistentStoreCoordinator:configurationName:URL:options:")]
		IntPtr Constructor (NSPersistentStoreCoordinator coordinator, string configurationName, NSUrl url, NSDictionary options);

		[Export ("load:")]
		bool Load (out NSError error);

		[Export ("save:")]
		bool Save (out NSError error);

		[Export ("newCacheNodeForManagedObject:")]
		NSAtomicStoreCacheNode NewCacheNodeForManagedObject (NSManagedObject managedObject);

		[Export ("updateCacheNode:fromManagedObject:")]
		void UpdateCacheNode (NSAtomicStoreCacheNode node, NSManagedObject managedObject);

		[Export ("cacheNodes")]
		NSSet CacheNodes { get; }

		[Export ("addCacheNodes:")]
		void AddCacheNodes (NSSet cacheNodes);

		[Export ("willRemoveCacheNodes:")]
		void WillRemoveCacheNodes (NSSet cacheNodes);

		[Export ("cacheNodeForObjectID:")]
		NSAtomicStoreCacheNode CacheNodeForObjectID (NSManagedObjectID objectID);

		[Export ("objectIDForEntity:referenceObject:")]
		NSManagedObjectID ObjectIDForEntity (NSEntityDescription entity, NSObject data);

		[Export ("newReferenceObjectForManagedObject:")]
		NSAtomicStore NewReferenceObjectForManagedObject (NSManagedObject managedObject);

		[Export ("referenceObjectForObjectID:")]
		NSAtomicStore ReferenceObjectForObjectID (NSManagedObjectID objectID);

	}
	[BaseType (typeof (NSObject))]
	interface NSAtomicStoreCacheNode {

		[Export ("initWithObjectID:")]
		IntPtr Constructor (NSManagedObjectID moid);

		[Export ("objectID")]
		NSManagedObjectID ObjectID { get; }

		[Export ("propertyCache")]
		NSDictionary PropertyCache { get; set; }

		[Export ("valueForKey:")]
		NSAtomicStoreCacheNode ValueForKey (string key);

		[Export ("setValue:forKey:")]
		void SetValue (NSObject value, string key);

	}
	[BaseType (typeof (NSPropertyDescription))]
	interface NSAttributeDescription {

		[Export ("attributeType")]
		NSAttributeType AttributeType { get; set; }

		[Export ("attributeValueClassName")]
		string AttributeValueClassName { get; set; }

		[Export ("defaultValue")]
		NSAttributeDescription DefaultValue { get; }

		[Export ("setDefaultValue:")]
		void SetDefaultValue (NSObject value);

		[Export ("versionHash")]
		NSData VersionHash { get; }

		[Export ("valueTransformerName")]
		string ValueTransformerName { get; set; }

	}
	[BaseType (typeof (NSObject))]
	interface NSEntityDescription {

		[Static, Export ("entityForName:inManagedObjectContext:")]
		NSEntityDescription EntityForName (string entityName, NSManagedObjectContext context);

		[Static, Export ("insertNewObjectForEntityForName:inManagedObjectContext:")]
		NSEntityDescription InsertNewObjectForEntityForName (string entityName, NSManagedObjectContext context);

		[Export ("managedObjectModel")]
		NSManagedObjectModel ManagedObjectModel { get; }

		[Export ("managedObjectClassName")]
		string ManagedObjectClassName { get; set; }

		[Export ("name")]
		string Name { get; set; }

		[Export ("isAbstract")]
		bool Abstract { [Bind("isAbstract")] get; set; }

		[Export ("subentitiesByName")]
		NSDictionary SubentitiesByName { get; }

		[Export ("subentities")]
		NSEntityDescription[] Subentities { get; set; }

		[Export ("superentity")]
		NSEntityDescription Superentity { get; }

		[Export ("propertiesByName")]
		NSDictionary PropertiesByName { get; }

		[Export ("properties")]
		NSPropertyDescription[] Properties { get; set; }

		[Export ("userInfo")]
		NSDictionary UserInfo { get; set; }

		[Export ("attributesByName")]
		NSDictionary AttributesByName { get; }

		[Export ("relationshipsByName")]
		NSDictionary RelationshipsByName { get; }

		[Export ("relationshipsWithDestinationEntity:")]
		NSRelationshipDescription[] RelationshipsWithDestinationEntity (NSEntityDescription entity);

		[Export ("isKindOfEntity:")]
		bool IsKindOfEntity (NSEntityDescription entity);

		[Export ("versionHash")]
		NSData VersionHash { get; }

		[Export ("versionHashModifier")]
		string VersionHashModifier { get; set; }

	}
	[BaseType (typeof (NSObject))]
	interface NSEntityMapping {

		[Export ("name")]
		string Name { get; set; }

		[Export ("mappingType")]
		NSEntityMappingType MappingType { get; set; }

		[Export ("sourceEntityName")]
		string SourceEntityName { get; set; }

		[Export ("sourceEntityVersionHash")]
		NSData SourceEntityVersionHash { get; set; }

		[Export ("destinationEntityName")]
		string DestinationEntityName { get; set; }

		[Export ("destinationEntityVersionHash")]
		NSData DestinationEntityVersionHash { get; set; }

		[Export ("attributeMappings")]
		NSPropertyMapping[] AttributeMappings { get; set; }

		[Export ("relationshipMappings")]
		NSPropertyMapping[] RelationshipMappings { get; set; }

		[Export ("sourceExpression")]
		NSExpression SourceExpression { get; set; }

		[Export ("userInfo")]
		NSDictionary UserInfo { get; set; }

		[Export ("entityMigrationPolicyClassName")]
		string EntityMigrationPolicyClassName { get; set; }

	}
	[BaseType (typeof (NSObject))]
	interface NSEntityMigrationPolicy {

		[Export ("beginEntityMapping:manager:error:")]
		bool BeginEntityMapping (NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

		[Export ("createDestinationInstancesForSourceInstance:entityMapping:manager:error:")]
		bool CreateDestinationInstancesForSourceInstance (NSManagedObject sInstance, NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

		[Export ("endInstanceCreationForEntityMapping:manager:error:")]
		bool EndInstanceCreationForEntityMapping (NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

		[Export ("createRelationshipsForDestinationInstance:entityMapping:manager:error:")]
		bool CreateRelationshipsForDestinationInstance (NSManagedObject dInstance, NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

		[Export ("endRelationshipCreationForEntityMapping:manager:error:")]
		bool EndRelationshipCreationForEntityMapping (NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

		[Export ("performCustomValidationForEntityMapping:manager:error:")]
		bool PerformCustomValidationForEntityMapping (NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

		[Export ("endEntityMapping:manager:error:")]
		bool EndEntityMapping (NSEntityMapping mapping, NSMigrationManager manager, out NSError error);

	}
	[BaseType (typeof (NSPropertyDescription))]
	interface NSFetchedPropertyDescription {

		[Export ("fetchRequest")]
		NSFetchRequest FetchRequest { get; set; }

	}
	[BaseType (typeof (NSObject))]
	interface NSFetchRequest {

		[Export ("entity")]
		NSEntityDescription Entity { get; set; }

		[Export ("predicate")]
		NSPredicate Predicate { get; set; }

		[Export ("sortDescriptors")]
		NSSortDescriptor[] SortDescriptors { get; set; }

		[Export ("fetchLimit")]
		uint FetchLimit { get; set; }

		[Export ("affectedStores")]
		NSPersistentStore[] AffectedStores { get; set; }

		[Export ("resultType")]
		NSFetchRequestResultType ResultType { get; set; }

		[Export ("includesSubentities")]
		bool IncludesSubentities { get; set; }

		[Export ("includesPropertyValues")]
		bool IncludesPropertyValues { get; set; }

		[Export ("returnsObjectsAsFaults")]
		bool ReturnsObjectsAsFaults { get; set; }

		[Export ("relationshipKeyPathsForPrefetching")]
		string[] RelationshipKeyPathsForPrefetching { get; set; }

	}
	[BaseType (typeof (NSObject))]
	interface NSManagedObject {

		[Export ("initWithEntity:insertIntoManagedObjectContext:")]
		IntPtr Constructor (NSEntityDescription entity, NSManagedObjectContext context);

		[Export ("managedObjectContext")]
		NSManagedObjectContext ManagedObjectContext { get; }

		[Export ("entity")]
		NSEntityDescription Entity { get; }

		[Export ("objectID")]
		NSManagedObjectID ObjectID { get; }

		[Export ("isInserted")]
		bool IsInserted { get; }

		[Export ("isUpdated")]
		bool IsUpdated { get; }

		[Export ("isDeleted")]
		bool IsDeleted { get; }

		[Export ("isFault")]
		bool IsFault { get; }

		[Export ("hasFaultForRelationshipNamed:")]
		bool HasFaultForRelationshipNamed (string key);

		[Export ("willAccessValueForKey:")]
		void WillAccessValueForKey (string key);

		[Export ("didAccessValueForKey:")]
		void DidAccessValueForKey (string key);

		[Export ("willChangeValueForKey:")]
		void WillChangeValueForKey (string key);

		[Export ("didChangeValueForKey:")]
		void DidChangeValueForKey (string key);

		[Export ("willChangeValueForKey:withSetMutation:usingObjects:")]
		void WillChangeValueForKey (string inKey, NSKeyValueSetMutationKind inMutationKind, NSSet inObjects);

		[Export ("didChangeValueForKey:withSetMutation:usingObjects:")]
		void DidChangeValueForKey (string inKey, NSKeyValueSetMutationKind inMutationKind, NSSet inObjects);

		[Export ("observationInfo")]
		IntPtr ObservationInfo { get; set; }

		[Export ("awakeFromFetch")]
		void AwakeFromFetch ();

		[Export ("awakeFromInsert")]
		void AwakeFromInsert ();

		[Export ("willSave")]
		void WillSave ();

		[Export ("didSave")]
		void DidSave ();

		[Export ("willTurnIntoFault")]
		void WillTurnIntoFault ();

		[Export ("didTurnIntoFault")]
		void DidTurnIntoFault ();

		[Export ("valueForKey:")]
		IntPtr ValueForKey (string key);

		[Export ("setValue:forKey:")]
		void SetValue (IntPtr value, string key);

		[Export ("primitiveValueForKey:")]
		IntPtr PrimitiveValueForKey (string key);

		[Export ("setPrimitiveValue:forKey:")]
		void SetPrimitiveValue (IntPtr value, string key);

		[Export ("committedValuesForKeys:")]
		NSDictionary CommittedValuesForKeys (string[] keys);

		[Export ("changedValues")]
		NSDictionary ChangedValues { get; }

		[Export ("validateValue:forKey:error:")]
		bool ValidateValue (NSObject value, string key, out NSError error);

		[Export ("validateForDelete:")]
		bool ValidateForDelete (out NSError error);

		[Export ("validateForInsert:")]
		bool ValidateForInsert (out NSError error);

		[Export ("validateForUpdate:")]
		bool ValidateForUpdate (out NSError error);

	}
	[BaseType (typeof (NSObject))]
	interface NSManagedObjectContext {

		[Export ("persistentStoreCoordinator")]
		NSPersistentStoreCoordinator PersistentStoreCoordinator { get; set; }

		[Export ("undoManager")]
		NSUndoManager UndoManager { get; set; }

		[Export ("hasChanges")]
		bool HasChanges { get; }

		[Export ("objectRegisteredForID:")]
		NSManagedObject ObjectRegisteredForID (NSManagedObjectID objectID);

		[Export ("objectWithID:")]
		NSManagedObject ObjectWithID (NSManagedObjectID objectID);

		[Export ("executeFetchRequest:error:")]
		NSObject[] ExecuteFetchRequest (NSFetchRequest request, out NSError error);

		[Export ("countForFetchRequest:error:")]
		uint CountForFetchRequest (NSFetchRequest request, out NSError error);

		[Export ("insertObject:")]
		void InsertObject (NSManagedObject object1);

		[Export ("deleteObject:")]
		void DeleteObject (NSManagedObject object1);

		[Export ("refreshObject:mergeChanges:")]
		void RefreshObject (NSManagedObject object1, bool flag);

		[Export ("detectConflictsForObject:")]
		void DetectConflictsForObject (NSManagedObject object1);

		[Export ("observeValueForKeyPath:ofObject:change:context:")]
		void ObserveValueForKeyPath (string keyPath, IntPtr object1, NSDictionary change, IntPtr context);

		[Export ("processPendingChanges")]
		void ProcessPendingChanges ();

		[Export ("assignObject:toPersistentStore:")]
		void AssignObject (IntPtr object1, NSPersistentStore store);

		[Export ("insertedObjects")]
		NSSet InsertedObjects { get; }

		[Export ("updatedObjects")]
		NSSet UpdatedObjects { get; }

		[Export ("deletedObjects")]
		NSSet DeletedObjects { get; }

		[Export ("registeredObjects")]
		NSSet RegisteredObjects { get; }

		[Export ("undo")]
		void Undo ();

		[Export ("redo")]
		void Redo ();

		[Export ("reset")]
		void Reset ();

		[Export ("rollback")]
		void Rollback ();

		[Export ("save:")]
		bool Save (out NSError error);

		[Export ("lock")]
		void Lock ();

		[Export ("unlock")]
		void Unlock ();

		[Export ("tryLock")]
		bool TryLock { get; }

		[Export ("propagatesDeletesAtEndOfEvent")]
		bool PropagatesDeletesAtEndOfEvent { get; set; }

		[Export ("retainsRegisteredObjects")]
		bool RetainsRegisteredObjects { get; set; }

		[Export ("stalenessInterval")]
		double StalenessInterval { get; set; }

		[Export ("mergePolicy")]
		IntPtr MergePolicy { get; set; }

		[Export ("obtainPermanentIDsForObjects:error:")]
		bool ObtainPermanentIDsForObjects (NSManagedObject[] objects, out NSError error);

		[Export ("mergeChangesFromContextDidSaveNotification:")]
		void MergeChangesFromContextDidSaveNotification (NSNotification notification);
	}
	[BaseType (typeof (NSObject))]
	interface NSManagedObjectID {

		[Export ("entity")]
		NSEntityDescription Entity { get; }

		[Export ("persistentStore")]
		NSPersistentStore PersistentStore { get; }

		[Export ("isTemporaryID")]
		bool IsTemporaryID { get; }

		[Export ("URIRepresentation")]
		NSUrl URIRepresentation { get; }

	}
	[BaseType (typeof (NSObject))]
	interface NSManagedObjectModel {

		[Static, Export ("mergedModelFromBundles:")]
		NSManagedObjectModel MergedModelFromBundles (NSBundle[] bundles);

		[Static, Export ("modelByMergingModels:")]
		NSManagedObjectModel ModelByMergingModels (NSManagedObjectModel[] models);

		[Export ("init")]
		IntPtr Init { get; }

		[Export ("initWithContentsOfURL:")]
		IntPtr Constructor (NSUrl url);

		[Export ("entitiesByName")]
		NSDictionary EntitiesByName { get; }

		[Export ("entities")]
		NSEntityDescription[] Entities { get; set; }

		[Export ("configurations")]
		string[] Configurations { get; }

		[Export ("entitiesForConfiguration:")]
		string[] EntitiesForConfiguration (string configuration);

		[Export ("setEntities:forConfiguration:")]
		void SetEntities (NSEntityDescription[] entities, string configuration);

		[Export ("setFetchRequestTemplate:forName:")]
		void SetFetchRequestTemplate (NSFetchRequest fetchRequestTemplate, string name);

		[Export ("fetchRequestTemplateForName:")]
		NSFetchRequest FetchRequestTemplateForName (string name);

		[Export ("fetchRequestFromTemplateWithName:substitutionVariables:")]
		NSFetchRequest FetchRequestFromTemplateWithName (string name, NSDictionary variables);

		[Export ("localizationDictionary")]
		NSDictionary LocalizationDictionary { get; set; }

		[Static, Export ("mergedModelFromBundles:forStoreMetadata:")]
		NSManagedObjectModel MergedModelFromBundles (NSBundle[] bundles, NSDictionary metadata);

		[Static, Export ("modelByMergingModels:forStoreMetadata:")]
		NSManagedObjectModel ModelByMergingModels (NSManagedObjectModel[] models, NSDictionary metadata);

		[Export ("fetchRequestTemplatesByName")]
		NSDictionary FetchRequestTemplatesByName { get; }

		[Export ("versionIdentifiers")]
		NSSet VersionIdentifiers { get; set; }

		[Export ("isConfiguration:compatibleWithStoreMetadata:")]
		bool IsConfiguration (string configuration, NSDictionary metadata);

		[Export ("entityVersionHashesByName")]
		NSDictionary EntityVersionHashesByName { get; }
	}
	[BaseType (typeof (NSObject))]
	interface NSMappingModel {

		[Static, Export ("mappingModelFromBundles:forSourceModel:destinationModel:")]
		NSMappingModel MappingModelFromBundles (NSBundle[] bundles, NSManagedObjectModel sourceModel, NSManagedObjectModel destinationModel);

		[Export ("initWithContentsOfURL:")]
		IntPtr Constructor (NSUrl url);

		[Export ("entityMappings")]
		NSEntityMapping[] EntityMappings { get; set; }

		[Export ("entityMappingsByName")]
		NSDictionary EntityMappingsByName { get; }

	}
	[BaseType (typeof (NSObject))]
	interface NSMigrationManager {

		[Export ("initWithSourceModel:destinationModel:")]
		IntPtr Constructor (NSManagedObjectModel sourceModel, NSManagedObjectModel destinationModel);

		[Export ("migrateStoreFromURL:type:options:withMappingModel:toDestinationURL:destinationType:destinationOptions:error:")]
		bool MigrateStoreFromUrl (NSUrl sourceURL, string sStoreType, NSDictionary sOptions, NSMappingModel mappings, NSUrl dURL, string dStoreType, NSDictionary dOptions, out NSError error);

		[Export ("reset")]
		void Reset ();

		[Export ("mappingModel")]
		NSMappingModel MappingModel { get; }

		[Export ("sourceModel")]
		NSManagedObjectModel SourceModel { get; }

		[Export ("destinationModel")]
		NSManagedObjectModel DestinationModel { get; }

		[Export ("sourceContext")]
		NSManagedObjectContext SourceContext { get; }

		[Export ("destinationContext")]
		NSManagedObjectContext DestinationContext { get; }

		[Export ("sourceEntityForEntityMapping:")]
		NSEntityDescription SourceEntityForEntityMapping (NSEntityMapping mEntity);

		[Export ("destinationEntityForEntityMapping:")]
		NSEntityDescription DestinationEntityForEntityMapping (NSEntityMapping mEntity);

		[Export ("associateSourceInstance:withDestinationInstance:forEntityMapping:")]
		void AssociateSourceInstance (NSManagedObject sourceInstance, NSManagedObject destinationInstance, NSEntityMapping entityMapping);

		[Export ("destinationInstancesForEntityMappingNamed:sourceInstances:")]
		NSManagedObject[] DestinationInstancesForEntityMappingNamed (string mappingName, NSManagedObject[] sourceInstances);

		[Export ("sourceInstancesForEntityMappingNamed:destinationInstances:")]
		NSManagedObject[] SourceInstancesForEntityMappingNamed (string mappingName, NSManagedObject[] destinationInstances);

		[Export ("currentEntityMapping")]
		NSEntityMapping CurrentEntityMapping { get; }

		[Export ("migrationProgress")]
		float MigrationProgress { get; }

		[Export ("userInfo")]
		NSDictionary UserInfo { get; set; }

		[Export ("cancelMigrationWithError:")]
		void CancelMigrationWithError (NSError error);

	}
	[BaseType (typeof (NSObject))]
	interface NSPersistentStore {

		[Static, Export ("metadataForPersistentStoreWithURL:error:")]
		NSDictionary MetadataForPersistentStoreWithUrl (NSUrl url, out NSError error);

		[Static, Export ("setMetadata:forPersistentStoreWithURL:error:")]
		bool SetMetadata (NSDictionary metadata, NSUrl url, out NSError error);

		[Export ("initWithPersistentStoreCoordinator:configurationName:URL:options:")]
		IntPtr Constructor (NSPersistentStoreCoordinator root, string name, NSUrl url, NSDictionary options);

		[Export ("persistentStoreCoordinator")]
		NSPersistentStoreCoordinator PersistentStoreCoordinator { get; }

		[Export ("configurationName")]
		string ConfigurationName { get; }

		[Export ("options")]
		NSDictionary Options { get; }

		[Export ("URL")]
		NSUrl Url { get; set; }

		[Export ("identifier")]
		string Identifier { get; set; }

		[Export ("type")]
		string Type { get; }

		[Export ("isReadOnly")]
		bool ReadOnly { get; [Bind("setReadOnly:")] set; }

		[Export ("metadata")]
		NSDictionary Metadata { get; set; }

		[Export ("didAddToPersistentStoreCoordinator:")]
		void DidAddToPersistentStoreCoordinator (NSPersistentStoreCoordinator coordinator);

		[Export ("willRemoveFromPersistentStoreCoordinator:")]
		void WillRemoveFromPersistentStoreCoordinator (NSPersistentStoreCoordinator coordinator);

	}
	[BaseType (typeof (NSObject))]
	interface NSPersistentStoreCoordinator {

		[Static, Export ("registeredStoreTypes")]
		NSDictionary RegisteredStoreTypes { get; }

		[Static, Export ("registerStoreClass:forStoreType:")]
		void RegisterStoreClass (Class storeClass, string storeType);

		[Static, Export ("metadataForPersistentStoreOfType:URL:error:")]
		NSDictionary MetadataForPersistentStoreOfType (string storeType, NSUrl url, out NSError error);

		[Static, Export ("setMetadata:forPersistentStoreOfType:URL:error:")]
		bool SetMetadata (NSDictionary metadata, string storeType, NSUrl url, out NSError error);

		[Export ("setMetadata:forPersistentStore:")]
		void SetMetadata (NSDictionary metadata, NSPersistentStore store);

		[Export ("metadataForPersistentStore:")]
		NSDictionary MetadataForPersistentStore (NSPersistentStore store);

		[Export ("initWithManagedObjectModel:")]
		IntPtr Constructor (NSManagedObjectModel model);

		[Export ("managedObjectModel")]
		NSManagedObjectModel ManagedObjectModel { get; }

		[Export ("persistentStores")]
		NSPersistentStore[] PersistentStores { get; }

		[Export ("persistentStoreForURL:")]
		NSPersistentStore PersistentStoreForUrl (NSUrl URL);

		[Export ("URLForPersistentStore:")]
		NSUrl UrlForPersistentStore (NSPersistentStore store);

		[Export ("setURL:forPersistentStore:")]
		bool SetUrl (NSUrl url, NSPersistentStore store);

		[Export ("addPersistentStoreWithType:configuration:URL:options:error:")]
		NSPersistentStore AddPersistentStoreWithType (string storeType, string configuration, NSUrl storeURL, NSDictionary options, out NSError error);

		[Export ("removePersistentStore:error:")]
		bool RemovePersistentStore (NSPersistentStore store, out NSError error);

		[Export ("migratePersistentStore:toURL:options:withType:error:")]
		NSPersistentStore MigratePersistentStore (NSPersistentStore store, NSUrl URL, NSDictionary options, string storeType, out NSError error);

		[Export ("managedObjectIDForURIRepresentation:")]
		NSManagedObjectID ManagedObjectIDForURIRepresentation (NSUrl url);

		[Export ("lock")]
		void Lock ();

		[Export ("unlock")]
		void Unlock ();

		[Export ("tryLock")]
		bool TryLock { get; }

		[Obsolete("Deprecated in MAC OSX 10.5 and later")]
		[Static, Export ("metadataForPersistentStoreWithURL:error:")]
		NSDictionary MetadataForPersistentStoreWithUrl (NSUrl url, out NSError error);

	}
	[BaseType (typeof (NSObject))]
	interface NSPropertyDescription {

		[Export ("entity")]
		NSEntityDescription Entity { get; }

		[Export ("name")]
		string Name { get; set; }

		[Export ("isOptional")]
		bool Optional { get; [Bind("setOptional:")] set; }

		[Export ("isTransient")]
		bool Transient { get; [Bind("setTransient:")] set; }

		[Export ("validationPredicates")]
		NSPredicate[] ValidationPredicates { get; }

		[Export ("validationWarnings")]
		string[] ValidationWarnings { get; }

		[Export ("setValidationPredicates:withValidationWarnings:")]
		void SetValidationPredicates (NSPredicate[] validationPredicates, string[] validationWarnings);

		[Export ("userInfo")]
		NSDictionary UserInfo { get; set; }

		[Export ("isIndexed")]
		bool Indexed { get; [Bind("setIndexed:")] set; }

		[Export ("versionHash")]
		NSData VersionHash { get; }

		[Export ("versionHashModifier")]
		string VersionHashModifier { get; set; }
	}
	[BaseType (typeof (NSObject))]
	interface NSPropertyMapping {

		[Export ("name")]
		string Name { get; set; }

		[Export ("valueExpression")]
		NSExpression ValueExpression { get; set; }

		[Export ("userInfo")]
		NSDictionary UserInfo { get; set; }

	}
	[BaseType (typeof (NSPropertyDescription))]
	interface NSRelationshipDescription {

		[Export ("destinationEntity")]
		NSEntityDescription DestinationEntity { get; set; }

		[Export ("inverseRelationship")]
		NSRelationshipDescription InverseRelationship { get; set; }

		[Export ("maxCount")]
		uint MaxCount { get; set; }

		[Export ("minCount")]
		uint MinCount { get; set; }

		[Export ("deleteRule")]
		NSDeleteRule DeleteRule { get; set; }

		[Export ("isToMany")]
		bool IsToMany { get; }

		[Export ("versionHash")]
		NSData VersionHash { get; }
	}
}

