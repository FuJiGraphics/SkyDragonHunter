# Sky Dragon Hunter - 2D Idle Game

Sky Dragon Hunter는 2D 방치형 RPG 게임으로, 자동 전투를 기반으로 한 <br>
캐릭터 성장, 스킬 조합, 마스터리 트리 시스템을 제공합니다.  <br>
게임 클라이언트는 Unity로 개발되었으며, 리소스는 원격 서버 AWS S3와 연동되어 있습니다.<br><br>

## 링크
- 안드로이드 출시 페이지: https://play.google.com/store/apps/details?id=com.Kyungil.SkyDragonHunter

## 담당 역할
- 전투 시스템 및 캐릭터 성장 시스템 구축
- Addressable을 통한 리소스 관리 및 리소스 비동기 원격 다운로드 구축 (Amazon S3 활용)
- 마스터리, 아이템, 인벤토리, 보물 시스템 구축

## 주요 기능

- 자동 전투 및 능력치 성장 시스템
- 스킬 조합 및 마스터리 트리
- 원격 서버 연동 (AWS S3 기반)
- ScriptableObject + CSV 기반 데이터 설계
- Spine 애니메이션 연동
- Addressables 기반 리소스 관리
- 기획/디자이너를 위한 커스텀 Unity 에디터 툴 제공

## 기술 스택

- Unity 2022 LTS
- C#
- Spine
- TextMeshPro
- CSV Helper
- AWS S3
- Addressable
