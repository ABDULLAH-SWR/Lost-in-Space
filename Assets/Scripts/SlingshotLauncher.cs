using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // --- NEW: Required to detect UI clicks ---

[RequireComponent(typeof(Rigidbody2D))]
public class SlingshotLauncher : MonoBehaviour
{
    [Header("Slingshot Settings")]
    public float launchPower = 10f;
    public float maxDragDistance = 3f;

    [Header("Trajectory Visuals")]
    [Tooltip("Drag your new TrajectoryDot Prefab here!")]
    public GameObject dotPrefab;
    [Tooltip("How many dots make up the arc")]
    public int trajectoryPoints = 15;
    [Tooltip("How far apart the dots are spaced (time in seconds)")]
    public float trajectoryTimeStep = 0.05f;

    [Header("Oxygen / Respawn Settings")]
    public float maxFlightTime = 10f;
    public Slider oxygenBar;

    public Rigidbody2D rb;
    private Camera cam;

    private GameObject[] dotsPool;

    private bool isDragging = false;
    public bool hasFired = false;

    private Vector2 clickStartPos;
    private Vector2 initialSpawnPosition;

    private bool isHandlingRespawn = false;
    private float flightTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        initialSpawnPosition = transform.position;
        rb.bodyType = RigidbodyType2D.Kinematic;

        if (oxygenBar == null)
        {
            GameObject foundBar = GameObject.Find("OxygenBar");
            if (foundBar != null) oxygenBar = foundBar.GetComponent<Slider>();
        }

        if (oxygenBar != null)
        {
            oxygenBar.maxValue = maxFlightTime;
            oxygenBar.value = maxFlightTime;
            oxygenBar.gameObject.SetActive(false);
        }

        dotsPool = new GameObject[trajectoryPoints];
        for (int i = 0; i < trajectoryPoints; i++)
        {
            dotsPool[i] = Instantiate(dotPrefab, transform.position, Quaternion.identity);
            dotsPool[i].SetActive(false);
        }
    }

    void Update()
    {
        if (hasFired && !isHandlingRespawn && Input.GetKeyDown(KeyCode.R))
        {
            isHandlingRespawn = true;
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            if (levelManager != null) levelManager.RegisterMiss();
            return;
        }

        if (hasFired && !isHandlingRespawn)
        {
            if (oxygenBar != null && !oxygenBar.gameObject.activeInHierarchy)
            {
                oxygenBar.gameObject.SetActive(true);
            }

            flightTimer += Time.deltaTime;

            if (oxygenBar != null) oxygenBar.value = maxFlightTime - flightTimer;

            if (flightTimer >= maxFlightTime)
            {
                isHandlingRespawn = true;
                LevelManager levelManager = FindObjectOfType<LevelManager>();
                if (levelManager != null) levelManager.RegisterMiss();
                return;
            }
        }

        if (hasFired) return;

        // 1. ON CLICK (Anywhere on screen)
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.timeScale == 0f) return;

            // --- NEW: If the mouse is hovering over a UI element (like the Tutorial Panel), do nothing! ---
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            // --- UPDATED: Block dragging during BOTH Story Sequence and Select Card states ---
            if (TutorialManager.instance != null)
            {
                if (TutorialManager.instance.currentState == TutorialManager.TutorialState.StorySequence ||
                    TutorialManager.instance.currentState == TutorialManager.TutorialState.SelectCard)
                {
                    return;
                }
            }

            clickStartPos = cam.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;

            if (TutorialManager.instance != null) TutorialManager.instance.OnSlingshotGrabbed();
        }

        // 2. ON DRAG
        if (isDragging && Input.GetMouseButton(0))
        {
            Vector2 currentMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dragVector = clickStartPos - currentMousePos;

            if (dragVector.magnitude > maxDragDistance) dragVector = dragVector.normalized * maxDragDistance;

            DrawTrajectory(dragVector);
        }

        // 3. ON RELEASE
        if (isDragging && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            HideTrajectory();

            Vector2 releaseMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 launchVector = clickStartPos - releaseMousePos;

            // --- NEW: Cancel the launch if it was just a tap/click instead of a real drag ---
            if (launchVector.magnitude < 0.2f) return;

            if (TutorialManager.instance != null) TutorialManager.instance.OnSlingshotReleased();

            hasFired = true;
            rb.bodyType = RigidbodyType2D.Dynamic;

            if (SoundManager.instance != null) SoundManager.instance.PlaySlingshotRelease();

            if (launchVector.magnitude > maxDragDistance) launchVector = launchVector.normalized * maxDragDistance;

            rb.WakeUp();
            rb.AddForce(launchVector * launchPower, ForceMode2D.Impulse);
        }
    }

    private void DrawTrajectory(Vector2 launchDirection)
    {
        Vector2 initialVelocity = (launchDirection * launchPower) / rb.mass;
        Vector2 currentGravity = Physics2D.gravity * rb.gravityScale;

        if (launchDirection.magnitude < 0.1f)
        {
            HideTrajectory();
            return;
        }

        for (int i = 0; i < trajectoryPoints; i++)
        {
            float time = i * trajectoryTimeStep;
            Vector2 pointPos = rb.position + (initialVelocity * time) + (0.5f * currentGravity * (time * time));

            if (dotsPool[i] != null)
            {
                dotsPool[i].transform.position = pointPos;
                dotsPool[i].SetActive(true);
            }
        }
    }

    private void HideTrajectory()
    {
        for (int i = 0; i < trajectoryPoints; i++)
        {
            if (dotsPool[i] != null) dotsPool[i].SetActive(false);
        }
    }

    public void ResetShot()
    {
        hasFired = false;
        isHandlingRespawn = false;
        flightTimer = 0f;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;

        transform.position = initialSpawnPosition;
        transform.rotation = Quaternion.identity;

        HideTrajectory();

        if (oxygenBar != null)
        {
            oxygenBar.value = maxFlightTime;
            oxygenBar.gameObject.SetActive(false);
        }
    }
}