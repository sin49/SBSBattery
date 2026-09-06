배터리 프로젝트

김규태 포트폴리오....


| 구분 | 파일명 (링크) | 담당 역할 및 핵심 기술 |
| :--- | :--- | :--- |
| **물리/충돌 보정** | [`Platform2DFixer.cs`](./Assets/ScriptsFolder) | 3D에서 2D 시점 전환 시 Z축 강제 스냅(Z-Lock)을 통한 물리 충돌 엇갈림 방어 로직 |
| **상태 & 이벤트** | [`PlayerHandler.cs`](./Assets/ScriptsFolder/Character) | Action 델리게이트와 FSM을 결합하여 플레이어 이동 상태(2D/3D) 및 변신 폼 통제 |
| **AI / 아키텍처** | [`EnemyAction.cs`](./Assets/ScriptsFolder) | 추상 클래스(`abstract`)와 이벤트를 활용해 Update 의존도를 없앤 AI 행동 제어 시스템 |
| **협업 툴 (에디터)** | [`shmCreaterEditor.cs`](./Assets/editor) | 기획/아트 직군이 프로그래머 없이 포스트 프로세싱을 제어할 수 있는 Custom Editor 제작 |

....................................
