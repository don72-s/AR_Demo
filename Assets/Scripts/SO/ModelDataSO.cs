using System;
using UnityEngine;

public enum DataType { TYPE1 = 1, TYPE2, TYPE3, TYPE4, TYPE5 }

[CreateAssetMenu(menuName = "Models DataSet")]
public class ModelDataSO : ScriptableObject {

    [Serializable]
    public struct CombineInfo {

        [SerializeField]
        public DataType[] datas;
        [SerializeField]
        public ModelPosFitter modelPrefab;

    }

    [SerializeField]
    public CombineInfo[] modelsInfo;

}
