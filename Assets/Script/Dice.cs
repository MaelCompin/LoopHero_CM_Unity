using UnityEngine;

// Équivalent de BP_Dice.
public class Dice : MonoBehaviour
{
    // GetDiceResult : Random Integer in Range (1, 6).
    // Attention : en Unity, Random.Range avec des int EXCLUT le max,
    // donc on écrit 7 pour obtenir un résultat entre 1 et 6.
    public int GetDiceResult()
    {
        return Random.Range(1, 7);
    }
}
