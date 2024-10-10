using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    static GraphManager instance = null;
    Dictionary<int, Node> nodes;

    [SerializeField]
    ModelPosFitter[] objPrefabs;
    [SerializeField]
    ModelPosFitter emptyPrefab;//존재하지않는 조합의 모델 ㅇㅇ

    ModelPosFitter[] models;

    private void Awake() {

        if (instance == null) {

            instance = this;
            nodes = new Dictionary<int, Node>();

            models = new ModelPosFitter[objPrefabs.Length];

        } else {

            Destroy(gameObject);

        }

    }

    public static GraphManager GetInstance() {

        return instance;

    }

    public ModelPosFitter GetModel(int _modelId) {

        if (models[_modelId] == null) {

            if (objPrefabs[_modelId] == null) {
                models[_modelId] = Instantiate(emptyPrefab);
            } else {
                models[_modelId] = Instantiate(objPrefabs[_modelId]);
            }

            models[_modelId].gameObject.transform.SetParent(transform);

        }

        return models[_modelId];

    }

}
