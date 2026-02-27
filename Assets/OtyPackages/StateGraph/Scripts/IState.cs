using UnityEngine;

public interface IState
{
    public bool OnStateCheck(GameObject actor);
    public void OnStateEnter(GameObject actor);
    public void OnStateExit();
}
