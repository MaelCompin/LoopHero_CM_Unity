using System.Collections.Generic;

// Équivalent de BP_GameInstance : la "fiche perso" qui survit aux changements de scène.
// Une classe static n'appartient à aucune scène, donc elle n'est jamais détruite
// quand on charge une autre scène (pour un mini-jeu par exemple).
public static class GameData
{
    // ActualPlayer : quel joueur joue
    public static int ActualPlayer = 0;

    // PositionPlayer : la case de chaque joueur (un élément par défaut, comme dans Unreal)
    public static List<int> PositionPlayer = new List<int> { 0 };
}
