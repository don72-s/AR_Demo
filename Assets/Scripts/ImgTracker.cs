using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImgTracker : MonoBehaviour
{
    [SerializeField]
    ARTrackedImageManager imageManager;

    [SerializeField]
    GameObject prefab;

    /*    [SerializeField]
        XRReferenceImageLibrary lib;*/

    private void OnEnable() {

        imageManager.trackedImagesChanged += Changed;

    }

    private void OnDisable() {

        imageManager.trackedImagesChanged -= Changed;

    }

    void Changed(ARTrackedImagesChangedEventArgs _args) {

        foreach (ARTrackedImage _img in _args.added) {

            Debug.LogWarning("����");

            GameObject obj = null;

            //todo : so�� ���������.
            switch (_img.referenceImage.name) {

                case "ToyTank":
                    obj = Instantiate(prefab);
                    break;

                default:
                    return;

            }

            obj.transform.SetParent(_img.transform);
            obj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

        }

        foreach (ARTrackedImage _img in _args.updated) {

            Debug.Log("������Ʈ" + _img.trackingState);
            _img.transform.GetChild(0).transform.localPosition = Vector3.zero;

        }

        /*        foreach (ARTrackedImage _img in _args.removed) {

                    Debug.Log("�����");
                    Destroy(_img.transform.GetChild(0).gameObject);

                }*/


    }
}
