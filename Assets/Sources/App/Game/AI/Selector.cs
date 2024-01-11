using System.Collections.Generic;

namespace AI {
    public class Selector : Node {
        private readonly IEnumerable<INode> _nodes;
        
        public Selector(IEnumerable<INode> nodes) => _nodes = nodes;

        protected override Status Execute(MapAgent agent, MobData data) {
            foreach (var node in _nodes)
                switch (node.Evaluate(agent, data)) {
                    case Status.Failure: continue;
                    
                    case Status.Success: return CurrentStatus = Status.Success;
                    
                    case Status.Running: return CurrentStatus = Status.Running;
                }
            
            return CurrentStatus = Status.Failure;
        }
        
    }
}