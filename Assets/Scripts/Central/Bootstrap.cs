using UnityEngine;

public static class Bootstrap
{
    [RuntimeInitializeOnLoadMethod(
        RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (Core.GameManager.Gman != null)
            return;

        GameObject prefab = Resources.Load<GameObject>(
            "Prefabs/GameManager"
        );

        if (prefab == null)
        {
            Debug.LogError("Prefab GameManager não encontrado!");
            return;
        }

        Object.Instantiate(prefab);
    }
}