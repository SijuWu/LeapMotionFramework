using UnityEngine;
using System;
using System.Collections.Generic;
using Leap;

namespace AssemblyCSharp
{
	public class LeapHand
	{
		//Hand sphere displayed 
		GameObject handObject;

		//Hand scale
		const float scale = 5.0f;
		
		//Leap hand data
		Leap.Hand hand;
		
		//Last hand position
		Vector3 lastPosition;
		
		//Hand position
		Vector3 position;
		
		//Last hand speed
		Vector3 lastSpeed;
		
		//Hand speed
		Vector3 speed;
		
		//Thumb
		LeapFinger leapThumb;
		
		//Index
		LeapFinger leapIndex;
		
		//Middle
		LeapFinger leapMiddle;
		
		//Ring
		LeapFinger leapRing;
		
		//Pinky
		LeapFinger leapPinky;
		
		//Get thumb
		public LeapFinger getThumb ()
		{
			return leapThumb;
		}
		
		//Get Index
		public LeapFinger getIndex ()
		{
			return leapIndex;
		}
		
		//Get middle
		public LeapFinger getMiddle ()
		{
			return leapMiddle;
		}
		
		//Get ring
		public LeapFinger getRing ()
		{
			return leapRing;
		}
		
		//Get pinky
		public LeapFinger getPinky ()
		{
			return leapPinky;
		}
		
		//Return hand object
		public GameObject getHandObject ()
		{
			return handObject;
		}
		
		//Get hand data
		public Leap.Hand getHand ()
		{
			return hand;
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
		
		public LeapHand (Leap.Hand hand)
		{	
			//Create hand object
			handObject = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			
			//Set hand object name
			if (hand.IsLeft)
				handObject.name = "LeftHand";
			if (hand.IsRight)
				handObject.name = "RightHand";
			
			//Save leap hand data
			this.hand = hand;
			
			//Set handObject position
			//Reverse z coordinate
			handObject.transform.position = new Vector3 (hand.PalmPosition.x, hand.PalmPosition.y, -hand.PalmPosition.z);
			
			//Set hand position
			position = handObject.transform.position;
			
			//Set hand speed
			speed = new Vector3 (hand.PalmVelocity.x, hand.PalmVelocity.y, -hand.PalmVelocity.z);
			
			//Set palm color
			handObject.transform.renderer.material.color = Color.yellow;
			
			//Set palm size
			handObject.transform.localScale = new Vector3 (scale, scale, scale);

			//Initialize leap fingers
			InitializeFingers ();
		}
		
		//Initialize fingers
		public void InitializeFingers ()
		{
			foreach (Leap.Finger finger in hand.Fingers) {
				
				switch (finger.Type ()) {
				case Leap.Finger.FingerType.TYPE_THUMB:
					leapThumb = new LeapFinger (finger);
					break;
					
				case Leap.Finger.FingerType.TYPE_INDEX:
					leapIndex = new LeapFinger (finger);
					break;
					
				case Leap.Finger.FingerType.TYPE_MIDDLE:
					leapMiddle = new LeapFinger (finger);
					break;
					
				case Leap.Finger.FingerType.TYPE_RING:
					leapRing = new LeapFinger (finger);
					break;
					
				case Leap.Finger.FingerType.TYPE_PINKY:
					leapPinky = new LeapFinger (finger);
					break;
				}
			}	
		}
		
		//Update the hand
		public void UpdateHand (Leap.Hand hand)
		{
			if (!hand.IsValid)
				return;
			
			//Refresh leap hand data
			this.hand = hand;
			
			//Set handObject position
			//Reverse z coordinate
			handObject.transform.position = new Vector3 (hand.PalmPosition.x, hand.PalmPosition.y, -hand.PalmPosition.z);
			
			//Set last hand position
			lastPosition = position;
			
			//Set hand position
			position = handObject.transform.position;
			
			//Set last hand speed
			lastSpeed = speed;
			
			//Set hand speed
			speed = new Vector3 (hand.PalmVelocity.x, hand.PalmVelocity.y, -hand.PalmVelocity.z);
			
			//Refresh leap finger data
			UpdateFingers ();
		}
		
		//Update fingers
		public void UpdateFingers ()
		{
			foreach (Leap.Finger finger in hand.Fingers) {
				switch (finger.Type ()) {
				case Leap.Finger.FingerType.TYPE_THUMB:
					leapThumb.UpdateFinger (finger);
					break;
					
				case Leap.Finger.FingerType.TYPE_INDEX:
					leapIndex.UpdateFinger (finger);
					break;
					
				case Leap.Finger.FingerType.TYPE_MIDDLE:
					leapMiddle.UpdateFinger (finger);
					break;
					
				case Leap.Finger.FingerType.TYPE_RING:
					leapRing.UpdateFinger (finger);
					break;
					
				case Leap.Finger.FingerType.TYPE_PINKY:
					leapPinky.UpdateFinger (finger);
					break;
				}
			}
		}
		

	}
}

