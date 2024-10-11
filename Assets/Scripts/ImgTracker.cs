using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImgTracker : MonoBehaviour
{
    [SerializeField]
    ARTrackedImageManager imageManager;
    [SerializeField]
    Str2ModelSO imgNameMapperSO;
    Dictionary<string, DataType> nodeTypeDic = new Dictionary<string, DataType>();

    [Header("base prefab of cards")]
    [SerializeField]
    Node nodePrefab;

    Dictionary<DataType, Node> createdNodeDic = new Dictionary<DataType, Node>();

    private void Start() {
        
        foreach (var imgNameData in imgNameMapperSO.imgNameDatas)
            nodeTypeDic.Add(imgNameData.imgName, imgNameData.dataType);

    }

    private void OnEnable() {

        imageManager.trackedImagesChanged += Changed;

    }

    private void OnDisable() {

        imageManager.trackedImagesChanged -= Changed;

    }

    void Changed(ARTrackedImagesChangedEventArgs _args) {

        foreach (ARTrackedImage _img in _args.added) {

            if (nodeTypeDic.ContainsKey(_img.referenceImage.name)) {

                DataType type = nodeTypeDic[_img.referenceImage.name];
                Node tmpNode;

                if (createdNodeDic.ContainsKey(type)) {//이미 만들어진적이 있음.

                    //가져오자
                    tmpNode = createdNodeDic[type];
                    tmpNode.gameObject.SetActive(true);

                } else { 

                    tmpNode = Instantiate(nodePrefab);
                    tmpNode.SetIdx(type);

                    createdNodeDic.Add(type, tmpNode);
                                                                                 
                }

                tmpNode.transform.SetParent(_img.transform);
                tmpNode.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                tmpNode.transform.localScale = new Vector3(0.6f, 0.2f, 0.8f);

            }

        }

        foreach (ARTrackedImage _img in _args.updated) {

            Debug.Log(_img.trackingState);

            if (_img.trackingState == TrackingState.Limited) {

                createdNodeDic[nodeTypeDic[_img.referenceImage.name]].gameObject.SetActive(false);


            } else if (_img.trackingState == TrackingState.Tracking) {

                createdNodeDic[nodeTypeDic[_img.referenceImage.name]].gameObject.SetActive(true);
                _img.transform.GetChild(0).transform.localPosition = Vector3.zero;

            }


        }

        foreach (ARTrackedImage _img in _args.removed) {

            Debug.Log("사라짐");
            _img.transform.GetChild(0).transform.SetParent(null);
            _img.transform.GetChild(0).gameObject.SetActive(false);

        }


    }

}
