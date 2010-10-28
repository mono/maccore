// 
// Items.cs: Implements the KeyChain query access APIs
//
// We use strong types and a helper SecQuery class to simplify the
// creation of the dictionary used to query the Keychain
// 
// Authors: Miguel de Icaza
//     
// Copyright 2010 Novell, Inc
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
using MonoMac.CoreFoundation;
using MonoMac.Foundation;
using System.Runtime.InteropServices;

namespace MonoMac.Security {

	public class SecurityException : Exception {
		static string ToMessage (SecStatusCode code)
		{
			
			switch (code){
			case SecStatusCode.Success: 
			case SecStatusCode.Unimplemented: 
			case SecStatusCode.Param:
			case SecStatusCode.Allocate:
			case SecStatusCode.NotAvailable:
			case SecStatusCode.DuplicateItem:
			case SecStatusCode.ItemNotFound:
			case SecStatusCode.InteractionNotAllowed:
			case SecStatusCode.Decode:
				return code.ToString ();
			}
			return String.Format ("Unknown error: 0x{0:x}", code);
		}
		
		public SecurityException (SecStatusCode code) : base (ToMessage (code))
		{
		}
	}
	
	public class SecKeyChain {
		public static SecStatusCode TryQueryAsData (SecRecord query, bool wantPersistentReference, out NSData result)
		{
			if (query == null){
				result = null;
				return SecStatusCode.Param;
			}

			using (var copy = NSMutableDictionary.FromDictionary (query.queryDict)){
				copy.SetObject (CFBoolean.TrueObject, SecItem.ReturnData);

				IntPtr ptr;
				var ret = SecItem.CopyMatching (copy, out ptr);
				if (ret == SecStatusCode.Success)
					result = new NSData (ptr);
				else
					result = null;
				return ret;
			}
		}

		public static NSData QueryAsData (SecRecord query, bool wantPersistentReference)
		{
			if (query == null)
				throw new ArgumentNullException ("query");
			
			NSData result;
			
			var code = TryQueryAsData (query, wantPersistentReference, out result);
			if (code != SecStatusCode.Success)
				throw new SecurityException (code);
			return result;
		}

		public static SecStatusCode TryQueryAsRecord (SecRecord query, out SecRecord result)
		{
			if (query == null){
				result = null;
				return SecStatusCode.Param;
			}
			
			using (var copy = NSMutableDictionary.FromDictionary (query.queryDict)){
				copy.SetObject (CFBoolean.TrueObject, SecItem.ReturnAttributes);

				IntPtr ptr;
				var ret = SecItem.CopyMatching (copy, out ptr);
				if (ret == SecStatusCode.Success)
					result = new SecRecord (new NSMutableDictionary (new NSDictionary (ptr)));
				else
					result = null;
				return ret;
			}
		}
		
		public static SecRecord QueryAsRecord (SecRecord query)
		{
			if (query == null)
				throw new ArgumentNullException ("query");
			
			SecRecord result;
			
			var code = TryQueryAsRecord (query, out result);
			if (code != SecStatusCode.Success)
				throw new SecurityException (code);
			return result;
		}

		public static SecStatusCode TryQueryAsConcreteType (SecRecord query, out object result)
		{
			if (query == null){
				result = null;
				return SecStatusCode.Param;
			}
			
			using (var copy = NSMutableDictionary.FromDictionary (query.queryDict)){
				copy.SetObject (CFBoolean.TrueObject, SecItem.ReturnRef);

				IntPtr ptr;
				var ret = SecItem.CopyMatching (copy, out ptr);
				if (ret == SecStatusCode.Success){
					int cfType = CFType.GetTypeID (ptr);

					if (cfType == SecCertificate.GetTypeID ())
						result = new SecCertificate (ptr, true);
					else if (cfType == SecKey.GetTypeID ())
						result = new SecKey (ptr, true);
					else if (cfType == SecIdentity.GetTypeID ())
						result = new SecIdentity (ptr, true);
					else
						throw new Exception (String.Format ("Unexpected type: 0x{0:x}", ret));
					result = null;
				} else
					result = null;
				return ret;
			}
		}
		
		static object QueryAsConcreteType (SecRecord query)
		{
			if (query == null)
				throw new ArgumentNullException ("query");
			
			object result;
			
			var code = TryQueryAsConcreteType (query, out result);
			if (code != SecStatusCode.Success)
				throw new SecurityException (code);
			return result;
		}
	}
	
	public class SecRecord {
		internal NSMutableDictionary queryDict;

		internal SecRecord (NSMutableDictionary dict)
		{
			queryDict = dict;
		}
		
		public SecRecord (SecClass secClassKind)
		{
			queryDict = NSMutableDictionary.FromObjectAndKey (secClassKind, SecClass.SecClassKey);
		}

		//
		// Attributes
		//
		public SecAttributeAccesssible Accessible {
			get {
				return new SecAttributeAccesssible (queryDict.ObjectForKey (SecAttributeKey.AttrAccessible).Handle);
			}
			
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (value, SecAttributeKey.AttrAccessible);
			}
		}

		public NSDate CreationDate {
			get {
				return (NSDate) (queryDict.ObjectForKey (SecAttributeKey.AttrCreationDate));
			}
			
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (value, SecAttributeKey.AttrCreationDate);
			}
		}

		public NSDate ModificationDate {
			get {
				return (NSDate) (queryDict.ObjectForKey (SecAttributeKey.AttrModificationDate));
			}
			
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (value, SecAttributeKey.AttrModificationDate);
			}
		}

		public string Description {
			get {
				return (string)((NSString) queryDict.ObjectForKey (SecAttributeKey.AttrDescription));
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (new NSString (value), SecAttributeKey.AttrDescription);
			}
		}

		public string Comment {
			get {
				return (string)((NSString) queryDict.ObjectForKey (SecAttributeKey.AttrComment));
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (new NSString (value), SecAttributeKey.AttrComment);
			}
		}

		public int Creator {
			get {
				return ((NSNumber)queryDict.ObjectForKey (SecAttributeKey.AttrCreator)).Int32Value;
			}
					
			set {
				queryDict.SetObject (new NSNumber (value), SecAttributeKey.AttrCreator);
			}
		}

		public int CreatorType {
			get {
				return ((NSNumber) queryDict.ObjectForKey (SecAttributeKey.AttrType)).Int32Value;
			}
					
			set {
				queryDict.SetObject (new NSNumber (value), SecAttributeKey.AttrType);
			}
		}

		public string Label {
			get {
				return (string)((NSString) queryDict.ObjectForKey (SecAttributeKey.AttrLabel));
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (new NSString (value), SecAttributeKey.AttrLabel);
			}
		}

		public bool Invisible {
			get {
				return queryDict.ObjectForKey (SecAttributeKey.AttrIsInvisible).Handle == CFBoolean.True.Handle;
			}
			
			set {
				queryDict.SetObject (CFBoolean.GetBoolObject (value), SecAttributeKey.AttrIsInvisible);
			}
		}

		public bool IsNegative {
			get {
				return queryDict.ObjectForKey (SecAttributeKey.AttrIsNegative).Handle == CFBoolean.True.Handle;
			}
			
			set {
				queryDict.SetObject (CFBoolean.GetBoolObject (value), SecAttributeKey.AttrIsNegative);
			}
		}

		public string Account {
			get {
				return (string)((NSString) queryDict.ObjectForKey (SecAttributeKey.AttrAccount));
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (new NSString (value), SecAttributeKey.AttrAccount);
			}
		}

		public string Service {
			get {
				return (string)((NSString) queryDict.ObjectForKey (SecAttributeKey.AttrService));
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (new NSString (value), SecAttributeKey.AttrService);
			}
		}

		public string Generic {
			get {
				return (string)((NSString) queryDict.ObjectForKey (SecAttributeKey.AttrGeneric));
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (new NSString (value), SecAttributeKey.AttrGeneric);
			}
		}

		public string SecurityDomain {
			get {
				return (string)((NSString) queryDict.ObjectForKey (SecAttributeKey.AttrSecurityDomain));
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (new NSString (value), SecAttributeKey.AttrSecurityDomain);
			}
		}

		public string Server {
			get {
				return (string)((NSString) queryDict.ObjectForKey (SecAttributeKey.AttrServer));
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (new NSString (value), SecAttributeKey.AttrServer);
			}
		}

		public SecProtocol Protocol {
			get {
				return new SecProtocol (queryDict.ObjectForKey (SecAttributeKey.AttrProtocol).Handle);
			}
			
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (value, SecAttributeKey.AttrProtocol);
			}
		}

		public SecAuthenticationType AuthenticationType {
			get {
				return new SecAuthenticationType (queryDict.ObjectForKey (SecAttributeKey.AttrAuthenticationType).Handle);
			}
			
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			
				queryDict.SetObject (value, SecAttributeKey.AttrAuthenticationType);
			}
		}

		public int Port {
			get {
				return ((NSNumber) queryDict.ObjectForKey (SecAttributeKey.AttrPort)).Int32Value;
			}
					
			set {
				queryDict.SetObject (new NSNumber (value), SecAttributeKey.AttrPort);
			}
		}

		public string Path {
			get {
				return (string)((NSString) queryDict.ObjectForKey (SecAttributeKey.AttrPath));
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (new NSString (value), SecAttributeKey.AttrPath);
			}
		}

		// read only
		public string Subject {
			get {
				return (string)((NSString) queryDict.ObjectForKey (SecAttributeKey.AttrSubject));
			}
		}

		// read only
		public NSData Issuer {
			get {
				return (NSData) (queryDict.ObjectForKey (SecAttributeKey.AttrIssuer));
			}
		}

		// read only
		public NSData SerialNumber {
			get {
				return (NSData) (queryDict.ObjectForKey (SecAttributeKey.AttrSerialNumber));
			}
		}

		// read only
		public NSData SubjectKeyID {
			get {
				return (NSData) (queryDict.ObjectForKey (SecAttributeKey.AttrSubjectKeyID));
			}
		}

		// read only
		public NSData PublicKeyHash {
			get {
				return (NSData) (queryDict.ObjectForKey (SecAttributeKey.AttrPublicKeyHash));
			}
		}

		// read only
		public NSNumber CertificateType {
			get {
				return (NSNumber) (queryDict.ObjectForKey (SecAttributeKey.AttrCertificateType));
			}
		}

		// read only
		public NSNumber CertificateEncoding {
			get {
				return (NSNumber) (queryDict.ObjectForKey (SecAttributeKey.AttrCertificateEncoding));
			}
		}

		public SecKeyClass KeyClass {
			get {
				return new SecKeyClass (queryDict.ObjectForKey (SecAttributeKey.AttrKeyClass).Handle);
			}
		}

		public string ApplicationLabel {
			get {
				return (string)((NSString) queryDict.ObjectForKey (SecAttributeKey.AttrApplicationLabel));
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (new NSString (value), SecAttributeKey.AttrApplicationLabel);
			}
		}

		public bool IsPermanent {
			get {
				return queryDict.ObjectForKey (SecAttributeKey.AttrIsPermanent).Handle == CFBoolean.True.Handle;
			}
			
			set {
				queryDict.SetObject (CFBoolean.GetBoolObject (value), SecAttributeKey.AttrIsPermanent);
			}
		}

		public NSData ApplicationTag {
			get {
				return (NSData) (queryDict.ObjectForKey (SecAttributeKey.AttrApplicationTag));
			}
			
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (value, SecAttributeKey.AttrApplicationTag);
			}
		}

		public SecKeyType KeyType {
			get {
				return new SecKeyType (queryDict.ObjectForKey (SecAttributeKey.AttrKeyType).Handle);
			}
			
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (value, SecAttributeKey.AttrKeyType);
			}
		}

		public int KeySizeInBits {
			get {
				return ((NSNumber) queryDict.ObjectForKey (SecAttributeKey.AttrKeySizeInBits)).Int32Value;
			}
					
			set {
				queryDict.SetObject (new NSNumber (value), SecAttributeKey.AttrKeySizeInBits);
			}
		}

		public int EffectiveKeySize {
			get {
				return ((NSNumber) queryDict.ObjectForKey (SecAttributeKey.AttrEffectiveKeySize)).Int32Value;
			}
					
			set {
				queryDict.SetObject (new NSNumber (value), SecAttributeKey.AttrEffectiveKeySize);
			}
		}

		public bool CanEncrypt {
			get {
				return queryDict.ObjectForKey (SecAttributeKey.AttrCanEncrypt).Handle == CFBoolean.True.Handle;
			}
			
			set {
				queryDict.SetObject (CFBoolean.GetBoolObject (value), SecAttributeKey.AttrCanEncrypt);
			}
		}

		public bool CanDecrypt {
			get {
				return queryDict.ObjectForKey (SecAttributeKey.AttrCanDecrypt).Handle == CFBoolean.True.Handle;
			}
			
			set {
				queryDict.SetObject (CFBoolean.GetBoolObject (value), SecAttributeKey.AttrCanDecrypt);
			}
		}

		public bool CanDerive {
			get {
				return queryDict.ObjectForKey (SecAttributeKey.AttrCanDerive).Handle == CFBoolean.True.Handle;
			}
			
			set {
				queryDict.SetObject (CFBoolean.GetBoolObject (value), SecAttributeKey.AttrCanDerive);
			}
		}

		public bool CanSign {
			get {
				return queryDict.ObjectForKey (SecAttributeKey.AttrCanSign).Handle == CFBoolean.True.Handle;
			}
			
			set {
				queryDict.SetObject (CFBoolean.GetBoolObject (value), SecAttributeKey.AttrCanSign);
			}
		}

		public bool CanVerify {
			get {
				return queryDict.ObjectForKey (SecAttributeKey.AttrCanVerify).Handle == CFBoolean.True.Handle;
			}
			
			set {
				queryDict.SetObject (CFBoolean.GetBoolObject (value), SecAttributeKey.AttrCanVerify);
			}
		}

		public bool CanWrap {
			get {
				return queryDict.ObjectForKey (SecAttributeKey.AttrCanWrap).Handle == CFBoolean.True.Handle;
			}
			
			set {
				queryDict.SetObject (CFBoolean.GetBoolObject (value), SecAttributeKey.AttrCanWrap);
			}
		}

		public bool CanUnwrap {
			get {
				return queryDict.ObjectForKey (SecAttributeKey.AttrCanUnwrap).Handle == CFBoolean.True.Handle;
			}
			
			set {
				queryDict.SetObject (CFBoolean.GetBoolObject (value), SecAttributeKey.AttrCanUnwrap);
			}
		}

		public string AccessGroup {
			get {
				return (string)((NSString) queryDict.ObjectForKey (SecAttributeKey.AttrAccessGroup));
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (new NSString (value), SecAttributeKey.AttrAccessGroup);
			}
		}

		//
		// Matches
		//

		public SecPolicy MatchPolicy {
			get {
				return new SecPolicy (queryDict.ObjectForKey (SecItem.MatchPolicy).Handle);
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (value, SecItem.MatchPolicy);
			}
		}

		public NSArray MatchItemList {
			get {
				return (NSArray) queryDict.ObjectForKey (SecItem.MatchItemList);
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (value, SecItem.MatchItemList);
			}
		}

		public NSData [] MatchIssuers {
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				
				queryDict.SetObject (NSArray.FromNSObjects (value), SecItem.MatchIssuers);
			}
		}

		public string MatchEmailAddressIfPresent {
			get {
				return (string)((NSString) queryDict.ObjectForKey (SecItem.MatchEmailAddressIfPresent));
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (new NSString (value), SecItem.MatchEmailAddressIfPresent);
			}
		}

		public string MatchSubjectContains {
			get {
				return (string)((NSString) queryDict.ObjectForKey (SecItem.MatchSubjectContains));
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (new NSString (value), SecItem.MatchSubjectContains);
			}
		}

		public bool MatchCaseInsensitive {
			get {
				return queryDict.ObjectForKey (SecItem.MatchCaseInsensitive).Handle == CFBoolean.True.Handle;
			}
			
			set {
				queryDict.SetObject (CFBoolean.GetBoolObject (value), SecItem.MatchCaseInsensitive);
			}
		}

		public bool MatchTrustedOnly {
			get {
				return queryDict.ObjectForKey (SecItem.MatchTrustedOnly).Handle == CFBoolean.True.Handle;
			}
			
			set {
				queryDict.SetObject (CFBoolean.GetBoolObject (value), SecItem.MatchTrustedOnly);
			}
		}

		public NSDate MatchValidOnDate {
			get {
				return (NSDate) (queryDict.ObjectForKey (SecItem.MatchValidOnDate));
			}
			
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (value, SecItem.MatchValidOnDate);
			}
		}

		public SecMatchLimit MatchLimit {
			get {
				return new SecMatchLimit (queryDict.ObjectForKey (SecItem.MatchLimit).Handle);
			}
					
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				queryDict.SetObject (value, SecItem.MatchLimit);
			}
		}
	}
	
	public class SecItem {
		internal static IntPtr securityLibrary = Dlfcn.dlopen (Constants.SecurityLibrary, 0);

		[DllImport (Constants.SecurityLibrary)]
		extern static SecStatusCode SecItemCopyMatching (IntPtr cfDictRef, out IntPtr result);

		public static SecStatusCode CopyMatching (NSDictionary queryDictionary, out IntPtr result)
		{
			if (queryDictionary == null)
				throw new ArgumentNullException ("queryDictionary");
			return SecItemCopyMatching (queryDictionary.Handle, out result);
		}

		static NSObject _MatchPolicy;
		public static NSObject MatchPolicy {
			get {
				if (_MatchPolicy == null)
					_MatchPolicy = new NSObject (Dlfcn.GetIntPtr (securityLibrary, "kSecMatchPolicy"));
				return _MatchPolicy;
			}
		}
		
		static NSObject _MatchItemList;
		public static NSObject MatchItemList {
			get {
				if (_MatchItemList == null)
					_MatchItemList = new NSObject (Dlfcn.GetIntPtr (securityLibrary, "kSecMatchItemList"));
				return _MatchItemList;
			}
		}
		
		static NSObject _MatchSearchList;
		public static NSObject MatchSearchList {
			get {
				if (_MatchSearchList == null)
					_MatchSearchList = new NSObject (Dlfcn.GetIntPtr (securityLibrary, "kSecMatchSearchList"));
				return _MatchSearchList;
			}
		}
		
		static NSObject _MatchIssuers;
		public static NSObject MatchIssuers {
			get {
				if (_MatchIssuers == null)
					_MatchIssuers = new NSObject (Dlfcn.GetIntPtr (securityLibrary, "kSecMatchIssuers"));
				return _MatchIssuers;
			}
		}
		
		static NSObject _MatchEmailAddressIfPresent;
		public static NSObject MatchEmailAddressIfPresent {
			get {
				if (_MatchEmailAddressIfPresent == null)
					_MatchEmailAddressIfPresent = new NSObject (Dlfcn.GetIntPtr (securityLibrary, "kSecMatchEmailAddressIfPresent"));
				return _MatchEmailAddressIfPresent;
			}
		}
		
		static NSObject _MatchSubjectContains;
		public static NSObject MatchSubjectContains {
			get {
				if (_MatchSubjectContains == null)
					_MatchSubjectContains = new NSObject (Dlfcn.GetIntPtr (securityLibrary, "kSecMatchSubjectContains"));
				return _MatchSubjectContains;
			}
		}
		
		static NSObject _MatchCaseInsensitive;
		public static NSObject MatchCaseInsensitive {
			get {
				if (_MatchCaseInsensitive == null)
					_MatchCaseInsensitive = new NSObject (Dlfcn.GetIntPtr (securityLibrary, "kSecMatchCaseInsensitive"));
				return _MatchCaseInsensitive;
			}
		}
		
		static NSObject _MatchTrustedOnly;
		public static NSObject MatchTrustedOnly {
			get {
				if (_MatchTrustedOnly == null)
					_MatchTrustedOnly = new NSObject (Dlfcn.GetIntPtr (securityLibrary, "kSecMatchTrustedOnly"));
				return _MatchTrustedOnly;
			}
		}
		
		static NSObject _MatchValidOnDate;
		public static NSObject MatchValidOnDate {
			get {
				if (_MatchValidOnDate == null)
					_MatchValidOnDate = new NSObject (Dlfcn.GetIntPtr (securityLibrary, "kSecMatchValidOnDate"));
				return _MatchValidOnDate;
			}
		}
		
		static NSObject _MatchLimit;
		public static NSObject MatchLimit {
			get {
				if (_MatchLimit == null)
					_MatchLimit = new NSObject (Dlfcn.GetIntPtr (securityLibrary, "kSecMatchLimit"));
				return _MatchLimit;
			}
		}

		static NSObject _ReturnData;
		public static NSObject ReturnData {
			get {
				if (_ReturnData == null)
					_ReturnData = new NSObject (Dlfcn.GetIntPtr (securityLibrary, "kSecReturnData"));
				return _ReturnData;
			}
		}
		
		static NSObject _ReturnAttributes;
		public static NSObject ReturnAttributes {
			get {
				if (_ReturnAttributes == null)
					_ReturnAttributes = new NSObject (Dlfcn.GetIntPtr (securityLibrary, "kSecReturnAttributes"));
				return _ReturnAttributes;
			}
		}
		
		static NSObject _ReturnRef;
		public static NSObject ReturnRef {
			get {
				if (_ReturnRef == null)
					_ReturnRef = new NSObject (Dlfcn.GetIntPtr (securityLibrary, "kSecReturnRef"));
				return _ReturnRef;
			}
		}
		
		static NSObject _ReturnPersistentRef;
		public static NSObject ReturnPersistentRef {
			get {
				if (_ReturnPersistentRef == null)
					_ReturnPersistentRef = new NSObject (Dlfcn.GetIntPtr (securityLibrary, "kSecReturnPersistentRef"));
				return _ReturnPersistentRef;
			}
		}
		
		static NSObject _ValueData;
		public static NSObject ValueData {
			get {
				if (_ValueData == null)
					_ValueData = new NSObject (Dlfcn.GetIntPtr (securityLibrary, "kSecValueData"));
				return _ValueData;
			}
		}
		
		static NSObject _ValueRef;
		public static NSObject ValueRef {
			get {
				if (_ValueRef == null)
					_ValueRef = new NSObject (Dlfcn.GetIntPtr (securityLibrary, "kSecValueRef"));
				return _ValueRef;
			}
		}
		
		static NSObject _ValuePersistentRef;
		public static NSObject ValuePersistentRef {
			get {
				if (_ValuePersistentRef == null)
					_ValuePersistentRef = new NSObject (Dlfcn.GetIntPtr (securityLibrary, "kSecValuePersistentRef"));
				return _ValuePersistentRef;
			}
		}
		
		static NSObject _UseItemList;
		public static NSObject UseItemList {
			get {
				if (_UseItemList == null)
					_UseItemList = new NSObject (Dlfcn.GetIntPtr (securityLibrary, "kSecUseItemList"));
				return _UseItemList;
			}
		}
	}

	public class SecClass : NSObject {
		internal SecClass (IntPtr handle) : base (handle)
		{
		}
		
		static SecClass _SecClassKey;
		public static SecClass SecClassKey {
			get {
				if (_SecClassKey == null)
					_SecClassKey = new SecClass (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecClass"));
				return _SecClassKey;
			}
		}
		
		static SecClass _GenericPassword;
		public static SecClass GenericPassword {
			get {
				if (_GenericPassword == null)
					_GenericPassword = new SecClass (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecClassGenericPassword"));
				return _GenericPassword;
			}
		}

		static SecClass _InternetPassword;
		public static SecClass InternetPassword {
			get {
				if (_InternetPassword == null)
					_InternetPassword = new SecClass (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecClassInternetPassword"));
				return _InternetPassword;
			}
		}
		
		static SecClass _Certificate;
		public static SecClass Certificate {
			get {
				if (_Certificate == null)
					_Certificate = new SecClass (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecClassCertificate"));
				return _Certificate;
			}
		}
		
		static SecClass _Key;
		public static SecClass Key {
			get {
				if (_Key == null)
					_Key = new SecClass (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecClassKey"));
				return _Key;
			}
		}
		
		static SecClass _Identity;
		public static SecClass Identity {
			get {
				if (_Identity == null)
					_Identity = new SecClass (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecClassIdentity"));
				return _Identity;
			}
		}
	}

	public class SecAttributeKey : NSObject {
		internal SecAttributeKey (IntPtr handle) : base (handle)
		{
		}

		static SecAttributeKey _AttrAccessible;
		public static SecAttributeKey AttrAccessible {
			get {
				if (_AttrAccessible == null)
					_AttrAccessible = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAccessible"));
				return _AttrAccessible;
			}
		}
		
		static SecAttributeKey _AttrAccessGroup;
		public static SecAttributeKey AttrAccessGroup {
			get {
				if (_AttrAccessGroup == null)
					_AttrAccessGroup = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAccessGroup"));
				return _AttrAccessGroup;
			}
		}
		
		static SecAttributeKey _AttrCreationDate;
		public static SecAttributeKey AttrCreationDate {
			get {
				if (_AttrCreationDate == null)
					_AttrCreationDate = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCreationDate"));
				return _AttrCreationDate;
			}
		}
		
		static SecAttributeKey _AttrModificationDate;
		public static SecAttributeKey AttrModificationDate {
			get {
				if (_AttrModificationDate == null)
					_AttrModificationDate = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrModificationDate"));
				return _AttrModificationDate;
			}
		}
		
		static SecAttributeKey _AttrDescription;
		public static SecAttributeKey AttrDescription {
			get {
				if (_AttrDescription == null)
					_AttrDescription = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrDescription"));
				return _AttrDescription;
			}
		}
		
		static SecAttributeKey _AttrComment;
		public static SecAttributeKey AttrComment {
			get {
				if (_AttrComment == null)
					_AttrComment = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrComment"));
				return _AttrComment;
			}
		}
		
		static SecAttributeKey _AttrCreator;
		public static SecAttributeKey AttrCreator {
			get {
				if (_AttrCreator == null)
					_AttrCreator = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCreator"));
				return _AttrCreator;
			}
		}
		
		static SecAttributeKey _AttrType;
		public static SecAttributeKey AttrType {
			get {
				if (_AttrType == null)
					_AttrType = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrType"));
				return _AttrType;
			}
		}
		
		static SecAttributeKey _AttrLabel;
		public static SecAttributeKey AttrLabel {
			get {
				if (_AttrLabel == null)
					_AttrLabel = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrLabel"));
				return _AttrLabel;
			}
		}
		
		static SecAttributeKey _AttrIsInvisible;
		public static SecAttributeKey AttrIsInvisible {
			get {
				if (_AttrIsInvisible == null)
					_AttrIsInvisible = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrIsInvisible"));
				return _AttrIsInvisible;
			}
		}
		
		static SecAttributeKey _AttrIsNegative;
		public static SecAttributeKey AttrIsNegative {
			get {
				if (_AttrIsNegative == null)
					_AttrIsNegative = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrIsNegative"));
				return _AttrIsNegative;
			}
		}
		
		static SecAttributeKey _AttrAccount;
		public static SecAttributeKey AttrAccount {
			get {
				if (_AttrAccount == null)
					_AttrAccount = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAccount"));
				return _AttrAccount;
			}
		}
		
		static SecAttributeKey _AttrService;
		public static SecAttributeKey AttrService {
			get {
				if (_AttrService == null)
					_AttrService = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrService"));
				return _AttrService;
			}
		}
		
		static SecAttributeKey _AttrGeneric;
		public static SecAttributeKey AttrGeneric {
			get {
				if (_AttrGeneric == null)
					_AttrGeneric = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrGeneric"));
				return _AttrGeneric;
			}
		}
		
		static SecAttributeKey _AttrSecurityDomain;
		public static SecAttributeKey AttrSecurityDomain {
			get {
				if (_AttrSecurityDomain == null)
					_AttrSecurityDomain = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrSecurityDomain"));
				return _AttrSecurityDomain;
			}
		}
		
		static SecAttributeKey _AttrServer;
		public static SecAttributeKey AttrServer {
			get {
				if (_AttrServer == null)
					_AttrServer = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrServer"));
				return _AttrServer;
			}
		}
		
		static SecAttributeKey _AttrProtocol;
		public static SecAttributeKey AttrProtocol {
			get {
				if (_AttrProtocol == null)
					_AttrProtocol = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocol"));
				return _AttrProtocol;
			}
		}
		
		static SecAttributeKey _AttrAuthenticationType;
		public static SecAttributeKey AttrAuthenticationType {
			get {
				if (_AttrAuthenticationType == null)
					_AttrAuthenticationType = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAuthenticationType"));
				return _AttrAuthenticationType;
			}
		}
		
		static SecAttributeKey _AttrPort;
		public static SecAttributeKey AttrPort {
			get {
				if (_AttrPort == null)
					_AttrPort = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrPort"));
				return _AttrPort;
			}
		}
		
		static SecAttributeKey _AttrPath;
		public static SecAttributeKey AttrPath {
			get {
				if (_AttrPath == null)
					_AttrPath = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrPath"));
				return _AttrPath;
			}
		}
		
		static SecAttributeKey _AttrSubject;
		public static SecAttributeKey AttrSubject {
			get {
				if (_AttrSubject == null)
					_AttrSubject = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrSubject"));
				return _AttrSubject;
			}
		}
		
		static SecAttributeKey _AttrIssuer;
		public static SecAttributeKey AttrIssuer {
			get {
				if (_AttrIssuer == null)
					_AttrIssuer = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrIssuer"));
				return _AttrIssuer;
			}
		}
		
		static SecAttributeKey _AttrSerialNumber;
		public static SecAttributeKey AttrSerialNumber {
			get {
				if (_AttrSerialNumber == null)
					_AttrSerialNumber = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrSerialNumber"));
				return _AttrSerialNumber;
			}
		}
		
		static SecAttributeKey _AttrSubjectKeyID;
		public static SecAttributeKey AttrSubjectKeyID {
			get {
				if (_AttrSubjectKeyID == null)
					_AttrSubjectKeyID = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrSubjectKeyID"));
				return _AttrSubjectKeyID;
			}
		}
		
		static SecAttributeKey _AttrPublicKeyHash;
		public static SecAttributeKey AttrPublicKeyHash {
			get {
				if (_AttrPublicKeyHash == null)
					_AttrPublicKeyHash = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrPublicKeyHash"));
				return _AttrPublicKeyHash;
			}
		}
		
		static SecAttributeKey _AttrCertificateType;
		public static SecAttributeKey AttrCertificateType {
			get {
				if (_AttrCertificateType == null)
					_AttrCertificateType = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCertificateType"));
				return _AttrCertificateType;
			}
		}
		
		static SecAttributeKey _AttrCertificateEncoding;
		public static SecAttributeKey AttrCertificateEncoding {
			get {
				if (_AttrCertificateEncoding == null)
					_AttrCertificateEncoding = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCertificateEncoding"));
				return _AttrCertificateEncoding;
			}
		}
		
		static SecAttributeKey _AttrKeyClass;
		public static SecAttributeKey AttrKeyClass {
			get {
				if (_AttrKeyClass == null)
					_AttrKeyClass = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrKeyClass"));
				return _AttrKeyClass;
			}
		}
		
		static SecAttributeKey _AttrApplicationLabel;
		public static SecAttributeKey AttrApplicationLabel {
			get {
				if (_AttrApplicationLabel == null)
					_AttrApplicationLabel = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrApplicationLabel"));
				return _AttrApplicationLabel;
			}
		}
		
		static SecAttributeKey _AttrIsPermanent;
		public static SecAttributeKey AttrIsPermanent {
			get {
				if (_AttrIsPermanent == null)
					_AttrIsPermanent = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrIsPermanent"));
				return _AttrIsPermanent;
			}
		}
		
		static SecAttributeKey _AttrApplicationTag;
		public static SecAttributeKey AttrApplicationTag {
			get {
				if (_AttrApplicationTag == null)
					_AttrApplicationTag = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrApplicationTag"));
				return _AttrApplicationTag;
			}
		}
		
		static SecAttributeKey _AttrKeyType;
		public static SecAttributeKey AttrKeyType {
			get {
				if (_AttrKeyType == null)
					_AttrKeyType = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrKeyType"));
				return _AttrKeyType;
			}
		}
		
		static SecAttributeKey _AttrKeySizeInBits;
		public static SecAttributeKey AttrKeySizeInBits {
			get {
				if (_AttrKeySizeInBits == null)
					_AttrKeySizeInBits = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrKeySizeInBits"));
				return _AttrKeySizeInBits;
			}
		}
		
		static SecAttributeKey _AttrEffectiveKeySize;
		public static SecAttributeKey AttrEffectiveKeySize {
			get {
				if (_AttrEffectiveKeySize == null)
					_AttrEffectiveKeySize = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrEffectiveKeySize"));
				return _AttrEffectiveKeySize;
			}
		}
		
		static SecAttributeKey _AttrCanEncrypt;
		public static SecAttributeKey AttrCanEncrypt {
			get {
				if (_AttrCanEncrypt == null)
					_AttrCanEncrypt = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCanEncrypt"));
				return _AttrCanEncrypt;
			}
		}
		
		static SecAttributeKey _AttrCanDecrypt;
		public static SecAttributeKey AttrCanDecrypt {
			get {
				if (_AttrCanDecrypt == null)
					_AttrCanDecrypt = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCanDecrypt"));
				return _AttrCanDecrypt;
			}
		}
		
		static SecAttributeKey _AttrCanDerive;
		public static SecAttributeKey AttrCanDerive {
			get {
				if (_AttrCanDerive == null)
					_AttrCanDerive = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCanDerive"));
				return _AttrCanDerive;
			}
		}
		
		static SecAttributeKey _AttrCanSign;
		public static SecAttributeKey AttrCanSign {
			get {
				if (_AttrCanSign == null)
					_AttrCanSign = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCanSign"));
				return _AttrCanSign;
			}
		}
		
		static SecAttributeKey _AttrCanVerify;
		public static SecAttributeKey AttrCanVerify {
			get {
				if (_AttrCanVerify == null)
					_AttrCanVerify = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCanVerify"));
				return _AttrCanVerify;
			}
		}
		
		static SecAttributeKey _AttrCanWrap;
		public static SecAttributeKey AttrCanWrap {
			get {
				if (_AttrCanWrap == null)
					_AttrCanWrap = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCanWrap"));
				return _AttrCanWrap;
			}
		}
		
		static SecAttributeKey _AttrCanUnwrap;
		public static SecAttributeKey AttrCanUnwrap {
			get {
				if (_AttrCanUnwrap == null)
					_AttrCanUnwrap = new SecAttributeKey (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCanUnwrap"));
				return _AttrCanUnwrap;
			}
		}
	}

	public class SecAttributeAccesssible : NSObject {
		internal SecAttributeAccesssible (IntPtr handle) : base (handle)
		{
		}
		
		static SecAttributeAccesssible _AttrAccessibleWhenUnlocked;
		public static SecAttributeAccesssible AttrAccessibleWhenUnlocked {
			get {
				if (_AttrAccessibleWhenUnlocked == null)
					_AttrAccessibleWhenUnlocked = new SecAttributeAccesssible (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAccessibleWhenUnlocked"));
				return _AttrAccessibleWhenUnlocked;
			}
		}
		
		static SecAttributeAccesssible _AttrAccessibleAfterFirstUnlock;
		public static SecAttributeAccesssible AttrAccessibleAfterFirstUnlock {
			get {
				if (_AttrAccessibleAfterFirstUnlock == null)
					_AttrAccessibleAfterFirstUnlock = new SecAttributeAccesssible (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAccessibleAfterFirstUnlock"));
				return _AttrAccessibleAfterFirstUnlock;
			}
		}
		
		static SecAttributeAccesssible _AttrAccessibleAlways;
		public static SecAttributeAccesssible AttrAccessibleAlways {
			get {
				if (_AttrAccessibleAlways == null)
					_AttrAccessibleAlways = new SecAttributeAccesssible (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAccessibleAlways"));
				return _AttrAccessibleAlways;
			}
		}
		
		static SecAttributeAccesssible _AttrAccessibleWhenUnlockedThisDeviceOnly;
		public static SecAttributeAccesssible AttrAccessibleWhenUnlockedThisDeviceOnly {
			get {
				if (_AttrAccessibleWhenUnlockedThisDeviceOnly == null)
					_AttrAccessibleWhenUnlockedThisDeviceOnly = new SecAttributeAccesssible (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAccessibleWhenUnlockedThisDeviceOnly"));
				return _AttrAccessibleWhenUnlockedThisDeviceOnly;
			}
		}
		
		static SecAttributeAccesssible _AttrAccessibleAfterFirstUnlockThisDeviceOnly;
		public static SecAttributeAccesssible AttrAccessibleAfterFirstUnlockThisDeviceOnly {
			get {
				if (_AttrAccessibleAfterFirstUnlockThisDeviceOnly == null)
					_AttrAccessibleAfterFirstUnlockThisDeviceOnly = new SecAttributeAccesssible (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAccessibleAfterFirstUnlockThisDeviceOnly"));
				return _AttrAccessibleAfterFirstUnlockThisDeviceOnly;
			}
		}
		
		static SecAttributeAccesssible _AttrAccessibleAlwaysThisDeviceOnly;
		public static SecAttributeAccesssible AttrAccessibleAlwaysThisDeviceOnly {
			get {
				if (_AttrAccessibleAlwaysThisDeviceOnly == null)
					_AttrAccessibleAlwaysThisDeviceOnly = new SecAttributeAccesssible (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAccessibleAlwaysThisDeviceOnly"));
				return _AttrAccessibleAlwaysThisDeviceOnly;
			}
		}

		public static bool operator == (SecAttributeAccesssible a, SecAttributeAccesssible b)
		{
			if (a == null)
				return b == null;
			else if (b == null)
				return false;
			
			return a.Handle == b.Handle;
		}

		public static bool operator != (SecAttributeAccesssible a, SecAttributeAccesssible b)
		{
			if (a == null)
				return b != null;
			else if (b == null)
				return true;
			return a.Handle != b.Handle;
		}

		public override bool Equals (object other)
		{
			var o = other as SecAttributeAccesssible;
			return this == o;
		}

		public override int GetHashCode ()
		{
			return (int) Handle;
		}
	}

	public class SecProtocol : NSNumber {
		internal SecProtocol (IntPtr handle): base (handle) {}
		
		static SecProtocol _AttrProtocolFTP;
		public static SecProtocol AttrProtocolFTP {
			get {
				if (_AttrProtocolFTP == null)
					_AttrProtocolFTP = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolFTP"));
				return _AttrProtocolFTP;
			}
		}
		
		static SecProtocol _AttrProtocolFTPAccount;
		public static SecProtocol AttrProtocolFTPAccount {
			get {
				if (_AttrProtocolFTPAccount == null)
					_AttrProtocolFTPAccount = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolFTPAccount"));
				return _AttrProtocolFTPAccount;
			}
		}
		
		static SecProtocol _AttrProtocolHTTP;
		public static SecProtocol AttrProtocolHTTP {
			get {
				if (_AttrProtocolHTTP == null)
					_AttrProtocolHTTP = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolHTTP"));
				return _AttrProtocolHTTP;
			}
		}
		
		static SecProtocol _AttrProtocolIRC;
		public static SecProtocol AttrProtocolIRC {
			get {
				if (_AttrProtocolIRC == null)
					_AttrProtocolIRC = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolIRC"));
				return _AttrProtocolIRC;
			}
		}
		
		static SecProtocol _AttrProtocolNNTP;
		public static SecProtocol AttrProtocolNNTP {
			get {
				if (_AttrProtocolNNTP == null)
					_AttrProtocolNNTP = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolNNTP"));
				return _AttrProtocolNNTP;
			}
		}
		
		static SecProtocol _AttrProtocolPOP3;
		public static SecProtocol AttrProtocolPOP3 {
			get {
				if (_AttrProtocolPOP3 == null)
					_AttrProtocolPOP3 = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolPOP3"));
				return _AttrProtocolPOP3;
			}
		}
		
		static SecProtocol _AttrProtocolSMTP;
		public static SecProtocol AttrProtocolSMTP {
			get {
				if (_AttrProtocolSMTP == null)
					_AttrProtocolSMTP = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolSMTP"));
				return _AttrProtocolSMTP;
			}
		}
		
		static SecProtocol _AttrProtocolSOCKS;
		public static SecProtocol AttrProtocolSOCKS {
			get {
				if (_AttrProtocolSOCKS == null)
					_AttrProtocolSOCKS = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolSOCKS"));
				return _AttrProtocolSOCKS;
			}
		}
		
		static SecProtocol _AttrProtocolIMAP;
		public static SecProtocol AttrProtocolIMAP {
			get {
				if (_AttrProtocolIMAP == null)
					_AttrProtocolIMAP = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolIMAP"));
				return _AttrProtocolIMAP;
			}
		}
		
		static SecProtocol _AttrProtocolLDAP;
		public static SecProtocol AttrProtocolLDAP {
			get {
				if (_AttrProtocolLDAP == null)
					_AttrProtocolLDAP = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolLDAP"));
				return _AttrProtocolLDAP;
			}
		}
		
		static SecProtocol _AttrProtocolAppleTalk;
		public static SecProtocol AttrProtocolAppleTalk {
			get {
				if (_AttrProtocolAppleTalk == null)
					_AttrProtocolAppleTalk = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolAppleTalk"));
				return _AttrProtocolAppleTalk;
			}
		}
		
		static SecProtocol _AttrProtocolAFP;
		public static SecProtocol AttrProtocolAFP {
			get {
				if (_AttrProtocolAFP == null)
					_AttrProtocolAFP = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolAFP"));
				return _AttrProtocolAFP;
			}
		}
		
		static SecProtocol _AttrProtocolTelnet;
		public static SecProtocol AttrProtocolTelnet {
			get {
				if (_AttrProtocolTelnet == null)
					_AttrProtocolTelnet = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolTelnet"));
				return _AttrProtocolTelnet;
			}
		}
		
		static SecProtocol _AttrProtocolSSH;
		public static SecProtocol AttrProtocolSSH {
			get {
				if (_AttrProtocolSSH == null)
					_AttrProtocolSSH = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolSSH"));
				return _AttrProtocolSSH;
			}
		}
		
		static SecProtocol _AttrProtocolFTPS;
		public static SecProtocol AttrProtocolFTPS {
			get {
				if (_AttrProtocolFTPS == null)
					_AttrProtocolFTPS = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolFTPS"));
				return _AttrProtocolFTPS;
			}
		}
		
		static SecProtocol _AttrProtocolHTTPS;
		public static SecProtocol AttrProtocolHTTPS {
			get {
				if (_AttrProtocolHTTPS == null)
					_AttrProtocolHTTPS = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolHTTPS"));
				return _AttrProtocolHTTPS;
			}
		}
		
		static SecProtocol _AttrProtocolHTTPProxy;
		public static SecProtocol AttrProtocolHTTPProxy {
			get {
				if (_AttrProtocolHTTPProxy == null)
					_AttrProtocolHTTPProxy = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolHTTPProxy"));
				return _AttrProtocolHTTPProxy;
			}
		}
		
		static SecProtocol _AttrProtocolHTTPSProxy;
		public static SecProtocol AttrProtocolHTTPSProxy {
			get {
				if (_AttrProtocolHTTPSProxy == null)
					_AttrProtocolHTTPSProxy = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolHTTPSProxy"));
				return _AttrProtocolHTTPSProxy;
			}
		}
		
		static SecProtocol _AttrProtocolFTPProxy;
		public static SecProtocol AttrProtocolFTPProxy {
			get {
				if (_AttrProtocolFTPProxy == null)
					_AttrProtocolFTPProxy = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolFTPProxy"));
				return _AttrProtocolFTPProxy;
			}
		}
		
		static SecProtocol _AttrProtocolSMB;
		public static SecProtocol AttrProtocolSMB {
			get {
				if (_AttrProtocolSMB == null)
					_AttrProtocolSMB = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolSMB"));
				return _AttrProtocolSMB;
			}
		}
		
		static SecProtocol _AttrProtocolRTSP;
		public static SecProtocol AttrProtocolRTSP {
			get {
				if (_AttrProtocolRTSP == null)
					_AttrProtocolRTSP = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolRTSP"));
				return _AttrProtocolRTSP;
			}
		}
		
		static SecProtocol _AttrProtocolRTSPProxy;
		public static SecProtocol AttrProtocolRTSPProxy {
			get {
				if (_AttrProtocolRTSPProxy == null)
					_AttrProtocolRTSPProxy = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolRTSPProxy"));
				return _AttrProtocolRTSPProxy;
			}
		}
		
		static SecProtocol _AttrProtocolDAAP;
		public static SecProtocol AttrProtocolDAAP {
			get {
				if (_AttrProtocolDAAP == null)
					_AttrProtocolDAAP = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolDAAP"));
				return _AttrProtocolDAAP;
			}
		}
		
		static SecProtocol _AttrProtocolEPPC;
		public static SecProtocol AttrProtocolEPPC {
			get {
				if (_AttrProtocolEPPC == null)
					_AttrProtocolEPPC = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolEPPC"));
				return _AttrProtocolEPPC;
			}
		}
		
		static SecProtocol _AttrProtocolIPP;
		public static SecProtocol AttrProtocolIPP {
			get {
				if (_AttrProtocolIPP == null)
					_AttrProtocolIPP = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolIPP"));
				return _AttrProtocolIPP;
			}
		}
		
		static SecProtocol _AttrProtocolNNTPS;
		public static SecProtocol AttrProtocolNNTPS {
			get {
				if (_AttrProtocolNNTPS == null)
					_AttrProtocolNNTPS = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolNNTPS"));
				return _AttrProtocolNNTPS;
			}
		}
		
		static SecProtocol _AttrProtocolLDAPS;
		public static SecProtocol AttrProtocolLDAPS {
			get {
				if (_AttrProtocolLDAPS == null)
					_AttrProtocolLDAPS = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolLDAPS"));
				return _AttrProtocolLDAPS;
			}
		}
		
		static SecProtocol _AttrProtocolTelnetS;
		public static SecProtocol AttrProtocolTelnetS {
			get {
				if (_AttrProtocolTelnetS == null)
					_AttrProtocolTelnetS = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolTelnetS"));
				return _AttrProtocolTelnetS;
			}
		}
		
		static SecProtocol _AttrProtocolIMAPS;
		public static SecProtocol AttrProtocolIMAPS {
			get {
				if (_AttrProtocolIMAPS == null)
					_AttrProtocolIMAPS = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolIMAPS"));
				return _AttrProtocolIMAPS;
			}
		}
		
		static SecProtocol _AttrProtocolIRCS;
		public static SecProtocol AttrProtocolIRCS {
			get {
				if (_AttrProtocolIRCS == null)
					_AttrProtocolIRCS = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolIRCS"));
				return _AttrProtocolIRCS;
			}
		}
		
		static SecProtocol _AttrProtocolPOP3S;
		public static SecProtocol AttrProtocolPOP3S {
			get {
				if (_AttrProtocolPOP3S == null)
					_AttrProtocolPOP3S = new SecProtocol (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolPOP3S"));
				return _AttrProtocolPOP3S;
			}
		}

		public static bool operator == (SecProtocol a, SecProtocol b)
		{
			if (a == null)
				return b == null;
			else if (b == null)
				return false;
			
			return a.Handle == b.Handle;
		}

		public static bool operator != (SecProtocol a, SecProtocol b)
		{
			if (a == null)
				return b != null;
			else if (b == null)
				return true;
			return a.Handle != b.Handle;
		}

		public override bool Equals (object other)
		{
			var o = other as SecProtocol;
			return this == o;
		}

		public override int GetHashCode ()
		{
			return (int) Handle;
		}
	}

	public class SecAuthenticationType : NSNumber {
		internal SecAuthenticationType (IntPtr handle) : base (handle) {}
		
		static SecAuthenticationType _AttrAuthenticationTypeNTLM;
		public static SecAuthenticationType AttrAuthenticationTypeNTLM {
			get {
				if (_AttrAuthenticationTypeNTLM == null)
					_AttrAuthenticationTypeNTLM = new SecAuthenticationType (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAuthenticationTypeNTLM"));
				return _AttrAuthenticationTypeNTLM;
			}
		}
		
		static SecAuthenticationType _AttrAuthenticationTypeMSN;
		public static SecAuthenticationType AttrAuthenticationTypeMSN {
			get {
				if (_AttrAuthenticationTypeMSN == null)
					_AttrAuthenticationTypeMSN = new SecAuthenticationType (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAuthenticationTypeMSN"));
				return _AttrAuthenticationTypeMSN;
			}
		}
		
		static SecAuthenticationType _AttrAuthenticationTypeDPA;
		public static SecAuthenticationType AttrAuthenticationTypeDPA {
			get {
				if (_AttrAuthenticationTypeDPA == null)
					_AttrAuthenticationTypeDPA = new SecAuthenticationType (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAuthenticationTypeDPA"));
				return _AttrAuthenticationTypeDPA;
			}
		}
		
		static SecAuthenticationType _AttrAuthenticationTypeRPA;
		public static SecAuthenticationType AttrAuthenticationTypeRPA {
			get {
				if (_AttrAuthenticationTypeRPA == null)
					_AttrAuthenticationTypeRPA = new SecAuthenticationType (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAuthenticationTypeRPA"));
				return _AttrAuthenticationTypeRPA;
			}
		}
		
		static SecAuthenticationType _AttrAuthenticationTypeHTTPBasic;
		public static SecAuthenticationType AttrAuthenticationTypeHTTPBasic {
			get {
				if (_AttrAuthenticationTypeHTTPBasic == null)
					_AttrAuthenticationTypeHTTPBasic = new SecAuthenticationType (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAuthenticationTypeHTTPBasic"));
				return _AttrAuthenticationTypeHTTPBasic;
			}
		}
		
		static SecAuthenticationType _AttrAuthenticationTypeHTTPDigest;
		public static SecAuthenticationType AttrAuthenticationTypeHTTPDigest {
			get {
				if (_AttrAuthenticationTypeHTTPDigest == null)
					_AttrAuthenticationTypeHTTPDigest = new SecAuthenticationType (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAuthenticationTypeHTTPDigest"));
				return _AttrAuthenticationTypeHTTPDigest;
			}
		}
		
		static SecAuthenticationType _AttrAuthenticationTypeHTMLForm;
		public static SecAuthenticationType AttrAuthenticationTypeHTMLForm {
			get {
				if (_AttrAuthenticationTypeHTMLForm == null)
					_AttrAuthenticationTypeHTMLForm = new SecAuthenticationType (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAuthenticationTypeHTMLForm"));
				return _AttrAuthenticationTypeHTMLForm;
			}
		}
		
		static SecAuthenticationType _AttrAuthenticationTypeDefault;
		public static SecAuthenticationType AttrAuthenticationTypeDefault {
			get {
				if (_AttrAuthenticationTypeDefault == null)
					_AttrAuthenticationTypeDefault = new SecAuthenticationType (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAuthenticationTypeDefault"));
				return _AttrAuthenticationTypeDefault;
			}
		}

		public static bool operator == (SecAuthenticationType a, SecAuthenticationType b)
		{
			if (a == null)
				return b == null;
			else if (b == null)
				return false;
			
			return a.Handle == b.Handle;
		}

		public static bool operator != (SecAuthenticationType a, SecAuthenticationType b)
		{
			if (a == null)
				return b != null;
			else if (b == null)
				return true;
			return a.Handle != b.Handle;
		}

		public override bool Equals (object other)
		{
			var o = other as SecAuthenticationType;
			return this == o;
		}

		public override int GetHashCode ()
		{
			return (int) Handle;
		}
	}

	public class SecKeyClass : NSNumber {
		internal SecKeyClass (IntPtr handle):base(handle) {}
		
		static SecKeyClass _AttrKeyClassPublic;
		public static SecKeyClass AttrKeyClassPublic {
			get {
				if (_AttrKeyClassPublic == null)
					_AttrKeyClassPublic = new SecKeyClass (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrKeyClassPublic"));
				return _AttrKeyClassPublic;
			}
		}
		
		static SecKeyClass _AttrKeyClassPrivate;
		public static SecKeyClass AttrKeyClassPrivate {
			get {
				if (_AttrKeyClassPrivate == null)
					_AttrKeyClassPrivate = new SecKeyClass (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrKeyClassPrivate"));
				return _AttrKeyClassPrivate;
			}
		}
		
		static SecKeyClass _AttrKeyClassSymmetric;
		public static SecKeyClass AttrKeyClassSymmetric {
			get {
				if (_AttrKeyClassSymmetric == null)
					_AttrKeyClassSymmetric = new SecKeyClass (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrKeyClassSymmetric"));
				return _AttrKeyClassSymmetric;
			}
		}

		public static bool operator == (SecKeyClass a, SecKeyClass b)
		{
			if (a == null)
				return b == null;
			else if (b == null)
				return false;
			
			return a.Handle == b.Handle;
		}

		public static bool operator != (SecKeyClass a, SecKeyClass b)
		{
			if (a == null)
				return b != null;
			else if (b == null)
				return true;
			return a.Handle != b.Handle;
		}

		public override bool Equals (object other)
		{
			var o = other as SecKeyClass;
			return this == o;
		}

		public override int GetHashCode ()
		{
			return (int) Handle;
		}
	}

	public class SecKeyType : NSObject {
		internal SecKeyType (IntPtr handle) : base (handle)
		{
		}

		static SecKeyType _AttrKeyTypeRSA;
		public static SecKeyType AttrKeyTypeRSA {
			get {
				if (_AttrKeyTypeRSA == null)
					_AttrKeyTypeRSA = new SecKeyType (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrKeyTypeRSA"));
				return _AttrKeyTypeRSA;
			}
		}
		
		static SecKeyType _AttrKeyTypeEC;
		public static SecKeyType AttrKeyTypeEC {
			get {
				if (_AttrKeyTypeEC == null)
					_AttrKeyTypeEC = new SecKeyType (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrKeyTypeEC"));
				return _AttrKeyTypeEC;
			}
		}

		public static bool operator == (SecKeyType a, SecKeyType b)
		{
			if (a == null)
				return b == null;
			else if (b == null)
				return false;
			
			return a.Handle == b.Handle;
		}

		public static bool operator != (SecKeyType a, SecKeyType b)
		{
			if (a == null)
				return b != null;
			else if (b == null)
				return true;
			return a.Handle != b.Handle;
		}

		public override bool Equals (object other)
		{
			var o = other as SecKeyType;
			return this == o;
		}

		public override int GetHashCode ()
		{
			return (int) Handle;
		}
	}

	public class SecMatchLimit : NSNumber {
		internal SecMatchLimit (IntPtr handle) : base (handle) {}

		public SecMatchLimit (int limit) : base (limit)
		{
		}
		
		static SecMatchLimit _MatchLimitOne;
		public static SecMatchLimit MatchLimitOne {
			get {
				if (_MatchLimitOne == null)
					_MatchLimitOne = new SecMatchLimit (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecMatchLimitOne"));
				return _MatchLimitOne;
			}
		}
		
		static SecMatchLimit _MatchLimitAll;
		public static SecMatchLimit MatchLimitAll {
			get {
				if (_MatchLimitAll == null)
					_MatchLimitAll = new SecMatchLimit (Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecMatchLimitAll"));
				return _MatchLimitAll;
			}
		}

		public static bool operator == (SecMatchLimit a, SecMatchLimit b)
		{
			if (a == null)
				return b == null;
			else if (b == null)
				return false;
			
			return a.Handle == b.Handle;
		}

		public static bool operator != (SecMatchLimit a, SecMatchLimit b)
		{
			if (a == null)
				return b != null;
			else if (b == null)
				return true;
			return a.Handle != b.Handle;
		}
		
		public override bool Equals (object other)
		{
			var o = other as SecMatchLimit;
			return this == o;
		}

		public override int GetHashCode ()
		{
			return (int) Handle;
		}
	}
}