using UnityEngine;
using UnityEditor;

static class StoryAssetIntegration {

	[MenuItem("Assets/Create/StoryAsset")]
	public static void CreateYourScriptableObject() {
		ScriptableObjectUtility2.CreateAsset<StoryAsset>();
	}

}
