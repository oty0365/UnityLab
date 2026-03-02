using System.Collections;
using System.Collections.Generic;
using OtyPackages.StateGraph.Editor;
using UnityEngine;

namespace OtyPackages.StateGraph.Scripts
{
    public class StateGraphController : MonoBehaviour
    {
        private string _currentID;
        [SerializeField] private SGDataSO sgDataSO;
        private Dictionary<string,NodeData> _nodeDict =  new Dictionary<string,NodeData>();
        private HashSet<string> visited = new();
        void Start()
        {
            foreach (var data in sgDataSO.nodeDatas)
            {
                _nodeDict[data.nodeID] =  data;
            }
            _currentID = sgDataSO.entryNode.nodeID;
            if (sgDataSO.entryNode.stateLogic is IState stateLogic)
            {
                stateLogic.OnStateEnter(gameObject);
            }
        }

        public Coroutine ExecuteCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }
        public void KillCoroutine(Coroutine coroutine)
        {
            StopCoroutine(coroutine);
        }

        public void CheckStatesToEnter()
        {
            if (_nodeDict.ContainsKey(_currentID))
            {
                foreach (var dataCode in _nodeDict[_currentID].connections)
                {
                    switch (_nodeDict[dataCode].nodeType)
                    {
                        case NodeType.Portal:
                            {
                                if (_nodeDict[_nodeDict[_currentID].jumpID].stateLogic is not IState stateLogic) continue;
                                if (!stateLogic.OnStateCheck(gameObject)) continue;
                                if (_nodeDict[_currentID].stateLogic is IState currenStateLogic)
                                {
                                    currenStateLogic.OnStateExit();
                                }
                                if (!visited.Contains(dataCode))
                                {
                                    stateLogic.OnStateEnter(gameObject);
                                }

                                _currentID = _nodeDict[_nodeDict[_currentID].jumpID].nodeID;
                                break;
                            }
                        case NodeType.State:
                            {
                                if (_nodeDict[dataCode].stateLogic is not IState stateLogic) continue;
                                if (!stateLogic.OnStateCheck(gameObject)) continue;
                                if (_nodeDict[_currentID].stateLogic is IState currenStateLogic)
                                {
                                    currenStateLogic.OnStateExit();
                                }
                                stateLogic.OnStateEnter(gameObject);
                                _currentID = _nodeDict[dataCode].nodeID;
                                break;
                            }
                        case NodeType.Exit:
                             {
                                 if (_nodeDict[_currentID].stateLogic is IState currenStateLogic)
                                 {
                                     currenStateLogic.OnStateExit();
                                 }
                                 _currentID = string.Empty;
                                 break;
                            }

                        default:
                            break;
                    }
                }
            }
        }
        void Update()
        {

        }
    }
}
