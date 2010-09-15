//
// Enums.cs: enumeration definitions for Foundation
//
// Copyright 2009-2010, Novell, Inc.
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

	public enum NSComparisonResult {
		Ascending = -1,
		Same,
		Descending
	}

	public enum NSUrlRequestCachePolicy {
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

	public enum NSPropertyListFormat {
		OpenStep = 1,
		Xml = 100,
		Binary = 200
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
	public enum NSCalendarUnit {
		NSEra = 2, 
		NSYear = 4,
		NSMonth = 8,
		NSDay = 16,
		NSHour = 32,
		NSMinute = 64,
		NSSecond = 128,
		NSWeek = 256,
		NSWeekday = 512,
		NSWeekdayOrdinal = 1024,
		NSQuarter = 2048,

		[Since (4,0)]
		NSCalendar = (1 << 20),
		[Since (4,0)]
		NSTimeZone = (1 << 21),
	}

	[Flags]
	public enum NSDataWritingOptions : uint {
		Atomic = 1,

		[Since (4,0)]
		FileProtectionNone = 0x10000000,
		[Since (4,0)]
		FileProtectionComplete = 0x20000000,
		[Since (4,0)]
		NSDataWritingFileProtectionMask = 0xf0000000
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
	public enum NSDataSearchOptions {
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
	
	public enum  NSNotificationSuspensionBehavior {
        Drop = 1,
        Coalesce = 2,
        Hold = 3,
        DeliverImmediately = 4,
    }
    
    [Flags]
    public enum NSNotificationFlags : uint{
       DeliverImmediately = (1 << 0),
       PostToAllSessions = (1 << 1),
    }
}
