//
// Enums.cs: enumeration definitions for Foundation
//
// Copyright 2009-2010, Novell, Inc.
// Copyright 2011, 2012 Xamarin Inc
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation  {
	public enum NSUrlCredentialPersistence {
		None,
		ForSession,
		Permanent
	}

	public enum NSBundleExecutableArchitecture {
		I386   = 0x00000007,
		PPC    = 0x00000012,
		X86_64 = 0x01000007,
		PPC64  = 0x01000012
	}

#if MAC64
	public enum NSComparisonResult : long {
#else
	public enum NSComparisonResult {
#endif
		Ascending = -1,
		Same,
		Descending
	}

#if MAC64
	public enum NSUrlRequestCachePolicy : ulong {
#else
	public enum NSUrlRequestCachePolicy {
#endif
		UseProtocolCachePolicy = 0,
		ReloadIgnoringLocalCacheData = 1,
		ReloadIgnoringLocalAndRemoteCacheData = 4, // Unimplemented
		ReloadIgnoringCacheData = ReloadIgnoringLocalCacheData,

		ReturnCacheDataElseLoad = 2,
		ReturnCacheDataDoNotLoad = 3,

		ReloadRevalidatingCacheData = 5, // Unimplemented
	}

	public enum NSUrlCacheStoragePolicy {
		Allowed, AllowedInMemoryOnly, NotAllowed
	}
	
	public enum NSStreamStatus {
		NotOpen = 0,
		Opening = 1,
		Open = 2,
		Reading = 3,
		Writing = 4,
		AtEnd = 5,
		Closed = 6,
		Error = 7
	}

#if MAC64
	public enum NSPropertyListFormat : ulong {
#else
	public enum NSPropertyListFormat {
#endif
		OpenStep = 1,
		Xml = 100,
		Binary = 200
	}

	public enum NSPropertyListMutabilityOptions {
		Immutable = 0,
		MutableContainers = 1,
		MutableContainersAndLeaves = 2
	}

	// Should mirror NSPropertyListMutabilityOptions
	public enum NSPropertyListWriteOptions {
		Immutable = 0,
		MutableContainers = 1,
		MutableContainersAndLeaves = 2
	}

	// Should mirror NSPropertyListMutabilityOptions, but currently
	// not implemented (always use Immutable/0)
	public enum NSPropertyListReadOptions {
		Immutable = 0,
		MutableContainers = 1,
		MutableContainersAndLeaves = 2
	}

	public enum NSNetServicesStatus {
		UnknownError = -72000,
		CollisionError = -72001,
		NotFoundError	= -72002,
		ActivityInProgress = -72003,
		BadArgumentError = -72004,
		CancelledError = -72005,
		InvalidError = -72006,
		TimeoutError = -72007
	}
	
	public enum NSNetServiceOptions {
		NoAutoRename = 1 << 0
	}

	public enum NSDateFormatterStyle {
		None,
		Short,
		Medium,
		Long, 
		Full
	}

	public enum NSDateFormatterBehavior {
		Default = 0, Mode_10_4 = 1040
	}

	public enum NSHttpCookieAcceptPolicy {
		Always, Never, OnlyFromMainDocumentDomain
	}

	[Flags]
#if MAC64
	public enum NSCalendarUnit : ulong {
#else
	public enum NSCalendarUnit {
#endif
		Era = 2, 
		Year = 4,
		Month = 8,
		Day = 16,
		Hour = 32,
		Minute = 64,
		Second = 128,
		Week = 256,
		Weekday = 512,
		WeekdayOrdinal = 1024,
		Quarter = 2048,

		[Since (5,0)]
		WeekOfMonth = (1 << 12),
		[Since (5,0)]
		WeekOfYear = (1 << 13),
		[Since (5,0)]
		YearForWeakOfYear = (1 << 14),
			 
		[Since (4,0)]
		Calendar = (1 << 20),
		[Since (4,0)]
		TimeZone = (1 << 21),
	}

	[Flags]
#if MAC64
	public enum NSDataReadingOptions: ulong {
#else
	public enum NSDataReadingOptions: uint {
#endif
		   Mapped =   1 << 0,
		   Uncached = 1 << 1,

		   [Since (5,0)]
		   Coordinated = 1 << 2,
		   [Since (5,0)]
		   MappedAlways = 1 << 3
	}

	[Flags]
	public enum NSDataWritingOptions : uint {
		Atomic = 1,

		WithoutOverwriting  = 2,
			
		[Obsolete ("No longer available")]
		Coordinated = 1 << 2,
			
		[Since (4,0)]
		FileProtectionNone = 0x10000000,
		[Since (4,0)]
		FileProtectionComplete = 0x20000000,
		[Since (4,0)]
		FileProtectionMask = 0xf0000000,
		[Since (5,0)]
		FileProtectionCompleteUnlessOpen = 0x30000000,
		[Since (5,0)]
		FileProtectionCompleteUntilFirstUserAuthentication = 0x40000000,
	}
	
	public delegate void NSSetEnumerator (NSObject obj, ref bool stop);

	[Since (4,0)]
	public enum NSOperationQueuePriority {
		VeryLow = -8, Low = -4, Normal = 0, High = 4, VeryHigh = 8
	}

	[Flags]
	public enum NSNotificationCoalescing {
		NoCoalescing = 0,
		CoalescingOnName = 1,
		CoalescingOnSender = 2
	}

	public enum NSPostingStyle {
		PostWhenIdle = 1, PostASAP = 2, Now = 3
	}

	[Flags]
#if MAC64
	public enum NSDataSearchOptions : ulong {
#else
	public enum NSDataSearchOptions {
#endif
		SearchBackwards = 1,
		SearchAnchored = 2
	}

	public enum NSExpressionType {
		ConstantValue = 0, 
		EvaluatedObject, 
		Variable, 
		KeyPath, 
		Function,
		UnionSet, 
		IntersectSet, 
		MinusSet, 
		Subquery = 13,
		NSAggregate,
		Block = 19
	}

	public enum NSUrlError {
		Unknown = 			-1,
		Cancelled = 			-999,
		BadURL = 				-1000,
		TimedOut = 			-1001,
		UnsupportedURL = 			-1002,
		CannotFindHost = 			-1003,
		CannotConnectToHost = 		-1004,
		NetworkConnectionLost = 		-1005,
		DNSLookupFailed = 		-1006,
		HTTPTooManyRedirects = 		-1007,
		ResourceUnavailable = 		-1008,
		NotConnectedToInternet = 		-1009,
		RedirectToNonExistentLocation = 	-1010,
		BadServerResponse = 		-1011,
		UserCancelledAuthentication = 	-1012,
		UserAuthenticationRequired = 	-1013,
		ZeroByteResource = 		-1014,
		CannotDecodeRawData =             -1015,
		CannotDecodeContentData =         -1016,
		CannotParseResponse =             -1017,
		FileDoesNotExist = 		-1100,
		FileIsDirectory = 		-1101,
		NoPermissionsToReadFile = 	-1102,
		//#if MAC_OS_X_VERSION_10_5 <= MAC_OS_X_VERSION_MAX_ALLOWED
		DataLengthExceedsMaximum =	-1103,
		//#endif
		SecureConnectionFailed = 		-1200,
		ServerCertificateHasBadDate = 	-1201,
		ServerCertificateUntrusted = 	-1202,
		ServerCertificateHasUnknownRoot = -1203,
		ServerCertificateNotYetValid = 	-1204,
		ClientCertificateRejected = 	-1205,
		CannotLoadFromNetwork = 		-2000,

		// Download and file I/O errors
		CannotCreateFile = 		-3000,
		CannotOpenFile = 			-3001,
		CannotCloseFile = 		-3002,
		CannotWriteToFile = 		-3003,
		CannotRemoveFile = 		-3004,
		CannotMoveFile = 			-3005,
		DownloadDecodingFailedMidStream = -3006,
		DownloadDecodingFailedToComplete =-3007,
	}

	[Flags]
	public enum NSKeyValueObservingOptions {
		New = 1, Old = 2, OldNew = 3, Initial = 4, Prior = 8, 
	}

	public enum NSKeyValueChange {
		Setting = 1, Insertion, Removal, Replacement
	}

	public enum NSKeyValueSetMutationKind {
		UnionSet = 1, MinusSet, IntersectSet, SetSet
	}

	[Flags]
	public enum NSEnumerationOptions {
		SortConcurrent = 1,
		Reverse = 2
	}
	
#if MONOMAC
	public enum NSNotificationSuspensionBehavior {
		Drop = 1,
		Coalesce = 2,
		Hold = 3,
		DeliverImmediately = 4,
	}
    
	[Flags]
#if MAC64
	public enum NSNotificationFlags : ulong {
#else
	public enum NSNotificationFlags {
#endif
		DeliverImmediately = (1 << 0),
		PostToAllSessions = (1 << 1),
	}
#endif

	public enum NSStreamEvent : uint {
		None = 0,
		OpenCompleted = 1 << 0,
		HasBytesAvailable = 1 << 1,
		HasSpaceAvailable = 1 << 2,
		ErrorOccurred = 1 << 3,
		EndEncountered = 1 << 4
	}

#if MAC64
	public enum NSComparisonPredicateModifier : ulong {
#else
	public enum NSComparisonPredicateModifier {
#endif
		Direct,
		All,
		Any
	}

#if MAC64
	public enum NSPredicateOperatorType : ulong {
#else
	public enum NSPredicateOperatorType {
#endif
		LessThan,
		LessThanOrEqualTo,
		GreaterThan,
		GreaterThanOrEqualTo,
		EqualTo,
		NotEqualTo,
		Matches,
		Like,
		BeginsWith,
		EndsWith,
		In,
		CustomSelector,
		Contains,
		Between
	}

	[Flags]
#if MAC64
	public enum NSComparisonPredicateOptions : ulong {
#else
	public enum NSComparisonPredicateOptions {
#endif
		None=0x00,
		CaseInsensitive=0x01,
		DiacriticInsensitive=0x02
	}	
	
	public enum NSCompoundPredicateType {
		Not,
		And,
		Or
	}	

	[Since (4,0)]
	[Flags]
	public enum NSVolumeEnumerationOptions {
		None                     = 0,
		// skip                  = 1 << 0,
		SkipHiddenVolumes        = 1 << 1,
		ProduceFileReferenceUrls = 1 << 2,
	}

	[Since (4,0)]
	[Flags]
	public enum NSDirectoryEnumerationOptions {
		SkipsNone                    = 0,
		SkipsSubdirectoryDescendants = 1 << 0,
		SkipsPackageDescendants      = 1 << 1,
		SkipsHiddenFiles             = 1 << 2,
	}

	[Since (4,0)]
	[Flags]
	public enum NSFileManagerItemReplacementOptions {
		None                      = 0,
		UsingNewMetadataOnly      = 1 << 0,
		WithoutDeletingBackupItem = 1 << 1,
	}

	public enum NSSearchPathDirectory {
		ApplicationDirectory = 1,
		DemoApplicationDirectory,
		DeveloperApplicationDirectory,
		AdminApplicationDirectory,
		LibraryDirectory,
		DeveloperDirectory,
		UserDirectory,
		DocumentationDirectory,
		DocumentDirectory,
		CoreServiceDirectory,
		AutosavedInformationDirectory = 11,
		DesktopDirectory = 12,
		CachesDirectory = 13,
		ApplicationSupportDirectory = 14,
		DownloadsDirectory = 15,
		InputMethodsDirectory = 16,
		MoviesDirectory = 17,
		MusicDirectory = 18,
		PicturesDirectory = 19,
		PrinterDescriptionDirectory = 20,
		SharedPublicDirectory = 21,
		PreferencePanesDirectory = 22,
		ApplicationScriptsDirectory = 23,
		ItemReplacementDirectory = 99,
		AllApplicationsDirectory = 100,
		AllLibrariesDirectory = 101,
		TrashDirectory = 102,
	}

	[Flags]
	public enum NSSearchPathDomain {
		None    = 0,
		User    = 1 << 0,
		Local   = 1 << 1,
		Network = 1 << 2,
		System  = 1 << 3,
		All     = 0x0ffff,
	}

	public enum NSRoundingMode {
		Plain, Down, Up, Bankers
	}

	public enum NSCalculationError {
		None, PrecisionLoss, Underflow, Overflow, DivideByZero
	}
	
#if MAC64
	public enum NSStringDrawingOptions : long {
#else
	public enum NSStringDrawingOptions : uint {
#endif
		UsesLineFragmentOrigin = (1 << 0),
		UsesFontLeading = (1 << 1),
		DisableScreenFontSubstitution = (1 << 2),
		UsesDeviceMetrics = (1 << 3),
		OneShot = (1 << 4),
		TruncatesLastVisibleLine = (1 << 5)
	}		

#if MAC64
	public enum NSNumberFormatterStyle : ulong {
#else
	public enum NSNumberFormatterStyle {
#endif	
		None = 0,
		Decimal = 1,
		Currency = 2,
		Percent = 3,
		Scientific = 4,
		SpellOut = 5
	}

#if MAC64
	public enum NSNumberFormatterBehavior : ulong {
#else
	public enum NSNumberFormatterBehavior {
#endif
		Default = 0,
		Version_10_0 = 1000,
		Version_10_4 = 1040
	}

#if MAC64
	public enum NSNumberFormatterPadPosition : ulong {
#else
	public enum NSNumberFormatterPadPosition {
#endif
		BeforePrefix, AfterPrefix, BeforeSuffix, AfterSuffix
	}

#if MAC64
	public enum NSNumberFormatterRoundingMode : ulong {
#else
	public enum NSNumberFormatterRoundingMode {
#endif
		Ceiling, Floor, Down, Up, HalfEven, HalfDown, HalfUp
	}

	[Flags]
	public enum NSFileVersionReplacingOptions {
		ByMoving = 1 << 0
	}

	public enum NSFileVersionAddingOptions {
		ByMoving = 1 << 0
	}

	[Flags]
#if MAC64
	public enum NSFileCoordinatorReadingOptions : ulong {
#else
	public enum NSFileCoordinatorReadingOptions {
#endif
		WithoutChanges = 1
	}

	[Flags]
#if MAC64
	public enum NSFileCoordinatorWritingOptions : ulong {
#else
	public enum NSFileCoordinatorWritingOptions {
#endif
		ForDeleting = 1,
		ForMoving = 2,
		ForMerging = 4,
		ForReplacing = 8
	}

	[Flags]
#if MAC64
	public enum NSLinguisticTaggerOptions : ulong {
#else
	public enum NSLinguisticTaggerOptions {
#endif
		OmitWords = 1,
		OmitPunctuation = 2,
		OmitWhitespace = 4,
		OmitOther = 8,
		JoinNames = 16
	}

	public enum NSUbiquitousKeyValueStoreChangeReason {
		ServerChange, InitialSyncChange, QuotaViolationChange, AccountChange
	}

	[Flags]
#if MAC64
	public enum NSJsonReadingOptions : ulong {
#else
	public enum NSJsonReadingOptions {
#endif	
		MutableContainers = 1,
		MutableLeaves = 2,
		AllowFragments = 4
	}

	[Flags]
#if MAC64
	public enum NSJsonWritingOptions : ulong {
#else
	public enum NSJsonWritingOptions {
#endif
		PrettyPrinted = 1
	}

#if MAC64
	public enum NSLocaleLanguageDirection : ulong {
#else
	public enum NSLocaleLanguageDirection {
#endif
		Unknown, LeftToRight, RightToLeft, TopToBottom, BottomToTop,
	}

	[Flags]
	public enum NSAlignmentOptions : long {
		MinXInward   = 1 << 0,
		MinYInward   = 1 << 1,
		MaxXInward   = 1 << 2,
		MaxYInward   = 1 << 3,
		WidthInward  = 1 << 4,
		HeightInward = 1 << 5,

		MinXOutward   = 1 << 8,
		MinYOutward   = 1 << 9,
		MaxXOutward   = 1 << 10,
		MaxYOutward   = 1 << 11,
		WidthOutward  = 1 << 12,
		HeightOutward = 1 << 13,

		MinXNearest   = 1 << 16,
		MinYNearest   = 1 << 17,
		MaxXNearest   = 1 << 18,
		MaxYNearest   = 1 << 19,
		WidthNearest  = 1 << 20,
		HeightNearest = 1 << 21,

		RectFlipped   = 1 << 63,

		AllEdgesInward = MinXInward|MaxXInward|MinYInward|MaxYInward,
		AllEdgesOutward = MinXOutward|MaxXOutward|MinYOutward|MaxYOutward,
		AllEdgesNearest = MinXNearest|MaxXNearest|MinYNearest|MaxYNearest,
	}

	[Flags]
	public enum NSFileWrapperReadingOptions {
		Immediate = 1 << 0,
		WithoutMapping = 1 << 1
	}

	[Flags]
	public enum NSFileWrapperWritingOptions {
		Atomic = 1 << 0,
		WithNameUpdating = 1 << 1
	}

	[Flags]
	public enum NSAttributedStringEnumeration {
		None = 0,
		Reverse = 1 << 1,
		LongestEffectiveRangeNotRequired = 1 << 20
	}

#if !MONOMAC
	// MonoMac AppKit redefines this with more values
	public enum NSUnderlineStyle {
		None, Single
	}
#endif

	public enum NSWritingDirection {
		Natural = -1, LeftToRight = 0, RightToLeft = -1
	}

	[Flags]
#if MAC64
	public enum NSByteCountFormatterUnits : ulong {
#else
	public enum NSByteCountFormatterUnits {
#endif
		UseDefault      = 0,
		UseBytes        = 1 << 0,
		UseKB           = 1 << 1,
		UseMB           = 1 << 2,
		UseGB           = 1 << 3,
		UseTB           = 1 << 4,
		UsePB           = 1 << 5,
		UseEB           = 1 << 6,
		UseZB           = 1 << 7,
		UseYBOrHigher   = 0x0FF << 8,
		UseAll          = 0x0FFFF
	}

#if MAC64
	public enum NSByteCountFormatterCountStyle : long {
#else
	public enum NSByteCountFormatterCountStyle {
#endif
		File, Memory, Decimal, Binary
	}

	[Flags]
	public enum NSUrlBookmarkCreationOptions {
		PreferFileIDResolution = 1 << 8,
		MinimalBookmark = 1 << 9,
		SuitableForBookmarkFile = 1 << 10,
		WithSecurityScope = 1 << 11,
		SecurityScopeAllowOnlyReadAccess = 1 << 12
	}

	[Flags]
	public enum NSUrlBookmarkResolutionOptions {
		WithoutUI = 1 << 8,
		WithoutMounting = 1 << 9,
		WithSecurityScope = 1 << 10,
	}

	public enum NSLigatureType {
		None, Default, All 
	}
	
#if MAC64
	public enum NSDateComponentsWrappingBehavior : ulong {
#else
	public enum NSDateComponentsWrappingBehavior {
#endif
		None = 0,
		WrapCalendarComponents = 1 << 0,
	}

#if MAC64
	public enum NSUrlRequestNetworkServiceType : ulong {
#else
	public enum NSUrlRequestNetworkServiceType {
#endif
		Default,
		VoIP,
		Video,
		Background,
		Voice
	}
}
