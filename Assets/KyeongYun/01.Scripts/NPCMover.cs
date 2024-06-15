using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCMover : MonoBehaviour
{
    private GameManager gameManager;
    private PrefabSpawner prefabSpawner;
    private Animator animator;
    private GoldManager goldManager;
    private Vector3[] path;
    private GameObject emoteInstance;
    private SpriteRenderer emoteSpriteRenderer;

    public int targetIndex;
    public int emoteNum;
    public GameObject targetObject;

    public PosManager reservationSystem;
    public Vector3Int destinationPosition;

    public float moveSpeed = 3f;
    public string NPCName;
    public string badNPC;
    public string goodFood;
    public GameObject[] emotes;
    public Vector3 exitPos;

    private bool conditionMet = false;
    private bool hasReachedDestination = false; // 추가된 변수

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        prefabSpawner = FindObjectOfType<PrefabSpawner>();
        reservationSystem = FindObjectOfType<PosManager>();
        goldManager = FindObjectOfType<GoldManager>();
        targetObject = gameManager.GetRandomUnusedTargetObject();
        animator = GetComponent<Animator>();

        exitPos = new Vector3(10, 0, 0);
    }

    public void FindPathFromCurrentPosition()
    {
        if (targetObject == null || hasReachedDestination) return;

        Vector3 currentPos = transform.position;
        Vector2Int startPos = new Vector2Int(Mathf.RoundToInt(currentPos.x), Mathf.RoundToInt(currentPos.y));
        Vector3 endPos = targetObject.transform.position;
        Vector2Int targetPos = new Vector2Int(Mathf.RoundToInt(endPos.x), Mathf.RoundToInt(endPos.y));

        Debug.Log($"Start Position: {startPos}");
        Debug.Log($"Target Position: {targetPos}");

        gameManager.startPos = startPos;
        gameManager.targetPos = targetPos;
        gameManager.PathFinding();

        path = new Vector3[gameManager.FinalNodeList.Count];
        for (int i = 0; i < gameManager.FinalNodeList.Count; i++)
        {
            path[i] = new Vector3(gameManager.FinalNodeList[i].x, gameManager.FinalNodeList[i].y, 0);
        }
        targetIndex = 0;

        MoveToNextTarget();
    }

    void MoveToNextTarget()
    {
        if (path == null || path.Length == 0)
            return;

        targetIndex = 1;
    }

    void Update()
    {
        if (path == null || path.Length == 0 || targetIndex >= path.Length)
            return;

        Vector3 targetPosition = path[targetIndex];
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetIndex++;
        }

        UpdateAnimation(direction);

        if (targetIndex == path.Length && !conditionMet)
        {
            OnDestinationReached();
        }

        if (conditionMet && Vector3.Distance(transform.position, exitPos) < 0.1f)
        {
            OnExitReached();
        }
    }

    void UpdateAnimation(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            float moveX = direction.x > 0 ? 1 : direction.x < 0 ? -1 : 0;
            float moveY = direction.y > 0 ? 1 : direction.y < 0 ? -1 : 0;

            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);
        }
    }

    void OnDestinationReached()
    {
        hasReachedDestination = true; // 목적지에 도착했음을 표시
        int randomNum = Random.Range(1, 10);

        int currentRate = 0;
        switch (NPCName)
        {
            case "Human":
                currentRate = prefabSpawner.humanRate;
                prefabSpawner.UpdateRate("humanRate", currentRate);
                break;
            case "Dwarf":
                currentRate = prefabSpawner.dwarfRate;
                prefabSpawner.UpdateRate("dwarfRate", currentRate);
                break;
            case "Elf":
                currentRate = prefabSpawner.elfRate;
                prefabSpawner.UpdateRate("elfRate", currentRate);
                break;
            default:
                Debug.LogError("Invalid NPCName: " + NPCName);
                break;
        }

        switch (randomNum)
        {
            case int n when (n > 0 && n <= 6):
                Invoke(nameof(GoodEmote), 3);
                emoteNum = 0;
                currentRate += 2;
                break;
            case int n when (n > 6 && n < 10):
                Invoke(nameof(NormalEmote), 3);
                emoteNum = 3;
                currentRate += 1;
                break;
            case int n when (n >= 10):
                Invoke(nameof(BadEmote), 3);
                emoteNum = 2;
                break;
        }

        Invoke(nameof(Thinking), 1);

        Invoke(nameof(NPCExit), 10);
    }

    void CheckConditionAndFindNewPath()
    {
        conditionMet = true;
        Vector3 newDestination = exitPos;
        destinationPosition = new Vector3Int((int)newDestination.x, (int)newDestination.y, 0);
        reservationSystem.ReserveDestination(destinationPosition, gameObject);
        FindPathFromCurrentPosition();
    }

    void NPCExit()
    {
        conditionMet = true;
        Vector3 newDestination = exitPos;
        destinationPosition = new Vector3Int((int)newDestination.x, (int)newDestination.y, 0);
        reservationSystem.ReserveDestination(destinationPosition, gameObject);
        FindPathFromCurrentPosition();

        goldManager.gold += 100;
        reservationSystem.CancelReservation(destinationPosition);
    }

    void OnExitReached()
    {
        prefabSpawner.ReturnObjectToPool(gameObject, 1);
        gameManager.acheivement++;
    }

    void Thinking()
    {
        Vector3 emotePosition = transform.position + new Vector3(0, 0.75f, 0);
        emoteInstance = Instantiate(emotes[1], emotePosition, Quaternion.identity);
        emoteSpriteRenderer = emoteInstance.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeInAndMoveUp(emoteInstance.transform, emoteSpriteRenderer, 0.2f));

        Invoke(nameof(DestroyEmote), 3f);
    }

    IEnumerator FadeInAndMoveUp(Transform emoteTransform, SpriteRenderer spriteRenderer, float duration)
    {
        Color c = spriteRenderer.color;
        c.a = 0;
        spriteRenderer.color = c;

        Vector3 startPos = emoteTransform.position;
        Vector3 endPos = emoteTransform.position + new Vector3(0, 0.5f, 0);

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            c.a = Mathf.Lerp(0f, 1f, t);
            spriteRenderer.color = c;

            emoteTransform.position = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }
    }

    void Emotes(int emoteNum)
    {
        Vector3 emotePosition = transform.position + new Vector3(0, 0.75f, 0);
        emoteInstance = Instantiate(emotes[emoteNum], emotePosition, Quaternion.identity);
        emoteSpriteRenderer = emoteInstance.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeInAndMoveUp(emoteInstance.transform, emoteSpriteRenderer, 0.2f));

        Invoke(nameof(DestroyEmote), 3f);
    }

    void DestroyEmote()
    {
        if (emoteInstance != null)
        {
            Destroy(emoteInstance);
        }
    }

    void DestroyText()
    {
        if (emoteInstance != null)
        {
            Destroy(emoteInstance);
        }
    }

    void RandomFunc()
    {
        int randomNum = Random.Range(1, 10);
        if (randomNum > 8)
        {
            Vector3 emotePosition = transform.position + new Vector3(0, 0.75f, 0);
            emoteInstance = Instantiate(emotes[4], emotePosition, Quaternion.identity);
            emoteSpriteRenderer = emoteInstance.GetComponent<SpriteRenderer>();
            StartCoroutine(FadeInAndMoveUp(emoteInstance.transform, emoteSpriteRenderer, 0.2f));

            Invoke(nameof(DestroyEmote), 3f);
        }
    }

    void GoodEmote()
    {
        Debug.Log("선호 음식 주문, 좋은 이모티콘 출력, +2");
    }

    void NormalEmote()
    {
        Debug.Log("일반 음식 주문, 평범한 이모티콘 출력, +1");
    }

    void BadEmote()
    {
        Debug.Log("불호 음식 주문, 싫은 이모티콘 출력, +0");
    }
}
