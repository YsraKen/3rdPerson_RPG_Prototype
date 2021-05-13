using UnityEngine;
using System.Collections;

public class DashStrike : PlayerCombat
{
	public static bool isCurrentlyAttacking;
	
	[SerializeField] private string
		frontAttackClip,
		spinAttackClip;
		
	[SerializeField] private string[] comboClips;
	
	public int comboIndex;
	public bool firstAttack, hasComboPotential, wasComboSuccessful;
	
	public ParticleSystem particle;
	public int partcleEmit = 20;
	
	protected override void Start(){
		base.Start();
		
		JSS_DashStrike.onDashEnter = OnAnimationEnter;
		JSS_DashStrike.onDashExit = OnAnimationExit;
	}
	
	protected override void OnAttacking(){
		if(!isGrounded){ return; }
		base.OnAttacking();
/* 		
		if(angle > 90){
			anim.Play(spinAttackClip);
			comboIndex = 0;
			
			return;
		} */
		
		if(firstAttack){
			FirstAttack();
			firstAttack = false;
		}
		
		if(hasComboPotential){
			// do the combo
			var newClip = comboClips[comboIndex];
			anim.Play(newClip);
				
			comboIndex++;
			comboIndex = comboIndex % comboClips.Length;
			
			wasComboSuccessful = true;
			hasComboPotential = false;
			
			particle.Emit(partcleEmit);
		}
	}
	
	void FirstAttack(){
		if(angle > 90){
			anim.Play(spinAttackClip);
			comboIndex = 0;
		}else{
			anim.Play(frontAttackClip);
		}
		
		particle.Emit(partcleEmit);
	}
	
	public void ComboEventListener(){
		hasComboPotential = true;
	}
	
	void OnAnimationEnter(){
		isCurrentlyAttacking = true;
	}
	
	void OnAnimationExit(){
		isCurrentlyAttacking = false;
		
		if(!wasComboSuccessful){
			comboIndex = 0;
			
			hasComboPotential = false;
			firstAttack = true;
		}
		
		wasComboSuccessful = false; // false for next combo
	}
}