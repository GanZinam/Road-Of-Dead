/**
 * 오직 유저들 정보만을 위한 클래스
 *   -! 오브젝트로 만들어 놓지 않고 사용
 */
public static class PlayerInfo 
{
    // 기본 정보
    public static int hp = 1;
    public static int dmg = 1;
    public static byte carType = 0;

    public static int map = 1;

    public static ITEM item0 = new ITEM(0, 0);
    public static ITEM item1 = new ITEM(0, 0);

    // 맵 선택에 따른 정보
    public static int loadNum = 0; // 지나갈 거리 번호

    public static int[] quest = new int[3] { 0, 0, 0 };   // 퀘스트 타입
}
