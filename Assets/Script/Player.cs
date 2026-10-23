using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

// Équivalent de BP_Player : le hub, il connaît le board et le dé.
public class Player : MonoBehaviour
{
    [Header("Saut")]
    [SerializeField] private float jumpHeight = 1f;      // JumpHeight (Unity compte en mètres : 1 = 100 dans Unreal)
    [SerializeField] private float hopDuration = 0.3f;   // durée d'un saut (= Length de la HopTimeline)
    [SerializeField] private float heightOffset = 0.5f;  // le "+ (0,0,50)" : pose le pion SUR la case

    // Board et Dice (récupérés au Start, comme Get Actor Of Class)
    private Board board;
    private Dice dice;

    private int currentIndex;    // CurrentIndex
    private int stepsRemaining;  // StepsRemaining
    private bool isMoving;       // IsMoving

    // Variables du portail (lues/écrites par CellAction_Portail)
    public bool IsChoosing { get; set; }
    public int PortalA { get; set; }
    public int PortalB { get; set; }

    public Board Board => board;

    // ===== Event BeginPlay =====
    private void Start()
    {
        board = FindFirstObjectByType<Board>();
        dice = FindFirstObjectByType<Dice>();

        // On se replace sur la case enregistrée dans la fiche :
        // GameData.PositionPlayer [ GameData.ActualPlayer ]
        PlaceOnCell(GameData.PositionPlayer[GameData.ActualPlayer]);
    }

    // ===== Les touches (Space Bar, One, Two) =====
    private void Update()
    {
        if (SpacePressed())
        {
            PrepareMove();
        }

        if (OnePressed() && IsChoosing)
        {
            IsChoosing = false;
            PlaceOnCell(PortalA);
        }

        if (TwoPressed() && IsChoosing)
        {
            IsChoosing = false;
            PlaceOnCell(PortalB);
        }

        // Test de la persistance : M recharge la scène, le pion doit revenir sur sa case
        if (MPressed())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // ===== PrepareMove =====
    public void PrepareMove()
    {
        // Premier Branch : IsMoving OR IsChoosing -> on ne fait rien
        if (isMoving || IsChoosing) return;

        int result = dice.GetDiceResult();
        Debug.Log("Dé : " + result);

        // (CurrentIndex + Result) % GetCellCount -> le plateau boucle
        int target = (currentIndex + result) % board.GetCellCount();

        if (board.TryMoveToCell(target))
        {
            MoveToCell(target);
        }
        else
        {
            Debug.Log("Trop Loin !");
        }
    }

    // ===== MoveToCell : calcule le nombre de pas puis lance l'animation =====
    public void MoveToCell(int index)
    {
        int count = board.GetCellCount();
        stepsRemaining = (index - currentIndex + count) % count;
        isMoving = true;
        StartCoroutine(StepToNextCell());
    }

    // ===== PlaceOnCell : téléportation instantanée (BeginPlay, Tornade, Portail) =====
    public void PlaceOnCell(int index)
    {
        currentIndex = index;
        transform.position = board.GetCellByIndex(index).transform.position + Vector3.up * heightOffset;
        SavePosition();
    }

    // ===== SavePosition : écrit la case actuelle dans la fiche =====
    // = Set Array Elem (PositionPlayer, ActualPlayer, CurrentIndex) avec Size to Fit
    private void SavePosition()
    {
        // "Size to Fit" : on agrandit la liste s'il manque une place pour ce joueur
        while (GameData.PositionPlayer.Count <= GameData.ActualPlayer)
        {
            GameData.PositionPlayer.Add(0);
        }

        GameData.PositionPlayer[GameData.ActualPlayer] = currentIndex;
    }

    // ===== StepToNextCell : le saut en arc, case par case =====
    // En Unity, on utilise une Coroutine à la place du Custom Event + Timeline :
    // c'est le seul endroit où il y a du temps, comme dans la consigne.
    private IEnumerator StepToNextCell()
    {
        // Branch : StepsRemaining > 0
        while (stepsRemaining > 0)
        {
            // (CurrentIndex + 1) % GetCellCount
            int nextIndex = (currentIndex + 1) % board.GetCellCount();

            // Set StartLocation / Set EndLocation
            Vector3 startLocation = transform.position;
            Vector3 endLocation = board.GetCellByIndex(nextIndex).transform.position + Vector3.up * heightOffset;

            // HopTimeline : Alpha va de 0 à 1 pendant hopDuration (= Update)
            float time = 0f;
            while (time < hopDuration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Clamp01(time / hopDuration);

                // Lerp(Start, End, Alpha) + Z : Sin(Alpha x 180°) x JumpHeight
                // (en Unity, la hauteur c'est Y, et Mathf.Sin travaille en radians : 180° = PI)
                Vector3 position = Vector3.Lerp(startLocation, endLocation, alpha);
                position.y += Mathf.Sin(alpha * Mathf.PI) * jumpHeight;
                transform.position = position;

                yield return null; // attend l'image suivante
            }

            // Finished : Set CurrentIndex, StepsRemaining --
            currentIndex = nextIndex;
            stepsRemaining--;
        }

        // False : arrivé
        isMoving = false;
        SavePosition();
        board.GetCellByIndex(currentIndex).ActivateCell(this);
    }

    // ===== Lecture des touches (marche avec l'ancien ET le nouveau Input System) =====
    private bool SpacePressed()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.Space);
#endif
    }

    private bool OnePressed()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.digit1Key.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.Alpha1);
#endif
    }

    private bool TwoPressed()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.digit2Key.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.Alpha2);
#endif
    }

    private bool MPressed()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.M);
#endif
    }
}