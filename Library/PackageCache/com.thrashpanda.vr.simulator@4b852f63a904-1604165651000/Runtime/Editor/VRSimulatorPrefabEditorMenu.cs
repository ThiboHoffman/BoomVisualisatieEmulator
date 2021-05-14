using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if USE_INTERACTION_TOOLKIT
using UnityEngine.XR.Interaction.Toolkit;
#endif

[InitializeOnLoad]
public static class VRSimulatorPrefabEditorMenu
{
    static string packageName = "com.thrashpanda.vr.simulator";

    [MenuItem("GameObject/XR/VR Simulator"), MenuItem("VRSimulator/Create/VR Simulator")]
    public static void InstantiateVRSimulatorInScene()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Packages/" + packageName + "/Runtime/Assets/Prefab/VRSimulator.prefab", typeof(GameObject));
        GameObject simulator = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
#if USE_INTERACTION_TOOLKIT
        XRRig xRRig;
        if (xRRig = GameObject.FindObjectOfType<XRRig>())
            simulator.GetComponent<VRSimulator>().xRRigToFollow = xRRig.transform;
#endif
    }
}