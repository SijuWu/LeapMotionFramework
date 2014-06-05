using UnityEngine;
using System;
using Leap;

namespace AssemblyCSharp
{
	public class LeapBone
	{
		//Bone sphere displayed 
		GameObject boneObject;
		
		//Bone renderer
		LineRenderer boneRenderer;
		
		//Prveious joint displayed
		GameObject preJointObject;

		//Leap bone data
		Leap.Bone bone;
		
		//Bone scale
		const float scale = 5.0f;
		
		//Get bone data
		public Leap.Bone getBone ()
		{
			return bone;
		}
		
		//Get bone object
		public GameObject getBoneObject ()
		{
			return boneObject;
		}
		
		//Get previous joint object
		public GameObject getPreJointObject ()
		{
			return preJointObject;
		}
		
		public LeapBone (Leap.Bone bone)
		{
			//Create bone object
			boneObject = new GameObject ();
			
			//Set bone object name
			boneObject.name="Bone";
			
			//Create bone renderer
			boneRenderer = boneObject.AddComponent<LineRenderer> ();
			
			//Set renderer properties
			boneRenderer.material = new Material (Shader.Find ("Particles/Additive"));
			boneRenderer.SetWidth (2, 2);
			boneRenderer.SetColors (Color.white, Color.white);
			
			//Draw bone
			boneRenderer.SetVertexCount (2);
			boneRenderer.SetPosition (0, new Vector3 (bone.PrevJoint.x, bone.PrevJoint.y, -bone.PrevJoint.z));
			boneRenderer.SetPosition (1, new Vector3 (bone.NextJoint.x, bone.NextJoint.y, -bone.NextJoint.z));
			
			//Create previous joint object
			preJointObject = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			
			//Set previous joint object name
			preJointObject.name="Joint";
			
			//Save leap bone data
			this.bone = bone;
		
			
			//Set previous joint position
			//Reverse z coordinate
			preJointObject.transform.position = new Vector3 (bone.PrevJoint.x, bone.PrevJoint.y, -bone.PrevJoint.z);
			
			//Set bone color
			boneObject.transform.renderer.material.color = Color.white;
			
			//Set previous joint color
			preJointObject.transform.renderer.material.color = Color.yellow;
			
			//Set bone size
			boneObject.transform.localScale = new Vector3 (scale, scale, scale);
			
			//Set previous joint size
			preJointObject.transform.localScale = new Vector3 (scale, scale, scale);
		}
		
		//Update the bone
		public void UpdateBone (Leap.Bone bone)
		{
			if (!bone.IsValid)
				return;
			
			//Refresh leap bone data
			this.bone = bone;
			
			//Draw bone
			boneRenderer.SetPosition (0, new Vector3 (bone.PrevJoint.x, bone.PrevJoint.y, -bone.PrevJoint.z));
			boneRenderer.SetPosition (1, new Vector3 (bone.NextJoint.x, bone.NextJoint.y, -bone.NextJoint.z));
			
			//Set previous joint posiiton
			//Reverse z coordinate
			preJointObject.transform.position = new Vector3 (bone.PrevJoint.x, bone.PrevJoint.y, -bone.PrevJoint.z);
		}
	}
}

