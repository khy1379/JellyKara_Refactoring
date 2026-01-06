# JellyKara_Refactoring
Refactoring assignment

# 프로젝트 리팩토링 
기존 프로젝트 중 1 택하여, 일부 영역에 대해 디자인 패턴을 적용하여 Refactoring 을 수행하는 과제용 Repository 입니다.
리팩토링 과제 목적 : 기존 프로젝트 진행 시점보다 더 많이 알게 된 현재, 어떻게 수정할 수 있는지 여부를 확인

## 대상 프로젝트
- JellyKara (JetKara Custom)

### 간단 게임 소개
- Jelly가 날아다니면서 기둥 사이를 통과하며 점수를 얻는 게임
  - 기존 JetKara project를 Custom 하는 과제로 제출한 프로젝트

### 특징
- Jelly 변경 : 메인 화면에서 Jelly를 직접 변경하거나, 게임 중간에 Jelly가 변경되는 구간이 나옴
- Jelly 별 특징
  - 1. Slime Jelly : Key 입력 시마다 위로 점프 (기존 Kara와 동일)
  - 2. Bear Jelly : Key 입력 중 상승, 입력하지 않는 중 하강
  - 3. Earth Jelly : Key 입력 시마다 중력 반전

# 리팩토링 영역 1 
- 게임 상 Key Input 시 Jelly 움직임
  - 변경 Script : JellyKara, PlayerTypeChanger

## 선정 이유
- 기존 Key Input 시 Jelly 움직임 로직은 JellyKara (게임 상 Player) Script에서 작성하였음
  - Update 함수 한 곳에 모아놔서 가독성이 떨어짐

## 리팩토링에 사용한 디자인 패턴
- 전략 패턴

## 리팩토링 내역
  1. PlayerTypeChanger Script에 전략패턴 용 class 생성
  -  JellyStrategy class 
  2. JellyStrategy를 상속받는 Jelly 별 Strategy class 생성
  - Slime/Bear/EartuStrategy
  3. JellyKara class의 Jelly움직임 영역을 Jelly 별 Strategy class로 옮김
  - 기존 움직임에 사용한 rigidbody, speed 등은 PlayerTypeChanger의 (static)JellyKara 멤버변수를 받아서 사용
  - 기존 Key Input 시점에 바로 움직임이 변동되는 경우를 bool 변수를 통해 변경 가능 여부를 확인 후 움직이도록 변경
    - 기존 방식대로 진행할 시 동작하지 못하는 경우가 존재하기 때문 : Bear
  4. JellyStrategy의 각 객체 및 현재 JellyStrategy들을 PlayerTypeChanger class에서 static 멤버변수로 할당
  - strategyArr, curStrategy
  5. JellyStrategy에서 사용할 PlayerTypeChanger의 JellyKara 변수에, JellyKara class에서 Awake, OnDestroy 시점에 this 객체를 주입 및 제거
  6. JellyKara class에서 PlayerTypeChanger의 static curStrategy 변수를 통해 Jelly 움직임 구현

# 리팩토링 영역 2 
- 특정 class에 다수 존재하는 static 변수
  - 변경 Script : PlayerTypeChange, JellyKara, JellySpriteControl 외 다수
    - 위 3개 명시 script를 제외하면 타입 확인용으로만 사용하긴 함

## 선정 이유
- 외부 script 파일과 연동 시킬 방법이 public static 변수 및 함수 외에는 생각나지 않아서 static 변수를 많이 사용
- 또한 class가 신경 쓰지 않아도 되는 class에서도 가져다 쓰게 되어 해당 class에 대한 의존성이 매우 깊어짐
- 해당 class가 Singleton 등의 1개만 존재하며 공유하는 것이 중요한 class라면 몰라도, 특정 영역에서만 사용되어 굳이 static 변수를 사용하여 사용 가능한 메모리 양을 줄이고, 의존성을 깊게 가지게 만드는 것은 좋지 못한 습관이라 판단되어 리팩토링

## 리팩토링에 사용한 디자인 패턴
- 옵저버 패턴
  - interface 및 interface의 함수를 사용하는 class 형식의 observer 패턴
- 싱글톤 패턴의 일부(public static 객체)

## 리팩토링 내역
  1. observer 패턴 용 interface 및 class 생성
    - interface : IKaraEventable, IPlayerTypeChangeable
    - class : KaraEventObserver, TypeChagngeObserver
  2. 옵저버 대상 class에 interface 상속 
    - IKaraEventable : PlayerTypeChanger, JellySpriteControl
    - IPlayerTypeChangeable : JellySpriteControl
  3. JellyKara를 싱글톤 패턴 처럼, public static JellyKara 객체를 선언
    - 외부에서 JellyKara에 접근할 수 있도록 하기 위함 -> 이를 이용해 4번 observer 대상 등록
  4. observer를 사용할 class에 observer 객체 선언 및 Init/Awake 시점에 observer 등록
    - KaraEventObserver 객체 선언 : JellyKara
    - KaraEventObserver 대상 등록 : PlayerTypeChanger, JellySpriteControl
    - TypeChagngeObserver 객체 선언 : PlayerTypeChanger
    - TypeChagngeObserver 대상 등록 : JellySpriteControl
  5. PlayerTypeChanger class에서 위 리팩토링 내역 중 JellyKara 객체 사용을 3번의 JellyKara 객체를 이용하도록 변경
  6. PlayerTypeChanger class에 있었던 static 변수/함수를 non-static 변수/함수로 변경
  7. interface를 통해 추가된 함수에, 각 시점마다 실행할 함수 등록
  8. Observer class 객체를 선언한 class에서 필요한 시점에 observer 함수 실행
  9. 그 외 영역에서 PlayerTypeChanger 를 통해 타입을 확인하던 내용을 GetInt() 를 통해 확인하도록 변경

# 리팩토링 영역 3
- object의 생성 및 파괴
  - 변경 Script : GameManager, ObjectMove, JellyKara
 
## 선정 이유
- 게임 볼륨이 크지 않다고는 하나, 게임을 오래 플레이할 경우 object의 생성과 파괴가 많이 발생하게 됨
- 생성, 파괴보다 오브젝트 풀링 을 통해 관리하는 것이 좋다고 판단되어 리팩토링

## 리팩토링에 사용한 디자인 패턴
- 오브젝트 풀링

## 리팩토링 내역
  1. GameManager class에서 Queue<GameObject> 선언
    - 오브젝트 풀링 용 변수
  2. 1번의 Queue에 object를 Enqueue, Dequeue 시킬 함수 생성
    - EnqueueScoreObjects, DequeueScoreObjects
  3. ObjectMove class에서 Score collider를 등록할 변수 선언 및 해당 함수의 SetActive 상태를 지정할 수 있는 함수, GameManager 객체를 주입받는 함수 생성
    - ScoreObjectSet, GameManagerSet
    - 이 프로젝트에서 GameManager는 싱글톤 객체가 아니기 때문에, 주입받아야 접근 가능
  5. ObjectMove class에서 OnDisble 시점에 Score collider 활성화 및 1번의 Queue에 Enqueue 되도록 지정
  6. ObjectMove, JellyKara class에서 기존 Object 및 Score 등이 Destroy 되는 내용 -> SetActive(false)를 통해 비활성화 되도록 변경
  7. GameManager class에서 object가 queue에 없을 경우 object 신규 생성, 그 외에는 queue에서 dequeue 하여 사용하도록 설정

# 아쉬운 점
- 다른 영역들에서도 아쉬운 점이 많이 보임
  - GameManager : 싱글톤으로 변경 가능하나, 오래 걸릴 것으로 예상되어 넘김
  - Type 체크 : Player나 GameManager 등을 통하지 않고 PlayerPrefs.GetInt를 통해 확인하도록 되어있으나, 오래 걸릴 것으로 예상되어 넘김 
