using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Str2Enum")]
public class Str2ModelSO : ScriptableObject {

    [Serializable]
    public struct ImgNameData {

        public string imgName;
        public DataType dataType;

    }

    public ImgNameData[] imgNameDatas;

}
