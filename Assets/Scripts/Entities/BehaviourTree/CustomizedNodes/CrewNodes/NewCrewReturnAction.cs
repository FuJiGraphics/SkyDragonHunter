using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class NewCrewReturnAction : ActionNode<NewCrewControllerBT>
    {
        private Vector3 targetPos;
        private static readonly float s_Threshold = 0.15f;

        private float DistanceToTargetPos => Vector3.Distance(targetPos, m_Context.transform.position);

        public NewCrewReturnAction(NewCrewControllerBT context) : base(context)
        {

        }

        protected override void OnStart()
        {
            base.OnStart();
            targetPos = m_Context.onFieldOriginPosition;
        }

        protected override NodeStatus OnUpdate()
        {
            if (m_Context.IsTargetInAggroRange)
            {
                return NodeStatus.Failure;
            }

            if(DistanceToTargetPos < s_Threshold)
            {
                m_Context.transform.position = m_Context.onFieldOriginPosition;
                //var newPos = new Vector2(m_Context.onFieldOriginPosition.x, m_Context.transform.position.y);
                //m_Context.transform.position = newPos;
                // m_Context.floater.StartY = m_Context.onFieldOriginPosition.y;
                return NodeStatus.Success;
            }

            if (UpdatePos())
            {
                return NodeStatus.Success;
            }

            return NodeStatus.Running; 
        }

        private bool UpdatePos()
        {
            bool success = false;
            var direction = (targetPos - m_Context.transform.position).normalized;

            var newPos = m_Context.transform.position + direction * Time.deltaTime * m_Context.Speed;

            //var newPos = m_Context.AdjustedPosition + direction * Time.deltaTime * m_Context.Speed;
            //newPos.y = m_Context.transform.position.y;

            if (DistanceToTargetPos < m_Context.Speed * Time.deltaTime)
            {
                //newPos.x = targetPos.x;
                newPos = targetPos;
                success = true;
            }

            m_Context.transform.position = newPos;
            // m_Context.floater.StartY += direction.y * Time.deltaTime * m_Context.Speed;

            return success;
        }
    } // Scope by class NewCrewReturnAction

} // namespace Root