namespace STUDENT_NAME
{
	using UnityEngine;

    public class CameraController : SimpleGameStateObserver
	{
		[SerializeField] Transform m_Target;
		Transform m_Transform;
		Vector3 m_InitPosition;

        public GameObject player;
        public GameObject ground;
        private Vector3 offset;

        void Start()
        {
            m_InitPosition = transform.position;
            offset = transform.position - player.transform.position;
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
            if (ground.transform.position.y <= player.transform.position.y)
            {
                transform.position = new Vector3(m_InitPosition.x, player.transform.position.y + offset.y, player.transform.position.z + offset.z);
            }
        }

		protected override void GameMenu(GameMenuEvent e)
		{
            ResetCamera();
		}
	}
}