using UnityEngine;

public interface IDamageables
{
	void TakeDamage();
	void TakeDamage(Vector3 hitPosition);
}