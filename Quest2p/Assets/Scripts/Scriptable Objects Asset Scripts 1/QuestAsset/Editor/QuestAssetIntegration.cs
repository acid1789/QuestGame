using UnityEngine;
using UnityEditor;

static class QuestAssetIntegration {

	[MenuItem("Assets/Create/QuestAsset")]
	public static void CreateYourScriptableObject() {
		ScriptableObjectUtility2.CreateAsset<QuestAsset>();
	}

}
