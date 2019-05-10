namespace STUDENT_NAME
{
	using UnityEngine;

    public class CameraController : SimpleGameStateObserver
	{
		[SerializeField] Transform m_Target;
		Transform m_Transform;
		Vector3 m_InitPosition;

        public GameObject player;
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
			m_InitPosition = m_Transform.position;
		}

		void Update()
		{
            //if (!GameManager.Instance.IsPlaying) return;
           
        }

        void FixedUpdate()
        {
            if (transform.position.y > 0)
            {
                transform.position = new Vector3(m_InitPosition.x, player.transform.position.y + offset.y, player.transform.position.z + offset.z);
            }
            else
            {
                Debug.Log("CHUTE !");
            }
        }

		protected override void GameMenu(GameMenuEvent e)
		{
			ResetCamera();
		}
	}
}