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
        void Start()
        {
            _nodeDict[sgDataSO.entryNode.nodeID] =  sgDataSO.entryNode;
            foreach (var data in sgDataSO.nodeDatas)
            {
                _nodeDict[data.nodeID] =  data;
            }
            if (sgDataSO.entryNode.stateLogic is IState stateLogic)
            {
                stateLogic.OnStateEnter(gameObject);
            }
        }

        public void CheckStatesToEnter()
        {
            if (_nodeDict.ContainsKey(_currentID))
            {
                foreach (var data in _nodeDict[_currentID].connections)
                {
                    if (_nodeDict[_currentID].nodeType == NodeType.Portal)
                    {
                        if (_nodeDict[_nodeDict[_currentID].jumpID].stateLogic is not IState stateLogic) continue;
                        if (!stateLogic.OnStateCheck(gameObject)) continue;
                        if (_nodeDict[_currentID].stateLogic is IState currenStateLogic)
                        {
                            currenStateLogic.OnStateExit();
                        }
                        stateLogic.OnStateEnter(gameObject);
                        _currentID = _nodeDict[_nodeDict[_currentID].jumpID].nodeID;
                    }
                    else
                    {
                        
                    }
                }
            }
        }
        
        void Update()
        {
            
        }
    }
}
