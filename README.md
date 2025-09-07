<img width="1258" alt="Image" src="https://github.com/user-attachments/assets/6fd1f76b-775d-4923-9df8-08f78b863515" />

# Re-Call
**시간 루프 기반 서바이벌 퍼즐 어드벤처 게임**

## 📖 게임 개요
Re-Call은 Unity 3D 엔진으로 개발된 시간 루프 메커니즘을 가진 서바이벌 퍼즐 게임입니다. 플레이어는 24시간이라는 제한된 시간 안에서 반복되는 루프를 통해 퍼즐을 해결하고 생존해야 합니다.

<img width="1268" alt="Image" src="https://github.com/user-attachments/assets/00e3bf1e-8f2d-4f74-ab3d-d899adc402d9" />

<img width="1266" alt="Image" src="https://github.com/user-attachments/assets/a1a4f4de-7ca4-4e90-bab1-bd483802363a" />

## 🛠️ 개발 환경
- **엔진**: Unity 2022.3.17f1
- **언어**: C#
- **렌더링**: Universal Render Pipeline (URP)
- **플랫폼**: PC (Windows)

### 주요 의존성

#### Unity 공식 패키지
- **Universal Render Pipeline (URP)**: 렌더링 파이프라인 (14.0.9)
- **TextMeshPro**: UI 텍스트 렌더링 (3.0.6)
- **Input System**: 입력 시스템 (1.7.0)
- **AI Navigation**: NavMesh 기반 AI 내비게이션 (1.1.6)

## 개발 기간
- **5/26 ~ 6/1**

## 🏃‍♂️ 빌드 및 실행
1. Unity Hub에서 Unity 2022.3 LTS 설치
2. 프로젝트 폴더를 Unity Hub에서 열기
4. Build and Run으로 게임 실행

## 🎮 게임 조작법

- **WASD**: 이동
- **마우스**: 시점 조작
- **E**: 상호작용
- **Tab**: 인벤토리 열기
- **Q**: 장비 해제
- **마우스 좌클릭**: 아이템 사용/공격
- **마우스 우클릭**: 건축물 배치

## 🎮 핵심 게임플레이 특징

### ⚙️ 기본 기능 시스템
- **플레이어 이동**: WASD 키를 이용한 3D 공간 이동
- **상호작용**: E 키를 통한 오브젝트 및 NPC와의 상호작용
- **인벤토리 관리**: I 키로 인벤토리 열기/닫기
- **생존 시스템**: 아이템 수집 및 사용으로 배고픔 상태 관리
- **퍼즐 시스템**: 오브젝트와의 상호작용을 통한 퍼즐 해결

### 🔄 시간 루프 시스템
- **24시간 타이머**: 실시간으로 흘러가는 24시간 게임 내 시간 (인게임에서는 시간 가속화)
- **루프 메커니즘**: 시간이 끝나거나 사망 시 자동으로 루프 재시작
- **루프 저장**: 특정 지역에 건축 구조물과 아이템을 두면 다음 루프에서도 유지
- **시간 기반 이벤트**: 특정 시간대에 달라지는 이벤트 (NPC 대화 내용이 시간대별로 변화)

### 🎒 아이템 & 인벤토리 시스템
- **다양한 아이템 타입**: 무기, 방어구, 소모품, 자원 아이템
- **스택 가능한 인벤토리**: 동일 아이템의 수량 관리
- **제작 시스템**: 재료를 조합하여 새로운 아이템 제작
- **건설 시스템**: 배치 가능한 오브젝트로 환경 구성

### 🏗️ 제작 & 건설
- **크래프팅 테이블**: 복잡한 아이템 제작을 위한 전용 시설
- **레시피 시스템**: 필요 재료와 수량을 기반으로 한 제작
- **건설 시스템**: 3D 공간에서의 오브젝트 배치 및 미리보기

### ⚔️ 전투 시스템
- **기본 적 AI**: NavMesh 기반의 추적, 공격, 순찰 AI, 특정 시야 내 적 감지, 처치 시 설정된 아이템 드롭
- **TeleportEnemy**: 플레이어 감지 시 텔레포트로 기습 공격하는 특수 적
- **InvisibleEnemy**: 일정 시간마다 투명화되어 은신하는 고위험 적

### 🏆 업적 시스템
업적은 6가지 카테고리로 분류됩니다:
- **Hunt**: 적 처치 관련 업적
- **Floor**: 층별 진행 관련 업적
- **Item**: 아이템 수집 관련 업적
- **Key**: 주요 스토리 진행 업적
- **Feature**: 게임 기능 사용 관련 업적
- **CheckPoint**: 특정 지점 도달 업적

## 🎯 게임 진행 방식

1. **탐색**: 환경을 돌아다니며 아이템과 단서 수집
2. **퍼즐 해결**: 다양한 퍼즐을 통해 새로운 영역 해금
3. **제작**: 수집한 재료로 생존에 필요한 도구 제작
4. **건설**: 환경을 개선하거나 퍼즐 해결을 위한 구조물 건설
5. **루프 활용**: 시간 루프를 전략적으로 활용하여 진행

## 🏗️ 프로젝트 구조

### 📁 주요 폴더 구조
```
Assets/
├── Scripts/
│   ├── Manager/           # 게임 매니저들 (GameManager, ArchiveManager 등)
│   ├── Entity/
│   │   ├── Player/        # 플레이어 관련 (Movement, Inventory, Condition)
│   │   ├── Enemy/         # 적 시스템
│   │   ├── NPC/           # NPC 시스템
│   │   └── Interaction/   # 상호작용 시스템
│   ├── Item/              # 아이템 시스템 (Crafting, Building, Database)
│   ├── Puzzle/            # 퍼즐 시스템
│   ├── UI/                # 사용자 인터페이스
│   ├── Data/              # 데이터 관리 (Save/Load, ScriptableObjects)
│   ├── Sound/             # 사운드 시스템
│   └── Util/              # 유틸리티 클래스들
├── Scenes/                # 게임 씬들
├── Prefab/                # 프리팹 에셋들
├── Material/              # 머티리얼 에셋들
└── Sound/                 # 사운드 에셋들
```

### 🔧 디자인 패턴
- **Singleton Pattern** - 매니저 클래스들 (GameManager, SoundManager, ItemDatabase)
- **EventBus Pattern** - 타입 안전한 전역 이벤트 시스템으로 컴포넌트 간 느슨한 결합 구현
- **Observer Pattern** - 이벤트 시스템
- **Factory Pattern** - 아이템 생성 및 관리
- **ScriptableObject Pattern** - 데이터 관리 및 설정

### 🔧 핵심 시스템

#### 🎮 게임 관리
- `GameManager`, `Timer`: 게임 상태 관리, 시간 루프 제어, 24시간 타이머
- `ArchiveManager`: 업적 시스템 관리

#### 👤 플레이어 시스템
- `PlayerController`: 입력 처리 및 전체 제어
- `PlayerMovement`: 3D 캐릭터 이동 (이동, 점프, 달리기 등)
- `PlayerCondition`, `PlayerStat`: 체력/허기/스태미나 관리 및 스탯 계산
- `PlayerInventory`, `Equipment`: 아이템 관리 및 장비 착용 시스템

#### 🎒 아이템 & 제작
- `ItemDatabase`, `ItemDataSO`: 아이템 데이터베이스 및 ScriptableObject 관리
- `CraftingSystem`, `BuildSystem`: 레시피 제작 및 3D 건축 시스템
- `RecipeBook`, `DroppedItemManager`: 레시피 학습 및 월드 아이템 관리

#### 🤖 AI & 상호작용
- `EnemyController`, 특수적(`TeleportEnemy`, `InvisibleEnemy`): NavMesh 기반 AI 시스템
- `NPCController`, `DialogHandler`: NPC 대화 및 상호작용 시스템
- `IInteractable`: 상호작용 인터페이스

#### 🎵 UI & 오디오
- `UIManager`, `UIInventory`, `ConditionHandler`: UI 시스템 관리 및 주요 컴포넌트
- `SoundManager`: 사운드 효과 적용
- `EventBus`, `Singleton<T>`: 전역 이벤트 시스템 및 패턴 기반 클래스

  <img width="1237" alt="Image" src="https://github.com/user-attachments/assets/9bc1e248-ba93-4459-b7b7-96c99110e6c3" />
