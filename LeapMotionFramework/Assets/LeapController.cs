using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;
using AssemblyCSharp;

public class LeapController : MonoBehaviour
{
	//Leap motion controller
	Controller controller;
	
	//Hand list
	List<LeapHand> handList = new List<LeapHand> ();
	
	//Leap motion frame
	Frame frame;
	
//	//Left index stroke
//	Stroke leftSingleStroke;
//	
//	//Right index stroke
//	Stroke rightSingleStroke;
//	
//	//Left index plus middle stroke
//	Stroke leftDoubleStroke;
//	
//	//Right index plus middle stroke
//	Stroke rightDoubleStroke;
//	
//	//List of left index stroke
//	List<Stroke> leftSingeStrokeList;
//	
//	//List of right index stroke
//	List<Stroke> rightSingleStrokeList;
//	
//	//List of left index middle stroke
//	List<Stroke> leftDoubleStrokeList;
//	
//	//List of right index middle stroke
//	List<Stroke> rightDoubleStrokeList;
//	
//	//Left index stroke object
//	GameObject leftSingleDraw;
//	
//	//Right index stroke object
//	GameObject rightSingleDraw;
//	
//	//Left index plus middle stroke object
//	GameObject leftDoubleDraw;
//	
//	//Right index plus middle stroke object
//	GameObject rightDoubleDraw;
//	
//	//Left index renderer
//	LineRenderer leftSingleRenderer;
//	
//	//Right index renderer
//	LineRenderer rightSingleRenderer;
//	
//	//Left index plus middle renderer
//	LineRenderer leftDoubleRenderer;
//	
//	//Right index plus middle renderer
//	LineRenderer rightDoubleRenderer;
//	
//	
//	//Threshold distance between index and middle
//	const float indexMiddleThreshold = 20;
	
	public List<LeapHand> getHandList ()
	{
		return handList;
	}
	
	// Use this for initialization
	void Start ()
	{
		controller = new Controller ();
		
//		leftSingleStroke = new Stroke ();
//		rightSingleStroke = new Stroke ();
//		leftDoubleStroke = new Stroke ();
//		rightDoubleStroke = new Stroke ();
//		
//		leftSingeStrokeList=new List<Stroke>();
//		rightSingleStrokeList=new List<Stroke>();
//		leftDoubleStrokeList=new List<Stroke>();
//		rightDoubleStrokeList=new List<Stroke>();
//		
//		leftSingleDraw = new GameObject ();
//		leftSingleDraw.name = "LeftIndexStroke";
//		
//		rightSingleDraw = new GameObject ();
//		rightSingleDraw.name = "RighIndexStroke";
//		
//		leftDoubleDraw = new GameObject ();
//		leftDoubleDraw.name = "LeftDoubleStroke";
//		
//		rightDoubleDraw = new GameObject ();
//		rightDoubleDraw.name = "RightDoubleStroke";
//		
//		leftSingleRenderer = leftSingleDraw.AddComponent<LineRenderer> ();
//		rightSingleRenderer = rightSingleDraw.AddComponent<LineRenderer> ();
//		leftDoubleRenderer = leftDoubleDraw.AddComponent<LineRenderer> ();
//		rightDoubleRenderer = rightDoubleDraw.AddComponent<LineRenderer> ();
//		
//		
//		leftSingleRenderer.material = new Material (Shader.Find ("Particles/Additive"));
//		leftSingleRenderer.SetWidth (1, 1);
//		leftSingleRenderer.SetColors (Color.white, Color.white);
//		
//		rightSingleRenderer.material = new Material (Shader.Find ("Particles/Additive"));
//		rightSingleRenderer.SetWidth (1, 1);
//		rightSingleRenderer.SetColors (Color.white, Color.white);
//		
//		
//		leftDoubleRenderer.material = new Material (Shader.Find ("Particles/Additive"));
//		leftDoubleRenderer.SetWidth (1, 1);
//		leftDoubleRenderer.SetColors (Color.white, Color.white);
//		
//		rightDoubleRenderer.material = new Material (Shader.Find ("Particles/Additive"));
//		rightDoubleRenderer.SetWidth (1, 1);
//		rightDoubleRenderer.SetColors (Color.white, Color.white);
		
		if (controller == null) {
			Debug.Log ("Controller invalid!");
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Get the new frame
		frame = controller.Frame ();
		
		if (frame == null || !frame.IsValid) {
			Debug.Log ("Invalid frame!");
			return;
		}		
			
		//Update hands;
		UpdateHands ();
	}
	
	
	//Update hands
	void UpdateHands ()
	{
		if (!frame.Hands.IsEmpty) {
			foreach (Leap.Hand hand in frame.Hands) {
				//If the hand is not in the list, add it to the list
				bool checkHand = false;
				int handIndex = -1;
				foreach (LeapHand leapHand in handList) {
					if (leapHand.getHand ().Id == hand.Id) {
						checkHand = true;
						handIndex = handList.IndexOf (leapHand);
						break;
					}
				}
				
				//Check whether it is a new hand
				if (!checkHand) {
					//A new hand
					LeapHand newHand = new LeapHand (hand);
					handList.Add (newHand);
				} else {
					//An existing hand
					handList [handIndex].UpdateHand (hand);
				}	
			}
			
			//Check if there is any hand that should be removed
			for (int i=handList.Count-1; i>=0; --i) {
				LeapHand leapHand = handList [i];

				bool handInclude = false;
				foreach (Hand hand in frame.Hands) {
					if (leapHand.getHand ().Id == hand.Id) {
						handInclude = true;
						break;
					}
				}
				
				//Remove the hand
				if (handInclude == false) {
					DestroyHandComponents (leapHand);
					handList.Remove (leapHand);
				}
			}
		} else {
			//Remove all the hands
			foreach (LeapHand leapHand in handList) {
				DestroyHandComponents (leapHand);
			}
			handList.Clear ();
		}
	}
	
	//Destroy Unity components of the hand
	public void DestroyHandComponents (LeapHand leapHand)
	{
		Destroy (leapHand.getHandObject ());
			
		DestroyFingerComponents (leapHand.getThumb ());
			
		DestroyFingerComponents (leapHand.getIndex ());
			
		DestroyFingerComponents (leapHand.getMiddle ());
			
		DestroyFingerComponents (leapHand.getRing ());
			
		DestroyFingerComponents (leapHand.getPinky ());
	}
	
	//Destroy Unity components of the finger
	public void DestroyFingerComponents (LeapFinger leapFinger)
	{
		Destroy (leapFinger.getFingerObject ());
		
		Destroy (leapFinger.getMetacarpal ().getBoneObject ());
		Destroy (leapFinger.getMetacarpal ().getPreJointObject ());
		
		Destroy (leapFinger.getProximal ().getBoneObject ());
		Destroy (leapFinger.getProximal ().getPreJointObject ());
		
		Destroy (leapFinger.getIntermediate ().getBoneObject ());
		Destroy (leapFinger.getIntermediate ().getPreJointObject ());
		
		Destroy (leapFinger.getDistal ().getBoneObject ());
		Destroy (leapFinger.getDistal ().getPreJointObject ());
	}
}
