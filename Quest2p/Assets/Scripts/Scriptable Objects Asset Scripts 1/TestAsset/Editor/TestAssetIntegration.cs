using UnityEngine;
using UnityEditor;

static class TestAssetIntegration {

	[MenuItem("Assets/Create/TestAsset")]
	public static void CreateYourScriptableObject() {
		ScriptableObjectUtility2.CreateAsset<TestAsset>();
	}

}
