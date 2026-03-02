
using OtyPackages.StateGraph.Scripts;
using System.Collections;
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
        _actor = actor;
        Debug.Log("OpenStackFrame");
        _actor.GetComponent<StateGraphController>().ExecuteCoroutine(OnStateUpdate());
        Debug.Log("CloseStackFrame");
    }

    public void OnStateExit()
    {
        Debug.Log("Exited New Entry State");
    }
    public IEnumerator OnStateUpdate()
    {
        yield return null;
        _actor.GetComponent<StateGraphController>().CheckStatesToEnter();
        yield break;
    }
}
