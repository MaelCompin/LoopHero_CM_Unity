using UnityEngine;

// Équivalent de BP_Board : le plateau connaît les cellules.
public class Board : MonoBehaviour
{
    // Équivalent de la variable "Cells" (Instance Editable).
    // À remplir dans l'Inspector, DANS L'ORDRE du parcours (élément 0 = case départ).
    [SerializeField] private Cell[] cells;

    // GetCellByIndex : donne le numéro, récupère la case.
    public Cell GetCellByIndex(int index)
    {
        if (index < 0 || index >= cells.Length) return null;
        return cells[index];
    }

    // TryMoveToCell : oui ou non, est-ce que cette case existe ? (Is Valid Index)
    public bool TryMoveToCell(int index)
    {
        return index >= 0 && index < cells.Length;
    }

    // GetCellCount : nombre de cases (Length).
    public int GetCellCount()
    {
        return cells.Length;
    }
}
