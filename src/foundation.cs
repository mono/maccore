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

#if !MONOMAC
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

		[Field ("NSFontAttributeName")]
		NSString FontAttributeName { get; }

		[Field ("NSParagraphStyleAttributeName")]
		NSString ParagraphStyleAttributeName { get; }
	}

	[BaseType (typeof (NSObject),
		   Delegates=new string [] { "WeakDelegate" },
		   Events=new Type [] { typeof (NSCacheDelegate)} )]
	[Since (4,0)]
	interface NSCache {
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
	interface NSCacheDelegate {
		[Export ("cache:willEvictObject:"), EventArgs ("NSObject")]
		void WillEvictObject (NSCache cache, NSObject obj);
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
	interface NSDateComponents {
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
	interface NSDateFormatter {
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
	interface NSFormatter {
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
		
		[Export ("archiver:willEncodeObject:"), EventArgs ("NSEncodeHook"), DefaultValue (null)]
		NSObject WillEncode (NSKeyedArchiver archiver, NSObject obj);
		
		[Export ("archiverWillFinish:")]
		void Finishing (NSKeyedArchiver archiver);
		
		[Export ("archiver:willReplaceObject:withObject:"), EventArgs ("NSArchiveReplace")]
		void ReplacingObject (NSKeyedArchiver archiver, NSObject oldObject, NSObject newObject);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSKeyedUnarchiverDelegate {
		[Export ("unarchiver:didDecodeObject:"), EventArgs ("NSDecoderCallback"), DefaultValue (null)]
		NSObject DecodedObject (NSKeyedUnarchiver unarchiver, NSObject obj);
		
		[Export ("unarchiverDidFinish:")]
		void Finished (NSKeyedUnarchiver unarchiver);
		
		[Export ("unarchiver:cannotDecodeObjectOfClassName:originalClasses:"), EventArgs ("NSDecoderHandler"), DefaultValue (null)]
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
		void AddAttribute (string name, NSObject value, NSRange range);

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
		[Static][Internal]
		NSDictionary FromObjectsAndKeys (NSObject [] objects, NSObject [] Keys, int count);

		[Export ("dictionaryWithObjects:forKeys:")]
		[Static]
		NSDictionary FromObjectsAndKeys (NSObject [] objects, NSObject [] Keys);

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
		[Export ("domain")]
		string Domain { get; }

		[Export ("code")]
		int Code { get; }

		[Export ("userInfo")]
		NSDictionary UserInfo { get; }

		[Export ("localizedDescription")]
		string LocalizedDescription { get; }
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
		string CurrentMode { get; }

		[Export ("getCFRunLoop")]
		CFRunLoop GetCFRunLoop ();

		[Export ("addTimer:forMode:")]
		void AddTimer (NSTimer timer, string forMode);

		[Export ("limitDateForMode:")]
		NSDate LimitDateForMode (string mode);

		[Export ("acceptInputForMode:beforeDate:")]
		void AcceptInputForMode (string mode, NSDate limitDate);

		[Export ("run")]
		void Run ();

		[Export ("runUntilDate:")]
		void RunUntil (NSDate date);
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
	interface NSSortDescriptor {
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
		
		//[Export ("port")]
		//NSNumber Port { get;}
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

		//[Export ("connection:willCacheResponse:")]
		//NSCachedUrlResponse WillCacheResponse (NSUrlConnection connection, NSCachedUrlResponse cachedResponse);
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
		NSMutableDictionary FromObjectsAndKeys (NSObject [] objects, NSObject [] Keys, int count);

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

	[BaseType (typeof (NSObject))]
	public interface NSStream {
		[Export ("close")]
		void Close ();
	
		[Export ("delegate")]
		NSObject Delegate { get; set; }

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
	}

#if !MONOMAC
	[BaseType (typeof (NSObject)), Bind ("NSString")]
	interface NSString2 {
		[Bind ("sizeWithFont:")]
		SizeF StringSize (UIFont font);
		
		[Bind ("sizeWithFont:forWidth:lineBreakMode:")]
		SizeF StringSize (UIFont font, float forWidth, UILineBreakMode breakMode);
		
		[Bind ("sizeWithFont:constrainedToSize:")]
		SizeF StringSize (UIFont font, SizeF constrainedToSize);
		
		[Bind ("sizeWithFont:constrainedToSize:lineBreakMode:")]
		SizeF StringSize (UIFont font, SizeF constrainedToSize, UILineBreakMode lineBreakMode);
	}
#endif
	
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

	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	interface NSOperation {
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
	interface NSBlockOperation {
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
	interface NSOperationQueue {
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
		string PathForResourceAbsolute (string name, [NullAllowed]string ofType, string bundleDirectory);
		
		[Export ("pathForResource:ofType:")]
		string PathForResource (string name, string ofType);

		// TODO: this conflicts with the above with our generator.
		//[Export ("pathForResource:ofType:inDirectory:")]
		//string PathForResource (string name, string ofType, string subpath);
		
		[Export ("pathForResource:ofType:inDirectory:forLocalization:")]
		string PathForResource (string name, string ofType, string subpath, string localizationName);

		[Export ("localizedStringForKey:value:table:")]
		string LocalizedString (string key, string value, string table);

		[Export ("objectForInfoDictionaryKey:")]
		NSObject ObjectForInfoDictionary (string key);

		[Export ("developmentLocalization")]
		string DevelopmentLocalization { get; }

		// Additions from AppKit
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
	interface NSNetService {
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
		bool TxtRecordData { get; set; }

		[Export ("startMonitoring")]
		void StartMonitoring ();

		[Export ("stopMonitoring")]
		void StopMonitoring ();
	}

	[Model, BaseType (typeof (NSObject))]
	interface NSNetServiceDelegate {
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
	interface NSNetServiceBrowser {
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
	interface NSNetServiceBrowserDelegate {
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
	interface NSNotification {
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
	interface NSNotificationCenter {
		[Static][Export ("defaultCenter")]
		NSNotificationCenter DefaultCenter { get; }
	
		[Export ("addObserver:selector:name:object:")]
		void AddObserver ([RetainList (true, "ObserverList")] NSObject observer, Selector aSelector, [NullAllowed] string aName, [NullAllowed] NSObject anObject);
	
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

	[BaseType (typeof (NSObject))]
	interface NSNotificationQueue {
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

	delegate void NSNotificationHandler (NSNotification notification);
	
	[BaseType (typeof (NSObject))]
	interface NSValue {
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
	
		[Export ("CGPointValue")]
		System.Drawing.PointF PointFValue { get; }

		[Export ("CGRectValue")]
		System.Drawing.RectangleF RectangleFValue { get; }
		
		[Export ("CGSizeValue")]
		System.Drawing.SizeF SizeFValue { get; }

		[Export ("CATransform3DValue")]
		MonoMac.CoreAnimation.CATransform3D CATransform3DValue { get; }

#if !MONOMAC
		[Export ("CGAffineTransformValue")]
		MonoMac.CoreGraphics.CGAffineTransform CGAffineTransformValue { get; }
		
		[Export ("UIEdgeInsetsValue")]
		MonoMac.UIKit.UIEdgeInsets UIEdgeInsetsValue { get; }

		[Export ("valueWithCGAffineTransform:")][Static]
		NSValue FromCGAffineTransform (MonoMac.CoreGraphics.CGAffineTransform tran);

		[Export ("valueWithUIEdgeInsets:")][Static]
		NSValue FromUIEdgeInsets (MonoMac.UIKit.UIEdgeInsets insets);
#endif
		[Export ("valueWithCGPoint:")][Static]
		NSValue FromPointF (System.Drawing.PointF point);
		
		[Export ("valueWithCGRect:")][Static]
		NSValue FromRectangleF (System.Drawing.RectangleF rect);
		
		[Export ("valueWithCGSize:")][Static]
		NSValue FromSizeF (System.Drawing.SizeF size);

		[Export ("valueWithCATransform3D:")][Static]
		NSValue FromCATransform3D (MonoMac.CoreAnimation.CATransform3D transform);
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
		
		//[Export ("initWithDecimal:")]
		//IntPtr Constructor (NSDecimal dec);

		[Export ("initWithString:")]
		IntPtr Constructor (string numberValue);

		[Export ("initWithString:locale:")]
		IntPtr Constructor (string numberValue, NSObject locale);

		[Export ("descriptionWithLocale:")]
		string DescriptionWithLocale (NSLocale locale);

		//[Export ("decimalValue")]
		//NSDecimal NSDecimalValue { get; }

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

	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	interface NSPredicate {
		[Export ("evaluateWithObject:")]
		bool EvaluateWithObject (NSObject obj);
	}

	[BaseType (typeof (NSMutableData))]
	[Since (4,0)]
	interface NSPurgeableData {
		
	}
}

