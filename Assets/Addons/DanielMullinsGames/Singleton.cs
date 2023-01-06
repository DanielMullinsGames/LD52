using UnityEngine;

public class Singleton<T> : ManagedBehaviour where T : ManagedBehaviour
{
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static T m_Instance;

    public static T Instance
    {
        get
        {
            if (m_ShuttingDown)
            {
                return null;
            }

            lock (m_Lock)
            {
                FindInstance();

                return m_Instance;
            }
        }
    }

    protected static void FindInstance()
    {
        if (m_Instance == null)
        {
            m_Instance = (T)FindObjectOfType(typeof(T));
        }
    }

    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }
}