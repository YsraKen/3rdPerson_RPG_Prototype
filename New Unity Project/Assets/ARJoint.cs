using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
public class ARJoint : MonoBehaviour
{
	[HideInInspector] public Transform animatorBone;
	[HideInInspector] public Quaternion startRot;
	
	[HideInInspector] public JointDrive drive;
	[HideInInspector] public SoftJointLimitSpring spring;
	
	[HideInInspector] public ConfigurableJoint joint;
	[HideInInspector] public Rigidbody rb;
	
	void Update(){
		if(rb.IsSleeping()){ rb.WakeUp(); }
		joint.targetRotation = animatorBone.localRotation * startRot;
	}
}