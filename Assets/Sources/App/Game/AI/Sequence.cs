using System.Collections.Generic;

namespace AI {
    
    public class Sequence : Node {

        private readonly IEnumerable<INode> _nodes;
        
        public Sequence(IEnumerable<INode> nodes) => _nodes = nodes;

        protected override Status Execute(MapAgent agent, MobData data) {
            
            foreach (var node in _nodes)
                switch (node.Evaluate(agent, data)) {
                    case Status.Failure: return CurrentStatus = Status.Failure;
                    case Status.Success:  continue;
                    case Status.Running: return CurrentStatus = Status.Running;
                }

            return Status.Success;
        }
        
    }
}