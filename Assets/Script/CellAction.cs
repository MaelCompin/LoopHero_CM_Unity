using UnityEngine;

// Équivalent de BPC_CellAction : le composant parent de toutes les cases spéciales.
// Il ne fait rien lui-même, les enfants remplissent Execute (override).
public class CellAction : MonoBehaviour
{
    public virtual void Execute(Player player)
    {
        // Vide, comme dans Unreal.
    }
}
