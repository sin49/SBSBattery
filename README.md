BattaBatta (2D/3D 하이브리드 액션 플랫포머)

 개요
2D와 3D 시점을 넘나드는 기믹이 특징인 하이브리드 플랫포머 게임으로, 2024 지스타(G-Star)에 출품되었으며 모바일 플랫폼으로 정식 출시된 프로젝트입니다
메인 클라이언트 프로그래머로서 게임의 코어 아키텍처 설계와 최적화를 주도했으며, 기획 및 아트 직군과의 원활한 협업을 위해 유니티 Custom Editor와 Scriptable Object를 활용한 자체 데이터 파이프라인을 구축했습니다

 주요 기술 
* Engine/Language: Unity, C#
* Architecture: FSM(Finite State Machine), Observer Pattern (Event-Driven)
* Optimization: Coroutine, Profiler
* Tooling & Pipeline: Custom Editor, Scriptable Object, Cinemachine, Post-Processing, Git



| 구분 | 파일명 (링크) | 담당 역할 및 핵심 기술 |
| :--- | :--- | :--- |
| **물리/충돌 보정** | [`Platform2DFixer.cs`](./Assets/ScriptsFolder) | 3D에서 2D 시점 전환 시 Z축 강제 스냅(Z-Lock)을 통한 물리 충돌 엇갈림 방어 로직 |
| **상태 & 이벤트** | [`PlayerHandler.cs`](./Assets/ScriptsFolder/Character) | Action 델리게이트와 FSM을 결합하여 플레이어 이동 상태(2D/3D) 및 변신 폼 통제 |
| **AI / 아키텍처** | [`EnemyAction.cs`](./Assets/ScriptsFolder) | 추상 클래스(`abstract`)와 이벤트를 활용해 Update 의존도를 없앤 AI 행동 제어 시스템 |
| **협업 툴 (에디터)** | [`shmCreaterEditor.cs`](./Assets/editor) | 기획/아트 직군이 프로그래머 없이 포스트 프로세싱을 제어할 수 있는 Custom Editor 제작 |

....................................
