
using UnityEngine;
public class NewEntryStateSO : ScriptableObject, IState
{
    private GameObject _actor;

    public bool OnStateCheck(GameObject actor)
    {
        return true;
    }

    public void OnStateEnter(GameObject actor)
    {
        Debug.Log("Enter");
        Debug.Log(actor);
    }

    public void OnStateExit()
    {
    }
}
