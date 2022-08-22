using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.GameBehaviors;

namespace Assets.Scripts.GameMenu
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CharacterBase m_mainPlayer;
        [SerializeField][Min(float.Epsilon)] private float m_widthTreshold = 9f;
        [SerializeField] private AnimationCurve m_vieportFactor = AnimationCurve.Linear(0, 1, 1, 1);
        private Camera currentCamera;

        private void Awake()
        {
            currentCamera = GetComponent<Camera>();
        }

        public float playerX;
        public float cameraW;

        private void Update()
        {
            if (m_mainPlayer != null)
            {
                playerX = m_vieportFactor.Evaluate((m_mainPlayer.transform.position.x + m_widthTreshold) / (2 * m_widthTreshold));
                cameraW = currentCamera.ViewportToWorldPoint(new Vector3(1,1,10)).x - transform.position.x;

                if (m_widthTreshold > cameraW)
                {
                    Vector3 cameraPosition = transform.position;
                    cameraPosition.x = (m_widthTreshold - cameraW) * playerX;
                    transform.position = cameraPosition;
                }
            }
        }
    }
}