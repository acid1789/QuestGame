using UnityEngine;
using UnityEditor;

static class FoeAssetIntegration {

	[MenuItem("Assets/Create/FoeAsset")]
	public static void CreateYourScriptableObject() {
		ScriptableObjectUtility2.CreateAsset<FoeAsset>();
	}

}
