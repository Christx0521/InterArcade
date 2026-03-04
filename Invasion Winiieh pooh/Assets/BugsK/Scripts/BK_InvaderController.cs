using UnityEngine;


    public class BK_InvaderController : MonoBehaviour
    {
        public GameObject laser;
        public GameObject explosion;
        public int points = 10;

        BK_ScoreManager scoreMan;
        BK_InvaderMovement invManager;

        void Awake()
        {
            invManager = GetComponentInParent<BK_InvaderMovement>();
            scoreMan = Object.FindFirstObjectByType<BK_ScoreManager>();
        }

        void Start()
        {
            InvokeRepeating(nameof(Shoot), Random.Range(1f, 10f), Random.Range(2f, 5f));
        }

        void Shoot()
        {
            if (!gameObject.activeInHierarchy) return;

            Vector2 origin = (Vector2)transform.position + Vector2.down * 0.6f;
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 10f);

            if (hit.collider == null || !hit.collider.CompareTag("Invader"))
            {
                Instantiate(laser, origin, Quaternion.identity);
            }
        }

        private void OnDisable()
        {
            CancelInvoke();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Laser"))
            {
                if (explosion != null)
                    Instantiate(explosion, transform.position, transform.rotation);

                scoreMan.AddScore(points);
                Destroy(other.gameObject);
                gameObject.SetActive(false);
            }
        }
    }
