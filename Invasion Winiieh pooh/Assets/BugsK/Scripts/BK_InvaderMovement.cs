using UnityEngine;


    public class BK_InvaderMovement : MonoBehaviour
    {
        public float baseSpeed = 1f;
        public float speedIncreasePerRound = 0.1f;

        float currentSpeed;
        int round = 0;

        public bool edgeContact;
        public BK_GameManagerBugs gM;

        [SerializeField] float loseLineY = -3.5f;
        bool initialized;
        public Vector2 startPosition;

        void Start()
        {
            startPosition = transform.position;
            UpdateSpeed();
            Invoke(nameof(EnableCheck), 0.5f);
        }

        void EnableCheck() => initialized = true;

        void UpdateSpeed()
        {
            currentSpeed = baseSpeed + (round * speedIncreasePerRound);
        }

        public void NextRound()
        {
            round++;
            UpdateSpeed();
        }

        void Update()
        {
            transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);

            int activeAliens = 0;

            foreach (Transform alien in transform)
            {
                if (!alien.gameObject.activeInHierarchy) continue;
                activeAliens++;

                if (alien.position.y <= loseLineY)
                {
                    gM.GameOver();
                    return;
                }

                if ((alien.position.x >= 6.5f || alien.position.x <= -6.5f) && !edgeContact)
                {
                    ChangePos();
                    break;
                }
            }

            if (initialized && activeAliens <= 0)
            {
                initialized = false;
                gM.StartCoroutine(gM.WaveReset());
            }
        }

        void ChangePos()
        {
            edgeContact = true;
            currentSpeed = -currentSpeed;

            transform.position = new Vector2(transform.position.x, transform.position.y - 0.5f);
            Invoke(nameof(ContactCheck), 0.4f);
        }

        void ContactCheck() => edgeContact = false;
    }

