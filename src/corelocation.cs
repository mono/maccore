//
// This file describes the API that the generator will produce
//
// Authors:
//   Geoff Norton
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
//
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
using MonoMac.CoreGraphics;
using MonoMac.CoreLocation;
#if !MONOMAC
using MonoMac.UIKit;
#endif
using System;
using System.Drawing;

namespace MonoMac.CoreLocation {

	[BaseType (typeof (NSObject))]
	interface CLHeading {
		[Export ("magneticHeading")]
		double MagneticHeading { get;  }
	
		[Export ("trueHeading")]
		double TrueHeading { get;  }
	
		[Export ("headingAccuracy")]
		double HeadingAccuracy { get;  }
	
		[Export ("x")]
		double X { get;  }
	
		[Export ("y")]
		double Y { get;  }
	
		[Export ("z")]
		double Z { get;  }
	
		[Export ("timestamp")]
		NSDate Timestamp { get;  }
	
		[Export ("description")]
		string Description ();
	
	}
	
	[BaseType (typeof (NSObject))]
	interface CLLocation {
		[Export ("coordinate")]
		CLLocationCoordinate2D Coordinate { get;  }
	
		[Export ("altitude")]
		double Altitude { get;  }
	
		[Export ("horizontalAccuracy")]
		double HorizontalAccuracy { get;  }
	
		[Export ("verticalAccuracy")]
		double VerticalAccuracy { get;  }
	
		[Export ("course")]
		double Course { get;  }
	
		[Export ("speed")]
		double Speed { get;  }
	
		[Export ("timestamp")]
		NSDate Timestamp { get;  }
	
		[Export ("initWithLatitude:longitude:")]
		IntPtr Constructor (double latitude, double longitude);
	
		[Export ("initWithCoordinate:altitude:horizontalAccuracy:verticalAccuracy:timestamp:")]
		IntPtr Constructor (CLLocationCoordinate2D coordinate, double altitude, double hAccuracy, double vAccuracy, NSDate timestamp);
	
		[Export ("description")]
		string Description ();
	
		[Export ("getDistanceFrom:")]
		double Distancefrom (CLLocation  location);

		// NOTE: The old selector was renamed to this guy in 3.2
		[Since (3,2)]
		[Export ("distanceFromLocation:")]
		double DistanceFrom (CLLocation location);
	}
	
	[BaseType (typeof (NSObject), Delegates=new string [] {"WeakDelegate"}, Events=new Type [] {typeof (CLLocationManagerDelegate)})]
	interface CLLocationManager {
		[Wrap ("WeakDelegate")]
		CLLocationManagerDelegate Delegate { get; set;  }

		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set;  }
	
		[Export ("locationServicesEnabled"), Internal]
		bool _LocationServicesEnabledInstance { get;  }
	
		[Export ("distanceFilter", ArgumentSemantic.Assign)]
		double DistanceFilter { get; set;  }
	
		[Export ("desiredAccuracy", ArgumentSemantic.Assign)]
		double DesiredAccuracy { get; set;  }
	
		[Export ("location")]
		CLLocation Location { get;  }
	
		[Export ("startUpdatingLocation")]
		void StartUpdatingLocation ();
	
		[Export ("stopUpdatingLocation")]
		void StopUpdatingLocation ();
	
		[Export ("headingFilter", ArgumentSemantic.Assign)]
		double HeadingFilter { get; set;  }
	
		[Export ("headingAvailable"), Internal]
		bool _HeadingAvailableInstance { get;  }
	
		[Export ("startUpdatingHeading")]
		void StartUpdatingHeading ();
	
		[Export ("stopUpdatingHeading")]
		void StopUpdatingHeading ();
	
		[Export ("dismissHeadingCalibrationDisplay")]
		void DismissHeadingCalibrationDisplay ();
	
		[Since (3,2)]
		[Export ("purpose", ArgumentSemantic.Copy)]
		string Purpose { get; set; }

		[Since (4,0)]
		[Export ("locationServicesEnabled"), Static, Internal]
		bool _LocationServicesEnabledStatic { get; }

		[Since (4,0)]
		[Export ("headingAvailable"), Static, Internal]
		bool _HeadingAvailableStatic { get; }

		[Since (4,0)]
		[Export ("significantLocationChangeMonitoringAvailable"), Static]
		bool SignificantLocationChangeMonitoringAvailable { get; }

		[Since (4,0)]
		[Export ("regionMonitoringAvailable"), Static]
		bool RegionMonitoringAvailable { get; }

		[Since (4,0)]
		[Export ("regionMonitoringEnabled"), Static]
		bool RegionMonitoringEnabled { get; }

		[Since (4,0)]
		[Export ("headingOrientation")]
		CLDeviceOrientation HeadingOrientation { get; set; }

		[Export ("heading")]
		[Since (4,0)]
		CLHeading Heading { get; }

		[Export ("maximumRegionMonitoringDistance")]
		[Since (4,0)]
		double MaximumRegionMonitoringDistance { get; }

		[Export ("monitoredRegions")]
		[Since (4,0)]
		NSSet MonitoredRegions { get; }

		[Since (4,0)]
		[Export ("startMonitoringSignificantLocationChanges")]
		void StartMonitoringSignificantLocationChanges ();

		[Since (4,0)]
		[Export ("stopMonitoringSignificantLocationChanges")]
		void StopMonitoringSignificantLocationChanges ();

		[Since (4,0)]
		[Export ("startMonitoringForRegion:desiredAccuracy:")]
		void StartMonitoring (CLRegion region, double desiredAccuracy);

		[Since (4,0)]
		[Export ("stopMonitoringForRegion:")]
		void StopMonitoring (CLRegion region);
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	interface CLLocationManagerDelegate {
		[Export ("locationManager:didUpdateToLocation:fromLocation:"), EventArgs ("CLLocationUpdated")]
		void UpdatedLocation (CLLocationManager  manager, CLLocation newLocation, CLLocation oldLocation);
	
		[Export ("locationManager:didUpdateHeading:"), EventArgs ("CLHeadingUpdated")]
		void UpdatedHeading (CLLocationManager  manager, CLHeading newHeading);
	
		[Export ("locationManagerShouldDisplayHeadingCalibration:"), EventArgs ("CLLocationManagerEventArgs"), DefaultValue (true)]
		bool ShouldDisplayHeadingCalibration (CLLocationManager manager);
	
		[Export ("locationManager:didFailWithError:"), EventArgs ("NSError")]
		void Failed (CLLocationManager manager, NSError error);

		[Since (4,0)]
		[Export ("locationManager:didEnterRegion:"), EventArgs ("CLRegion")]
		void RegionEntered (CLLocationManager manager, CLRegion region);

		[Since (4,0)]
		[Export ("locationManager:didExitRegion:"), EventArgs ("CLRegion")]
		void RegionLeft (CLLocationManager manager, CLRegion region);

		[Since (4,0)]
		[Export ("locationManager:monitoringDidFailForRegion:withError:"), EventArgs ("CLRegionError")]
		void MonitoringFailed (CLLocationManager manager, CLRegion region, NSError error);
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface CLRegion {
		[Export ("center")]
		CLLocationCoordinate2D Center { get;  }

		[Export ("radius")]
		double Radius { get;  }

		[Export ("identifier")]
		string Identifier { get;  }

		[Export ("initCircularRegionWithCenter:radius:identifier:")]
		IntPtr Constructor (CLLocationCoordinate2D center, double radius, string identifier);

		[Export ("containsCoordinate:")]
		bool Contains (CLLocationCoordinate2D coordinate);
	}
}

