//
// This file describes the API that the generator will produce
//
// Authors:
//   Geoff Norton
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2010, Novell, Inc.
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
//
using MonoMac.ObjCRuntime;
using MonoMac.CoreFoundation;
using MonoMac.Foundation;
using MonoMac.CoreGraphics;

#if MONOMAC
using MonoMac.AppKit;
#else
using MonoMac.CoreLocation;
using MonoMac.UIKit;
#endif

using System;
using System.Drawing;

namespace MonoMac.Foundation
{
	[BaseType (typeof (NSObject))]
	public interface NSArray {
		[Export ("count")]
		uint Count { get; }

		[Export ("objectAtIndex:")]
		IntPtr ValueAt (uint idx);

		[Export ("arrayWithObjects:count:")][Static][Internal]
		NSArray FromObjects (IntPtr array, int count);

		[Export ("valueForKey:")]
		NSObject ValueForKey (NSString key);

		[Export ("setValue:forKey:")]
		void SetValueForKey (NSObject value, NSString key);

		[Export ("writeToFile:atomically:")]
                bool WriteToFile (string path, bool useAuxiliaryFile);

		[Export ("arrayWithContentsOfFile:")][Static]
                NSArray FromFile (string path);
	}

	[Since (3,2)]
	[BaseType (typeof (NSObject))]
	public interface NSAttributedString {
		[Export ("string")]
		string Value { get; }

		[Export ("attributesAtIndex:effectiveRange:")]
		NSDictionary GetAttributes (int location, out NSRange effectiveRange);

		[Export ("length")]
		int Length { get; }

		// TODO: figure out the type, this deserves to be strongly typed if possble
		[Export ("attribute:atIndex:effectiveRange:")]
		NSObject GetAttribute (string attribute, int location, out NSRange effectiveRange);

		[Export ("attributedSubstringFromRange:"), Internal]
		NSAttributedString Substring (NSRange range);

		[Export ("attributesAtIndex:longestEffectiveRange:inRange:")]
		NSDictionary GetAttributes (int location, out NSRange longestEffectiveRange, NSRange rangeLimit);

		[Export ("attribute:atIndex:longestEffectiveRange:inRange:")]
		NSObject GetAttribute (string attribute, int location, out NSRange longestEffectiveRange, NSRange rangeLimit);

		[Export ("isEqualToAttributedString:")]
		bool IsEqual (NSAttributedString other);

		[Export ("initWithString:")]
		IntPtr Constructor (string str);
		
		[Export ("initWithString:attributes:")]
		IntPtr Constructor (string str, NSDictionary attributes);

		[Export ("initWithAttributedString:")]
		IntPtr Constructor (NSAttributedString other);

#if MONOMAC
		[Export("size")]
		SizeF Size { get; }
#endif


#if MONOMAC
		[Field ("NSFontAttributeName", "AppKit")]
#else
		[Field ("NSFontAttributeName")]
#endif
		NSString FontAttributeName { get; }

#if MONOMAC
		[Field ("NSParagraphStyleAttributeName", "AppKit")]
#else
		[Field ("NSParagraphStyleAttributeName")]
#endif
		NSString ParagraphStyleAttributeName { get; }
                
#if MONOMAC
		[Field ("NSForegroundColorAttributeName", "AppKit")]
#else
		[Field ("NSForegroundColorAttributeName")]
#endif
                NSString ForegroundColorAttributeName { get; }
                
#if MONOMAC
                [Field ("NSLigatureAttributeName", "AppKit")]
#else
                [Field ("NSLigatureAttributeName")]
#endif
                NSString LigatureAttributeName { get; } 
#if MONOMAC
		[Export ("initWithData:options:documentAttributes:error:")]
		IntPtr Constructor (NSData data, NSDictionary options, out NSDictionary docAttributes, out NSError error);

		[Export ("initWithDocFormat:documentAttributes:")]
		IntPtr Constructor(NSData wordDocFormat, out NSDictionary docAttributes);

		[Export ("initWithHTML:baseURL:documentAttributes:")]
		IntPtr Constructor (NSData htmlData, NSUrl baseUrl, out NSDictionary docAttributes);
		
		[Export ("drawAtPoint:")]
		void DrawString (PointF point);
		
		[Export ("drawInRect:")]
		void DrawString (RectangleF rect);
		
		[Export ("drawWithRect:options:")]
		void DrawString (RectangleF rect, NSStringDrawingOptions options);
		
#endif
	}

	[BaseType (typeof (NSObject),
		   Delegates=new string [] { "WeakDelegate" },
		   Events=new Type [] { typeof (NSCacheDelegate)} )]
	[Since (4,0)]
	public interface NSCache {
		[Export ("objectForKey:")]
		NSObject ObjectForKey (NSObject key);

		[Export ("setObject:forKey:")]
		void SetObjectforKey (NSObject obj, NSObject key);

		[Export ("setObject:forKey:cost:")]
		void SetCost (NSObject obj, NSObject key, uint cost);

		[Export ("removeObjectForKey:")]
		void RemoveObjectForKey (NSObject key);

		[Export ("removeAllObjects")]
		void RemoveAllObjects ();

		//Detected properties
		[Export ("name")]
		string Name { get; set; }

		[Export ("delegate")]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSCacheDelegate Delegate { get; set; }

		[Export ("totalCostLimit")]
		uint TotalCostLimit { get; set; }

		[Export ("countLimit")]
		uint CountLimit { get; set; }

		[Export ("evictsObjectsWithDiscardedContent")]
		bool EvictsObjectsWithDiscardedContent { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSCacheDelegate {
		[Export ("cache:willEvictObject:"), EventArgs ("NSObject")]
		void WillEvictObject (NSCache cache, NSObject obj);
	}

	[BaseType (typeof (NSObject), Name="NSCachedURLResponse")]
	public interface NSCachedUrlResponse {
		[Export ("initWithResponse:data:userInfo:storagePolicy:")]
		IntPtr Constructor (NSUrlResponse response, NSData data, NSDictionary userInfo, NSUrlCacheStoragePolicy storagePolicy);

		[Export ("response")]
		NSUrlResponse Response { get; }

		[Export ("data")]
		NSData Data { get; }

		[Export ("userInfo")]
		NSDictionary UserInfo { get; }

		[Export ("storagePolicy")]
		NSUrlCacheStoragePolicy StoragePolicy { get; }
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSCalendar {
		[Export ("initWithCalendarIdentifier:")]
		IntPtr Constructor (string identifier);

		[Export ("calendarIdentifier")]
		string Identifier { get; }

		[Export ("currentCalendar")] [Static]
		NSCalendar CurrentCalendar { get; }

		[Export ("locale")]
		NSLocale Locale { get; set; }

		[Export ("timeZone")]
		NSTimeZone TimeZone { get; set; } 

		[Export ("firstWeekday")]
		uint FirstWeekDay { get; set; } 

		[Export ("minimumDaysInFirstWeek")]
		uint MinimumDaysInFirstWeek { get; set; }

		//- (NSRange)minimumRangeOfUnit:(NSCalendarUnit)unit;
		//- (NSRange)maximumRangeOfUnit:(NSCalendarUnit)unit;
		//- (NSRange)rangeOfUnit:(NSCalendarUnit)smaller inUnit:(NSCalendarUnit)larger forDate:(NSDate *)date;
		//- (NSUInteger)ordinalityOfUnit:(NSCalendarUnit)smaller inUnit:(NSCalendarUnit)larger forDate:(NSDate *)date;
		//- (BOOL)rangeOfUnit:(NSCalendarUnit)unit startDate:(NSDate **)datep interval:(NSTimeInterval *)tip forDate:(NSDate *)date AVAILABLE_MAC_OS_X_VERSION_10_5_AND_LATER;
		//- (NSDate *)dateFromComponents:(NSDateComponents *)comps;
		//- (NSDateComponents *)components:(NSUInteger)unitFlags fromDate:(NSDate *)date;
		//- (NSDate *)dateByAddingComponents:(NSDateComponents *)comps toDate:(NSDate *)date options:(NSUInteger)opts;
		//- (NSDateComponents *)components:(NSUInteger)unitFlags fromDate:(NSDate *)startingDate toDate:(NSDate *)resultDate options:(NSUInteger)opts;
	}

	[Since (3,2)]
	[BaseType (typeof (NSObject))]
	public interface NSCharacterSet {
		[Static, Export ("alphanumericCharacterSet")]
		NSCharacterSet Alphanumerics {get;}

		[Static, Export ("capitalizedLetterCharacterSet")]
		NSCharacterSet Capitalized {get;}

		// TODO/FIXME: constructor?
		[Static, Export ("characterSetWithBitmapRepresentation:")]
		NSCharacterSet FromBitmap (NSData data);

		// TODO/FIXME: constructor?
		[Static, Export ("characterSetWithCharactersInString:")]
		NSCharacterSet FromString (string aString);

		[Static, Export ("characterSetWithContentsOfFile:")]
		NSCharacterSet FromFile (string path);

		[Static, Export ("characterSetWithRange:")]
		NSCharacterSet FromRange (NSRange aRange);

		[Static, Export ("controlCharacterSet")]
		NSCharacterSet Controls {get;}

		[Static, Export ("decimalDigitCharacterSet")]
		NSCharacterSet DecimalDigits {get;}

		[Static, Export ("decomposableCharacterSet")]
		NSCharacterSet Decomposables {get;}

		[Static, Export ("illegalCharacterSet")]
		NSCharacterSet Illegals {get;}

		[Static, Export ("letterCharacterSet")]
		NSCharacterSet Letters {get;}

		[Static, Export ("lowercaseLetterCharacterSet")]
		NSCharacterSet LowercaseLetters {get;}

		[Static, Export ("newlineCharacterSet")]
		NSCharacterSet Newlines {get;}

		[Static, Export ("nonBaseCharacterSet")]
		NSCharacterSet Marks {get;}

		[Static, Export ("punctuationCharacterSet")]
		NSCharacterSet Punctuation {get;}

		[Static, Export ("symbolCharacterSet")]
		NSCharacterSet Symbols {get;}

		[Static, Export ("uppercaseLetterCharacterSet")]
		NSCharacterSet UppercaseLetters {get;}

		[Static, Export ("whitespaceAndNewlineCharacterSet")]
		NSCharacterSet WhitespaceAndNewlines {get;}

		[Static, Export ("whitespaceCharacterSet")]
		NSCharacterSet Whitespaces {get;}

		[Export ("bitmapRepresentation")]
		NSData GetBitmapRepresentation ();

		[Export ("characterIsMember:")]
		bool Contains (char aCharacter);

		[Export ("hasMemberInPlane:")]
		bool HasMemberInPlane (byte thePlane);

		[Export ("invertedSet")]
		NSCharacterSet InvertedSet {get;}

		[Export ("isSupersetOfSet:")]
		bool IsSupersetOf (NSCharacterSet theOtherSet);

		[Export ("longCharacterIsMember:")]
		bool Contains (uint theLongChar);
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSCoder {

		//
		// Encoding and decoding
		//
		[Export ("encodeObject:")]
		void Encode (NSObject obj);
		
		[Export ("encodeRootObject:")]
		void EncodeRoot (NSObject obj);

		[Export ("decodeObject")]
		NSObject DecodeObject ();

		//
		// Encoding and decoding with keys
		// 
		[Export ("encodeConditionalObject:forKey:")]
		void EncodeConditionalObject (NSObject val, string key);
		
		[Export ("encodeObject:forKey:")]
		void Encode (NSObject val, string key);
		
		[Export ("encodeBool:forKey:")]
		void Encode (bool val, string key);
		
		[Export ("encodeDouble:forKey:")]
		void Encode (double val, string key);
		
		[Export ("encodeFloat:forKey:")]
		void Encode (float val, string key);
		
		[Export ("encodeInt32:forKey:")]
		void Encode (int val, string key);
		
		[Export ("encodeInt64:forKey:")]
		void Encode (long val, string key);
		
		[Export ("encodeBytes:length:forKey:")]
		void EncodeBlock (IntPtr bytes, int length, string key);

		[Export ("containsValueForKey:")]
		bool ContainsKey (string key);
		
		[Export ("decodeBoolForKey:")]
		bool DecodeBool (string key);

		[Export ("decodeDoubleForKey:")]
		double DecodeDouble (string key);

		[Export ("decodeFloatForKey:")]
		float DecodeFloat (string key);

		[Export ("decodeInt32ForKey:")]
		int DecodeInt (string key);

		[Export ("decodeInt64ForKey:")]
		long DecodeLong (string key);

		[Export ("decodeObjectForKey:")]
		NSObject DecodeObject (string key);

		[Internal, Export ("decodeBytesForKey:returnedLength:")]
		IntPtr DecodeBytes (string key, IntPtr length_ptr);
	}
	
	[BaseType (typeof (NSPredicate))]
	public interface NSComparisonPredicate {
		[Static, Export ("predicateWithLeftExpression:rightExpression:modifier:type:options:")]
		NSPredicate Create (NSExpression leftExpression, NSExpression rightExpression, NSComparisonPredicateModifier comparisonModifier, NSPredicateOperatorType operatorType, NSComparisonPredicateOptions comparisonOptions);

		[Static, Export ("predicateWithLeftExpression:rightExpression:customSelector:")]
		NSPredicate FromSelector (NSExpression leftExpression, NSExpression rightExpression, Selector selector);

		[Export ("initWithLeftExpression:rightExpression:modifier:type:options:")]
		IntPtr Constructor (NSExpression leftExpression, NSExpression rightExpression, NSComparisonPredicateModifier comparisonModifier, NSPredicateOperatorType operatorType, NSComparisonPredicateOptions comparisonOptions);
		
		[Export ("initWithLeftExpression:rightExpression:customSelector:")]
		IntPtr Constructor (NSExpression leftExpression, NSExpression rightExpression, Selector selector);

		[Export ("predicateOperatorType")]
		NSPredicateOperatorType PredicateOperatorType { get; }

		[Export ("comparisonPredicateModifier")]
		NSComparisonPredicateModifier ComparisonPredicateModifier { get; }

		[Export ("leftExpression")]
		NSExpression LeftExpression { get; }

		[Export ("rightExpression")]
		NSExpression RightExpression { get; }

		[Export ("customSelector")]
		Selector CustomSelector { get; }

		[Export ("options")]
		NSComparisonPredicateOptions Options { get; }
	}

	[BaseType (typeof (NSPredicate))]
	public interface NSCompoundPredicate {
		[Export ("initWithType:subpredicates:")]
		IntPtr Constructor (NSCompoundPredicateType type, NSPredicate[] subpredicates);

		[Export ("compoundPredicateType")]
		NSCompoundPredicateType Type { get; }

		[Export ("subpredicates")]
		NSPredicate[] Subpredicates { get; } 

		[Static]
		[Export ("andPredicateWithSubpredicates:")]
		NSPredicate CreateAndPredicate (NSPredicate[] subpredicates);

		[Static]
		[Export ("orPredicateWithSubpredicates:")]
		NSPredicate CreateOrPredicate (NSPredicate [] subpredicates);

		[Static]
		[Export ("notPredicateWithSubpredicate:")]
		NSPredicate CreateNotPredicate (NSPredicate predicate);

	}

	[BaseType (typeof (NSObject))]
	public interface NSData {
		[Export ("dataWithContentsOfURL:")]
		[Static]
		NSData FromUrl (NSUrl url);

		[Export ("dataWithContentsOfFile:")][Static]
		NSData FromFile (string path);

		[Export ("dataWithBytes:length:"), Static]
		NSData FromBytes (IntPtr bytes, uint size);

		[Export ("bytes")]
		IntPtr Bytes { get; }

		[Export ("length")]
		uint Length { get; }

		[Export ("writeToFile:options:error:")]
		bool _Save (string file, int options, IntPtr addr);
		
		[Export ("writeToURL:options:error:")]
		bool _Save (NSUrl url, int options, IntPtr addr);

		[Export ("rangeOfData:options:range:")]
		[Since (4,0)]
		NSRange Find (NSData dataToFind, NSDataSearchOptions searchOptions, NSRange searchRange);
	}

	[BaseType (typeof (NSObject))]
	public interface NSDateComponents {
		[Since (4,0)]
		[Export ("timeZone")]
		NSTimeZone TimeZone { get; set; }

		[Export ("calendar")]
		[Since (4,0)]
		NSCalendar Calendar { get; set; }

		[Export ("quarter")]
		[Since (4,0)]
		int Quarter { get; set; }

		[Export ("date")]
		[Since (4,0)]
		NSDate Date { get; }

		//Detected properties
		[Export ("era")]
		int Era { get; set; }

		[Export ("year")]
		int Year { get; set; }

		[Export ("month")]
		int Month { get; set; }

		[Export ("day")]
		int Day { get; set; }

		[Export ("hour")]
		int Hour { get; set; }

		[Export ("minute")]
		int Minute { get; set; }

		[Export ("second")]
		int Second { get; set; }

		[Export ("week")]
		int Week { get; set; }

		[Export ("weekday")]
		int Weekday { get; set; }

		[Export ("weekdayOrdinal")]
		int WeekdayOrdinal { get; set; }
	}
	
	[BaseType (typeof (NSFormatter))]
	public interface NSDateFormatter {
		[Export ("stringFromDate:")]
		string ToString (NSDate date);

		[Export ("dateFromString:")]
		NSDate Parse (string date);

		[Export ("dateFormat")]
		string DateFormat { get; set; }

		[Export ("dateStyle")]
		NSDateFormatterStyle DateStyle { get; set; }

		[Export ("timeStyle")]
		NSDateFormatterStyle TimeStyle { get; set; }

		[Export ("locale")]
		NSLocale Locale { get; set; }

		[Export ("generatesCalendarDates")]
		bool GeneratesCalendarDates { get; set; }

		[Export ("formatterBehavior")]
		NSDateFormatterBehavior Behavior { get; set; }

		[Export ("defaultFormatterBehavior"), Static]
		NSDateFormatterBehavior DefaultBehavior { get; set; }

		[Export ("timeZone")]
		NSTimeZone TimeZone { get; set; }

		[Export ("calendar")]
		NSCalendar Calendar { get; set; }

		[Export ("isLenient")]
		bool IsLenient { get; set; } 

		[Export ("twoDigitStartDate")]
		NSDate TwoDigitStartDate { get; set; }

		[Export ("defaultDate")]
		NSDate DefaultDate { get; set; }

		[Export ("eraSymbols")]
		string [] EraSymbols { get; set; }

		[Export ("monthSymbols")]
		string [] MonthSymbols { get; set; }

		[Export ("shortMonthSymbols")]
		string [] ShortMonthSymbols { get; set; }

		[Export ("weekdaySymbols")]
		string [] WeekdaySymbols { get; set; }

		[Export ("shortWeekdaySymbols")]
		string [] ShortWeekdaySymbols { get; set; } 

		[Export ("AMSymbol")]
		string AMSymbol { get; set; }

		[Export ("PMSymbol")]
		string PMSymbol { get; set; }

		[Export ("longEraSymbols")]
		string [] LongEraSymbols { get; set; }

		[Export ("veryShortMonthSymbols")]
		string [] VeryShortMonthSymbols { get; set; }
		
		[Export ("standaloneMonthSymbols")]
		string [] StandaloneMonthSymbols { get; set; }

		[Export ("shortStandaloneMonthSymbols")]
		string [] ShortStandaloneMonthSymbols { get; set; }

		[Export ("veryShortStandaloneMonthSymbols")]
		string [] VeryShortStandaloneMonthSymbols { get; set; }
		
		[Export ("veryShortWeekdaySymbols")]
		string [] VeryShortWeekdaySymbols { get; set; }

		[Export ("standaloneWeekdaySymbols")]
		string [] StandaloneWeekdaySymbols { get; set; }

		[Export ("shortStandaloneWeekdaySymbols")]
		string [] ShortStandaloneWeekdaySymbols { get; set; }
		
		[Export ("veryShortStandaloneWeekdaySymbols")]
		string [] VeryShortStandaloneWeekdaySymbols { get; set; }
		
		[Export ("quarterSymbols")]
		string [] QuarterSymbols { get; set; }

		[Export ("shortQuarterSymbols")]
		string [] ShortQuarterSymbols { get; set; }
		
		[Export ("standaloneQuarterSymbols")]
		string [] StandaloneQuarterSymbols { get; set; }

		[Export ("shortStandaloneQuarterSymbols")]
		string [] ShortStandaloneQuarterSymbols { get; set; }

		[Export ("gregorianStartDate")]
		NSDate GregorianStartDate { get; set; }
	}
	
	//@interface NSFormatter : NSObject <NSCopying, NSCoding>
	[BaseType (typeof (NSObject))]
	public interface NSFormatter {
		[Export ("stringForObjectValue:")]
		string StringFor (NSObject value);

		// - (NSAttributedString *)attributedStringForObjectValue:(id)obj withDefaultAttributes:(NSDictionary *)attrs;

		[Export ("editingStringForObjectValue:")]
		string EditingStringFor (NSObject value);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSKeyedArchiverDelegate {
		[Export ("archiver:didEncodeObject:"), EventArgs ("NSObject")]
		void EncodedObject (NSKeyedArchiver archiver, NSObject obj);
		
		[Export ("archiverDidFinish:")]
		void Finished (NSKeyedArchiver archiver);
		
		[Export ("archiver:willEncodeObject:"), DelegateName ("NSEncodeHook"), DefaultValue (null)]
		NSObject WillEncode (NSKeyedArchiver archiver, NSObject obj);
		
		[Export ("archiverWillFinish:")]
		void Finishing (NSKeyedArchiver archiver);
		
		[Export ("archiver:willReplaceObject:withObject:"), EventArgs ("NSArchiveReplace")]
		void ReplacingObject (NSKeyedArchiver archiver, NSObject oldObject, NSObject newObject);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSKeyedUnarchiverDelegate {
		[Export ("unarchiver:didDecodeObject:"), DelegateName ("NSDecoderCallback"), DefaultValue (null)]
		NSObject DecodedObject (NSKeyedUnarchiver unarchiver, NSObject obj);
		
		[Export ("unarchiverDidFinish:")]
		void Finished (NSKeyedUnarchiver unarchiver);
		
		[Export ("unarchiver:cannotDecodeObjectOfClassName:originalClasses:"), DelegateName ("NSDecoderHandler"), DefaultValue (null)]
		Class CannotDecodeClass (NSKeyedUnarchiver unarchiver, string klass, string [] classes);
		
		[Export ("unarchiverWillFinish:")]
		void Finishing (NSKeyedUnarchiver unarchiver);
		
		[Export ("unarchiver:willReplaceObject:withObject:"), EventArgs ("NSArchiveReplace")]
		void ReplacingObject (NSKeyedUnarchiver unarchiver, NSObject oldObject, NSObject newObject);
	}

	[BaseType (typeof (NSCoder),
		   Delegates=new string [] {"WeakDelegate"},
		   Events=new Type [] { typeof (NSKeyedArchiverDelegate) })]
	public interface NSKeyedArchiver {
		[Export ("initForWritingWithMutableData:")]
		IntPtr Constructor (NSMutableData data);
	
		[Export ("archivedDataWithRootObject:")]
		[Static]
		NSData ArchivedDataWithRootObject (NSObject root);
		
		[Export ("archiveRootObject:toFile:")]
		[Static]
		NSData ArchiveRootObjectToFile (NSObject root, string file);

		[Export ("finishEncoding")]
		void FinishEncoding ();

		[Export ("outputFormat")]
		NSPropertyListFormat PropertyListFormat { get; set; }

		[Wrap ("WeakDelegate")]
		NSKeyedArchiverDelegate Delegate { get; set; }

		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Export ("setClassName:forClass:")]
		void SetClassName (string name, Class kls);

		[Export ("classNameForClass:")]
		string GetClassName (Class kls);
	}
	
	[BaseType (typeof (NSCoder),
		   Delegates=new string [] {"WeakDelegate"},
		   Events=new Type [] { typeof (NSKeyedUnarchiverDelegate) })]
	public interface NSKeyedUnarchiver {
		[Export ("initForReadingWithData:")]
		IntPtr Constructor (NSData data);
	
		[Static, Export ("unarchiveObjectWithData:")]
		NSObject UnarchiveObject (NSData data);
		
		[Static, Export ("unarchiveObjectWithFile:")]
		NSObject UnarchiveFile (string file);

		[Export ("finishDecoding")]
		void FinishDecoding ();

		[Wrap ("WeakDelegate")]
		NSKeyedUnarchiverDelegate Delegate { get; set; }

		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Export ("setClass:forClassName:")]
		void SetClass (Class kls, string codedName);

		[Export ("classForClassName:")]
		Class GetClass (string codedName);
	}

#if MONOMAC
	[BaseType (typeof (NSObject), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSMetadataQueryDelegate)})]
	public interface NSMetadataQuery {
		[Export ("startQuery")]
		bool StartQuery ();

		[Export ("stopQuery")]
		void StopQuery ();

		[Export ("isStarted")]
		bool IsStarted { get; }

		[Export ("isGathering")]
		bool IsGathering { get; }

		[Export ("isStopped")]
		bool IsStopped { get; }

		[Export ("disableUpdates")]
		void DisableUpdates ();

		[Export ("enableUpdates")]
		void EnableUpdates ();

		[Export ("resultCount")]
		int ResultCount { get; }

		[Export ("resultAtIndex:")]
		NSObject ResultAtIndex (int idx);

		[Export ("results")]
		NSMetadataItem[] Results { get; }

		[Export ("indexOfResult:")]
		int IndexOfResult (NSObject result);

		[Export ("valueLists")]
		NSDictionary ValueLists { get; }

		[Export ("groupedResults")]
		NSObject [] GroupedResults { get; }

		[Export ("valueOfAttribute:forResultAtIndex:")]
		NSObject ValueOfAttribute (string attribyteName, int atIndex);

		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSMetadataQueryDelegate WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSMetadataQueryDelegate Delegate { get; set; }
		
		[Export ("predicate")]
		NSPredicate Predicate { get; set; }

		[Export ("sortDescriptors")]
		NSSortDescriptor[] SortDescriptors { get; set; }

		[Export ("valueListAttributes")]
		NSObject[] ValueListAttributes { get; set; }

		[Export ("groupingAttributes")]
		NSArray GroupingAttributes { get; set; }

		[Export ("notificationBatchingInterval")]
		double NotificationBatchingInterval { get; set; }

		[Export ("searchScopes")]
		NSObject [] SearchScopes { get; set; }
		
		// There is no info associated with these notifications
		[Field ("NSMetadataQueryDidStartGatheringNotification")]
		NSString DidStartGatheringNotification { get; }
	
		[Field ("NSMetadataQueryGatheringProgressNotification")]
		NSString GatheringProgressNotification { get; }
		
		[Field ("NSMetadataQueryDidFinishGatheringNotification")]
		NSString DidFinishGatheringNotification { get; }
		
		[Field ("NSMetadataQueryDidUpdateNotification")]
		NSString DidUpdateNotification { get; }
		
		[Field ("NSMetadataQueryResultContentRelevanceAttribute")]
		NSString ResultContentRelevanceAttribute { get; }
		
		// Scope constants for defined search locations
		[Field ("NSMetadataQueryUserHomeScope")]
		NSString UserHomeScope { get; }
		
		[Field ("NSMetadataQueryLocalComputerScope")]
		NSString LocalComputerScope { get; }
		
		[Field ("NSMetadataQueryNetworkScope")]
		NSString QueryNetworkScope { get; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSMetadataQueryDelegate {
		[Export ("metadataQuery:replacementObjectForResultObject:"), DelegateName ("NSMetadataQueryObject"), DefaultValue(null)]
		NSObject ReplacementObjectForResultObject (NSMetadataQuery query, NSMetadataItem result);

		[Export ("metadataQuery:replacementValueForAttribute:value:"), DelegateName ("NSMetadataQueryValue"), DefaultValue(null)]
		NSObject ReplacementValueForAttributevalue (NSMetadataQuery query, string attributeName, NSObject value);
	}

	[BaseType (typeof (NSObject))]
	public interface NSMetadataItem {
		[Export ("valueForAttribute:")]
		NSObject ValueForAttribute (string key);

		[Export ("valuesForAttributes:")]
		NSDictionary ValuesForAttributes (NSArray keys);

		[Export ("attributes")]
		NSObject [] Attributes { get; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSMetadataQueryAttributeValueTuple {
		[Export ("attribute")]
		string Attribute { get; }

		[Export ("value")]
		NSObject Value { get; }

		[Export ("count")]
		int Count { get; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSMetadataQueryResultGroup {
		[Export ("attribute")]
		string Attribute { get; }

		[Export ("value")]
		NSObject Value { get; }

		[Export ("subgroups")]
		NSObject [] Subgroups { get; }

		[Export ("resultCount")]
		int ResultCount { get; }

		[Export ("resultAtIndex:")]
		NSObject ResultAtIndex (uint idx);

		[Export ("results")]
		NSObject [] Results { get; }

	}
#endif

	[Since (3,2)]
	[BaseType (typeof (NSAttributedString))]
	public interface NSMutableAttributedString {
		[Export ("initWithString:")]
		IntPtr Constructor (string str);
		
		[Export ("initWithString:attributes:")]
		IntPtr Constructor (string str, NSDictionary attributes);

		[Export ("initWithAttributedString:")]
		IntPtr Constructor (NSAttributedString other);

		[Export ("replaceCharactersInRange:withString:")]
		void Replace (NSRange range, string newValue);

		[Export ("setAttributes:range:")]
		void SetAttributes (NSDictionary attrs, NSRange range);

		[Export ("addAttribute:value:range:")]
		void AddAttribute (NSString attributeName, NSObject value, NSRange range);

		[Export ("addAttributes:range:")]
		void AddAttributes (NSDictionary attrs, NSRange range);

		[Export ("removeAttribute:range:")]
		void RemoveAttribute (string name, NSRange range);
		
		[Export ("replaceCharactersInRange:withAttributedString:")]
		void Replace (NSRange range, NSAttributedString value);
		
		[Export ("insertAttributedString:atIndex:")]
		void Insert (NSAttributedString attrString, int location);

		[Export ("appendAttributedString:")]
		void Append (NSAttributedString attrString);

		[Export ("deleteCharactersInRange:")]
		void DeleteRange (NSRange range);

		[Export ("setAttributedString:")]
		void SetString (NSAttributedString attrString);

		[Export ("beginEditing")]
		void BeginEditing ();

		[Export ("endEditing")]
		void EndEditing ();
	}

	[BaseType (typeof (NSData))]
	public interface NSMutableData {
		[Static, Export ("dataWithCapacity:")]
		NSMutableData FromCapacity (int capacity);

		[Static, Export ("dataWithLength:")]
		NSMutableData FromLength (int length);
		
		[Static, Export ("data")]
		NSMutableData Create ();
		
		[Export ("setLength:")]
		void SetLength (uint len);

		[Export ("mutableBytes")]
		IntPtr MutableBytes { get; }

		[Export ("initWithCapacity:")]
		IntPtr Constructor (uint len);

		[Export ("appendData:")]
		void AppendData (NSData other);

		[Export ("appendBytes:length:")]
		void AppendBytes (IntPtr bytes, uint len);

		[Export ("setData:")]
		void SetData (NSData data);
	}

	[BaseType (typeof (NSObject))]
	public interface NSDate {
		[Export ("timeIntervalSinceReferenceDate")]
		double SecondsSinceReferenceDate { get; }

		[Export ("dateWithTimeIntervalSinceReferenceDate:")]
		[Static]
		NSDate FromTimeIntervalSinceReferenceDate (double secs);

		[Export ("date")]
		[Static]
		NSDate Now { get; }
		
		[Export ("distantPast")]
		[Static]
		NSDate DistantPast { get; }
		
		[Export ("distantFuture")]
		[Static]
		NSDate DistantFuture { get; }

		[Export ("dateByAddingTimeInterval:")]
		NSDate AddSeconds (double seconds);

		[Export ("description")]
		string Description { get; }

		[Export ("dateWithTimeIntervalSinceNow:")]
		[Static]
		NSDate FromTimeIntervalSinceNow (double secs);
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSDictionary {
		[Export ("dictionaryWithContentsOfFile:")]
		[Static]
		NSDictionary FromFile (string path);

		[Export ("dictionaryWithContentsOfURL:")]
		[Static]
		NSDictionary FromUrl (NSUrl url);

		[Export ("dictionaryWithObject:forKey:")]
		[Static]
		NSDictionary FromObjectAndKey (NSObject obj, NSObject key);

		[Export ("dictionaryWithDictionary:")]
		[Static]
		NSDictionary FromDictionary (NSDictionary source);

		[Export ("dictionaryWithObjects:forKeys:count:")]
		[Static, New, Internal]
		NSDictionary FromObjectsAndKeysInternal (NSArray objects, NSArray keys, int count);

		[Export ("dictionaryWithObjects:forKeys:")]
		[Static, New, Internal]
		NSDictionary FromObjectsAndKeysInternal (NSArray objects, NSArray keys);

		[Export ("initWithDictionary:")]
		IntPtr Constructor (NSDictionary other);

		[Export ("initWithContentsOfFile:")]
		IntPtr Constructor (string fileName);

		[Export ("initWithContentsOfURL:")]
		IntPtr Constructor (NSUrl url);
		
		[Export ("count")]
		uint Count { get; }

		[Export ("objectForKey:")]
		NSObject ObjectForKey (NSObject key);

		[Export ("allKeys")]
		NSObject [] Keys { get; }

		[Export ("allKeysForObject:")]
		NSObject [] KeysForObject (NSObject obj);

		[Export ("allValues")]
		NSObject [] Values { get; }

		[Export ("description")]
		string Description {get; }

		[Export ("descriptionInStringsFileFormat")]
		string DescriptionInStringsFileFormat { get; }

		[Export ("isEqualToDictionary:")]
		bool IsEqualToDictionary (NSDictionary other);
		
		[Export ("objectEnumerator")]
		NSEnumerator ObjectEnumerator { get; }

		[Export ("objectsForKeys:notFoundMarker:")]
		NSObject [] ObjectsForKeys (NSArray keys, NSObject marker);
		
		[Export ("writeToFile:atomically:")]
		bool WriteToFile (string path, bool useAuxiliaryFile);

		[Export ("writeToURL:atomically:")]
		bool WriteToUrl (NSUrl url, bool atomically);
	}

	[BaseType (typeof (NSObject))]
	public interface NSEnumerator {
		[Export ("nextObject")]
		NSObject NextObject (); 
	}

	[BaseType (typeof (NSObject))]
	public interface NSError {
		[Static, Export ("errorWithDomain:code:userInfo:")]
		NSError FromDomain (NSString domain, int code, [NullAllowed] NSDictionary userInfo);

		[Export ("initWithDomain:code:userInfo:")]
		IntPtr Constructor (NSString domain, int code, [NullAllowed] NSDictionary userInfo);
		
		[Export ("domain")]
		string Domain { get; }

		[Export ("code")]
		int Code { get; }

		[Export ("userInfo")]
		NSDictionary UserInfo { get; }

		[Export ("localizedDescription")]
		string LocalizedDescription { get; }

		[Field ("NSCocoaErrorDomain")]
		NSString CocoaErrorDomain { get;}
		[Field ("NSPOSIXErrorDomain")]
		NSString PosixErrorDomain { get; }
		[Field ("NSOSStatusErrorDomain")]
		NSString OsStatusErrorDomain { get; }
		[Field ("NSMachErrorDomain")]
		NSString MachErrorDomain { get; }

		[Field ("NSUnderlyingErrorKey")]
		NSString UnderlyingErrorKey { get; }

		[Field ("NSLocalizedDescriptionKey")]
		NSString LocalizedDescriptionKey { get; }

		[Field ("NSLocalizedFailureReasonErrorKey")]
		NSString LocalizedFailureReasonErrorKey { get; }

		[Field ("NSLocalizedRecoverySuggestionErrorKey")]
		NSString LocalizedRecoverySuggestionErrorKey { get; }

		[Field ("NSLocalizedRecoveryOptionsErrorKey")]
		NSString LocalizedRecoveryOptionsErrorKey { get; }

		[Field ("NSRecoveryAttempterErrorKey")]
		NSString RecoveryAttempterErrorKey { get; }

		[Field ("NSHelpAnchorErrorKey")]
		NSString HelpAnchorErrorKey { get; }

		[Field ("NSStringEncodingErrorKey")]
		NSString StringEncodingErrorKey { get; }

		[Field ("NSURLErrorKey")]
		NSString UrlErrorKey { get; }

		[Field ("NSFilePathErrorKey")]
		NSString FilePathErrorKey { get; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSException {
		[Export ("name")]
		string Name { get; }
	
		[Export ("reason")]
		string Reason { get; }
		
		[Export ("userInfo")]
		NSObject UserInfo { get; }

		[Export ("callStackReturnAddresses")]
		NSNumber[] CallStackReturnAddresses { get; }
	}

	public delegate void NSExpressionHandler (NSObject evaluatedObject, NSExpression [] expressions, NSMutableDictionary context);
	
	[BaseType (typeof (NSObject))]
	public interface NSExpression {

		[Static, Export ("expressionForConstantValue:")]
		NSExpression FromConstant (NSObject obj);

		[Static, Export ("expressionForEvaluatedObject")]
		NSExpression ExpressionForEvaluatedObject { get; }

		[Static, Export ("expressionForVariable:")]
		NSExpression FromVariable (string string1);

		[Static, Export ("expressionForKeyPath:")]
		NSExpression FromKeyPath (string keyPath);

		[Static, Export ("expressionForFunction:arguments:")]
		NSExpression FromFuction (string name, NSExpression[] parameters);

		//+ (NSExpression *)expressionForAggregate:(NSArray *)subexpressions; 
		[Export ("expressionForAggregate:")]
		NSExpression FromAggregate (NSExpression [] subexpressions);

		[Static, Export ("expressionForUnionSet:with:")]
		NSExpression FromUnionSet (NSExpression left, NSExpression right);

		[Static, Export ("expressionForIntersectSet:with:")]
		NSExpression FromIntersectSet (NSExpression left, NSExpression right);

		[Static, Export ("expressionForMinusSet:with:")]
		NSExpression FromMinusSet (NSExpression left, NSExpression right);

		//+ (NSExpression *)expressionForSubquery:(NSExpression *)expression usingIteratorVariable:(NSString *)variable predicate:(id)predicate; 
		[Static, Export ("expressionForSubquery:usingIteratorVariable:predicate:")]
		NSExpression FromSubquery (NSExpression expression, string variable, NSObject predicate);

		[Static, Export ("expressionForFunction:selectorName:arguments:")]
		NSExpression FromFunction (NSExpression target, string name, NSExpression[] parameters);

		[Static, Export ("expressionForBlock:selectorName:arguments:")]
		NSExpression FromFunction (NSExpressionHandler target, NSExpression[] parameters);

		[Export ("initWithExpressionType:")]
		IntPtr Constructor (NSExpressionType type);

		[Export ("expressionType")]
		NSExpressionType ExpressionType { get; }

		[Export ("constantValue")]
		NSObject ConstantValue { get; }

		[Export ("keyPath")]
		string KeyPath { get; }

		[Export ("function")]
		string Function { get; }

		[Export ("variable")]
		string Variable { get; }

		[Export ("operand")]
		NSExpression Operand { get; }

		[Export ("arguments")]
		NSExpression[] Arguments { get; }

		[Export ("collection")]
		NSObject Collection { get; }

		[Export ("predicate")]
		NSPredicate Predicate { get; }

		[Export ("leftExpression")]
		NSExpression LeftExpression { get; }

		[Export ("rightExpression")]
		NSExpression RightExpression { get; }

		[Export ("expressionValueWithObject:context:")]
		NSExpression ExpressionValueWithObject (NSObject object1, NSMutableDictionary context);
	}

	[BaseType (typeof (NSObject))]
	public interface NSNull {
		[Export ("null"), Static]
		NSNull Null { get; }
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSLocale {
		[Static]
		[Export ("systemLocale")]
		NSLocale SystemLocale { get; }

		[Static]
		[Export ("currentLocale")]
		NSLocale CurrentLocale { get; }

		[Export ("initWithLocaleIdentifier:")]
		IntPtr Constructor (string identifier);

		[Export ("localeIdentifier")]
		string LocaleIdentifier { get; }

		[Export ("availableLocaleIdentifiers")][Static]
		string [] AvailableLocaleIdentifiers { get; }

		[Export ("ISOLanguageCodes")][Static]
		string [] ISOLanguageCodes { get; }

		[Export ("ISOCurrencyCodes")][Static]
		string [] ISOCurrencyCodes { get; }

		[Export ("ISOCountryCodes")][Static]
		string ISOCountryCodes { get; }

		[Export ("commonISOCurrencyCodes")][Static]
		string [] CommonISOCurrencyCodes { get; }

		[Export ("preferredLanguages")][Static]
		string [] PreferredLanguages { get; }

		[Export ("componentsFromLocaleIdentifier:")][Static]
		NSDictionary ComponentsFromLocaleIdentifier (string identifier);

		[Export ("localeIdentifierFromComponents:")]
		string LocaleIdentifierFromComponents (NSDictionary dict);

		[Export ("canonicalLocaleIdentifierFromString:")]
		string CanonicalLocaleIdentifierFromString (string str);
	}

	
	[BaseType (typeof (NSObject))]
	public interface NSRunLoop {
		[Export ("currentRunLoop")][Static]
		NSRunLoop Current { get; }

		[Export ("mainRunLoop")][Static]
		NSRunLoop Main { get; }

		[Export ("currentMode")]
		NSString CurrentMode { get; }

		[Export ("getCFRunLoop")]
		CFRunLoop GetCFRunLoop ();

		[Export ("addTimer:forMode:")]
		void AddTimer (NSTimer timer, NSString forMode);

		[Export ("limitDateForMode:")]
		NSDate LimitDateForMode (NSString mode);

		[Export ("acceptInputForMode:beforeDate:")]
		void AcceptInputForMode (NSString mode, NSDate limitDate);

		[Export ("run")]
		void Run ();

		[Export ("runUntilDate:")]
		void RunUntil (NSDate date);

		[Export ("runMode:beforeDate:")]
		void RunUntil (NSString runLoopMode, NSDate limitdate);
		
		[Field ("NSDefaultRunLoopMode")]
		NSString NSDefaultRunLoopMode { get; }

		[Field ("NSRunLoopCommonModes")]
		NSString NSRunLoopCommonModes { get; }

#if MONOMAC
		[Field ("NSConnectionReplyMode")]
		NSString NSRunLoopConnectionReplyMode { get; }

		[Field ("NSModalPanelRunLoopMode", "AppKit")]
		NSString NSRunLoopModalPanelMode { get; }

		[Field ("NSEventTrackingRunLoopMode", "AppKit")]
		NSString NSRunLoopEventTracking { get; }
#endif
	}

	[BaseType (typeof (NSObject))]
	public interface NSSet {
		[Export ("set")][Static]
		NSSet CreateSet ();

		[Export ("initWithArray:")]
		IntPtr Constructor (NSArray other);
		
		[Export ("count")]
		uint Count { get; }

		[Export ("anyObject")]
		NSObject AnyObject { get; }

		[Export ("containsObject:")]
		bool Contains (NSObject id);

		[Export ("allObjects")][Internal]
		IntPtr _AllObjects ();

		[Export ("description")]
		string Description { get; }

		[Export ("isEqualToSet:")]
		bool IsEqualToSet (NSSet other);

		[Export ("isSubsetOfSet:")]
		bool IsSubsetOf (NSSet other);
		[Export ("enumerateObjectsUsingBlock:")]
		[Since (4,0)]
		void Enumerate (NSSetEnumerator enumerator);
	}

	[BaseType (typeof (NSObject))]
	public interface NSSortDescriptor {
		[Export ("initWithKey:ascending:")]
		IntPtr Constructor (string key, bool ascending);

		[Export ("initWithKey:ascending:selector:")]
		IntPtr Constructor (string key, bool ascending, Selector selector);

		[Export ("key")]
		string Key { get; }

		[Export ("ascending")]
		bool Ascending { get; }

		[Export ("selector")]
		Selector Selector { get; }

		[Export ("compareObject:toObject:")]
		NSComparisonResult Compare (NSObject object1, NSObject object2);

		[Export ("reversedSortDescriptor")]
		NSObject ReversedSortDescriptor { get; }
	}

	[BaseType (typeof(NSObject))]
	public interface NSTimer {
		// TODO: scheduledTimerWithTimeInterval:invocation:repeats:

		[Static, Export ("scheduledTimerWithTimeInterval:target:selector:userInfo:repeats:")]
		NSTimer CreateScheduledTimer (double seconds, NSObject target, Selector selector, [NullAllowed] NSObject userInfo, bool repeats);

		// TODO: timerWithTimeInterval:invocation:repeats:

		[Static, Export ("timerWithTimeInterval:target:selector:userInfo:repeats:")]
		NSTimer CreateTimer (double seconds, NSObject target, Selector selector, [NullAllowed] NSObject userInfo, bool repeats);

		[Export ("initWithFireDate:interval:target:selector:userInfo:repeats:")]
		IntPtr Constructor (NSDate date, double seconds, NSObject target, Selector selector, [NullAllowed] NSObject userInfo, bool repeats);

		[Export ("fire")]
		void Fire ();

		[Export ("fireDate")]
		NSDate FireDate { get; set; }

		[Export ("invalidate")]
		void Invalidate ();

		[Export ("isValid")]
		bool IsValid { get; }

		[Export ("timeInterval")]
		double TimeInterval { get; }

		[Export ("userInfo")]
		NSObject UserInfo { get; }
	}

	[BaseType (typeof(NSObject))]
	public interface NSTimeZone {
		[Export ("initWithName:")]
		IntPtr Constructor (string name);
		
		[Export ("name")]
		string Name { get; } 

		[Export ("data")]
		NSData Data { get; }

		[Export ("secondsFromGMTForDate:")]
		int SecondsFromGMT (NSDate date);
		
		[Export ("abbreviationForDate:")]
		string Abbreviation (NSDate date);

		[Export ("isDaylightSavingTimeForDate:")]
		bool IsDaylightSavingsTime (NSDate date);

		[Export ("daylightSavingTimeOffsetForDate:")]
		double DaylightSavingTimeOffset (NSDate date);

		[Export ("nextDaylightSavingTimeTransitionAfterDate:")]
		NSDate NextDaylightSavingTimeTransitionAfter (NSDate date);

		[Static, Export ("timeZoneWithName:")]
		NSTimeZone FromName (string tzName);

		[Static, Export ("localTimeZone")]
		NSTimeZone LocalTimeZone { get; }

		[Export ("description")]
		string Description { get; }

		[Export ("secondsFromGMT")]
		int GetSecondsFromGMT { get; }

		[Export ("defaultTimeZone"), Static]
		NSTimeZone DefaultTimeZone { get; set; }

		[Export ("resetSystemTimeZone"), Static]
		void ResetSystemTimeZone ();

		[Export ("systemTimeZone"), Static]
		NSTimeZone SystemTimeZone { get; }
		
		[Export ("timeZoneWithAbbreviation:"), Static]
		NSTimeZone FromAbbreviation (string abbreviation);
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSUserDefaults {
		[Static]
		[Export ("standardUserDefaults")]
		NSUserDefaults StandardUserDefaults { get; }
	
		[Static]
		[Export ("resetStandardUserDefaults")]
		void ResetStandardUserDefaults ();
	
		[Export ("initWithUser:")]
		IntPtr Constructor (string  username);
	
		[Export ("objectForKey:")][Internal]
		NSObject ObjectForKey (string defaultName);
	
		[Export ("setObject:forKey:")][Internal]
		void SetObjectForKey (NSObject value, string  defaultName);
	
		[Export ("removeObjectForKey:")]
		void RemoveObject (string defaultName);
	
		[Export ("stringForKey:")]
		string StringForKey (string defaultName);
	
		[Export ("arrayForKey:")]
		NSObject [] ArrayForKey (string defaultName);
	
		[Export ("dictionaryForKey:")]
		NSDictionary DictionaryForKey (string defaultName);
	
		[Export ("dataForKey:")]
		NSData DataForKey (string defaultName);
	
		[Export ("stringArrayForKey:")]
		string [] StringArrayForKey (string defaultName);
	
		[Export ("integerForKey:")]
		int IntForKey (string defaultName);
	
		[Export ("floatForKey:")]
		float FloatForKey (string defaultName);
	
		[Export ("doubleForKey:")]
		double DoubleForKey (string defaultName);
	
		[Export ("boolForKey:")]
		bool BoolForKey (string defaultName);
	
		[Export ("setInteger:forKey:")]
		void SetInt (int value, string defaultName);
	
		[Export ("setFloat:forKey:")]
		void SetFloat (float value, string defaultName);
	
		[Export ("setDouble:forKey:")]
		void SetDouble (double value, string defaultName);
	
		[Export ("setBool:forKey:")]
		void SetBool (bool value, string  defaultName);
	
		[Export ("registerDefaults:")]
		void RegisterDefaults (NSDictionary registrationDictionary);
	
		[Export ("addSuiteNamed:")]
		void AddSuite (string suiteName);
	
		[Export ("removeSuiteNamed:")]
		void RemoveSuite (string suiteName);
	
		[Export ("dictionaryRepresentation")]
		NSDictionary AsDictionary ();
	
		[Export ("volatileDomainNames")]
		string [] VolatileDomainNames ();
	
		[Export ("volatileDomainForName:")]
		NSDictionary GetVolatileDomain (string domainName);
	
		[Export ("setVolatileDomain:forName:")]
		void SetVolatileDomain (NSDictionary domain, string domainName);
	
		[Export ("removeVolatileDomainForName:")]
		void RemoveVolatileDomain (string domainName);
	
		[Export ("persistentDomainNames")]
		string [] PersistentDomainNames ();
	
		[Export ("persistentDomainForName:")]
		NSDictionary PersistentDomainForName (string domainName);
	
		[Export ("setPersistentDomain:forName:")]
		void SetPersistentDomain (NSDictionary domain, string domainName);
	
		[Export ("removePersistentDomainForName:")]
		void RemovePersistentDomain (string domainName);
	
		[Export ("synchronize")]
		bool Synchronize ();
	
		[Export ("objectIsForcedForKey:")]
		bool ObjectIsForced (string key);
	
		[Export ("objectIsForcedForKey:inDomain:")]
		bool ObjectIsForced (string key, string domain);
	
	}
	
	[BaseType (typeof (NSObject), Name="NSURL")]
	public interface NSUrl {
		[Export ("initWithScheme:host:path:")]
		IntPtr Constructor (string scheme, string host, string path);

		[Export ("initFileURLWithPath:isDirectory:")]
		IntPtr Constructor (string path, bool isDir);

		[Export ("initWithString:")]
		IntPtr Constructor (string path);

		[Export ("initWithString:relativeToURL:")]
		IntPtr Constructor (string path, string relativeToUrl);		

		[Export ("URLWithString:")][Static]
		NSUrl FromString (string s);

		[Export ("URLWithString:relativeToURL:")][Internal][Static]
		NSUrl _FromStringRelative (string url, NSUrl relative);
		
		[Export ("absoluteString")]
		string AbsoluteString { get; }

		[Export ("absoluteURL")]
		NSUrl AbsoluteUrl { get; }

		[Export ("baseURL")]
		NSUrl BaseUrl { get; }

		[Export ("fragment")]
		string Fragment { get; }

		[Export ("host")]
		string Host { get; }

		[Export ("isEqual:")]
		bool IsEqual (NSUrl other);

		[Export ("isFileURL")]
		bool IsFileUrl { get; }

		[Export ("parameterString")]
		string ParameterString { get;}

		[Export ("password")]
		string Password { get;}

		[Export ("path")]
		string Path { get;}

		[Export ("query")]
		string Query { get;}

		[Export ("relativePath")]
		string RelativePath { get;}

		[Export ("relativeString")]
		string RelativeString { get;}

		[Export ("resourceSpecifier")]
		string ResourceSpecifier { get;}

		[Export ("scheme")]
		string Scheme { get;}

		[Export ("user")]
		string User { get;}

		[Export ("standardizedURL")]
		NSUrl StandardizedUrl { get; }
		
#if MONOMAC && !MONOMAC_BOOTSTRAP

		/* These methods come from NURL_AppKitAdditions */

		[Export ("URLFromPasteboard:")][Static]
		NSUrl FromPasteboard (NSPasteboard pasteboard);

		[Export ("writeToPasteboard:")]
		void WriteToPasteboard (NSPasteboard pasteboard);
		
#endif
		
		//[Export ("port")]
		//NSNumber Port { get;}
	}

	[BaseType (typeof (NSObject), Name="NSURLCache")]
	public interface NSUrlCache {
		[Export ("sharedURLCache"), Static]
		NSUrlCache SharedCache { get; set; }

		[Export ("initWithMemoryCapacity:diskCapacity:diskPath:")]
		IntPtr Constructor (uint memoryCapacity, uint diskCapacity, string diskPath);

		[Export ("cachedResponseForRequest:")]
		NSCachedUrlResponse CachedResponseForRequest (NSUrlRequest request);

		[Export ("storeCachedResponse:forRequest:")]
		void StoreCachedResponse (NSCachedUrlResponse cachedResponse, NSUrlRequest forRequest);

		[Export ("removeCachedResponseForRequest:")]
		void RemoveCachedResponse (NSUrlRequest request);

		[Export ("removeAllCachedResponses")]
		void RemoveAllCachedResponses ();

		[Export ("memoryCapacity")]
		uint MemoryCapacity { get; set; }

		[Export ("diskCapacity")]
		uint DiskCapacity { get; set; }

		[Export ("currentMemoryUsage")]
		uint CurrentMemoryUsage { get; }

		[Export ("currentDiskUsage")]
		uint CurrentDiskUsage { get; }
	}
	
	[BaseType (typeof (NSObject), Name="NSURLAuthenticationChallenge")]
	public interface NSUrlAuthenticationChallenge {
		[Export ("initWithProtectionSpace:proposedCredential:previousFailureCount:failureResponse:error:sender:")]
		IntPtr Constructor (NSUrlProtectionSpace space, NSUrlCredential credential, int previousFailureCount, NSUrlResponse response, NSError error, NSUrlConnection sender);
		
		[Export ("initWithAuthenticationChallenge:sender:")]
		IntPtr Constructor (NSUrlAuthenticationChallenge  challenge, NSUrlConnection sender);
	
		[Export ("protectionSpace")]
		NSUrlProtectionSpace ProtectionSpace { get; }
	
		[Export ("proposedCredential")]
		NSUrlCredential ProposedCredential { get; }
	
		[Export ("previousFailureCount")]
		int PreviousFailureCount { get; }
	
		[Export ("failureResponse")]
		NSUrlResponse FailureResponse { get; }
	
		[Export ("error")]
		NSError Error { get; }
	
		[Export ("sender")]
		NSUrlConnection Sender { get; }
	}

	[BaseType (typeof (NSObject), Name="NSURLConnection")]
	public interface NSUrlConnection {
		[Export ("canHandleRequest:")][Static]
		bool CanHandleRequest (NSUrlRequest request);
	
		[Export ("connectionWithRequest:delegate:")][Static]
		NSUrlConnection FromRequest (NSUrlRequest request, NSUrlConnectionDelegate del);
	
		[Export ("initWithRequest:delegate:")]
		IntPtr Constructor (NSUrlRequest request, NSUrlConnectionDelegate del);
	
		[Export ("initWithRequest:delegate:startImmediately:")]
		IntPtr Constructor (NSUrlRequest request, NSUrlConnectionDelegate del, bool startImmediately);
	
		[Export ("start")]
		void Start ();
	
		[Export ("cancel")]
		void Cancel ();
	
		[Export ("scheduleInRunLoop:forMode:")]
		void Schedule (NSRunLoop aRunLoop, string forMode);
	
		[Export ("unscheduleFromRunLoop:forMode:")]
		void Unschedule (NSRunLoop aRunLoop, string forMode);

		/* Adopted by the NSUrlAuthenticationChallengeSender protocol */
		[Export ("useCredential:forAuthenticationChallenge:")]
		void UseCredentials (NSUrlCredential credential, NSUrlAuthenticationChallenge challenge);
	
		[Export ("continueWithoutCredentialForAuthenticationChallenge:")]
		void ContinueWithoutCredentialForAuthenticationChallenge (NSUrlAuthenticationChallenge  challenge);
	
		[Export ("cancelAuthenticationChallenge:")]
		void CancelAuthenticationChallenge (NSUrlAuthenticationChallenge  challenge);
	}

	[BaseType (typeof (NSObject), Name="NSURLConnectionDelegate")]
	[Model]
	public interface NSUrlConnectionDelegate {
		[Export ("connection:willSendRequest:redirectResponse:")]
		NSUrlRequest WillSendRequest (NSUrlConnection connection, NSUrlRequest request, NSUrlResponse response);

		[Export ("connection:canAuthenticateAgainstProtectionSpace:")]
		bool CanAuthenticateAgainstProtectionSpace (NSUrlConnection connection, NSUrlProtectionSpace protectionSpace);

		[Export ("connection:needNewBodyStream:")]
		NSInputStream NeedNewBodyStream (NSUrlConnection connection, NSUrlRequest request);

		[Export ("connection:didReceiveAuthenticationChallenge:")]
		void ReceivedAuthenticationChallenge (NSUrlConnection connection, NSUrlAuthenticationChallenge challenge);

		[Export ("connection:didCancelAuthenticationChallenge:")]
		void CanceledAuthenticationChallenge (NSUrlConnection connection, NSUrlAuthenticationChallenge challenge);

		[Export ("connectionShouldUseCredentialStorage:")]
		bool ConnectionShouldUseCredentialStorage (NSUrlConnection connection);

		[Export ("connection:didReceiveResponse:")]
		void ReceivedResponse (NSUrlConnection connection, NSUrlResponse response);

		[Export ("connection:didReceiveData:")]
		void ReceivedData (NSUrlConnection connection, NSData data);

		[Export ("connection:didSendBodyData:totalBytesWritten:totalBytesExpectedToWrite:")]
		void SentBodyData (NSUrlConnection connection, int bytesWritten, int totalBytesWritten, int totalBytesExpectedToWrite);

		[Export ("connectionDidFinishLoading:")]
		void FinishedLoading (NSUrlConnection connection);

		[Export ("connection:didFailWithError:")]
		void FailedWithError (NSUrlConnection connection, NSError error);

		[Export ("connection:willCacheResponse:")]
		NSCachedUrlResponse WillCacheResponse (NSUrlConnection connection, NSCachedUrlResponse cachedResponse);
	}

	[BaseType (typeof (NSObject), Name="NSURLCredential")]
	public interface NSUrlCredential {
		[Export ("persistence")]
		NSUrlCredentialPersistence Persistence { get; }

		[Export ("initWithUser:password:persistence:")]
		IntPtr Constructor (string  user, string password, NSUrlCredentialPersistence persistence);
	
		[Static]
		[Export ("credentialWithUser:password:persistence:")]
		NSUrlCredential FromUserPasswordPersistance (string user, string password, NSUrlCredentialPersistence persistence);

		[Export ("user")]
		string User { get; }
	
		[Export ("password")]
		string Password { get; }
	
		[Export ("hasPassword")]
		bool HasPassword {get; }
	
		//[Export ("initWithIdentity:certificates:persistence:")]
		//IntPtr Constructor (IntPtr SecIdentityRef, IntPtr [] secCertificateRefArray, NSUrlCredentialPersistence persistance);
	
		//[Static]
		//[Export ("credentialWithIdentity:certificates:persistence:")]
		//NSUrlCredential FromIdentityCertificatesPersistance (IntPtr SecIdentityRef, IntPtr [] secCertificateRefArray, NSUrlCredentialPersistence persistence);
	
		[Export ("identity")]
		IntPtr Identity  {get; }
	
		//[Export ("certificates")]
		//IntPtr [] Certificates { get; }
	
		[Export ("initWithTrust:")]
		IntPtr Constructor (IntPtr SecTrustRef_trust, bool ignored);
	
		[Static]
		[Export ("credentialForTrust:")]
		NSUrlCredential FromTrust (IntPtr SecTrustRef_trust);
	
	}
		
	[BaseType (typeof (NSObject))]
	public interface NSUndoManager {
		[Export ("beginUndoGrouping")]
		void BeginUndoGrouping ();
		
		[Export ("endUndoGrouping")]
		void EndUndoGrouping ();
		
		[Export ("groupingLevel")]
		int GroupingLevel { get; }
		
		[Export ("disableUndoRegistration")]
		void DisableUndoRegistration ();

		[Export ("enableUndoRegistration")]
		void EnableUndoRegistration ();

		[Export ("isUndoRegistrationEnabled")]
		bool IsUndoRegistrationEnabled { get; }
		
		[Export ("groupsByEvent")]
		bool GroupsByEvent { get; set; }
		
		[Export ("levelsOfUndo")]
		int LevelsOfUndo { get; set; }
		
		[Export ("runLoopModes")]
		string [] RunLoopModes { get; set; } 
		
		[Export ("undo")]
		void Undo ();
		
		[Export ("redo")]
		void Redo ();
		
		[Export ("undoNestedGroup")]
		void UndoNestedGroup ();
		
		[Export ("canUndo")]
		bool CanUndo { get; }
		
		[Export ("canRedo")]
		bool CanRedo { get; }

		[Export ("isUndoing")]
		bool IsUndoing { get; }

		[Export ("isRedoing")]
		bool IsRedoing { get; }

		[Export ("removeAllActions")]
		void RemoveAllActions ();

		[Export ("removeAllActionsWithTarget:")]
		void RemoveAllActions (NSObject target);

		[Export ("registerUndoWithTarget:selector:object:")]
		void RegisterUndoWithTarget (NSObject target, Selector selector, NSObject anObject);

		[Export ("prepareWithInvocationTarget:")]
		NSObject PrepareWithInvocationTarget (NSObject target);

		[Export ("undoActionName")]
		string UndoActionName { get; }

		[Export ("redoActionName")]
		string RedoActionName { get; }

		[Export ("setActionName:")]
		void SetActionname (string actionName);

		[Export ("undoMenuItemTitle")]
		string UndoMenuItemTitle { get; }

		[Export ("redoMenuItemTitle")]
		string RedoMenuItemTitle { get; }

		[Export ("undoMenuTitleForUndoActionName:")]
		string UndoMenuTitleForUndoActionName (string name);

		[Export ("redoMenuTitleForUndoActionName:")]
		string RedoMenuTitleForUndoActionName (string name);

		[Field ("NSUndoManagerCheckpointNotification")]
		NSString CheckpointNotification { get; }

		[Field ("NSUndoManagerDidOpenUndoGroupNotification")]
		NSString DidOpenUndoGroupNotification { get; }

		[Field ("NSUndoManagerDidRedoChangeNotification")]
		NSString DidRedoChangeNotification { get; }

		[Field ("NSUndoManagerDidUndoChangeNotification")]
		NSString DidUndoChangeNotification { get; }

		[Field ("NSUndoManagerWillCloseUndoGroupNotification")]
		NSString WillCloseUndoGroupNotification { get; }

		[Field ("NSUndoManagerWillRedoChangeNotification")]
		NSString WillRedoChangeNotification { get; }

		[Field ("NSUndoManagerWillUndoChangeNotification")]
		NSString WillUndoChangeNotification { get; }
	}
	
	[BaseType (typeof (NSObject), Name="NSURLProtectionSpace")]
	public interface NSUrlProtectionSpace {
		
		[Export ("initWithHost:port:protocol:realm:authenticationMethod:")]
		IntPtr Constructor (string host, int port, string protocol, string realm, string authenticationMethod);
	
		//[Export ("initWithProxyHost:port:type:realm:authenticationMethod:")]
		//IntPtr Constructor (string  host, int port, string type, string  realm, string authenticationMethod);
	
		[Export ("realm")]
		string Realm { get; }
	
		[Export ("receivesCredentialSecurely")]
		bool ReceivesCredentialSecurely { get; }
	
		[Export ("isProxy")]
		bool IsProxy { get; }
	
		[Export ("host")]
		string Host { get; }
	
		[Export ("port")]
		int  Port { get; }
	
		[Export ("proxyType")]
		string ProxyType { get; }
	
		[Export ("protocol")]
		string Protocol { get; }
	
		[Export ("authenticationMethod")]
		string AuthenticationMethod { get; }

		// NSURLProtectionSpace(NSClientCertificateSpace)

		[Export ("distinguishedNames")]
		NSData [] DistinguishedNames { get; }
		
		// NSURLProtectionSpace(NSServerTrustValidationSpace)
		[Export ("serverTrust")]
		IntPtr ServerTrust { get ; }
	}
	
	[BaseType (typeof (NSObject), Name="NSURLRequest")]
	public interface NSUrlRequest {
		[Export ("initWithURL:")]
		IntPtr Constructor (NSUrl url);

		[Export ("initWithURL:cachePolicy:timeoutInterval:")]
		IntPtr Constructor (NSUrl url, NSUrlRequestCachePolicy cachePolicy, double timeoutInterval);

		[Export ("requestWithURL:")][Static]
		NSUrlRequest FromUrl (NSUrl url);

		[Export ("URL")]
		NSUrl Url { get; }

		[Export ("cachePolicy")]
		NSUrlRequestCachePolicy CachePolicy { get; }

		[Export ("timeoutInterval")]
		double TimeoutInterval { get; }

		[Export ("mainDocumentURL")]
		NSUrl MainDocumentURL { get; }

		[Export ("HTTPMethod")]
		string HttpMethod { get; }

		[Export ("allHTTPHeaderFields")]
		NSDictionary Headers { get; }

		[Internal][Export ("valueForHTTPHeaderField:")]
		string Header (string field);

		[Export ("HTTPBody")]
		NSData Body { get; }

		[Export ("HTTPBodyStream")]
		NSInputStream BodyStream { get; }

		[Export ("HTTPShouldHandleCookies")]
		bool ShouldHandleCookies { get; }
	}

	[BaseType (typeof (NSDictionary))]
	public interface NSMutableDictionary {
		[Export ("dictionaryWithContentsOfFile:")]
		[Static]
		NSMutableDictionary FromFile (string path);

		[Export ("dictionaryWithContentsOfURL:")]
		[Static]
		NSMutableDictionary FromUrl (NSUrl url);

		[Export ("dictionaryWithObject:forKey:")]
		[Static]
		NSMutableDictionary FromObjectAndKey (NSObject obj, NSObject key);

		[Export ("dictionaryWithDictionary:")]
		[Static,New]
		NSMutableDictionary FromDictionary (NSDictionary source);

		[Export ("dictionaryWithObjects:forKeys:count:")]
		[Static, Internal]
		NSMutableDictionary FromObjectsAndKeysInternalCount (NSArray objects, NSArray keys, int count);

		[Export ("dictionaryWithObjects:forKeys:")]
		[Static, Internal]
		NSMutableDictionary FromObjectsAndKeysInternal (NSArray objects, NSArray Keys);
		
		[Export ("initWithDictionary:")]
		IntPtr Constructor (NSDictionary other);

		[Export ("initWithContentsOfFile:")]
		IntPtr Constructor (string fileName);

		[Export ("initWithContentsOfURL:")]
		IntPtr Constructor (NSUrl url);
		
		[Export ("removeAllObjects"), Internal]
		void RemoveAllObjects ();

		[Export ("removeObjectForKey:"), Internal]
		void RemoveObjectForKey (NSObject key);

		[Export ("setObject:forKey:"), Internal]
		void SetObject (NSObject obj, NSObject key);
	}

	[BaseType (typeof (NSSet))]
	public interface NSMutableSet {
		[Export ("addObject:")]
		void Add (NSObject nso);

		[Export ("removeObject:")]
		void Remove (NSObject nso);
	}
	
	[BaseType (typeof (NSUrlRequest), Name="NSMutableURLRequest")]
	public interface NSMutableUrlRequest {
		[Export ("initWithURL:")]
		IntPtr Constructor (NSUrl url);

		[Export ("initWithURL:cachePolicy:timeoutInterval:")]
		IntPtr Constructor (NSUrl url, NSUrlRequestCachePolicy cachePolicy, double timeoutInterval);

		[New][Export ("URL")]
		NSUrl Url { get; set; }

		[New][Export ("cachePolicy")]
		NSUrlRequestCachePolicy CachePolicy { get; set; }

		[New][Export ("timeoutInterval")]
		double TimeoutInterval { set; get; }

		[New][Export ("mainDocumentURL")]
		NSUrl MainDocumentURL { get; set; }

		[New][Export ("HTTPMethod")]
		string HttpMethod { get; set; }

		[New][Export ("allHTTPHeaderFields")]
		NSDictionary Headers { get; set; }

		[Internal][Export ("setValue:forHTTPHeaderField:")]
		void _SetValue (string value, string field);

		[New][Export ("HTTPBody")]
		NSData Body { get; set; }

		[New][Export ("HTTPBodyStream")]
		NSInputStream BodyStream { get; }

		[New][Export ("HTTPShouldHandleCookies")]
		bool ShouldHandleCookies { get; set; }
		
	}
	
	[BaseType (typeof (NSObject), Name="NSURLResponse")]
	public interface NSUrlResponse {
		[Export ("initWithURL:MIMEType:expectedContentLength:textEncodingName:")]
		IntPtr Constructor (NSUrl url, string mimetype, int expectedContentLength, [NullAllowed] string textEncodingName);

		[Export ("URL")]
		NSUrl Url { get; }

		[Export ("MIMEType")]
		string MimeType { get; }

		[Export ("expectedContentLength")]
		long ExpectedContentLength { get; }

		[Export ("textEncodingName")]
		string TextEncodingName { get; }

		[Export ("suggestedFilename")]
		string SuggestedFilename { get; }
	}

	[BaseType (typeof (NSObject), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (NSStreamDelegate)} )]
	public interface NSStream {
		[Export ("open")]
		void Open ();

		[Export ("close")]
		void Close ();
	
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSStreamDelegate Delegate { get; set; }

		[Export ("propertyForKey:")]
		NSObject PropertyForKey (string  key);
	
		[Export ("setProperty:forKey:")]
		bool SetPropertyForKey (NSObject property, string key);
	
		[Export ("scheduleInRunLoop:forMode:")]
		void Schedule (NSRunLoop aRunLoop, string mode);
	
		[Export ("removeFromRunLoop:forMode:")]
		void Unschedule (NSRunLoop aRunLoop, string mode);
	
		[Export ("streamStatus")]
		NSStreamStatus Status { get; }
	
		[Export ("streamError")]
		NSError Error { get; }

#if !MONOMAC
		[Field ("NSStreamNetworkServiceTypeVoIP")]
		NSString NetworkServiceTypeVoIP { get; }
#endif
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSStreamDelegate {
		[Export ("stream:handleEvent:"), EventArgs ("NSStream"), EventName ("OnEvent")]
		void HandleEvent (NSStream theStream, NSStreamEvent streamEvent);
	}

	[BaseType (typeof (NSObject)), Bind ("NSString")]
	public interface NSString2 {
#if MONOMAC
		[Bind ("sizeWithAttributes:")]
		SizeF StringSize (NSDictionary attributedStringAttributes);
		
		[Bind ("boundingRectWithSize:options:attributes:")]
		SizeF BoundingRectWithSize (SizeF size, NSStringDrawingOptions options, NSDictionary attributes);
		
		[Bind ("drawAtPoint:withAttributes:")]
		void DrawString (PointF point, NSDictionary attributes);
		
		[Bind ("drawInRect:withAttributes:")]
		void DrawString (RectangleF rect, NSDictionary attributes);
		
		[Bind ("drawWithRect:options:attributes:")]
		void DrawString (RectangleF rect, NSStringDrawingOptions options, NSDictionary attributes);
#else
		[Bind ("sizeWithFont:")]
		SizeF StringSize (UIFont font);
		
		[Bind ("sizeWithFont:forWidth:lineBreakMode:")]
		SizeF StringSize (UIFont font, float forWidth, UILineBreakMode breakMode);
		
		[Bind ("sizeWithFont:constrainedToSize:")]
		SizeF StringSize (UIFont font, SizeF constrainedToSize);
		
		[Bind ("sizeWithFont:constrainedToSize:lineBreakMode:")]
		SizeF StringSize (UIFont font, SizeF constrainedToSize, UILineBreakMode lineBreakMode);

		[Bind ("drawAtPoint:withFont:")]
		SizeF DrawString (PointF point, UIFont font);

		[Bind ("drawAtPoint:forWidth:withFont:lineBreakMode:")]
		SizeF DrawString (PointF point, float width, UIFont font, UILineBreakMode breakMode);

		[Bind ("drawAtPoint:forWidth:withFont:fontSize:lineBreakMode:baselineAdjustment:")]
		SizeF DrawString (PointF point, float width, UIFont font, float fontSize, UILineBreakMode breakMode, UIBaselineAdjustment adjustment);

		[Bind ("drawAtPoint:forWidth:withFont:minFontSize:actualFontSize:lineBreakMode:baselineAdjustment:")]
		SizeF DrawString (PointF point, float width, UIFont font, float minFontSize, float actualFontSize, UILineBreakMode breakMode, UIBaselineAdjustment adjustment);

		[Bind ("drawInRect:withFont:")]
		SizeF DrawString (RectangleF rect, UIFont font);

		[Bind ("drawInRect:withFont:lineBreakMode:")]
		SizeF DrawString (RectangleF rect, UIFont font, UILineBreakMode mode);

		[Bind ("drawInRect:withFont:lineBreakMode:alignment:")]
		SizeF DrawString (RectangleF rect, UIFont font, UILineBreakMode mode, UITextAlignment alignment);

		// [Bind ("sizeWithFont:minFontSize:actualFontSize:forWidth:lineBreakMode:")]
		// TODO: need "ref" support for floats.
#endif
		[Export ("characterAtIndex:")]
		char _characterAtIndex (int index);

		[Export ("length")]
		int Length {get;}
	}
	
	[BaseType (typeof (NSStream))]
	public interface NSInputStream {
		//[Export ("read:maxLength:")]
		//int Read (byte [] buffer, uint len);
		
		//[Export ("getBuffer:length:")]
		//bool GetBuffer (ref byte buffer, ref uint len);
	
		[Export ("hasBytesAvailable")]
		bool HasBytesAvailable ();
	
		[Export ("initWithFileAtPath:")]
		IntPtr Constructor (string path);
	
		[Static]
		[Export ("inputStreamWithData:")]
		NSInputStream FromData (NSData data);
	
		[Static]
		[Export ("inputStreamWithFileAtPath:")]
		NSInputStream FromFile (string  path);
	}

	//
	// We expose NSString versions of these methods because it could
	// avoid an extra lookup in cases where there is a large volume of
	// calls being made and the keys are mostly tokens
	//
	[BaseType (typeof (NSObject)), Bind ("NSObject")]
	public interface NSObject2 {
		[Export ("observeValueForKeyPath:ofObject:change:context:")]
		void ObserveValue (NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context);

		[Export ("addObserver:forKeyPath:options:context:")]
		void AddObserver (NSObject observer, NSString keyPath, NSKeyValueObservingOptions options, IntPtr context);

		[Export ("removeObserver:forKeyPath:")]
		void RemoveObserver (NSObject observer, NSString keyPath);

		[Export ("willChangeValueForKey:")]
		void WillChangeValue (string forKey);

		[Export ("didChangeValueForKey:")]
		void DidChangeValue (string forKey);

		[Export ("willChange:valuesAtIndexes:forKey:")]
		void WillChange (NSKeyValueChange changeKind, NSIndexSet indexes, NSString forKey);

		[Export ("didChange:valuesAtIndexes:forKey:")]
		void DidChange (NSKeyValueChange changeKind, NSIndexSet indexes, NSString forKey);

		[Export ("willChangeValueForKey:withSetMutation:usingObjects:")]
		void WillChange (NSString forKey, NSKeyValueSetMutationKind mutationKind, NSSet objects);

		[Export ("didChangeValueForKey:withSetMutation:usingObjects:")]
		void DidChange (NSString forKey, NSKeyValueSetMutationKind mutationKind, NSSet objects);

		[Static, Export ("keyPathsForValuesAffectingValueForKey:")]
		NSSet GetKeyPathsForValuesAffecting (NSString key);

		[Static, Export ("automaticallyNotifiesObserversForKey:")]
		bool AutomaticallyNotifiesObserversForKey (string key);

		[Export ("valueForKey:")]
		NSObject ValueForKey (NSString key);

		[Export ("setValue:forKey:")]
		void SetValueForKey (NSObject value, NSString key);

		[Export ("valueForKeyPath:")]
		NSObject ValueForKeyPath (NSString keyPath);

		[Export ("setValue:forKeyPath:")]
		void SetValueForKeyPath (NSObject value, NSString keyPath);

		[Export ("valueForUndefinedKey:")]
		NSObject ValueForUndefinedKey (NSString key);

		[Export ("setValue:forUndefinedKey:")]
		void SetValueForUndefinedKey (NSObject value, NSString undefinedKey);

		[Export ("setNilValueForKey:")]
		void SetNilValueForKey (NSString key);

		[Export ("dictionaryWithValuesForKeys:")]
		NSDictionary GetDictionaryOfValuesFromKeys (NSString [] keys);

		[Export ("setValuesForKeysWithDictionary:")]
		void SetValuesForKeysWithDictionary (NSDictionary keyedValues);
		
		[Field ("NSKeyValueChangeKindKey")]
		NSString ChangeKindKey { get; }

		[Field ("NSKeyValueChangeNewKey")]
		NSString ChangeNewKey { get; }

		[Field ("NSKeyValueChangeOldKey")]
		NSString ChangeOldKey { get; }

		[Field ("NSKeyValueChangeIndexesKey")]
		NSString ChangeIndexesKey { get; }

		[Field ("NSKeyValueChangeNotificationIsPriorKey")]
		NSString ChangeNotificationIsPriorKey { get; }

		// Cocoa Bindings added by Kenneth J. Pouncey 2010/11/17
		[Export ("exposedBindings")]
		NSString[] ExposedBindings ();

		[Export ("valueClassForBinding:")]
		Class BindingValueClass (string binding);

		[Export ("bind:toObject:withKeyPath:options:")]
		void Bind (string binding, NSObject observable, string keyPath, [NullAllowed] NSDictionary options);

		[Export ("unbind:")]
		void Unbind (string binding);

		[Export ("infoForBinding:")]
		NSDictionary BindingInfo (string binding);

		[Export ("optionDescriptionsForBinding:")]
		NSObject[] BindingOptionDescriptions (string aBinding);

		[Static]
		[Export ("defaultPlaceholderForMarker:withBinding:")]
		NSObject GetDefaultPlaceholder (NSObject marker, string binding);

		[Export ("objectDidEndEditing:")]
		void ObjectDidEndEditing (NSObject editor);

		[Export ("commitEditing")]
		bool CommitEditing ();

		[Export ("commitEditingWithDelegate:didCommitSelector:contextInfo:")]
		//void CommitEditingWithDelegateDidCommitSelectorContextInfo (NSObject objDelegate, Selector didCommitSelector, IntPtr contextInfo);
		void CommitEditing (NSObject objDelegate, Selector didCommitSelector, IntPtr contextInfo);
	}
	
	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	public interface NSOperation {
		[Export ("start")]
		void Start ();

		[Export ("main")]
		void Main ();

		[Export ("isCancelled")]
		bool IsCancelled { get; }

		[Export ("cancel")]
		void Cancel ();

		[Export ("isExecuting")]
		bool IsExecuting { get; }

		[Export ("isFinished")]
		bool IsFinished { get; }

		[Export ("isConcurrent")]
		bool IsConcurrent { get; }

		[Export ("isReady")]
		bool IsReady { get; }

		[Export ("addDependency:")]
		void AddDependency (NSOperation op);

		[Export ("removeDependency:")]
		void RemoveDependency (NSOperation op);

		[Export ("dependencies")]
		NSOperation [] Dependencies { get; }

		[Export ("waitUntilFinished")]
		void WaitUntilFinishedNS ();

		[Export ("threadPriority")]
		double ThreadPriority { get; set; }

		//Detected properties
		[Export ("queuePriority")]
		NSOperationQueuePriority QueuePriority { get; set; }
	}

	[BaseType (typeof (NSOperation))]
	[Since (4,0)]
	public interface NSBlockOperation {
		[Static]
		[Export ("blockOperationWithBlock:")]
		NSBlockOperation Create (NSAction method);

		[Export ("addExecutionBlock:")]
		void AddExecutionBlock (NSAction method);

		[Export ("executionBlocks")]
		NSObject [] ExecutionBlocks { get; }
	}

	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	public interface NSOperationQueue {
		[Export ("addOperation:")]
		void AddOperation (NSOperation op);

		[Export ("addOperations:waitUntilFinished:")]
		void AddOperations (NSOperation [] operations, bool waitUntilFinished);

		[Export ("addOperationWithBlock:")]
		void AddOperation (NSAction operation);

		[Export ("operations")]
		NSOperation [] Operations { get; }

		[Export ("operationCount")]
		int OperationCount { get; }

		[Export ("name")]
		string Name { get; set; }

		[Export ("cancelAllOperations")]
		void CancelAllOperations ();

		[Export ("waitUntilAllOperationsAreFinished")]
		void WaitUntilAllOperationsAreFinished ();

		[Static]
		[Export ("currentQueue")]
		NSOperationQueue CurrentQueue { get; }

		[Static]
		[Export ("mainQueue")]
		NSOperationQueue MainQueue { get; }

		//Detected properties
		[Export ("maxConcurrentOperationCount")]
		int MaxConcurrentOperationCount { get; set; }

		[Export ("suspended")]
		bool Suspended { [Bind ("isSuspended")]get; set; }
	}
	
	[BaseType (typeof (NSStream))]
	public interface NSOutputStream {
		[Export ("hasSpaceAvailable")]
		bool HasSpaceAvailable ();
	
		//[Export ("initToBuffer:capacity:")]
		//IntPtr Constructor (uint8_t  buffer, NSUInteger capacity);

		[Export ("initToFileAtPath:append:")]
		IntPtr Constructor (string  path, bool shouldAppend);

		[Static]
		[Export ("outputStreamToMemory")]
		NSObject OutputStreamToMemory ();

		//[Static]
		//[Export ("outputStreamToBuffer:capacity:")]
		//NSObject OutputStreamToBuffer (uint8_t  buffer, NSUInteger capacity);

		[Static]
		[Export ("outputStreamToFileAtPath:append:")]
		NSOutputStream CreateFile (string path, bool shouldAppend);
	}

	[BaseType (typeof (NSObject), Name="NSHTTPCookie")]
	public interface NSHttpCookie {
		[Export ("initWithProperties:")]
		IntPtr Constructor (NSDictionary properties);

		[Export ("cookieWithProperties:"), Static]
		NSHttpCookie CookieFromProperties (NSDictionary properties);

		[Export ("requestHeaderFieldsWithCookies:"), Static]
		NSDictionary RequestHeaderFieldsWithCookies (NSHttpCookie [] cookies);

		[Export ("cookiesWithResponseHeaderFields:forURL:"), Static]
		NSHttpCookie [] CookiesWithResponseHeaderFields (NSDictionary headerFields, NSUrl url);

		[Export ("properties")]
		NSDictionary Properties { get; }

		[Export ("version")]
		uint Version { get; }

		[Export ("value")]
		string Value { get; }

		[Export ("expiresDate")]
		NSDate ExpiresDate { get; }

		[Export ("isSessionOnly")]
		bool IsSessionOnly { get; }

		[Export ("domain")]
		string Domain { get; }

		[Export ("name")]
		string Name { get; }

		[Export ("path")]
		string Path { get; }

		[Export ("isSecure")]
		bool IsSecure { get; }

		[Export ("isHTTPOnly")]
		bool IsHttpOnly { get; }

		[Export ("comment")]
		string Comment { get; }

		[Export ("commentURL")]
		NSUrl CommentUrl { get; }

		[Export ("portList")]
		NSNumber [] PortList { get; }
	}

	[BaseType (typeof (NSObject), Name="NSHTTPCookieStorage")]
	public interface NSHttpCookieStorage {
		[Export ("sharedHTTPCookieStorage"), Static]
		NSHttpCookieStorage SharedStorage { get; }

		[Export ("cookies")]
		NSHttpCookie [] Cookies { get; }

		[Export ("setCookie:")]
		void SetCookie (NSHttpCookie cookie);

		[Export ("deleteCookie:")]
		void DeleteCookie (NSHttpCookie cookie);

		[Export ("cookiesForURL:")]
		NSHttpCookie [] CookiesForUrl (NSUrl url);

		[Export ("setCookies:forURL:mainDocumentURL:")]
		void SetCookies (NSHttpCookie [] cookies, NSUrl forUrl, NSUrl mainDocumentUrl);

		[Export ("cookieAcceptPolicy")]
		NSHttpCookieAcceptPolicy AcceptPolicy { get; set; }
	}
	
	[BaseType (typeof (NSUrlResponse), Name="NSHTTPURLResponse")]
	public interface NSHttpUrlResponse {
		[Export ("statusCode")]
		int StatusCode { get; }

		[Export ("allHeaderFields")]
		NSDictionary AllHeaderFields { get; }

		[Export ("localizedStringForStatusCode:")][Static]
		string LocalizedStringForStatusCode (int statusCode);
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSBundle {
		[Export ("mainBundle")][Static]
		NSBundle MainBundle { get; }

		[Export ("bundleWithPath:")][Static]
		NSBundle FromPath (string path);

		[Export ("initWithPath:")]
		IntPtr Constructor (string path);

		[Export ("bundleForClass:")][Static]
		NSBundle FromClass (Class c);

		[Export ("bundleWithIdentifier:")][Static]
		NSBundle FromIdentifier (string str);

		[Export ("allBundles")][Static]
		NSBundle [] _AllBundles { get; }

		[Export ("allFrameworks")][Static]
		NSBundle [] AllFrameworks { get; }

		[Export ("load")]
		void Load ();

		[Export ("isLoaded")]
		bool IsLoaded { get; }

		[Export ("unload")]
		void Unload ();

		[Export ("bundlePath")]
		string BundlePath { get; }
		
		[Export ("resourcePath")]
		string  ResourcePath { get; }
		
		[Export ("executablePath")]
		string ExecutablePath { get; }
		
		[Export ("pathForAuxiliaryExecutable:")]
		string PathForAuxiliaryExecutable (string s);
		

		[Export ("privateFrameworksPath")]
		string PrivateFrameworksPath { get; }
		
		[Export ("sharedFrameworksPath")]
		string SharedFrameworksPath { get; }
		
		[Export ("sharedSupportPath")]
		string SharedSupportPath { get; }
		
		[Export ("builtInPlugInsPath")]
		string BuiltinPluginsPath { get; }
		
		[Export ("bundleIdentifier")]
		string BundleIdentifier { get; }

		[Export ("classNamed:")]
		Class ClassNamed (string className);
		
		[Export ("principalClass")]
		Class PrincipalClass { get; }

		[Export ("pathForResource:ofType:inDirectory:")][Static]
		string PathForResourceAbsolute (string name, [NullAllowed] string ofType, string bundleDirectory);
		
		[Export ("pathForResource:ofType:")]
		string PathForResource (string name, [NullAllowed] string ofType);

		// TODO: this conflicts with the above with our generator.
		//[Export ("pathForResource:ofType:inDirectory:")]
		//string PathForResource (string name, string ofType, string subpath);
		
		[Export ("pathForResource:ofType:inDirectory:forLocalization:")]
		string PathForResource (string name, [NullAllowed] string ofType, string subpath, string localizationName);

		[Export ("localizedStringForKey:value:table:")]
		string LocalizedString (string key, string value, string table);

		[Export ("objectForInfoDictionaryKey:")]
		NSObject ObjectForInfoDictionary (string key);

		[Export ("developmentLocalization")]
		string DevelopmentLocalization { get; }
		
		[Export ("infoDictionary")]
		NSDictionary InfoDictionary{ get; }

		// Additions from AppKit
		[Static]
		[Export ("loadNibNamed:owner:")]
		bool LoadNib (string nibName, NSObject owner);
		
		[Export ("loadNibNamed:owner:options:")]
		NSArray LoadNib (string nibName, NSObject owner, [NullAllowed] NSDictionary options);

		[Export ("pathForImageResource:")]
		string PathForImageResource (string resource);

		[Export ("pathForSoundResource:")]
		string PathForSoundResource (string resource);

		[Export ("bundleURL")]
		[Since (4,0)]
		NSUrl BundleUrl { get; }
		
		[Export ("resourceURL")]
		[Since (4,0)]
		NSUrl ResourceUrl { get; }

		[Export ("executableURL")]
		[Since (4,0)]
		NSUrl ExecutableUrl { get; }

		[Export ("URLForAuxiliaryExecutable:")]
		[Since (4,0)]
		NSUrl UrlForAuxiliaryExecutable (string executable);

		[Export ("privateFrameworksURL")]
		[Since (4,0)]
		NSUrl PrivateFrameworksUrl { get; }

		[Export ("sharedFrameworksURL")]
		[Since (4,0)]
		NSUrl SharedFrameworksUrl { get; }

		[Export ("sharedSupportURL")]
		[Since (4,0)]
		NSUrl SharedSupportUrl { get; }

		[Export ("builtInPlugInsURL")]
		[Since (4,0)]
		NSUrl BuiltInPluginsUrl { get; }

		[Export ("initWithURL:")]
		[Since (4,0)]
		IntPtr Constructor (NSUrl url);
		
		[Static, Export ("bundleWithURL:")]
		[Since (4,0)]
		NSBundle FromUrl (NSUrl url);
	}

	[BaseType (typeof (NSObject))]
	public interface NSIndexPath {
		[Export ("indexPathWithIndex:")][Static]
		NSIndexPath FromIndex (uint index);

		[Export ("indexPathWithIndexes:length:")][Internal][Static]
		NSIndexPath _FromIndex (IntPtr indexes, int len);

		[Export ("indexPathByAddingIndex:")]
		NSIndexPath IndexPathByAddingIndex (uint index);
		
		[Export ("indexPathByRemovingLastIndex")]
		NSIndexPath IndexPathByRemovingLastIndex ();

		[Export ("indexAtPosition:")]
		uint IndexAtPosition (int position);

		[Export ("length")]
		int Length { get; } 

		[Export ("getIndexes:")][Internal]
		void _GetIndexes (IntPtr target);

		[Export ("compare:")]
		int Compare (NSIndexPath other);

		[Export ("indexPathForRow:inSection:")][Static]
		NSIndexPath FromRowSection (int row, int section);
		
		[Export ("row")]
		int Row { get; }

		[Export ("section")]
		int Section { get; }
		
	}

	[BaseType (typeof (NSObject))]
	public interface NSIndexSet {
		[Static, Export ("indexSetWithIndex:")]
		NSIndexSet FromIndex (int idx);

		[Static, Export ("indexSetWithIndexesInRange:")]
		NSIndexSet FromNSRange (NSRange indexRange);
		
		[Export ("initWithIndex:")]
		IntPtr Constructor (uint index);

		[Export ("initWithIndexSet:")]
		IntPtr Constructor (NSIndexSet other);

		[Export ("count")]
		int Count { get; }

		[Export ("isEqualToIndexSet:")]
		bool IsEqual (NSIndexSet other);

		[Export ("firstIndex")]
		uint FirstIndex { get; }

		[Export ("lastIndex")]
		uint LastIndex { get; }

		[Export ("indexGreaterThanIndex:")]
		uint IndexGreaterThan (uint index);

		[Export ("indexLessThanIndex:")]
		uint IndexLessThan (uint index);

		[Export ("indexGreaterThanOrEqualToIndex:")]
		uint IndexGreaterThanOrEqual (uint index);

		[Export ("indexLessThanOrEqualToIndex:")]
		uint IndexLessThanOrEqual (uint index);

		[Export ("containsIndex:")]
		bool Contains (uint index);

		[Export ("containsIndexes:")]
		bool Contains (NSIndexSet indexes);
	}
	
	[BaseType (typeof (NSIndexSet))]
	public interface NSMutableIndexSet {
		[Export ("initWithIndex:")]
		IntPtr Constructor (uint index);

		[Export ("initWithIndexSet:")]
		IntPtr Constructor (NSIndexSet other);

		[Export ("addIndexes:")]
		void Add (NSIndexSet other);

		[Export ("removeIndexes:")]
		void Remove (NSIndexSet other);

		[Export ("removeAllIndexes")]
		void Clear ();

		[Export ("addIndex:")]
		void Add (uint index);

		[Export ("removeIndex:")]
		void Remove (uint index);

		[Export ("shiftIndexesStartingAtIndex:by:")]
		void ShiftIndexes (uint startIndex, uint delta);
	}
	
	[BaseType (typeof (NSObject), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (NSNetServiceDelegate)})]
	public interface NSNetService {
		[Export ("initWithDomain:type:name:port:")]
		IntPtr Constructor (string domain, string type, string name, int port);

		[Export ("initWithDomain:type:name:")]
		IntPtr Constructor (string domain, string type, string name);
		
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }
		
		[Wrap ("WeakDelegate")]
		NSNetServiceDelegate Delegate { get; set; }

		[Export ("scheduleInRunLoop:forMode:")]
		void Schedule (NSRunLoop aRunLoop, string forMode);

		// For consistency with other APIs (NSUrlConnection) we call this Unschedule
		[Export ("removeFromRunLoop:forMode:")]
		void Unschedule (NSRunLoop aRunLoop, string forMode);

		[Export ("domain")]
		string Domain { get; }

		[Export ("type")]
		string Type { get; }

		[Export ("name")]
		string Name { get; }

		[Export ("addresses")]
		NSData [] Addresses { get; }

		[Export ("port")]
		int Port { get; }

		[Export ("publish")]
		void Publish ();

		[Export ("publishWithOptions:")]
		void Publish (NSNetServiceOptions options);

		[Export ("resolve")]
		void Resolve ();

		[Export ("resolveWithTimeout:")]
		void Resolve (double timeOut);

		[Export ("stop")]
		void Stop ();

		[Static, Export ("dictionaryFromTXTRecordData:")]
		NSDictionary DictionaryFromTxtRecord (NSData data);
		
		[Static, Export ("dataFromTXTRecordDictionary:")]
		NSData DataFromTxtRecord (NSDictionary dictionary);
		
		[Export ("hostName")]
		string HostName { get; }

		[Internal, Export ("getInputStream:outputStream:")]
		bool GetStreams (IntPtr ptrToInputStorage, IntPtr ptrToOutputStorage);
		
		[Export ("TXTRecordData")]
		NSData TxtRecordData { get; set; }

		[Export ("startMonitoring")]
		void StartMonitoring ();

		[Export ("stopMonitoring")]
		void StopMonitoring ();
	}

	[Model, BaseType (typeof (NSObject))]
	public interface NSNetServiceDelegate {
		[Export ("netServiceWillPublish:")]
		void WillPublish (NSNetService sender);

		[Export ("netServiceDidPublish:")]
		void Published (NSNetService sender);

		[Export ("netService:didNotPublish:"), EventArgs ("NSNetServiceError")]
		void PublishFailure (NSNetService sender, NSDictionary errors);

		[Export ("netServiceWillResolve:")]
		void WillResolve (NSNetService sender);

		[Export ("netServiceDidResolveAddress:")]
		void AddressResolved (NSNetService sender);

		[Export ("netService:didNotResolve:"), EventArgs ("NSNetServiceError")]
		void ResolveFailure (NSNetService sender, NSDictionary errors);

		[Export ("netServiceDidStop:")]
		void Stopped (NSNetService sender);

		[Export ("netService:didUpdateTXTRecordData:"), EventArgs ("NSNetServiceData")]
		void UpdatedTxtRecordData (NSNetService sender, NSData data);
	}
	
	[BaseType (typeof (NSObject),
		   Delegates=new string [] {"WeakDelegate"},
		   Events=new Type [] {typeof (NSNetServiceBrowserDelegate)})]
	public interface NSNetServiceBrowser {
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSNetServiceBrowserDelegate Delegate { get; set; }

		[Export ("scheduleInRunLoop:forMode:")]
		void Schedule (NSRunLoop aRunLoop, string forMode);

		// For consistency with other APIs (NSUrlConnection) we call this Unschedule
		[Export ("removeFromRunLoop:forMode:")]
		void Unschedule (NSRunLoop aRunLoop, string forMode);

		[Export ("searchForBrowsableDomains")]
		void SearchForBrowsableDomains ();

		[Export ("searchForRegistrationDomains")]
		void SearchForRegistrationDomains ();

		[Export ("searchForServicesOfType:inDomain:")]
		void SearchForServices (string type, string domain);

		[Export ("stop")]
		void Stop ();
	}

	[Model, BaseType (typeof (NSObject))]
	public interface NSNetServiceBrowserDelegate {
		[Export ("netServiceBrowserWillSearch:")]
		void SearchStarted (NSNetServiceBrowser sender);
		
		[Export ("netServiceBrowserDidStopSearch:")]
		void SearchStopped (NSNetServiceBrowser sender);
		
		[Export ("netServiceBrowser:didNotSearch:"), EventArgs ("NSNetServiceError")]
		void NotSearched (NSNetServiceBrowser sender, NSDictionary errors);
		
		[Export ("netServiceBrowser:didFindDomain:moreComing:"), EventArgs ("NSNetDomain")]
		void FoundDomain (NSNetServiceBrowser sender, string domain, bool moreComing);
		
		[Export ("netServiceBrowser:didFindService:moreComing:"), EventArgs ("NSNetService")]
		void FoundService (NSNetServiceBrowser sender, NSNetService service, bool moreComing);
		
		[Export ("netServiceBrowser:didRemoveDomain:moreComing:"), EventArgs ("NSNetDomain")]
		void DomainRemoved (NSNetServiceBrowser sender, string domain, bool moreComing);
		
		[Export ("netServiceBrowser:didRemoveService:moreComing:"), EventArgs ("NSNetService")]
		void ServiceRemoved (NSNetServiceBrowser sender, NSNetService service, bool moreComing);
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSNotification {
		[Export ("name")]
		string Name { get; }

		[Export ("object")]
		NSObject Object { get; }

		[Export ("userInfo")]
		NSDictionary UserInfo { get; }

		[Export ("notificationWithName:object:")][Static]
		NSNotification FromName (string name, NSObject obj);

		[Export ("notificationWithName:object:userInfo:")][Static]
		NSNotification FromName (string name, NSObject obj, NSDictionary userInfo);
	}

	[BaseType (typeof (NSObject))]
	public interface NSNotificationCenter {
		[Static][Export ("defaultCenter")]
		NSNotificationCenter DefaultCenter { get; }
	
		[Export ("addObserver:selector:name:object:")]
		void AddObserver ([RetainList (true, "ObserverList")] NSObject observer, Selector aSelector, [NullAllowed] NSString aName, [NullAllowed] NSObject anObject);
	
		[Export ("postNotification:")]
		void PostNotification (NSNotification notification);
	
		[Export ("postNotificationName:object:")]
		void PostNotificationName (string aName, NSObject anObject);
	
		[Export ("postNotificationName:object:userInfo:")]
		void PostNotificationName (string aName, NSObject anObject, [NullAllowed] NSDictionary aUserInfo);
	
		[Export ("removeObserver:")]
		void RemoveObserver ([RetainList (false, "ObserverList")] NSObject observer);
	
		[Export ("removeObserver:name:object:")]
		void RemoveObserver ([RetainList (false, "ObserverList")] NSObject observer, [NullAllowed] string aName, [NullAllowed] NSObject anObject);

		[Since (4,0)]
		[Export ("addObserverForName:object:queue:usingBlock:")]
		void AddObserver (string name, NSObject obj, NSOperationQueue queue, NSNotificationHandler handler);
	}

#if MONOMAC
	[BaseType (typeof (NSNotificationCenter))]
	public interface NSDistributedNotificationCenter {
		[Static]
		[Export ("defaultCenter")]
		NSObject DefaultCenter { get; }

		[Export ("addObserver:selector:name:object:suspensionBehavior:")]
		void AddObserver (NSObject observer, Selector selector, string name, string anObject, NSNotificationSuspensionBehavior suspensionBehavior);

		[Export ("postNotificationName:object:userInfo:deliverImmediately:")]
		void PostNotificationName (string name, string anObject, NSDictionary userInfo, bool deliverImmediately);
		
		[Export ("postNotificationName:object:userInfo:options:")]
		void PostNotificationName (string name, string anObject, NSDictionary userInfo, NSNotificationFlags options);

		[Export ("addObserver:selector:name:object:")]
		void AddObserver (NSObject observer, Selector aSelector, string aName, string anObject);

		[Export ("postNotificationName:object:")]
		void PostNotificationName (string aName, string anObject);

		[Export ("postNotificationName:object:userInfo:")]
		void PostNotificationName (string aName, string anObject, NSDictionary aUserInfo);

		[Export ("removeObserver:name:object:")]
		void RemoveObserver (NSObject observer, string aName, string anObject);

		//Detected properties
		[Export ("suspended")]
		bool Suspended { get; set; }
		
		[Field ("NSLocalNotificationCenterType")]
		NSString NSLocalNotificationCenterType {get;}
	}
#endif
	
	[BaseType (typeof (NSObject))]
	public interface NSNotificationQueue {
		[Static]
		[Export ("defaultQueue")]
		NSObject DefaultQueue { get; }

		[Export ("initWithNotificationCenter:")]
		IntPtr Constructor (NSNotificationCenter notificationCenter);

		[Export ("enqueueNotification:postingStyle:")]
		void EnqueueNotification (NSNotification notification, NSPostingStyle postingStyle);

		[Export ("enqueueNotification:postingStyle:coalesceMask:forModes:")]
		void EnqueueNotification (NSNotification notification, NSPostingStyle postingStyle, NSNotificationCoalescing coalesceMask, string [] modes);

		[Export ("dequeueNotificationsMatching:coalesceMask:")]
		void DequeueNotificationsMatchingcoalesceMask (NSNotification notification, NSNotificationCoalescing coalesceMask);
	}

	public delegate void NSNotificationHandler (NSNotification notification);
	
	[BaseType (typeof (NSObject))]
	public interface NSValue {
		[Export ("getValue:")]
		void StoreValueAtAddress (IntPtr value);

		//[Export ("objCType")][Internal]
		//// This returns a const char *, we need a partial class method
		//IntPtr ObjCTypePtr ();
		//[Export ("initWithBytes:objCType:")][Internal]
		//NSValue InitFromBytes (IntPtr byte_ptr, IntPtr char_ptr_type);
		//[Export ("valueWithBytes:objCType:")][Static][Internal]
		//+ (NSValue *)valueWithBytes:(const void *)value objCType:(const char *)type;
		//+ (NSValue *)value:(const void *)value withObjCType:(const char *)type;

		[Static]
		[Export ("valueWithNonretainedObject:")]
		NSValue ValueFromNonretainedObject (NSObject anObject);
	
		[Export ("nonretainedObjectValue")]
		NSObject NonretainedObjectValue { get; }
	
		[Static]
		[Export ("valueWithPointer:")]
		NSValue ValueFromPointer (IntPtr pointer);
	
		[Export ("pointerValue")]
		IntPtr PointerValue { get; }
	
		[Export ("isEqualToValue:")]
		bool IsEqualTo (NSValue value);
	
#if MONOMAC
		[Export ("valueWithRect:"), Static]
		NSValue FromRectangleF (System.Drawing.RectangleF rect);

		[Export ("valueWithSize:")][Static]
		NSValue FromSizeF (System.Drawing.SizeF size);

		[Export ("valueWithPoint:")][Static]
		NSValue FromPointF (System.Drawing.PointF point);

		[Export ("rectValue")]
		System.Drawing.RectangleF RectangleFValue { get; }

		[Export ("sizeValue")]
		System.Drawing.SizeF SizeFValue { get; }

		[Export ("pointValue")]
		System.Drawing.PointF PointFValue { get; }
#else
		[Export ("CGAffineTransformValue")]
		MonoMac.CoreGraphics.CGAffineTransform CGAffineTransformValue { get; }
		
		[Export ("UIEdgeInsetsValue")]
		MonoMac.UIKit.UIEdgeInsets UIEdgeInsetsValue { get; }

		[Export ("valueWithCGAffineTransform:")][Static]
		NSValue FromCGAffineTransform (MonoMac.CoreGraphics.CGAffineTransform tran);

		[Export ("valueWithUIEdgeInsets:")][Static]
		NSValue FromUIEdgeInsets (MonoMac.UIKit.UIEdgeInsets insets);

		[Export ("valueWithCGRect:")][Static]
		NSValue FromRectangleF (System.Drawing.RectangleF rect);

		[Export ("CGRectValue")]
		System.Drawing.RectangleF RectangleFValue { get; }

		[Export ("valueWithCGSize:")][Static]
		NSValue FromSizeF (System.Drawing.SizeF size);

		[Export ("CGSizeValue")]
		System.Drawing.SizeF SizeFValue { get; }

		[Export ("CGPointValue")]
		System.Drawing.PointF PointFValue { get; }

		[Export ("valueWithCGPoint:")][Static]
		NSValue FromPointF (System.Drawing.PointF point);
		
		[Export ("valueWithCATransform3D:")][Static]
		NSValue FromCATransform3D (MonoMac.CoreAnimation.CATransform3D transform);

		[Export ("CATransform3DValue")]
		MonoMac.CoreAnimation.CATransform3D CATransform3DValue { get; }
#endif
	}
	
	[BaseType (typeof (NSValue))]
	public interface NSNumber {
		[Export ("charValue")]
		sbyte SByteValue { get; }
	
		[Export ("unsignedCharValue")]
		byte ByteValue { get; }
	
		[Export ("shortValue")]
		short Int16Value { get; }
	
		[Export ("unsignedShortValue")]
		ushort UInt16Value { get; }
	
		[Export ("intValue")]
		int Int32Value { get; }
	
		[Export ("unsignedIntValue")]
		uint UInt32Value { get; } 
	
		//[Export ("longValue")]
		//int LongValue ();
		//
		//[Export ("unsignedLongValue")]
		//uint UnsignedLongValue ();
	
		[Export ("longLongValue")]
		long Int64Value { get; }
	
		[Export ("unsignedLongLongValue")]
		ulong UInt64Value { get; }
	
		[Export ("floatValue")]
		float FloatValue { get; }
	
		[Export ("doubleValue")]
		double DoubleValue { get; }

		[Export ("decimalValue")]
		NSDecimal NSDecimalValue { get; }
	
		[Export ("boolValue")]
		bool BoolValue { get; }
	
		[Export ("integerValue")]
		int IntValue { get; }
	
		[Export ("unsignedIntegerValue")]
		uint UnsignedIntegerValue { get; }
	
		[Export ("stringValue")]
		string StringValue { get; }

		[Export ("compare:")]
		int Compare (NSNumber otherNumber);
	
		[Export ("isEqualToNumber:")]
		bool IsEqualToNumber (NSNumber number);
	
		[Export ("descriptionWithLocale:")]
		string DescriptionWithLocale (NSLocale locale);

		[Export ("initWithChar:")]
		IntPtr Constructor (sbyte value);
	
		[Export ("initWithUnsignedChar:")]
		IntPtr Constructor (byte value);
	
		[Export ("initWithShort:")]
		IntPtr Constructor (short value);
	
		[Export ("initWithUnsignedShort:")]
		IntPtr Constructor (ushort value);
	
		[Export ("initWithInt:")]
		IntPtr Constructor (int value);
	
		[Export ("initWithUnsignedInt:")]
		IntPtr Constructor (uint value);
	
		//[Export ("initWithLong:")]
		//IntPtr Constructor (long value);
		//
		//[Export ("initWithUnsignedLong:")]
		//IntPtr Constructor (ulong value);
	
		[Export ("initWithLongLong:")]
		IntPtr Constructor (long value);
	
		[Export ("initWithUnsignedLongLong:")]
		IntPtr Constructor (ulong value);
	
		[Export ("initWithFloat:")]
		IntPtr Constructor (float value);
	
		[Export ("initWithDouble:")]
		IntPtr Constructor (double value);
	
		[Export ("initWithBool:")]
		IntPtr Constructor (bool value);
	
		[Export ("numberWithChar:")][Static]
		NSNumber FromSByte (sbyte value);
	
		[Static]
		[Export ("numberWithUnsignedChar:")]
		NSNumber FromByte (byte value);
	
		[Static]
		[Export ("numberWithShort:")]
		NSNumber FromInt16 (short value);
	
		[Static]
		[Export ("numberWithUnsignedShort:")]
		NSNumber FromUInt16 (ushort value);
	
		[Static]
		[Export ("numberWithInt:")]
		NSNumber FromInt32 (int value);
	
		[Static]
		[Export ("numberWithUnsignedInt:")]
		NSNumber FromUInt32 (uint value);

		//[Static]
		//[Export ("numberWithLong:")]
		//NSNumber * numberWithLong: (long value);
		//
		//[Static]
		//[Export ("numberWithUnsignedLong:")]
		//NSNumber * numberWithUnsignedLong: (unsigned long value);
	
		[Static]
		[Export ("numberWithLongLong:")]
		NSNumber FromInt64 (long value);
	
		[Static]
		[Export ("numberWithUnsignedLongLong:")]
		NSNumber FromUInt64 (ulong value);
	
		[Static]
		[Export ("numberWithFloat:")]
		NSNumber FromFloat (float value);
	
		[Static]
		[Export ("numberWithDouble:")]
		NSNumber FromDouble (double value);
	
		[Static]
		[Export ("numberWithBool:")]
		NSNumber FromBoolean (bool value);

		
	}

	[BaseType (typeof (NSNumber))]
	public interface NSDecimalNumber {
		[Export ("initWithMantissa:exponent:isNegative:")]
		IntPtr Constructor (long mantissa, short exponent, bool isNegative);
		
		[Export ("initWithDecimal:")]
		IntPtr Constructor (NSDecimal dec);

		[Export ("initWithString:")]
		IntPtr Constructor (string numberValue);

		[Export ("initWithString:locale:")]
		IntPtr Constructor (string numberValue, NSObject locale);

		[Export ("descriptionWithLocale:")]
		string DescriptionWithLocale (NSLocale locale);

		[Export ("decimalValue")]
		NSDecimal NSDecimalValue { get; }

		[Export ("zero")][Static]
		NSDecimalNumber Zero { get; }

		[Export ("one")][Static]
		NSDecimalNumber One { get; }
		
		[Export ("minimumDecimalNumber")][Static]
		NSDecimalNumber MinValue { get; }
		
		[Export ("maximumDecimalNumber")][Static]
		NSDecimalNumber MaxValue { get; }

		[Export ("notANumber")]
		NSDecimalNumber NaN { get; }

		//
		// All the behavior ones require:
		// id <NSDecimalNumberBehaviors>)behavior;

		[Export ("decimalNumberByAdding:")]
		NSDecimalNumber Add (NSDecimalNumber d);

		[Export ("decimalNumberByAdding:withBehavior:")]
		NSDecimalNumber Add (NSDecimalNumber d, NSObject Behavior);

		[Export ("decimalNumberBySubtracting:")]
		NSDecimalNumber Subtract (NSDecimalNumber d);

		[Export ("decimalNumberBySubtracting:withBehavior:")]
		NSDecimalNumber Subtract (NSDecimalNumber d, NSObject Behavior);
		
		[Export ("decimalNumberByMultiplyingBy:")]
		NSDecimalNumber Multiply (NSDecimalNumber d);

		[Export ("decimalNumberByMultiplyingBy:withBehavior:")]
		NSDecimalNumber Multiply (NSDecimalNumber d, NSObject Behavior);
		
		[Export ("decimalNumberByDividingBy:")]
		NSDecimalNumber Divide (NSDecimalNumber d);

		[Export ("decimalNumberByDividingBy:withBehavior:")]
		NSDecimalNumber Divide (NSDecimalNumber d, NSObject Behavior);

		[Export ("decimalNumberByRaisingToPower:")]
		NSDecimalNumber RaiseTo (NSDecimalNumber d);

		[Export ("decimalNumberByRaisingToPower:withBehavior:")]
		NSDecimalNumber RaiseTo (NSDecimalNumber d, NSObject Behavior);
		
		[Export ("decimalNumberByMultiplyingByPowerOf10:")]
		NSDecimalNumber MultiplyPowerOf10 (NSDecimalNumber d);

		[Export ("decimalNumberByMultiplyingByPowerOf10:withBehavior:")]
		NSDecimalNumber MultiplyPowerOf10 (NSDecimalNumber d, NSObject Behavior);

		[Export ("decimalNumberByRoundingAccordingToBehavior:")]
		NSDecimalNumber Rounding (NSObject behavior);

		[Export ("compare:")]
		int Compare (NSNumber other);

		[Export ("defaultBehavior")][Static]
		NSObject DefaultBehavior { get; set; }

		[Export ("doubleValue")]
		double DoubleValue { get; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSThread {
		[Static, Export ("currentThread")]
		NSThread Current { get; }

		//+ (void)detachNewThreadSelector:(SEL)selector toTarget:(id)target withObject:(id)argument;

		[Static, Export ("isMultiThreaded")]
		bool IsMultiThreaded { get; }

		//- (NSMutableDictionary *)threadDictionary;

		[Static, Export ("sleepUntilDate:")]
		void SleepUntil (NSDate date);
		
		[Static, Export ("sleepForTimeInterval:")]
		void SleepFor (double timeInterval);

		[Static, Export ("exit")]
		void Exit ();

		[Static, Export ("threadPriority")]
		double Priority { get; set; }

		//+ (NSArray *)callStackReturnAddresses;

		[Export ("name")]
		string Name { get; set; }

		[Export ("stackSize")]
		uint StackSize { get; set; }

		[Export ("isMainThread")]
		bool IsMainThread { get; }

		[Export ("mainThread")]
		NSThread MainThread { get; }

		[Export ("initWithTarget:selector:object:")]
		IntPtr Constructor (NSObject target, Selector selector, NSObject argument);

		[Export ("isExecuting")]
		bool IsExecuting { get; }

		[Export ("isFinished")]
		bool IsFinished { get; }

		[Export ("isCancelled")]
		bool IsCancelled { get; }

		[Export ("cancel")]
		void Cancel ();

		[Export ("start")]
		void Start ();

		[Export ("main")]
		void Main ();
	}

	[BaseType (typeof (NSObject))]
	public interface NSProcessInfo {
		[Export ("processInfo")][Static]
		NSProcessInfo ProcessInfo { get; }

		[Export ("arguments")]
		string [] Arguments { get; }
		[Export ("environment")]
		NSDictionary Environment { get; }
		[Export ("processIdentifier")]
		int ProcessIdentifier { get; }
		[Export ("globallyUniqueString")]
		string GloballyUniqueString { get; }
		[Export ("processName")]
		string ProcessName { get; set; }

		[Export ("hostName")]
		string HostName { get; }
		[Export ("operatingSystem")]
		uint OperatingSystem { get; }
		[Export ("operatingSystemName")]
		string OperatingSystemName { get; }
		[Export ("operatingSystemVersionString")]
		string OperatingSystemVersionString { get; }

		[Export ("physicalMemory")]
		UInt64 PhysicalMemory { get; }
		[Export ("processorCount")]
		uint ProcessorCount { get; }
		[Export ("activeProcessorCount")]
		uint ActiveProcessorCount { get; }
	}

	[BaseType (typeof (NSMutableData))]
	[Since (4,0)]
	public interface NSPurgeableData {
		
	}

	[BaseType (typeof (NSObject))]
	public interface NSFileManager {
		[Field("NSFileType")]
		NSString NSFileType { get; }

		[Field("NSFileTypeDirectory")]
		NSString TypeDirectory { get; }

		[Field("NSFileTypeRegular")]
		NSString TypeRegular { get; }

		[Field("NSFileTypeSymbolicLink")]
		NSString TypeSymbolicLink { get; }

		[Field("NSFileTypeSocket")]
		NSString TypeSocket { get; }

		[Field("NSFileTypeCharacterSpecial")]
		NSString TypeCharacterSpecial { get; }

		[Field("NSFileTypeBlockSpecial")]
		NSString TypeBlockSpecial { get; }

		[Field("NSFileTypeUnknown")]
		NSString TypeUnknown { get; }

		[Field("NSFileSize")]
		NSString Size { get; }

		[Field("NSFileModificationDate")]
		NSString ModificationDate { get; }

		[Field("NSFileReferenceCount")]
		NSString ReferenceCount { get; }

		[Field("NSFileDeviceIdentifier")]
		NSString DeviceIdentifier { get; }

		[Field("NSFileOwnerAccountName")]
		NSString OwnerAccountName { get; }

		[Field("NSFileGroupOwnerAccountName")]
		NSString GroupOwnerAccountName { get; }

		[Field("NSFilePosixPermissions")]
		NSString PosixPermissions { get; }

		[Field("NSFileSystemNumber")]
		NSString SystemNumber { get; }

		[Field("NSFileSystemFileNumber")]
		NSString SystemFileNumber { get; }

		[Field("NSFileExtensionHidden")]
		NSString ExtensionHidden { get; }

		[Field("NSFileHFSCreatorCode")]
		NSString HfsCreatorCode { get; }

		[Field("NSFileHFSTypeCode")]
		NSString HfsTypeCode { get; }

		[Field("NSFileImmutable")]
		NSString Immutable { get; }

		[Field("NSFileAppendOnly")]
		NSString AppendOnly { get; }

		[Field("NSFileCreationDate")]
		NSString CreationDate { get; }

		[Field("NSFileOwnerAccountID")]
		NSString OwnerAccountID { get; }

		[Field("NSFileGroupOwnerAccountID")]
		NSString GroupOwnerAccountID { get; }

		[Field("NSFileBusy")]
		NSString Busy { get; }

		[Field("NSFileSystemSize")]
		NSString SystemSize { get; }

		[Field("NSFileSystemFreeSize")]
		NSString SystemFreeSize { get; }

		[Field("NSFileSystemNodes")]
		NSString SystemNodes { get; }

		[Field("NSFileSystemFreeNodes")]
		NSString SystemFreeNodes { get; }

		[Static, Export ("defaultManager")]
		NSFileManager DefaultManager { get; }

		[Export ("delegate")]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSFileManagerDelegate Delegate { get; set; }

		[Export ("setAttributes:ofItemAtPath:error:")]
		bool SetAttributes (NSDictionary attributes, string path, out NSError error);

		[Export ("createDirectoryAtPath:withIntermediateDirectories:attributes:error:")]
		bool CreateDirectory (string path, bool createIntermediates, NSDictionary attributes, out NSError error);

		[Export ("contentsOfDirectoryAtPath:error:")]
		string[] GetDirectoryContent (string path, out NSError error);

		[Export ("subpathsOfDirectoryAtPath:error:")]
		string[] GetDirectoryContentRecursive (string path, out NSError error);

		[Export ("attributesOfItemAtPath:error:")][Internal]
		NSDictionary _GetAttributes (string path, out NSError error);

		[Export ("attributesOfFileSystemForPath:error:")]
		NSDictionary GetFileSystemAttributes (string path, out NSError error);

		[Export ("createSymbolicLinkAtPath:withDestinationPath:error:")]
		bool CreateSymbolicLink (string path, string destPath, out NSError error);

		[Export ("destinationOfSymbolicLinkAtPath:error:")]
		string GetSymbolicLinkDestination (string path, out NSError error);

		[Export ("copyItemAtPath:toPath:error:")]
		bool Copy (string srcPath, string dstPath, out NSError error);

		[Export ("moveItemAtPath:toPath:error:")]
		bool Move (string srcPath, string dstPath, out NSError error);

		[Export ("linkItemAtPath:toPath:error:")]
		bool Link (string srcPath, string dstPath, out NSError error);

		[Export ("removeItemAtPath:error:")]
		bool Remove (string path, out NSError error);

#if DEPRECATED
		// These are not available on iOS, and deprecated on OSX.
		[Export ("linkPath:toPath:handler:")]
		bool LinkPath (string src, string dest, IntPtr handler);

		[Export ("copyPath:toPath:handler:")]
		bool CopyPath (string src, string dest, IntPtr handler);

		[Export ("movePath:toPath:handler:")]
		bool MovePath (string src, string dest, IntPtr handler);

		[Export ("removeFileAtPath:handler:")]
		bool RemoveFileAtPath (string path, IntPtr handler);
#endif
		[Export ("currentDirectoryPath")]
		string CurrentDirectory { get; [Bind ("changeCurrentDirectoryPath:")] set; }

		[Export ("fileExistsAtPath:")]
		bool FileExists (string path);

		[Export ("fileExistsAtPath:isDirectory:")]
		bool FileExists (string path, bool isDirectory);

		[Export ("isReadableFileAtPath:")]
		bool IsReadableFile (string path);

		[Export ("isWritableFileAtPath:")]
		bool IsWritableFile (string path);

		[Export ("isExecutableFileAtPath:")]
		bool IsExecutableFile (string path);

		[Export ("isDeletableFileAtPath:")]
		bool IsDeletableFile (string path);

		[Export ("contentsEqualAtPath:andPath:")]
		bool ContentsEqual (string path1, string path2);

		[Export ("displayNameAtPath:")]
		string DisplayName (string path);

		[Export ("componentsToDisplayForPath:")]
		string[] ComponentsToDisplay (string path);

		[Export ("enumeratorAtPath:")]
		NSDirectoryEnumerator GetEnumerator (string path);

		[Export ("subpathsAtPath:")]
		string[] Subpaths (string path);

		[Export ("contentsAtPath:")]
		NSData Contents (string path);

		[Export ("createFileAtPath:contents:attributes:")]
		bool CreateFile (string path, NSData data, NSDictionary attr);

		[Since (4,0)]
		[Export ("contentsOfDirectoryAtURL:includingPropertiesForKeys:options:error:")]
		NSUrl[] GetDirectoryContent (NSUrl url, NSArray properties, NSDirectoryEnumerationOptions options, out NSError error);

		[Since (4,0)]
		[Export ("copyItemAtURL:toURL:error:")]
		bool Copy (NSUrl srcUrl, NSUrl dstUrl, out NSError error);

		[Since (4,0)]
		[Export ("moveItemAtURL:toURL:error:")]
		bool Move (NSUrl srcUrl, NSUrl dstUrl, out NSError error);

		[Since (4,0)]
		[Export ("linkItemAtURL:toURL:error:")]
		bool Link (NSUrl srcUrl, NSUrl dstUrl, out NSError error);

		[Since (4,0)]
		[Export ("removeItemAtURL:error:")]
		bool Remove (NSUrl url, out NSError error);

		[Since (4,0)]
		[Export ("enumeratorAtURL:includingPropertiesForKeys:options:errorHandler:")]
		NSDirectoryEnumerator GetEnumerator (NSUrl url, NSArray properties, NSDirectoryEnumerationOptions options, out NSError error);

		[Since (4,0)]
		[Export ("URLForDirectory:inDomain:appropriateForURL:create:error:")]
		NSUrl GetUrl (NSSearchPathDirectory directory, NSSearchPathDomain domain, NSUrl url, bool shouldCreate, out NSError error);

		[Since (4,0)]
		[Export ("URLsForDirectory:inDomains:")]
		NSUrl[] GetUrls (NSSearchPathDirectory directory, NSSearchPathDomain domains);

		[Since (4,0)]
		[Export ("replaceItemAtURL:withItemAtURL:backupItemName:options:resultingItemURL:error:")]
		bool Replace (NSUrl originalItem, NSUrl newItem, string backupItemName, NSFileManagerItemReplacementOptions options, out NSUrl resultingURL, out NSError error);

		[Since (4,0)]
		[Export ("mountedVolumeURLsIncludingResourceValuesForKeys:options:")]
		NSUrl[] GetMountedVolumes(NSArray properties, NSVolumeEnumerationOptions options);

		// Methods to convert paths to/from C strings for passing to system calls - Not implemented
		////- (const char *)fileSystemRepresentationWithPath:(NSString *)path;
		//[Export ("fileSystemRepresentationWithPath:")]
		//const char FileSystemRepresentationWithPath (string path);

		////- (NSString *)stringWithFileSystemRepresentation:(const char *)str length:(NSUInteger)len;
		//[Export ("stringWithFileSystemRepresentation:length:")]
		//string StringWithFileSystemRepresentation (const char str, uint len);

	}

	[BaseType(typeof(NSObject))]
	[Model]
	public interface NSFileManagerDelegate {
		[Export("fileManager:shouldCopyItemAtPath:toPath:")]
		bool ShouldCopyItemAtPath(NSFileManager fm, NSString srcPath, NSString dstPath);

#if MONOTOUCH
		[Export("fileManager:shouldCopyItemAtURL:toURL:")]
		bool ShouldCopyItemAtUrl(NSFileManager fm, NSUrl srcUrl, NSUrl dstUrl);
		
		[Export ("fileManager:shouldLinkItemAtURL:toURL:")]
		bool ShouldLinkItemAtUrl (NSFileManager fileManager, NSUrl srcUrl, NSUrl dstUrl);

		[Export ("fileManager:shouldMoveItemAtURL:toURL:")]
		bool ShouldMoveItemAtUrl (NSFileManager fileManager, NSUrl dstUrl, NSUrl dstUrl);

		[Export ("fileManager:shouldProceedAfterError:copyingItemAtURL:toURL:")]
		bool ShouldProceedAfterErrorCopyingItem (NSFileManager fileManager, NSError error, NSUrl srcUrl, NSUrl dstUrl);

		[Export ("fileManager:shouldProceedAfterError:linkingItemAtURL:toURL:")]
		bool ShouldProceedAfterErrorLinkingItem (NSFileManager fileManager, NSError error, NSUrl srcUrl, NSUrl dstUrl);

		[Export ("fileManager:shouldProceedAfterError:movingItemAtURL:toURL:")]
		bool ShouldProceedAfterErrorMovingItem (NSFileManager fileManager, NSError error, NSUrl srcUrl, NSUrl dstUrl);

		[Export ("fileManager:shouldRemoveItemAtURL:")]
		bool ShouldRemoveItemAtUrl (NSFileManager fileManager, NSUrl url);

		[Export ("fileManager:shouldProceedAfterError:removingItemAtURL:")]
		bool ShouldProceedAfterErrorRemovingItem (NSFileManager fileManager, NSError error, NSUrl url);
#endif
		
		[Export ("fileManager:shouldProceedAfterError:")]
		bool ShouldProceedAfterError (NSFileManager fm, NSDictionary errorInfo);

		// Deprecated
		//[Export ("fileManager:willProcessPath:")]
		//void WillProcessPath (NSFileManager fm, string path);

		[Export ("fileManager:shouldCopyItemAtPath:toPath:")]
		bool ShouldCopyItemAtPath (NSFileManager fileManager, string srcPath, string dstPath);

		[Export ("fileManager:shouldProceedAfterError:copyingItemAtPath:toPath:")]
		bool ShouldProceedAfterErrorCopyingItem (NSFileManager fileManager, NSError error, string srcPath, string dstPath);

		[Export ("fileManager:shouldMoveItemAtPath:toPath:")]
		bool ShouldMoveItemAtPath (NSFileManager fileManager, string srcPath, string dstPath);

		[Export ("fileManager:shouldProceedAfterError:movingItemAtPath:toPath:")]
		bool ShouldProceedAfterErrorMovingItem (NSFileManager fileManager, NSError error, string srcPath, string dstPath);

		[Export ("fileManager:shouldLinkItemAtPath:toPath:")]
		bool ShouldLinkItemAtPath (NSFileManager fileManager, string srcPath, string dstPath);

		[Export ("fileManager:shouldProceedAfterError:linkingItemAtPath:toPath:")]
		bool ShouldProceedAfterErrorLinkingItem (NSFileManager fileManager, NSError error, string srcPath, string dstPath);

		[Export ("fileManager:shouldRemoveItemAtPath:")]
		bool ShouldRemoveItemAtPath (NSFileManager fileManager, string path);

		[Export ("fileManager:shouldProceedAfterError:removingItemAtPath:")]
		bool ShouldProceedAfterErrorRemovingItem (NSFileManager fileManager, NSError error, string path);
	}
    
	[BaseType (typeof (NSEnumerator))]
	public interface NSDirectoryEnumerator {
		[Export ("fileAttributes")]
		NSDictionary FileAttributes { get; }

		[Export ("directoryAttributes")]
		NSDictionary DirectoryAttributes { get; }

		[Export ("skipDescendents")]
		void SkipDescendents ();

#if MONOTOUCH
		[Export ("level")]
		int Level { get; }
#endif
#if MONOMAC
		////- (unsigned long long)fileSize;
		//[Export ("fileSize")]
		//unsigned long long FileSize ([Target] NSDictionary fileAttributes);

		[Export ("fileModificationDate")]
		NSDate FileModificationDate ([Target] NSDictionary fileAttributes);

		[Export ("fileType")]
		string FileType ([Target] NSDictionary fileAttributes);

		[Export ("filePosixPermissions")]
		uint FilePosixPermissions ([Target] NSDictionary fileAttributes);

		[Export ("fileOwnerAccountName")]
		string FileOwnerAccountName ([Target] NSDictionary fileAttributes);

		[Export ("fileGroupOwnerAccountName")]
		string FileGroupOwnerAccountName ([Target] NSDictionary fileAttributes);

		[Export ("fileSystemNumber")]
		int FileSystemNumber ([Target] NSDictionary fileAttributes);

		[Export ("fileSystemFileNumber")]
		uint FileSystemFileNumber ([Target] NSDictionary fileAttributes);

		[Export ("fileExtensionHidden")]
		bool FileExtensionHidden ([Target] NSDictionary fileAttributes);

		[Export ("fileHFSCreatorCode")]
		uint FileHfsCreatorCode ([Target] NSDictionary fileAttributes);

		[Export ("fileHFSTypeCode")]
		uint FileHfsTypeCode ([Target] NSDictionary fileAttributes);

		[Export ("fileIsImmutable")]
		bool FileIsImmutable ([Target] NSDictionary fileAttributes);

		[Export ("fileIsAppendOnly")]
		bool FileIsAppendOnly ([Target] NSDictionary fileAttributes);

		[Export ("fileCreationDate")]
		NSDate FileCreationDate ([Target] NSDictionary fileAttributes);

		[Export ("fileOwnerAccountID")]
		NSNumber FileOwnerAccountID ([Target] NSDictionary fileAttributes);

		[Export ("fileGroupOwnerAccountID")]
		NSNumber FileGroupOwnerAccountID ([Target] NSDictionary fileAttributes);
#endif
	}

	public delegate bool NSPredicateEvaluator (NSObject evaluatedObject, NSDictionary bindings);
	
	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	public interface NSPredicate {
		[Static]
		[Export ("predicateWithFormat:argumentArray:")]
		NSPredicate FromFormat (string predicateFormat, NSObject[] arguments);

		[Static, Export ("predicateWithValue:")]
		NSPredicate FromValue (bool value);

		[Static, Export ("predicateWithBlock:")]
		NSPredicate FromExpression (NSPredicateEvaluator evaluator);

		[Export ("predicateFormat")]
		string PredicateFormat { get; }

		[Export ("predicateWithSubstitutionVariables:")]
		NSPredicate PredicateWithSubstitutionVariables (NSDictionary substitutionVariables);

		[Export ("evaluateWithObject:")]
		bool EvaluateWithObject (NSObject obj);

		[Export ("evaluateWithObject:substitutionVariables:")]
		bool EvaluateWithObject (NSObject obj, NSDictionary substitutionVariables);
	}

#if MONOMAC
	[BaseType (typeof (NSObject), Name="NSURLDownload")]
	public interface NSUrlDownload {
		[Static, Export ("canResumeDownloadDecodedWithEncodingMIMEType:")]
		bool CanResumeDownloadDecodedWithEncodingMimeType (string mimeType);

		[Export ("initWithRequest:delegate:")]
		IntPtr Constructor (NSUrlRequest request, NSObject delegate1);

		[Export ("initWithResumeData:delegate:path:")]
		IntPtr Constructor (NSData resumeData, NSObject delegate1, string path);

		[Export ("cancel")]
		void Cancel ();

		[Export ("setDestination:allowOverwrite:")]
		void SetDestination (string path, bool allowOverwrite);

		[Export ("request")]
		NSUrlRequest Request { get; }

		[Export ("resumeData")]
		NSData ResumeData { get; }

		[Export ("deletesFileUponFailure")]
		bool DeletesFileUponFailure { get; set; }
	}

    	[BaseType (typeof (NSObject))]
    	[Model]
	public interface NSUrlDownloadDelegate {
		[Export ("downloadDidBegin:")]
		void DownloadBegan (NSUrlDownload download);

		[Export ("download:willSendRequest:redirectResponse:")]
		NSUrlRequest WillSendRequest (NSUrlDownload download, NSUrlRequest request, NSUrlResponse redirectResponse);

		[Export ("download:didReceiveAuthenticationChallenge:")]
		void ReceivedAuthenticationChallenge (NSUrlDownload download, NSUrlAuthenticationChallenge challenge);

		[Export ("download:didCancelAuthenticationChallenge:")]
		void CanceledAuthenticationChallenge (NSUrlDownload download, NSUrlAuthenticationChallenge challenge);

		[Export ("download:didReceiveResponse:")]
		void ReceivedResponse (NSUrlDownload download, NSUrlResponse response);

		//- (void)download:(NSUrlDownload *)download willResumeWithResponse:(NSUrlResponse *)response fromByte:(long long)startingByte;
		[Export ("download:willResumeWithResponse:fromByte:")]
		void Resume (NSUrlDownload download, NSUrlResponse response, long startingByte);

		//- (void)download:(NSUrlDownload *)download didReceiveDataOfLength:(NSUInteger)length;
		[Export ("download:didReceiveDataOfLength:")]
		void ReceivedData (NSUrlDownload download, uint length);

		[Export ("download:shouldDecodeSourceDataOfMIMEType:")]
		bool DecodeSourceData (NSUrlDownload download, string encodingType);

		[Export ("download:decideDestinationWithSuggestedFilename:")]
		void DecideDestination (NSUrlDownload download, string suggestedFilename);

		[Export ("download:didCreateDestination:")]
		void CreatedDestination (NSUrlDownload download, string path);

		[Export ("downloadDidFinish:")]
		void Finished (NSUrlDownload download);

		[Export ("download:didFailWithError:")]
		void FailedWithError(NSUrlDownload download, NSError error);
	}
#endif

	[BaseType (typeof (NSObject), Name="NSURLProtocolClient")]
	[Model]
	interface NSUrlProtocolClient {
		[Abstract]
		[Export ("UrlProtocol:wasRedirectedToRequest:redirectResponse:"), EventArgs ("NSUrlProtocolRedirect")]
		void Redirected (NSUrlProtocol protocol, NSUrlRequest redirectedToEequest, NSUrlResponse redirectResponse);

		[Abstract]
		[Export ("UrlProtocol:cachedResponseIsValid:"), EventArgs ("NSUrlProtocolCachedResponse")]
		void CachedResponseIsValid (NSUrlProtocol protocol, NSCachedUrlResponse cachedResponse);

		[Abstract]
		[Export ("UrlProtocol:didReceiveResponse:cacheStoragePolicy:"), EventArgs ("NSUrlProtocolResponse")]
		void ReceivedResponse (NSUrlProtocol protocol, NSUrlResponse response, NSUrlCacheStoragePolicy policy);

		[Abstract]
		[Export ("UrlProtocol:didLoadData:"), EventArgs ("NSUrlProtocolData")]
		void DataLoaded (NSUrlProtocol protocol, NSData data);

		[Abstract]
		[Export ("UrlProtocolDidFinishLoading:")]
		void FinishedLoading (NSUrlProtocol protocol);

		[Abstract]
		[Export ("UrlProtocol:didFailWithError:"), EventArgs ("NSUrlProtocolError")]
		void FailedWithError (NSUrlProtocol protocol, NSError error);

		[Abstract]
		[Export ("UrlProtocol:didReceiveAuthenticationChallenge:"), EventArgs ("NSUrlProtocolChallenge")]
		void ReceivedAuthenticationChallenge (NSUrlProtocol protocol, NSUrlAuthenticationChallenge challenge);

		[Abstract]
		[Export ("UrlProtocol:didCancelAuthenticationChallenge:"), EventArgs ("NSUrlProtocolChallenge")]
		void CancelledAuthenticationChallenge (NSUrlProtocol protocol, NSUrlAuthenticationChallenge challenge);
	}

	[BaseType (typeof (NSObject),
		   Delegates=new string [] {"WeakClient"},
		   Events=new Type [] {typeof (NSUrlProtocolClient)})]
	interface NSUrlProtocol {
		[Export ("initWithRequest:cachedResponse:client:")]
		IntPtr Constructor (NSUrlRequest request, NSCachedUrlResponse cachedResponse, NSUrlProtocolClient client);

		[Export ("client")]
		NSObject WeakClient { get; set; }

		[Wrap ("WeakClient")]
		NSUrlProtocolClient Client { get; set; }

		[Export ("request")]
		NSUrlRequest Request { get; }

		[Export ("cachedResponse")]
		NSCachedUrlResponse CachedResponse { get; }

		[Export ("canInitWithRequest:")]
		bool CanInitWithRequest (NSUrlRequest request);

		[Static]
		[Export ("canonicalRequestForRequest:")]
		NSUrlRequest GetCanonicalRequest (NSUrlRequest forRequest);

		[Static]
		[Export ("requestIsCacheEquivalent:toRequest:")]
		bool IsRequestCacheEquivalent (NSUrlRequest first, NSUrlRequest second);

		[Export ("startLoading")]
		void StartLoading ();

		[Export ("stopLoading")]
		void StopLoading ();

		[Static]
		[Export ("propertyForKey:inRequest:")]
		NSObject GetProperty (string key, NSUrlRequest inRequest);

		[Static]
		[Export ("setProperty:forKey:inRequest:")]
		void SetProperty ([NullAllowed] NSObject value, string key, NSMutableUrlRequest inRequest);

		[Static]
		[Export ("removePropertyForKey:inRequest:")]
		void RemoveProperty (string propertyKey, NSMutableUrlRequest request);

		[Static]
		[Export ("registerClass:")]
		bool RegisterClass (Class protocolClass);

		[Static]
		[Export ("unregisterClass:")]
		void UnregisterClass (Class protocolClass);
	}

}

