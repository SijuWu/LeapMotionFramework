using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using Leap;

public class Processor : MonoBehaviour
{
	//Manipulation mode
	public enum Mode
	{
		None,
		CycloShake
	}
	
	//Left index stroke
	Stroke leftSingleStroke;
	
	//Right index stroke
	Stroke rightSingleStroke;
	
	//Left index plus middle stroke
	Stroke leftDoubleStroke;
	
	//Right index plus middle stroke
	Stroke rightDoubleStroke;
	
	//List of left index stroke
	List<Stroke> leftSingleStrokeList;
	
	//List of right index stroke
	List<Stroke> rightSingleStrokeList;
	
	//List of left index middle stroke
	List<Stroke> leftDoubleStrokeList;
	
	//List of right index middle stroke
	List<Stroke> rightDoubleStrokeList;
	
	//Left index stroke object
	GameObject leftSingleDraw;
	
	//Right index stroke object
	GameObject rightSingleDraw;
	
	//Left index plus middle stroke object
	GameObject leftDoubleDraw;
	
	//Right index plus middle stroke object
	GameObject rightDoubleDraw;
	
	//Left index renderer
	LineRenderer leftSingleRenderer;
	
	//Right index renderer
	LineRenderer rightSingleRenderer;
	
	//Left index plus middle renderer
	LineRenderer leftDoubleRenderer;
	
	//Right index plus middle renderer
	LineRenderer rightDoubleRenderer;
	
	//Left single mode
	Mode leftSingleMode = Mode.None;
	
	//Right single mode
	Mode rightSingleMode = Mode.None;
	
	//Left double mode
	Mode leftDoubleMode = Mode.None;
	
	//Right double mode
	Mode rightDoubleMode = Mode.None;
	
	//Threshold distance between index and middle
	const float indexMiddleThreshold = 20;
	
	// Use this for initialization
	void Start ()
	{
		leftSingleStroke = new Stroke ();
		rightSingleStroke = new Stroke ();
		leftDoubleStroke = new Stroke ();
		rightDoubleStroke = new Stroke ();
		
		leftSingleStrokeList = new List<Stroke> ();
		rightSingleStrokeList = new List<Stroke> ();
		leftDoubleStrokeList = new List<Stroke> ();
		rightDoubleStrokeList = new List<Stroke> ();
		
		leftSingleDraw = new GameObject ();
		leftSingleDraw.name = "LeftIndexStroke";
		
		rightSingleDraw = new GameObject ();
		rightSingleDraw.name = "RighIndexStroke";
		
		leftDoubleDraw = new GameObject ();
		leftDoubleDraw.name = "LeftDoubleStroke";
		
		rightDoubleDraw = new GameObject ();
		rightDoubleDraw.name = "RightDoubleStroke";
		
		leftSingleRenderer = leftSingleDraw.AddComponent<LineRenderer> ();
		rightSingleRenderer = rightSingleDraw.AddComponent<LineRenderer> ();
		leftDoubleRenderer = leftDoubleDraw.AddComponent<LineRenderer> ();
		rightDoubleRenderer = rightDoubleDraw.AddComponent<LineRenderer> ();
		
		
		leftSingleRenderer.material = new Material (Shader.Find ("Particles/Additive"));
		leftSingleRenderer.SetWidth (1, 1);
		leftSingleRenderer.SetColors (Color.white, Color.white);
		
		rightSingleRenderer.material = new Material (Shader.Find ("Particles/Additive"));
		rightSingleRenderer.SetWidth (1, 1);
		rightSingleRenderer.SetColors (Color.white, Color.white);
		
		
		leftDoubleRenderer.material = new Material (Shader.Find ("Particles/Additive"));
		leftDoubleRenderer.SetWidth (1, 1);
		leftDoubleRenderer.SetColors (Color.white, Color.white);
		
		rightDoubleRenderer.material = new Material (Shader.Find ("Particles/Additive"));
		rightDoubleRenderer.SetWidth (1, 1);
		rightDoubleRenderer.SetColors (Color.white, Color.white);
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Update stroke
		UpdateStroke ();
		
//		if (leftSingleStroke.getStrokePoints ().Count != 0)
//			SegmentStroke (ref leftSingleStroke, ref leftSingleStrokeList);
//		if (rightSingleStroke.getStrokePoints ().Count != 0)
//			SegmentStroke (ref rightSingleStroke, ref rightSingleStrokeList);
//		if (leftDoubleStroke.getStrokePoints ().Count != 0)
//			SegmentStroke (ref leftDoubleStroke, ref leftDoubleStrokeList);
//		if (rightDoubleStroke.getStrokePoints ().Count != 0)
//			SegmentStroke (ref rightDoubleStroke, ref rightDoubleStrokeList);
		
//		CheckMode(ref leftSingleStroke,ref leftSingleStrokeList,ref leftSingleMode);
		CheckMode(ref rightSingleStroke,ref rightSingleStrokeList,ref rightSingleMode);
//		CheckMode(ref leftDoubleStroke,ref leftDoubleStrokeList,ref leftDoubleMode);
//		CheckMode(ref rightDoubleStroke,ref rightDoubleStrokeList,ref rightDoubleMode);
		
//		Debug.Log ("LeftSingle: " + leftSingleStrokeList.Count + " RightSingle: " + rightSingleStrokeList.Count +
//			      "LeftDouble: " + leftDoubleStrokeList.Count + " RightDouble: " + rightDoubleStrokeList.Count);

	}
	
	public void UpdateStroke ()
	{
		LeapController leapController = GameObject.Find ("Leap").GetComponent<LeapController> ();
		
		//Indicate whether the left index is viewed and extended
		bool leftSingleCheck = false;
		
		//Indicate whether the right index is viewed and extended
		bool rightSingleCheck = false;
		
		//Indicate whether the left index and middle are connected
		bool leftDoubleCheck = false;
		
		//Indicate whether the right index and middle are connected
		bool rightDoubleCheck = false;
		
		foreach (LeapHand leapHand in leapController.getHandList()) {
			Vector3 indexPos = leapHand.getIndex ().getPosition ();
			Vector3 middlePos = leapHand.getMiddle ().getPosition ();
			
			Vector3 indexSpeed = leapHand.getIndex ().getSpeed ();
			Vector3 middleSpeed = leapHand.getMiddle ().getSpeed ();
			
			Vector3 doubleFingerPos = (indexPos + middlePos) / 2;
			Vector3 doubleFingerSpeed = (indexSpeed + middleSpeed) / 2;
			
			//Check the distance between the index and the middle
			float disDouble = Vector3.Distance (indexPos, middlePos);
			
			if (leapHand.getHand ().IsLeft && leapHand.getIndex ().getFinger ().IsExtended) {
				
				if (disDouble > indexMiddleThreshold) {
					
//					Debug.Log ("Left index");
					
					//Add the index position to the stroke
					leftSingleCheck = true;
					
					if (leftSingleStroke.getStrokePoints ().Count == 0)
						leftSingleStroke.setStartTime (Time.time);
					
					leftSingleStroke.getStrokePoints ().Add (indexPos);	
					leftSingleStroke.getStrokeSpeeds ().Add (indexSpeed);
				} else {
					
//					Debug.Log ("Left index middle");
					
					//Add the center of the index and the middle to the stroke
					leftDoubleCheck = true;
					
					if (leftDoubleStroke.getStrokePoints ().Count == 0)
						leftDoubleStroke.setStartTime (Time.time);
					
					leftDoubleStroke.getStrokePoints ().Add (doubleFingerPos);
					leftDoubleStroke.getStrokeSpeeds ().Add (doubleFingerSpeed);
					
				}
			}
			
			if (leapHand.getHand ().IsRight && leapHand.getIndex ().getFinger ().IsExtended) {

				if (disDouble > indexMiddleThreshold) {
					
//					Debug.Log ("Right index");
					
					//Add the index position to the stroke
					rightSingleCheck = true;
					
					if (rightSingleStroke.getStrokePoints ().Count == 0)
						rightSingleStroke.setStartTime (Time.time);
					
					rightSingleStroke.getStrokePoints ().Add (indexPos);
					rightSingleStroke.getStrokeSpeeds ().Add (indexSpeed);
				} else {
					
//					Debug.Log ("Right index middle");
					
					//Add the center of the index and the middle to the stroke
					rightDoubleCheck = true;
					
					if (rightDoubleStroke.getStrokePoints ().Count == 0)
						rightDoubleStroke.setStartTime (Time.time);
					
					rightDoubleStroke.getStrokePoints ().Add (doubleFingerPos);
					rightDoubleStroke.getStrokeSpeeds ().Add (doubleFingerSpeed);
				}
			}
		}
		if (!leftSingleCheck) {
			leftSingleStroke = new Stroke ();
			leftSingleStrokeList.Clear ();
		}
		
		if (!rightSingleCheck) {
			rightSingleStroke = new Stroke ();
			rightSingleStrokeList.Clear ();
		}
		
		if (!leftDoubleCheck) {
			leftDoubleStroke = new Stroke ();
			leftDoubleStrokeList.Clear ();
		}
		
		if (!rightDoubleCheck) {
			rightDoubleStroke = new Stroke ();
			rightDoubleStrokeList.Clear ();
		}
		
		//Draw stroke
		DrawStroke ();
	}
	
	//Draw stroke
	public void DrawStroke ()
	{
		leftSingleRenderer.SetVertexCount (leftSingleStroke.getStrokePoints ().Count);
		for (int i=0; i<leftSingleStroke.getStrokePoints().Count; ++i) {
			leftSingleRenderer.SetPosition (i, leftSingleStroke.getStrokePoints () [i]);
		}
		
		rightSingleRenderer.SetVertexCount (rightSingleStroke.getStrokePoints ().Count);
		for (int i=0; i<rightSingleStroke.getStrokePoints().Count; ++i) {
			rightSingleRenderer.SetPosition (i, rightSingleStroke.getStrokePoints () [i]);
		}
		
		leftDoubleRenderer.SetVertexCount (leftDoubleStroke.getStrokePoints ().Count);
		for (int i=0; i<leftDoubleStroke.getStrokePoints().Count; ++i) {
			leftDoubleRenderer.SetPosition (i, leftDoubleStroke.getStrokePoints () [i]);
		}
		
		rightDoubleRenderer.SetVertexCount (rightDoubleStroke.getStrokePoints ().Count);
		for (int i=0; i<rightDoubleStroke.getStrokePoints().Count; ++i) {
			rightDoubleRenderer.SetPosition (i, rightDoubleStroke.getStrokePoints () [i]);
		}
	}
	

	//Check operation mode
	public void CheckMode (ref Stroke stroke, ref List<Stroke> strokeList, ref Mode mode)
	{
		//Indicator of change of stroke direction
		bool strokeReverse = false;
		
		//Check wheter to segment the stroke
		if (stroke.getStrokePoints ().Count != 0)
			strokeReverse = SegmentStroke (ref stroke, ref strokeList);
		
		if (strokeReverse) {
			switch (mode) {
			//Check onset of an operation
			case Mode.None:
				checkOnset (strokeList, ref mode);
				break;
				
			case Mode.CycloShake:
				break;
			}
		}
	}
	
	//Segment stroke
	public bool SegmentStroke (ref Stroke stroke, ref List<Stroke> strokeList)
	{
		int indexDifference = 2;
		
		if (stroke.getStrokePoints ().Count < (1 + 2 * indexDifference))
			return false;
		
		//Indicate whether to segment the stroke
		bool corner = false;
		
		//The end point of the stroke
		Vector3 endPoint = stroke.getStrokePoints () [stroke.getStrokePoints ().Count - 1];
		
		//The last end point of the stroke
		Vector3 lastEndPOint = stroke.getStrokePoints () [stroke.getStrokePoints ().Count - 2];
		
		//The speed of the end point
		Vector3 endSpeed = stroke.getStrokeSpeeds () [stroke.getStrokeSpeeds ().Count - 1];
		
		//The speed of the last end point
		Vector3 lastEndSpeed = stroke.getStrokeSpeeds () [stroke.getStrokeSpeeds ().Count - 2];
			
		//Get three check points 
		int pointCount = stroke.getStrokePoints ().Count;
		Vector3 point1 = stroke.getStrokePoints () [pointCount - 1 - 2 * indexDifference];
		Vector3 point2 = stroke.getStrokePoints () [pointCount - 1 - indexDifference];
		Vector3 point3 = stroke.getStrokePoints () [pointCount - 1];
		Vector3 lineCenter = (point1 + point3) / 2;
		
		float distance1 = Vector3.Distance (point1, point3);
		float distance2 = Vector3.Distance (point2, lineCenter);
		
		//If the corner is steep, add the active stroke to the list, reconstruct a new stroke.
		if (distance1 < 3 * distance2) {
			stroke.setEndTime (Time.time);
			Stroke strokeCopy = new Stroke (stroke);
			strokeList.Add (strokeCopy);
			
			stroke = new Stroke ();
			stroke.setStartTime (Time.time);
			stroke.getStrokePoints ().Add (endPoint);
			stroke.getStrokeSpeeds ().Add (endSpeed);
			
			return true;
		} else
			return false;
	}
	
	//Check onset of an operation
	public void checkOnset (List<Stroke> strokeList, ref Mode mode)
	{
		//Find a projection plane of all the stroke points,
		//then fit projections points to an ellipse
		Vector2 ellipseCenter = new Vector2 ();
		float ellipseA = 0;
		float ellipseB = 0;
		float[,] rotationMatrix = new float[2, 2];
		
		Vector3 center = new Vector3 ();
		List<Vector3> planeVectors = new List<Vector3> ();
		List<Vector2> planePoints = new List<Vector2> ();
		
		//Project stroke points to a plane
		Stroke lastStroke = strokeList [strokeList.Count - 1];
		List<Vector3> projectionPoints = lastStroke.planeProjection (lastStroke.getStrokePoints (), ref center, ref planeVectors, ref planePoints);
		
		//Fit the stroke to an ellipse
		float eccentricity = lastStroke.FitStroke2Ellipse (planePoints, out ellipseCenter, out ellipseA, out ellipseB, out rotationMatrix);
		
		Vector3 startPoint = lastStroke.getStrokePoints () [0];
		Vector3 endPoint = lastStroke.getStrokePoints () [lastStroke.getStrokePoints ().Count - 1];
		
		//Calculate average speed of the stroke
		float distance = Vector3.Distance (startPoint, endPoint);			
		float averageSpeed = distance / (lastStroke.getEndTime () - lastStroke.getStartTime ());
		
		//Fit the stroke to a line
		Vector3 lineDirection = new Vector3 ();
		float fitLineCost = 0;
		lastStroke.fitLine (lastStroke.getStrokePoints (), out lineDirection, out fitLineCost);
		
//		Debug.Log(fitLineCost);
		if(fitLineCost>50||distance>150)
		{
			Debug.Log("Cost "+fitLineCost+" distance "+distance);
		}
	}
}
