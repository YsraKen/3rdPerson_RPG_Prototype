using UnityEngine;

public class OneHandClubCombo : PlayerCombat
{
	[SerializeField] private string[] comboClipsName;
	
	int comboIndex;
	
	protected override void Start(){
		base.Start();
		JSS_AttackBehaviour.onAttackExit += OnCombatStateExit;
	}
	
	protected override void OnAttacking(){
		if(!isGrounded){ return; }
		base.OnAttacking();
		
		comboIndex++;
		
		if(comboIndex > comboClipsName.Length){ return; }
		
		anim.Play(comboClipsName[comboIndex]);
	}
	
	protected override void OnCombatStateExit(){
		base.OnCombatStateExit();
		
		comboIndex = 0;
	}
}