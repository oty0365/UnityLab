
using UnityEngine;
using System.Collections;
public class NewStateStateSO : ScriptableObject, IState
{
    private GameObject _actor;

    public bool OnStateCheck(GameObject actor)
    {
        return true;
    }

    public void OnStateEnter(GameObject actor)
    {
        Debug.Log("Entered New State State");
    }

    public void OnStateExit()
    {
        Debug.Log("Exited New State State");
    }
    public IEnumerator OnStateUpdate()
    {
        yield break;
    }
}
