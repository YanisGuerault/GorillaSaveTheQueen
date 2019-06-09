using UnityEngine;

    public class CameraController : SimpleGameStateObserver
	{
		Transform m_Target;
        Transform m_Transform;
		Vector3 m_InitPosition;
        Transform m_Ground;

        void Start()
        {
            m_InitPosition = transform.position;
            m_Ground = m_Target;
        }

        public void setGround(Transform ground)
        {
            m_Ground = ground;
        }

        public void setTarget(Transform player)
        {
            m_Target = player;
        }

        void ResetCamera()
		{
			m_Transform.position = m_InitPosition;
		}

        protected override void Awake()
		{
			base.Awake();
			m_Transform = transform;
			//m_InitPosition = m_Transform.position;
		}

		void Update()
		{
            if (m_Target != null && m_Ground.position.y <= m_Target.position.y)
            {
                transform.position = new Vector3(m_Target.position.x, m_Target.position.y + m_InitPosition.y, m_InitPosition.z);
            }
        }

		protected override void GameMenu(GameMenuEvent e)
		{
            ResetCamera();
		}
	}