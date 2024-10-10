using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    [SerializeField]
    [Range(1, 31)]
    int myId = 0;
    int myGroupId = 0;

    GraphManager graphManager;

    List<Node> nodes;

    private void Awake() {

        nodes = new List<Node>();
        myId = 1 << (myId - 1);
        myGroupId = myId;

    }

    private void Start() {

        graphManager = GraphManager.GetInstance();

        if(myId != 0)
            graphManager.GetModel(myId).AddTransform(transform);

    }

    public void SetIdx(int _idx) {

        if (myId == 0) {

            myId = 1 << (myId - 1);

        }

    }

    public void ConnectNode(Node _node) {

        if (nodes.Contains(_node))
            return;

        nodes.Add(_node);

    }

    public void DisconnectNode(Node _node) {

        if (nodes.Contains(_node)) {

            nodes.Remove(_node);

        }

    }

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.layer == LayerMask.NameToLayer("NodeObj")) {

            Node node = other.gameObject.GetComponent<Node>();

            ConnectNode(node);
            node.ConnectNode(this);

            int newGroupId = GetGroupId();

            if (newGroupId != myGroupId) {

                ModelPosFitter tmp = graphManager.GetModel(newGroupId);

                tmp.gameObject.SetActive(true);
                SetGroupId_N_SetNewModelTransforms(newGroupId, tmp);


            }


        }

    }

    private void OnTriggerExit(Collider other) {

        if (other.gameObject.layer == LayerMask.NameToLayer("NodeObj")) {


            Node node = other.gameObject.GetComponent<Node>();

            DisconnectNode(node);
            node.DisconnectNode(this);

            int newGroupId = GetGroupId();

            if (myGroupId != newGroupId) {

                //새로운 그룹아이디와 대응하는 모델 가져옴.
                ModelPosFitter tmp = graphManager.GetModel(newGroupId);
                tmp.gameObject.SetActive(true);
                SetGroupId_N_SetNewModelTransforms(newGroupId, tmp);

            }


            newGroupId = node.GetGroupId();

            if (node.myGroupId != newGroupId) {

                ModelPosFitter tmp = graphManager.GetModel(newGroupId);

                tmp.gameObject.SetActive(true);
                node.SetGroupId_N_SetNewModelTransforms(newGroupId, tmp);

            }

        }

    }

    /// <summary>
    /// 속한 그룹의 그룹아이디를 구해옴
    /// </summary>
    /// <param name="_curVisitedId">현재까지 방문한 노드들 목록</param>
    /// <returns>최종 그룹 아이디</returns>
    public int GetGroupId(int _curVisitedId = 0) {

        if (!gameObject.activeSelf || (_curVisitedId & myId) != 0)//대상 아닌경우 || 이미 확인한 경우
            return _curVisitedId;

        _curVisitedId += myId;

        foreach (Node node in nodes) {

            _curVisitedId = node.GetGroupId(_curVisitedId);

        }

        return _curVisitedId;

    }

    //이 친구로 바꾸거라.
    public void SetGroupId_N_SetNewModelTransforms(int _newGroupId, ModelPosFitter _modelInfo) {

        if (!gameObject.activeSelf || _newGroupId == myGroupId)//이미 방문함 or 갱신필요x.
            return;

        //이전 모델이 있으면 비활성화
        ModelPosFitter tmp = graphManager.GetModel(myGroupId);

        if (tmp.gameObject.activeSelf) {

            tmp.RemoveAllTransforms();
            tmp.gameObject.SetActive(false);

        }

        myGroupId = _newGroupId;
        _modelInfo.AddTransform(transform);

        foreach (Node node in nodes) {

            node.SetGroupId_N_SetNewModelTransforms(_newGroupId, _modelInfo);

        }

    }


}
