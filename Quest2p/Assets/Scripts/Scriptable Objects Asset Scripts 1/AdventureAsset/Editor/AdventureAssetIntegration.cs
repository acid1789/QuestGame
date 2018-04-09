using UnityEngine;
using UnityEditor;

static class AdventureAssetIntegration {

	[MenuItem("Assets/Create/AdventureAsset")]
	public static void CreateYourScriptableObject() {
		ScriptableObjectUtility2.CreateAsset<AdventureAsset>();
	}

}
