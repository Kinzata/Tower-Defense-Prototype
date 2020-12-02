using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class LevelManager : MonoBehaviour {

	[Header("Auto Load Settings")]
	[SerializeField] bool shouldAutoLoad = false;
	[SerializeField] string autoLoadScene = "Start Screen";
	[SerializeField] float autoLoadWait = 3f;

	int currentSceneIndex;

	public void Start() {
		currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		if(shouldAutoLoad) {
			StartCoroutine(WaitAndLoad(autoLoadScene, autoLoadWait));
		}
	}

	public void LoadScene(string sceneName) {
        LoadScene(sceneName, 0f);
	}

	public void LoadScene(string sceneName, float waitTime = 0f) {
		StartCoroutine(WaitAndLoad(sceneName, waitTime));
	}

	public void LoadNextScene(float waitTime = 0f) {
		StartCoroutine(WaitAndLoad(currentSceneIndex++, waitTime));
	}

	private IEnumerator WaitAndLoad(string sceneToLoad, float secondsToWait) {
		yield return new WaitForSeconds(secondsToWait);
		SceneManager.LoadScene(sceneToLoad);
	}

	private IEnumerator WaitAndLoad(int sceneIndex, float secondsToWait) {
		yield return new WaitForSeconds(secondsToWait);
		SceneManager.LoadScene(sceneIndex);
	}

	public void QuitGame(){
		Application.Quit();
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(LevelManager))]
// [CanEditMultipleObjects]
public class LevelManagerEditor : Editor {

	SerializedProperty shouldAutoLoad;
	SerializedProperty autoLoadScene;
	SerializedProperty autoLoadWait;

	void OnEnable() {
		shouldAutoLoad = serializedObject.FindProperty("shouldAutoLoad");
		autoLoadScene = serializedObject.FindProperty("autoLoadScene");
		autoLoadWait = serializedObject.FindProperty("autoLoadWait");
	}

	public override void OnInspectorGUI(){
		serializedObject.Update();

		EditorGUILayout.PropertyField(shouldAutoLoad);

		using ( var autoLoadGroup = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(shouldAutoLoad.boolValue))){
			if( autoLoadGroup.visible) {
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(autoLoadScene);
				EditorGUILayout.PropertyField(autoLoadWait);
				EditorGUI.indentLevel--;
			}
		}

		serializedObject.ApplyModifiedProperties();
	}
}
#endif

