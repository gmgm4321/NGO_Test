using System.Collections;

using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] private AudioClip _destroyClip;
    [SerializeField] private GameObject _particles;
    [SerializeField] private float DestroyTime = 3;

    private Vector3 _dir;
    private float timeSinceSpawn = 0f;

    public void Init(Vector3 dir) {
        GetComponent<Rigidbody>().AddForce(dir);
        timeSinceSpawnResetting();
        Invoke(nameof(DestroyBall), DestroyTime);
    }

    private void DestroyBall() {
        AudioSource.PlayClipAtPoint(_destroyClip, transform.position);
        Instantiate(_particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private Camera mainCamera;
    private bool hasHitBoundary = false;
    private BoundaryDirection boundaryDirection;
    [SerializeField] private CommunicationLogic cl;

    public enum BoundaryDirection
    {
        None,
        Top,
        Bottom,
        Left,
        Right
    }

    private void Start()
    {
        mainCamera = Camera.main;
        cl = GameObject.Find("CommunicationLogic").GetComponent<CommunicationLogic>();

    }

    public void timeSinceSpawnResetting() {

        timeSinceSpawn = 0;
    }

    private void Update()
    {
        timeSinceSpawn += Time.deltaTime;
    }

    private void FixedUpdate()
    {

        // 獲取球體在世界座標系中的位置
        Vector3 ballPosition = transform.position;

        // 將球體的世界座標轉換為視口座標
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(ballPosition);

        // 檢查球體是否在視口邊界上（假設邊界為 0 和 1）
        if ((viewportPosition.x <= 0f || viewportPosition.x >= 1f ||
             viewportPosition.y <= 0f || viewportPosition.y >= 1f) &&
             !hasHitBoundary)
        {
            // 球體首次碰到了攝影機的可視範圍邊界

            // 獲取邊界上的點的世界座標
            Vector3 boundaryPosition = mainCamera.ViewportToWorldPoint(viewportPosition);

            // 回傳碰撞邊界的座標
            // ReturnBoundaryPosition(boundaryPosition, GetBoundaryDirection(viewportPosition));
            cl.OutOfBoundseClientRpc(boundaryPosition, GetBoundaryDirection(viewportPosition));

            // 標記已經接觸到邊界
            hasHitBoundary = true;
        }
    }

    private BoundaryDirection GetBoundaryDirection(Vector3 viewportPosition)
    {
        if (viewportPosition.x <= 0f)
        {
            return BoundaryDirection.Left;
        }
        else if (viewportPosition.x >= 1f)
        {
            return BoundaryDirection.Right;
        }
        else if (viewportPosition.y <= 0f)
        {
            return BoundaryDirection.Bottom;
        }
        else if (viewportPosition.y >= 1f)
        {
            return BoundaryDirection.Top;
        }

        return BoundaryDirection.None;
    }

    

    private void ReturnBoundaryPosition(Vector3 position, BoundaryDirection direction)
    {
        // 在這裡處理碰撞邊界的座標
        Debug.Log("碰撞邊界方向：" + direction);
        Debug.Log("碰撞邊界座標：" + position);
        float collisionTime = timeSinceSpawn;

        Vector3 mirrorPosition = Vector3.zero;

        switch (direction)
        {
            case BoundaryDirection.Right:
                mirrorPosition = new Vector3((position.x * -1) - 0.5f, position.y, position.z);
                break;

            case BoundaryDirection.Left:
                mirrorPosition = new Vector3((position.x * -1) + 0.5f, position.y, position.z);
                break;

            case BoundaryDirection.Top:
                mirrorPosition = new Vector3(position.x, position.y, (position.z * -1) - 0.5f);
                break;

            case BoundaryDirection.Bottom:
                mirrorPosition = new Vector3(position.x, position.y, (position.z * -1) + 0.5f);
                break;
        }

        GameObject BallClon = Instantiate(this.gameObject, mirrorPosition, Quaternion.identity);
        BallClon.GetComponent<Rigidbody>().velocity = this.gameObject.GetComponent<Rigidbody>().velocity;
        BallClon.GetComponent<Projectile>().hasHitBoundary = true;
        BallClon.GetComponent<Projectile>().StartCheckMovement(mirrorPosition);
        BallClon.GetComponent<Projectile>().Invoke(nameof(DestroyBall),DestroyTime- timeSinceSpawn);
        print(BallClon.transform.position);
    }

    public float moveDistance = 0.6f; // 移動距離
    public float moveTime = 1.5f; // 移動時間

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private void StartCheckMovement(Vector3 position)
    {
        startPosition = position;
        timeSinceSpawnResetting();
        StartCoroutine(CheckMovement());
    }

    private IEnumerator CheckMovement()
    {
        float elapsedTime = 0f;
        bool hasMoved = false;

        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;

            // 在移动过程中检查物体是否移动了指定距离
            if (!hasMoved && Vector3.Distance(transform.position, startPosition) >= moveDistance)
            {
                hasMoved = true;
                hasHitBoundary = false;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

}