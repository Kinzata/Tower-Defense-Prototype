using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

	private LevelPath levelPath;

	[SerializeField] WaveConfig[] waves;
	private int currentWaveIndex = 0;

	// Use this for initialization
	void Start () {
		levelPath = FindObjectOfType<LevelPath>();

		SpawnNextWave();
	}
	

	private void SpawnNextWave() {
		var routine = StartCoroutine(SpawnWave(currentWaveIndex++));
	}

    private IEnumerator SpawnWave(int index)
    {
		var waveConfig = waves[index];
        for(int i = 0; i < waveConfig.GetNumberOfEnemies(); i++)
        {
			var waitTime = 0f;
			if( i != 0 ) { waitTime = waveConfig.GetTimeBetweenSpawns();}
            yield return new WaitForSeconds(waitTime);
            var enemy = Instantiate(waveConfig.GetEnemyPrefab(),levelPath.GetWaypointAt(0).point,Quaternion.identity);
			enemy.GetComponent<Enemy>().SetCurrentWaypoint(levelPath.GetWaypointAt(0));
			enemy.GetComponent<Enemy>().SetMoveSpeed(waveConfig.GetMoveSpeed());
        }
    }
}
