using Sirenix.OdinInspector;
using UnityEngine;

public class ScriptableDisplayer : MonoBehaviour
{
    [InlineEditor]
    public CharacterStats characterStats;

    [InlineEditor]
    public EnemyStats enemyStats;

    private void OnValidate()
    {
        if (characterStats == null)
        {

        }

        if (enemyStats == null)
        {

        }
    }
}
