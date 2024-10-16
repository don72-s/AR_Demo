# CardCrafter AR
**AR을 활용한 카드 조합서 프로젝트**

Unity AR Foundation을 이용한 
Collectable Card Craft 방식의 카드조합 프로그램

<br>

![viw](https://github.com/user-attachments/assets/212edd17-dc7d-49e5-a85c-8373a5de6ef5)


<hr>

### [프로젝트 기술 문서](https://github.com/don72-s/AR_Demo/blob/main/Docs/description.md)

<hr>

### [시연 영상](https://www.youtube.com/watch?v=4dn4l6Rsqak)

<hr>
<br>

### 트래킹 이미지.

기본 이미지는 유희왕 카드게임의 **토이 솔저, 토이 탱크, 토이 박스** 세 종류의 카드를 인식.

트래킹 이미지의 변경은 `Assets > AR > RIL`의 속성을 추가하거나 변경.
이미지 변경 후 같은 디렉토리의 **ImgNameMapper** SO파일에 추가/변경된 이미지 명칭과 타입을 매칭.

이미지는 30종류까지 추가 가능.

<br>

### 대응 모델 추가.

`Assets > SO > ModelDataSet`에 **Models DataSet** 타입의 SO를 수정.

조합에 들어가는 요소의 갯수별로 SO를 다음과 같은 방식으로 작성.

![so_2](https://github.com/user-attachments/assets/36d14401-a573-4f92-bb86-cb8374d07f46)

2종류의 카드가 조합되면 출력될 모델을 지정.

ex) TYPE1, TYPE2가 조합된 경우 **12_SolderTank**에 대응되는 모델을 출력.

집어넣는 모델 오브젝트는 **Model Pos Fitter**스크립트 상속이 필요.


