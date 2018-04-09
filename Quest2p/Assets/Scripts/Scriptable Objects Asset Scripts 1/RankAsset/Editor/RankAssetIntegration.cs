using UnityEngine;
using UnityEditor;

static class RankAssetIntegration {

	[MenuItem("Assets/Create/RankAsset")]
	public static void CreateYourScriptableObject() {
		ScriptableObjectUtility2.CreateAsset<RankAsset>();
	}

}
