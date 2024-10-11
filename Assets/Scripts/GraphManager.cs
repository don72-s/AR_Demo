using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    static GraphManager instance = null;
    Dictionary<int, Node> nodes;

    ModelPosFitter[] objPrefabs;
    ModelPosFitter[] instantiatedModels;


    [Header("Model DataSet SOs")]
    [SerializeField]
    ModelDataSO[] modelDataSetSO;
    [Header("Model when there is no matching")]
    [SerializeField]
    ModelPosFitter emptyPrefab;//���������ʴ� ������ �� ����
    [Header("Count of base nodes")]
    [SerializeField]
    [Range(1,30)]
    int baseCardCount;//���� �⺻ ������� ī�尡 �������� �Է�

    private void Awake() {

        if (instance == null) {

            instance = this;
            nodes = new Dictionary<int, Node>();

            InitDictionary();

        } else {

            Destroy(gameObject);

        }

    }


    public static GraphManager GetInstance() {

        return instance;

    }

    void InitDictionary() {

        objPrefabs = new ModelPosFitter[(1 << baseCardCount)];//0�� ������� �������̹Ƿ� 1�� ���� ����.
        instantiatedModels = new ModelPosFitter[objPrefabs.Length];

        foreach (ModelDataSO _modelSO in modelDataSetSO) {

            foreach (ModelDataSO.CombineInfo _data in _modelSO.modelsInfo) {

                int idx = 0;

                foreach (DataType _type in _data.datas) {

                    idx += (1 << ((int)_type - 1));

                }

                objPrefabs[idx] = _data.modelPrefab;

            }

        }

    }

    public ModelPosFitter GetModel(int _modelId) {

        if (instantiatedModels[_modelId] == null) {

            if (objPrefabs[_modelId] == null) {
                instantiatedModels[_modelId] = Instantiate(emptyPrefab);
            } else {
                instantiatedModels[_modelId] = Instantiate(objPrefabs[_modelId]);
            }

            instantiatedModels[_modelId].gameObject.transform.SetParent(transform);

        }

        return instantiatedModels[_modelId];

    }

}
