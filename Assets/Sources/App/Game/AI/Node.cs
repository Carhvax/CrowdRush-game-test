using System;

namespace AI {

    public interface INode {

        Status Evaluate(MapAgent agent, MobData data);
    }
    
    public class Node : INode {
        
        protected Status CurrentStatus;

        protected Node() => CurrentStatus = Status.None;

        public Node(Func<MapAgent, MobData, Status> execute) : this() => ExecuteAction = execute;

        protected event Func<MapAgent, MobData, Status> ExecuteAction;

        protected virtual Status Execute(MapAgent agent, MobData data) => ExecuteAction!(agent, data);

        public Status Evaluate(MapAgent agent, MobData data) => CurrentStatus = Execute(agent, data);
    }

}