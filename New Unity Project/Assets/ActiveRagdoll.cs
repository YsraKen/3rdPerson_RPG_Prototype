using UnityEngine;

public class ActiveRagdoll : MonoBehaviour
{
	[SerializeField] private ARJoint[] myBones;
	[SerializeField] private Transform[] targetBones;
	
	CapsuleCollider[] capsuleColliders;
	
	[Header ("Joint Drive")]
		[SerializeField] private float torqueForce = 500f; // muscle force?
		[SerializeField] private float maxForce = 500f;
		[SerializeField] private float angularDamping;

	[Header ("Joint Spring")]
		[SerializeField] private float springForce;
		[SerializeField] private float springDamping;

	[Space(10)]
		[SerializeField] private Vector3 targetAngularVelocity;
		
	public void Setup(){
		SetupJoints();
		ConnectJoints();
	}
	
	public void UpdateSettings(){
		for(int i = 0; i < myBones.Length; i++){
			var current = myBones[i];
			
			current.drive.positionSpring = torqueForce;
			current.drive.positionDamper = angularDamping;
			current.drive.maximumForce = maxForce;

			current.spring.spring = springForce;
			current.spring.damper = springDamping;
			
			current.joint.slerpDrive = current.drive;
			current.joint.linearLimitSpring = current.spring;
			
			current.joint.targetAngularVelocity = targetAngularVelocity;
		}
	}
	
	public void AddColliders(){
		int length = myBones.Length;
		capsuleColliders = new CapsuleCollider[length];
		
		for(int i = 0; i < length; i++){
			var mb = myBones[i];
			
			CapsuleCollider newCollider = mb.gameObject.AddComponent<CapsuleCollider>();
			capsuleColliders[i] = newCollider;
		}
	}
	
	public void DeleteColliders(){
		foreach(var cc in capsuleColliders){
			DestroyImmediate(cc);
		}
		capsuleColliders = null;
	}
	
	void ConnectJoints(){
		foreach(var arj in myBones){
			var joint = arj.joint;
			
			var parentObj = arj.transform.parent;
			var parentArj = parentObj.GetComponent<ARJoint>();
			
			if(parentArj == null){
				Debug.LogWarning(joint + " has nothing to connect with!", joint.gameObject);
				continue;
			}
			
			joint.connectedBody = parentArj.rb;
		}
	}
	
	void SetupJoints(){
		int length = myBones.Length;
		
		for(int i = 0; i < length; i++){
			var current = myBones[i];
			
			current.joint = current.GetComponent<ConfigurableJoint>();
			current.rb = current.GetComponent<Rigidbody>();
			
			current.drive.positionSpring = torqueForce;
			current.drive.positionDamper = angularDamping;
			current.drive.maximumForce = maxForce;

			current.spring.spring = springForce;
			current.spring.damper = springDamping;
			
			current.joint.slerpDrive = current.drive;
			current.joint.linearLimitSpring = current.spring;
			
			current.joint.rotationDriveMode = RotationDriveMode.Slerp;
			current.joint.projectionMode = JointProjectionMode.None;
			current.joint.targetAngularVelocity = targetAngularVelocity;
			current.joint.configuredInWorldSpace = false;
			current.joint.swapBodies = true;

			current.joint.angularXMotion = ConfigurableJointMotion.Free;
			current.joint.angularYMotion = ConfigurableJointMotion.Free;
			current.joint.angularZMotion = ConfigurableJointMotion.Free;
			
			current.joint.xMotion = ConfigurableJointMotion.Locked;
			current.joint.yMotion = ConfigurableJointMotion.Locked;
			current.joint.zMotion = ConfigurableJointMotion.Locked;
			
			current.animatorBone = targetBones[i];
			current.startRot = Quaternion.Inverse(current.animatorBone.localRotation);
		}
	}
}