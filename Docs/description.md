# 프로젝트 문서

프로젝트의 전체적인 구조와 설명이 작성된 문서입니다.  

<br>

# 개요

**●Unity AR Foundation패키지를 이용한다.**

**●AR Foundation의 이미지 트래킹을 기반으로 한 모델 출력 프로그램을 제작한다.**

**●카드들이 가까워지면 서로 조합되며, 조합법에 대응되는 모델을 대신 출력한다.**

<br>

# 목차

<h3>

### [1. 이미지 트래킹](#이미지트래킹)
### [2. 조합 로직](#조합로직)
### [3. 모델 출력](#모델출력)
### [4. 데이터 관리](#데이터관리)

</h3>

<br>

# 이미지트래킹

기본적인 이미지 트래킹에 대해 기술한다.

<br>

## 여러 종류의 이미지 트래킹.

Unity AR Foundation의 기본 제공 컴포넌트인 **AR Tracked Image Manager**는 한가지 종류의 이미지 트래킹만을 제공한다.

여러 종류의 이미지 트래킹을 동시에 실현시키기 위하여 **Image Tracker**스크립트를 작성하여 이미지 트래킹 이벤트를 받아서 대응하도록 한다.

```cs
ARTrackedImageManager imageManager; //이미지 트래킹 메니져를 받아 참조.

//이미지 트래킹 변화 이벤트를 구독/해제
private void OnEnable() { imageManager.trackedImagesChanged += Changed; }
private void OnDisable() { imageManager.trackedImagesChanged -= Changed; }

void Changed(ARTrackedImagesChangedEventArgs _args) {

    foreach (ARTrackedImage _img in _args.added) { 모델 출력 처리 }        //등록된 이미지가 최초 실행되었을 때 호출
    foreach (ARTrackedImage _img in _args.updated) { 모델 이동, 회전 처리}  //등록된 이미지가 트래킹중일때 호출
    foreach (ARTrackedImage _img in _args.removed) {}                     //트래킹중인 이미지가 트래킹 종료되었을 때 호출

}
```

위와같이 foreach문의 내부에 대응하는 코드를 작성하여 다중 이미지 트래킹이 가능하도록 만든다.

<br>

## 이미지 등록.

<br>

### 이미지와 이름의 등록.

이벤트는 등록되어있는 이미지가 트래킹되었을 때 발생한다고 했다. 따라서 이미지를 등록하는 과정이 필요하다.

1. `Create > XR > ReferenceImageLibrary`를 선택하여 이미지 라이브러리 파일을 생성한다.
2. 이미지와 이름, 크기를 지정한다.
   
![9](https://github.com/user-attachments/assets/c67c53b2-4968-46ac-9c37-ef3eea28224b)

### 이미지와 이름의 관리.

효율적인 데이터 관리를 위하여 간단한 SO파일을 작성한다.

```CS
    [Serializable]
    public struct ImgNameData {

        public string imgName;
        public DataType dataType;

    }

    public ImgNameData[] imgNameDatas;
```

단순히 string과 DataType(enum)을 묶어 관리하는 SO파일을 생성하여 관리한다.

![10](https://github.com/user-attachments/assets/540761ee-81a2-496c-9a5e-732bb9af4842)


<br>

<hr>

<br>

# 조합로직

기본 카드들의 조합법을 관리하기 위한 조합 로직에 대해 기술한다.

조합법의 관리의 원리는 **그래프**를 기반으로 하며, 각 노드는 종류별로 1개를 넘기지 않는다.

<br>

## 이미지 트래킹의 맹점

여러 종류의 이미지 트래킹은 가능하지만, 같은 이미지가 여러개 있다고 해서 두곳 모두 모델을 띄울수는 없다.

이는 순차적으로 트래킹이 이루어지기 때문에 두개의 같은 이미지가 존재한다고 인식하는것이 아닌
하나의 이미지가 왔다갔다하는 일종의 **깜빡이 현상**으로 인식하기 때문이다.

따라서 여러종류의 이미지가 존재하더라도 각 종류별로 트래킹되는 이미지의 최대 갯수는 **1개**이며, 이는 각 카드들이
**유일한 ID**를 가질 수 있음을 의미한다.

<br>

## 조합의 방대함

3개의 카드가 존재한다고 하면 조합할 수 있는 모든 경우의 수는 7가지(1, 2, 3, 12, 13, 23, 123)가 된다.
적은 숫자라고 한다면 분기문으로 해결할 수 있는 정도이지만 10개만 되더라도 **2^10 - 1**가지의 경우의 수를 판단해야 한다.

따라서 이러한 조합법들을 구분하고 식별할 수 있는 로직을 구상할 필요가 있다.

<br>

## 유일성과 2진수

이미지트래킹의 특성상 카드 한장한장은 유니크하고, 카드들은 조합되는 특징을 가진다.

위와 같은 특징을 이용하여 각 데이터를 **2진수로 관리하기로 한다.**

<br>

3개의 카드 A, B, C가 있다고 가정하고, 다음과 같은 ID를 부여한다.

**A : (0b) 100**

**B : (0b) 010**

**C : (0b) 001**

모든 진수에서 비트가 **1개**만 활성화되어있는 수를 **Unique ID**로 정의한다.

<br>

Unique ID를 가지는 요소들이 하나만 존재할 수 있다면, 해당 요소들의 모든 조합으로 나타나는 id 또한 **반드시 Unique**한 특성을 가지게 된다.

논리적으로 보아도 n^2의 경우의 수를 나타낼 수 있으므로 **현재 조합 상태를 2진수**로 관리한다면 효율적으로 데이터를 다룰 수 있다.

<br>

## 결합과 분리

구성요소들을 조합하게 된다면, 해당 요소들을 하나로 묶어서 관리하는것이 가능하다.

해당 그룹을 이루는 카드들의 **Unique ID**모두 더하여 대표하는 그룹 ID로 취급할 수 있게 된다.

![456](https://github.com/user-attachments/assets/be8193a3-4fdc-4d38-a024-a98358760cca)

<br>

**하지만 그룹이 해체될때에는 조금 더 신중한 ID 갱신이 필요하다.**


분리된 상대 그룹의 GroupId를 알아야 하는데, 이를 전달받아야 하기 때문이다.

심지어 분리되는 현상은 일어났지만, 그룹ID는 그대로인 상황도 충분히 존재할 수 있다.

![457](https://github.com/user-attachments/assets/f24fc399-6900-4cc8-b5a3-078f94b48a1a)

(분리현상이 일어났음에도 GID가 변하지 않는 예시)

<br>

따라서 이상적인 구조 설계는 다음과 같다.

**결합 : 결합이 일어난 두 노드를 연결하고, 결합된 GID를 재계산.**

**분리 : 분리가 일어난 두 노드가 각자 자신이 소속된 그룹의 GID를 재계산.**

위와 같은 설계를 가진다면 분리가 일어나는 모든 상황에 대해서 오류없이 GID를 계산해낼 수 있다.

<br>

## 노드와 그래프

위의 예시에서 보이듯이, 해당 로직에는 그래프 데이터구조를 이용할 수 있다.

따라서 그래프 구조를 만들기 위하여 노드를 정의하기로 한다.

### 노드의 데이터

각 노드는 **Unique ID**를 가지며, 본인이 현재 속해있는 **Group ID**도 가질 필요가 있다.

또한, 인접 노드 리스트 그래프로 구현하기 위하여 접한 노드들의 리스트도 가진다.

```cs

public class Node : MonoBehaviour {

    [SerializeField]
    [Range(1, 31)]
    int myId = 0;
    int myGroupId = 0;
    List<Node> nodes;

    private void Awake() {

        nodes = new List<Node>();

        if (myId != 0) {
            //2진방식으로 관리를 위한 시프트
            myId = 1 << (myId - 1);
            myGroupId = myId;
        }

    }

    //다른 노드와 연결
    public void ConnectNode(Node _node) {

      if (nodes.Contains(_node))
          return;

      nodes.Add(_node);

   }

    //다른 노드와 연결 해제
    public void DisconnectNode(Node _node) {

        if (nodes.Contains(_node)) {

            nodes.Remove(_node);

        }

    }

}
```

기본적인 그래프의 노드 구조를 작성했다.


**다음으로는 본인이 속한 그룹의 ID를 구해오는 함수와 본인이 속한 그룹의 ID를 재설정하는 함수를 작성한다.**


```cs
    //현재 그룹의 GID를 가져옴
    public int GetGroupId(int _curVisitedId = 0) {

        if (!gameObject.activeSelf || (_curVisitedId & myId) != 0)//대상 아닌경우 || 이미 확인한 경우
            return _curVisitedId;

        _curVisitedId += myId;

        foreach (Node node in nodes) {

            _curVisitedId = node.GetGroupId(_curVisitedId);

        }

        return _curVisitedId;

    }


    //해당 노드가 속한 그룹의 GID를 전달받은 GID로 갱신
    public void SetGroupId(int _newGroupId) {

      if (!gameObject.activeSelf || _newGroupId == myGroupId)//이미 방문함 or 갱신필요x.
          return;

      myGroupId = _newGroupId;

      foreach (Node node in nodes) {

          node.SetGroupId(_newGroupId, _modelInfo);

      }

  }

```
다른 노드들을 깊이우선방식으로 순회하며 본인 그룹의 모든 노드를 순회하게 된다.

<br>


**마지막으로 노드간의 간선을 연결/해제하는 경우를 구현한다.**


<br>

우선, 노드간의 간선은 카드들의 물리적으로 접한상태, 떨어진 상태를 의미한다.

따라서 간선을 수정하고 그룹 ID를 재계산하는 순간은 **충돌을 시작했을 때, 충돌이 종료될 때** 두가지 순간이 된다.

그러므로 충돌체를 이용하여 다음과 같은 방식으로 간선 수정 코드를 작성한다.

```cs
    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.layer == LayerMask.NameToLayer("NodeObj")) {

            Node node = other.gameObject.GetComponent<Node>();

            ConnectNode(node);
            node.ConnectNode(this);

            int newGroupId = GetGroupId();

            if (newGroupId != myGroupId) { //노드간의 결합이므로 한쪽만 GID를 갱신해도 충분.

                SetGroupId(newGroupId, tmp);

            }

        }

    }

    private void OnTriggerExit(Collider other) {

        if (other.gameObject.layer == LayerMask.NameToLayer("NodeObj")) {


            Node node = other.gameObject.GetComponent<Node>();

            DisconnectNode(node);
            node.DisconnectNode(this);

            int newGroupId = GetGroupId();

            if (myGroupId != newGroupId) { //본인의 GID 재계산

                SetGroupId(newGroupId, tmp);

            }


            newGroupId = node.GetGroupId();

            if (node.myGroupId != newGroupId) { //연결을 해제한 노드 그룹의 GID 재계산산

                node.SetGroupId(newGroupId, tmp);

            }

        }

    }
```

이렇게 해서 ID를 기반으로 그룹화하는 그래프 자료구조 작성이 완료되었다.

<br>
<hr>
<br>

# 모델출력

자연스러운 모델 출력을 위한 로직에 대하여 기술한다.

<br>

## 모델의 속성

4장의 카드를 조합한다고 생각해보자.

<br>

4장을 일자로 연결한다면 모델은 중앙에 띄워주는것이 자연스러울 것이다.

4장을 ㅁ자로 연결한다면 가운데에 모델을 띄워주는것이 자연스러울 것이다.

<br>

즉, 구성요소가 되는 카드(노드)들의 무게중심의 역할을 하는 위치에 모델을 띄워야 할 것이며

카드의 실시간 위치변화에도 대응해야 한다.

같은 원리로 모델의 회전값도 평균값을 가져야 한다.

<br>

따라서 모델에 부착시킬 스크립트를 다음과 같이 작성하도록 한다.

```cs
List<Transform> posList = new List<Transform>();

Vector3 pos;
Quaternion rot;

//자율적으로 위치와 회전값을 갱신
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


//구성요소가 되는 카드의 Transform 추가
public void AddTransform(Transform _transform) { 

   posList.Add(_transform);

}

//현재 등록된 모든 Transform 제거
public void RemoveAllTransforms() {

   posList.Clear();

}

```

이전에 구현한 노드 스크립트에서 Group ID가 갱신될때마다 활성화된 모델을 비활성화시킨 뒤

목표 ID에 해당하는 모델을 가져와 GID를 갱신하는 시기에 Transform을 등록해주면 된다.

이후로는 활성화된 모델의 Update문 안에서 자율적으로 평균 위치를 계산하게 될 것이다.

<br>
<hr>
<br>

# 데이터관리

기하급수적으로 늘어나는 모델들의 데이터를 효율적으로 관리하기 위한 방식을 기술한다.

<br>

## 확장성과 복잡도

지금은 3개밖에 없는데도 7가지의 경우의수가 나왔다.

추후 조합의 확장을 위해서라도 체계적인 데이터 정리가 필요하다.

<br>

조합에 들어간 총 카드의 갯수를 기준으로 SO파일을 나눌 수 있도록 코드를 작성한다.

```cs
public class ModelDataSO : ScriptableObject {

    [Serializable]
    public struct CombineInfo {

        //조합을 구성하는 타입들
        [SerializeField]
        public DataType[] datas;
        //해당 조합의 출력 모델
        [SerializeField]
        public ModelPosFitter modelPrefab;

    }

    //조합 레시피의 리스트
    [SerializeField]
    public CombineInfo[] modelsInfo;

}
```

<br>

3가지 종류의 카드가 있다고 가정하면 다음과 같이 3개의 파일로 나누어서 정리할 수 있다.

![994](https://github.com/user-attachments/assets/7ee901ec-b2ed-43fb-8fcb-beea42237a40)

<br>

## 모델 데이터베이스의 초기화

데이터를 분산시켜 저장했으니 효율적으로 참조할 수 있도록 준비하는 작업이 필요하다.

따라서 모델 데이터를 관리하고 ID를 전달받아 대응하는 모델을 반환해주는 기능을 구현할 필요가 있다.

```cs
//데이터셋들의 배열
ModelDataSO[] modelDataSetSO;

void InitDictionary() {

    objPrefabs = new ModelPosFitter[(1 << 카드 종류의 수)];

    foreach (ModelDataSO _modelSO in modelDataSetSO) {

        foreach (ModelDataSO.CombineInfo _data in _modelSO.modelsInfo) {

            int idx = 0;

            //해당 조합법의 id를 구함.
            foreach (DataType _type in _data.datas) {

                idx += (1 << ((int)_type - 1));

            }

            //대응되는 배열 인덱스 위치에 대응하는 모델을 저장.
            objPrefabs[idx] = _data.modelPrefab;

        }

    }

}
```

다음과 같이 작성하면 아래와 같이 인스펙터에서 데이터를 관리할 수 있으며

objPrefabs 배열에 ID를 인덱스로 하는 요소를 반환하게 만들면 모델 데이터베이스가 완성된다.

![qe](https://github.com/user-attachments/assets/cb40afa5-e52f-4440-bfc9-a102628839d3)

<br>






