using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EntitySpawner : MonoBehaviour
{
	[System.Serializable] public class EntitiesSpawner{
		public string _name;
		public GameObject[] objectsToSpawn;
		
		public int maxSpawnRate, maxSpawnCount;
		public Vector2 spawnTime;
		
		public Vector3
			spawnAreaA = new Vector3(-40,20,-40),
			spawnAreaB = new Vector3(40,90,40);
		
		public bool randomizeRotation;
		public List<GameObject> spawns = new List<GameObject>();
		
		float time;
		
		public bool TimerElapsed(){
			var output = false;
			
			if(time <= 0){
				time = Randomize(spawnTime);
				output = true;
			}else{
				time -= Time.deltaTime;
				output = false;
			}
			
			return output;
		}
		
		public void Spawn(){
			spawns = spawns.Where(item => item != null).ToList();
			
			if(spawns.Count < maxSpawnCount){
				var newSpawnCount = Randomize(maxSpawnRate);
				var totalSpawnCount = spawns.Count + newSpawnCount;
				
				if(totalSpawnCount < maxSpawnCount){
					DoSpawning(newSpawnCount);
				}
			}
		}
		
		public void DoSpawning(int howMany, Vector3 newPosition){ // for start, give player some orbs
			for(int i = 0; i < howMany; i++){
				var objectCopy = objectsToSpawn[Randomize(objectsToSpawn.Length)];
				
				var newObject = Instantiate(
					objectCopy,
					newPosition,
					Quaternion.identity
				);
				
				newObject.SetActive(true);
				spawns.Add(newObject);
			}
		}
		
		void DoSpawning(int howMany){
			for(int i = 0; i < howMany; i++){
				var objectCopy = objectsToSpawn[Randomize(objectsToSpawn.Length)];
				
				var newPosition = Randomize(spawnAreaA, spawnAreaB);
				var newRotation = (randomizeRotation)?
					Randomize(Vector3.one * 180f):
					Quaternion.identity;
				
				var newObject = Instantiate(
					objectCopy,
					newPosition,
					newRotation
				);
				
				newObject.SetActive(true);
				spawns.Add(newObject);
			}
		}
		
		static int Randomize(int max){ // for indexes
			return Random.Range(0, max);
		}
		
		static float Randomize(Vector2 minMax){ // for timer
			return Random.Range(minMax.x, minMax.y);
		}
		
		static Vector3 Randomize(Vector3 spawnAreaA, Vector3 spawnAreaB){ // for positions
			return new Vector3(
				Random.Range(spawnAreaA.x, spawnAreaB.x),
				Random.Range(spawnAreaA.y, spawnAreaB.y),
				Random.Range(spawnAreaA.z, spawnAreaB.z)
			);
		}
		
		static Quaternion Randomize(Vector3 euler){ // for rotations
			var output = Randomize(-euler, euler);
			return Quaternion.Euler(output);
		}
	}
	
	[SerializeField] private EntitiesSpawner[] entities;
	
	[Header("Spawn at Start")]
	[SerializeField] int index = 0; 
	[SerializeField] int amount = 15; 
	
	void Start(){
		entities[index].DoSpawning(amount, Vector3.up * 20f);
	}
	
	void Update(){
		foreach(var e in entities){
			if(e.TimerElapsed()){ e.Spawn(); }
		}
	}
}