using UnityEngine;

namespace AI {
    public class BehaviourTree {
        
        private readonly IAgentEventsHandler _agentHandler;
        private readonly INode _tree;
        
        public BehaviourTree(IAgentEventsHandler agentHandler) {
            _agentHandler = agentHandler;

            _tree = new Sequence(new INode[] {
                new Node(SpawnTimer),
                    new Selector(new INode[] {
                        new Node(NoHealth),
                        new Sequence(new INode[] {
                                new Node(SelectTarget),
                                new Node(ChaseTarget),
                            }
                        ),
                        new Sequence(new INode[] {
                                new Node(ContactTarget),
                                new Node(AttackTarget),
                            }
                        )
                    }),
                new Node(WaitDeath),
            });
        }

        private Status SpawnTimer(MapAgent agent, MobData data) => (data.SpawnTimer -= Time.fixedDeltaTime) > 0? Status.Running: Status.Success;
        
        private Status WaitDeath(MapAgent agent, MobData data) => (data.DeathTimer -= Time.fixedDeltaTime) > 0? Status.Running: Status.Success;
        
        private Status NoHealth(MapAgent agent, MobData data) => data.HealthAmount > 0 ? Status.Failure : Status.Success;

        private Status ChaseTarget(MapAgent agent, MobData data) {
            var direction = data.PrimaryTarget.GetDirectionToContact(agent.transform.position);
            var distance = direction.magnitude;

            if (distance <= 1) {
                agent.Move(Vector3.zero);
                return Status.Failure;
            }

            agent.Move(direction.normalized * Time.fixedDeltaTime * data.MovementSpeed);
            
            return Status.Running;
        }
        
        private Status ContactTarget(MapAgent agent, MobData data) {
            return (data.AimTimer -= Time.fixedDeltaTime) > 0 ? Status.Running : Status.Success;
        }
        
        private Status AttackTarget(MapAgent agent, MobData data) {

            data.PrimaryTarget.ApplyDamage(5);
            
            data.AimTimer = 1f;
            
            agent.Attack();
            
            return data.PrimaryTarget.IsActive? Status.Success: Status.Failure;
        }

        private Status SelectTarget(MapAgent agent, MobData data) {
            data.PrimaryTarget = _agentHandler.NearestTarget(agent);
            return Status.Success;
        }

        public void Execute(MapAgent agent, MobData data) => _tree.Evaluate(agent, data);
    }
}
