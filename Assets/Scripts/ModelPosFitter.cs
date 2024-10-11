using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ModelPosFitter : MonoBehaviour
{
    
    List<Transform> posList = new List<Transform>();

    Vector3 pos;
    Quaternion rot;

    private void Update() {

        if (posList.Count <= 0 || !gameObject.activeSelf)
            return;

        pos = Vector3.zero;

        foreach (Transform t in posList) {

            pos += t.transform.position;

        }

        transform.position = pos / posList.Count;



        rot = posList[0].rotation;

        for (int i = 1; i < posList.Count; i++) {

            rot = Quaternion.Slerp(rot, posList[i].rotation, 1f / (i + 1f));

        }

        transform.rotation = rot;
        transform.Rotate(Vector3.up * 90);

    }


    public void AddTransform(Transform _transform) { 
    
        posList.Add(_transform);

    }

    public void RemoveAllTransforms() {

        posList.Clear();

    }

}
