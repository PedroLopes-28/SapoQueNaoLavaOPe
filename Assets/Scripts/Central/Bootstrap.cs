using UnityEngine;

//inicia o game manager independente da cena em que se está
public static class Bootstrap
{
    [RuntimeInitializeOnLoadMethod(
        RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (Core.GameManager.Gman != null)
            return;

        GameObject obj = new GameObject("GameManager");

        obj.AddComponent<Core.GameManager>();
    }
}