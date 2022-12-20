using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        #region variables

        [Header("Movement")]
        [SerializeField] private float forwardSpeed = 2f;
        [SerializeField] private float backwardSpeed = 1f;
        [SerializeField] private float rotationSpeed = 1f;

        [Header("Fuel")]
        [SerializeField] private GameObject flamePrefab;
        [SerializeField] private Transform flameSpawn;
        [SerializeField] private float flameRate = .1f;
        [SerializeField] private float fuel = 10;
        [SerializeField] private float fuelLoss = 1;
        [SerializeField] private Slider fuelUI;
        [SerializeField] private GameObject flameLight;
        [SerializeField] private GameObject frontLight;
        private float flameTimer;

        private Animator anim;
        private CharacterController cc;

        private Vector2 input;
        private bool aim;
        private bool fire;

        private bool isDead = false;

        #endregion

        #region built-in functions

        private void Start()
        {
            anim = GetComponent<Animator>();
            cc = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if(isDead) return;
            GetInput();
            AimAndFire();
            UpdateAnim();
        }

        private void FixedUpdate()
        {
            if(isDead) return;
            Locomotion();
        }

        #endregion

        #region Custom functions

        private void GetInput()
        {
            // Bad but works
            // Replace with Input system asap
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            aim = Input.GetMouseButton(1);
            fire = Input.GetMouseButton(0);
        }
        
        private void AimAndFire()
        {
            flameTimer += Time.deltaTime;
            fuel -= Time.deltaTime;

            fuelUI.value = fuel / 120;

            if (fuel > 0)
            {
                if (aim && fire && flameTimer >= flameRate)
                {
                    Instantiate(flamePrefab, flameSpawn.position, flameSpawn.rotation);
                    fuel -= fuelLoss;
                    flameTimer = 0;
                }
            }
            else
            {
                // No more fuel
                flameLight.SetActive(false);
                frontLight.SetActive(false);
                fuelUI.gameObject.SetActive(false);
            }
        }

        public void ChangeFuel(int amount)
        {
            if (fuel < 0)
            {
                fuel = 0;
                flameLight.SetActive(true);
                frontLight.SetActive(true);
                fuelUI.gameObject.SetActive(true);
            }
            fuel += amount;
            if (fuel > 120) fuel = 120;
        }
        
        private void Locomotion()
        {
            // "Resident-evil-like" controls, AKA Tank controls

            // Forward / Backward
            if (!aim)
            {
                float currentSpeed = input.y >= 0 ? forwardSpeed : backwardSpeed;
                cc.Move(transform.forward * input.y * currentSpeed * Time.fixedDeltaTime);
            }

            // Turn
            float targetRotation = transform.rotation.eulerAngles.x + input.x * rotationSpeed * Time.fixedDeltaTime;
            transform.Rotate(new Vector3(0, targetRotation,0));
        }

        private void UpdateAnim()
        {
            anim.SetFloat("Speed", input.y);
            anim.SetBool("Aiming", aim);
        }

        public void Die()
        {
            isDead = true;
            anim.enabled = false;

            // go ragdoll
            Rigidbody[] ragdoll = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in ragdoll)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.constraints = RigidbodyConstraints.None;
            }
            
            FindObjectOfType<EndScreenManager>().ShowMenu();
        }

        #endregion
    }
}