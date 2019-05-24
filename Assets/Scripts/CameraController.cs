namespace STUDENT_NAME
{
	using UnityEngine;

    public class CameraController : SimpleGameStateObserver
	{
		[SerializeField] Transform m_Target;
        [SerializeField] Transform m_Ground;
        Transform m_Transform;
		Vector3 m_InitPosition;

        void Start()
        {
            m_InitPosition = transform.position - m_Target.position;
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
            if (m_Ground.position.y <= m_Target.position.y)
            {
                transform.position = new Vector3(m_Target.position.x + m_InitPosition.x, m_Target.position.y + m_InitPosition.y, m_InitPosition.z);
            }
        }

		protected override void GameMenu(GameMenuEvent e)
		{
            ResetCamera();
		}
	}
}