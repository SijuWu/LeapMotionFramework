using UnityEngine;
using System;
using Leap;

namespace AssemblyCSharp
{
	public class LeapFinger
	{
		//Finger sphere displayed 
		GameObject fingerObject;
		
		//Leap finger data
		Leap.Finger finger;
		
		//Last tip position
		Vector3 lastPosition;
		
		//Tip position
		Vector3 position;
		
		//Last tip speed
		Vector3 lastSpeed;
		
		//Tip speed
		Vector3 speed;
		
		//Metacarpal
		LeapBone leapMetacarpal;
		
		//Proximal
		LeapBone leapProximal;
		
		//Intermediate
		LeapBone leapIntermediate;
		
		//Distal
		LeapBone leapDistal;
		
		//Finger scale
		const float scale = 5.0f;
		
		//Return finger object
		public GameObject getFingerObject ()
		{
			return fingerObject;
		}
		
		//Get fingerdata
		public Leap.Finger getFinger ()
		{
			return finger;
		}
		
		//Get metacarpal
		public LeapBone getMetacarpal ()
		{
			return leapMetacarpal;
		}
		
		//Get proximal
		public LeapBone getProximal ()
		{
			return leapProximal;
		}
		
		//Get intermediate
		public LeapBone getIntermediate ()
		{
			return leapIntermediate;
		}
		
		//Get distal
		public LeapBone getDistal ()
		{
			return leapDistal;
		}
		
		//Get last position
		public Vector3 getLastPosition ()
		{
			return lastPosition;
		}
		
		//Get position
		public Vector3 getPosition ()
		{
			return position;
		}
		
		//Get last speed
		public Vector3 getLastSpeed ()
		{
			return lastSpeed;
		}
		
		//Get speed
		public Vector3 getSpeed ()
		{
			return speed;
		}
		
		public LeapFinger (Leap.Finger finger)
		{	
			//Create finger object
			fingerObject = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			
			//Set finger object name
			switch (finger.Type ()) {
			case Finger.FingerType.TYPE_THUMB:
				if (finger.Hand.IsLeft)
					fingerObject.name = "LeftThumb";
				if (finger.Hand.IsRight)
					fingerObject.name = "RightThumb";
				break;
				
			case Finger.FingerType.TYPE_INDEX:
				if (finger.Hand.IsLeft)
					fingerObject.name = "LeftIndex";
				if (finger.Hand.IsRight)
					fingerObject.name = "RightIndex";
				break;
				
			case Finger.FingerType.TYPE_MIDDLE:
				if (finger.Hand.IsLeft)
					fingerObject.name = "LeftMiddle";
				if (finger.Hand.IsRight)
					fingerObject.name = "RightMiddle";
				break;
				
			case Finger.FingerType.TYPE_RING:
				if (finger.Hand.IsLeft)
					fingerObject.name = "LeftRing";
				if (finger.Hand.IsRight)
					fingerObject.name = "RightRing";
				break;
				
			case Finger.FingerType.TYPE_PINKY:
				if (finger.Hand.IsLeft)
					fingerObject.name = "LeftPinky";
				if (finger.Hand.IsRight)
					fingerObject.name = "RightPinky";
				break;
			}
				
			//Save leap finger data
			this.finger = finger;
			
			//Set fingerObject position
			//Reverse z coordinate
			fingerObject.transform.position = new Vector3 (finger.TipPosition.x, finger.TipPosition.y, -finger.TipPosition.z);
	
			//Set position
			position = fingerObject.transform.position;
			
			//Set speed
			speed = new Vector3 (finger.TipVelocity.x, finger.TipVelocity.y, -finger.TipVelocity.z);
			
			//Set finger color
			if (finger.IsExtended)
				fingerObject.transform.renderer.material.color = Color.cyan;
			else
				fingerObject.transform.renderer.material.color = Color.yellow;
			
			//Set finger size
			fingerObject.transform.localScale = new Vector3 (scale, scale, scale);
			
			//Initialize bones
			InitializeBones ();
		}
		
		//Initialize bones
		public void InitializeBones ()
		{
			leapMetacarpal = new LeapBone (finger.Bone (Bone.BoneType.TYPE_METACARPAL));
			leapProximal = new LeapBone (finger.Bone (Bone.BoneType.TYPE_PROXIMAL));
			leapIntermediate = new LeapBone (finger.Bone (Bone.BoneType.TYPE_INTERMEDIATE));
			leapDistal = new LeapBone (finger.Bone (Bone.BoneType.TYPE_DISTAL));
		}
		
		//Update the finger
		public void UpdateFinger (Leap.Finger finger)
		{
			if (!finger.IsValid)
				return;
			
			//Refresh leap finger data
			this.finger = finger;
			
			//Set fingerObject position
			//Reverse z coordinate
			fingerObject.transform.position = new Vector3 (finger.TipPosition.x, finger.TipPosition.y, -finger.TipPosition.z);
			
			//Set last position
			lastPosition = position;
			
			//Set last speed
			lastSpeed = speed;
			
			//Set position
			position = fingerObject.transform.position;
			
			//Set speed 
			speed = new Vector3 (finger.TipVelocity.x, finger.TipVelocity.y, -finger.TipVelocity.z);
			
			//Set finger color
			if (finger.IsExtended)
				fingerObject.transform.renderer.material.color = Color.cyan;
			else
				fingerObject.transform.renderer.material.color = Color.yellow;
			
			//Update bones
			UpdateBones ();
		}
		
		//Update bones
		public void UpdateBones ()
		{
			leapMetacarpal.UpdateBone (finger.Bone (Bone.BoneType.TYPE_METACARPAL));
			leapProximal.UpdateBone (finger.Bone (Bone.BoneType.TYPE_PROXIMAL));
			leapIntermediate.UpdateBone (finger.Bone (Bone.BoneType.TYPE_INTERMEDIATE));
			leapDistal.UpdateBone (finger.Bone (Bone.BoneType.TYPE_DISTAL));
		}
	}
}

