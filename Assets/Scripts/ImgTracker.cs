using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImgTracker : MonoBehaviour
{
    /*[SerializeField]
    ARTrackedImageManager imageManager;

    [SerializeField]
    GameObject[] prefab;


    Dictionary<string, GameObject> objsDic = new Dictionary<string, GameObject>();

    private void Awake() {

        GameObject n1 = Instantiate(prefab[0]);
        GameObject n2 = Instantiate(prefab[1]);

        n1.GetComponent<OTest>().SetObj(n2);

        n1.SetActive(false);
        n2.SetActive(false);

        objsDic.Add("1", n1);
        objsDic.Add("2", n2);

    }

    private void OnEnable() {

        imageManager.trackedImagesChanged += Changed;

    }

    private void OnDisable() {

        imageManager.trackedImagesChanged -= Changed;

    }

    void Changed(ARTrackedImagesChangedEventArgs _args) {

        foreach (ARTrackedImage _img in _args.added) {


            GameObject obj = null;

            if (objsDic.ContainsKey(_img.referenceImage.name)) {

                obj = objsDic[_img.referenceImage.name];
                obj.SetActive(true);

            }

            obj.transform.SetParent(_img.transform);
            obj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            //}



        }

        foreach (ARTrackedImage _img in _args.updated) {

                _img.transform.GetChild(0).transform.localPosition = Vector3.zero;

        }

        *//*        foreach (ARTrackedImage _img in _args.removed) {

                    Debug.Log("»ç¶óÁü");
                    Destroy(_img.transform.GetChild(0).gameObject);

                }*//*


    }*/

}
