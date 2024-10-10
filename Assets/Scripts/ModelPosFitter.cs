using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ModelPosFitter : MonoBehaviour
{
    
    List<Transform> posList = new List<Transform>();

    Vector3 pos;

    private void Update() {

        if (posList.Count <= 0 || !gameObject.activeSelf)
            return;

        pos = Vector3.zero;

        foreach (Transform t in posList) {

            pos += t.transform.position;

        }

        transform.position = pos / posList.Count;

    }


    public void AddTransform(Transform _transform) { 
    
        posList.Add(_transform);

    }

    public void RemoveAllTransforms() {

        posList.Clear();

    }

}
