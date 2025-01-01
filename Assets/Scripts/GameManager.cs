using TMPro;  // TextMeshPro 사용을 위한 네임스페이스 추가
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public GameObject[] Stages;

    // TextMeshProUGUI로 변경 (TextMeshPro 텍스트 컴포넌트 사용)
    public TextMeshProUGUI gameOverText;  // TextMeshPro 텍스트 필드로 변경

    public GameObject gameOverPanel;  // Game Over 화면

    void Start()
    {
        // 초기화: Game Over 화면 숨기기
        gameOverPanel.SetActive(false);
        gameOverText.gameObject.SetActive(false);
    }

    public void NextStage()
    {
        // Change Stage
        if (stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();
        }
        else
        { // Game Clear
            // Player Control Lock
            Time.timeScale = 0;
            // Result UI
            Debug.Log("게임 클리어!");
            // Restart Button UI (추가적으로 구현 필요)
        }

        // Calculate Point
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void HealthDown()
    {
        if (health > 1)
            health--;
        else
        {
            // Player Die Effect
            player.OnDie();

            // Result UI
            Debug.Log("죽었습니다!");

            TriggerGameOver();  // Game Over 화면 표시 함수 호출
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (health > 1)
            {
                PlayerReposition();

                HealthDown();
            }
            else
            {
                HealthDown();
            }
        }
        else if (collision.CompareTag("Finish"))
        {
            Debug.Log("Player reached Finish - Proceeding to the Next Stage");
            NextStage(); // NextStage 호출
        }
    }




    void PlayerReposition()
    {
        player.transform.position = new Vector3(0, 0, -1);
        player.VelocityZero();
    }

    // Game Over 화면 활성화 함수
    void TriggerGameOver()
    {
        gameOverPanel.SetActive(true);  // Game Over Panel 활성화
        gameOverText.gameObject.SetActive(true);  // "Game Over" 텍스트 활성화
    }
}
