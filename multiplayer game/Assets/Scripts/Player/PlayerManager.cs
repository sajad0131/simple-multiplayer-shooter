using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Project.Networking;
using Unity.VisualScripting;

namespace Project.Player
{
    public class PlayerManager : MonoBehaviour
    {
        private NetworkIdentity ni;
        [SerializeField]
        private float speed = 1.5f;
        [SerializeField]
        private float runSpeed = 3f;

        public Vector2 moveAxis;
        public bool shootButton;
        public bool secondShootButton;
        public Vector2 aimAxis;
        public bool reloadButton;
        public bool run;
        private bool isToggling;
        private bool ToggleOnOff;
        [SerializeField]
        private float walkMoveCameraTime = 1f;

        public Transform Camera;


        // [ Shooting variables ]
        [SerializeField]
        private float nextTimeToFire;
        [SerializeField]
        private int curentBullets;
        [SerializeField]
        private float fireRate;
        public GameObject hitEffect;
        public GameObject impactDust;

        public float maxDistance;
        public RaycastHit hit;

        public Transform GunOrigin;
        
        public ParticleSystem muzzle;
        [SerializeField]
        private GameObject muzzleLight;
        public string hitID;
        [SerializeField]
        private float timeOfMuzzleLight = 0.05f;
        [SerializeField]
        private AudioClip shootSound;
        
        public AudioSource gunAudioSource;
        //  [ Health Variables ]
        [SerializeField]
        public int gunDamageAmount { get; private set; } = 10;
        public int healthChecker = 0;
        public int health;



        [SerializeField]
        private float movingCameraHeight;

        [SerializeField]
        private TextMeshProUGUI totalBulletText;
        [SerializeField]
        private TextMeshProUGUI currentBulletText;
        private int totalBullets=180;
        private int magBullets=30;
        public bool canShoot;

        public Transform lookRoot;
        void Start()
        {
            canShoot = true;
            ni = gameObject.GetComponent<NetworkIdentity>();
            
            
            currentBulletText = GameObject.FindGameObjectWithTag("CurrentBulletText").GetComponent<TextMeshProUGUI>();
            totalBulletText = GameObject.FindGameObjectWithTag("TotalBulletText").GetComponent<TextMeshProUGUI>();
            totalBulletText.text = totalBullets.ToString();
            currentBulletText.text = curentBullets.ToString();
            


        }

        // Update is called once per frame
        void Update()
        {
            if (ni.IsControlling())
            {
                gameObject.GetComponent<CameraManager>().VirtualCamera(aimAxis,moveAxis);
                Camera = GameObject.FindGameObjectWithTag("TpsCamera").transform;
                checkShooting();
                checkMovement();
                Reload();
            }
            else
            {
                Camera.GetComponent<Camera>().enabled = false;
            }
        }


        private void checkShooting()
        {
            if (shootButton && Time.time >= nextTimeToFire && curentBullets > 0 && canShoot == true  || secondShootButton && Time.time >= nextTimeToFire && curentBullets > 0 && canShoot == true)
            {
                nextTimeToFire = Time.time + (1f / fireRate);
                ShootEffect();
                CallSendShoot();
                curentBullets--;
                currentBulletText.text = curentBullets.ToString();
                


                if (Physics.Raycast(Camera.position, Camera.transform.forward, out hit, maxDistance))
                {
                    Debug.DrawRay(GunOrigin.position, Camera.transform.forward);
                    
                    
                    if (hit.transform.tag == "Player")
                    {
                        hitID = hit.transform.GetComponent<NetworkIdentity>().GetID();
                        healthChecker++;

                    }
                    else
                    {
                        ShootEffect(hit);
                    }

                }

                
            }
            else
            {

            }
        }

        void ShootEffect(RaycastHit hit)
        {
            muzzle.Play(true);
            AudioSource shootSource = Instantiate(gunAudioSource, GunOrigin, GunOrigin);
            shootSource.clip = shootSound;
            shootSource.Play();
            StartCoroutine("Flicker");

            var hole = Instantiate(hitEffect);
            var dust = Instantiate(impactDust);
            hole.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            hole.transform.position = hit.point + hit.normal * 0.002f;
            dust.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            dust.transform.position = hit.point + hit.normal * 0.002f;

        }
        void ShootEffect()
        {
            muzzle.Play(true);
            AudioSource shootSource = Instantiate(gunAudioSource, GunOrigin, GunOrigin);
            shootSource.clip = shootSound;
            shootSource.Play();
            StartCoroutine("Flicker");
            

        }



        IEnumerator Flicker()
        {
            muzzleLight.GetComponent<Light>().enabled = true;
            yield return new WaitForSeconds(timeOfMuzzleLight);
            muzzleLight.GetComponent<Light>().enabled = false;
        }


        private void checkMovement()
        {

            float vertical = moveAxis.y;
            float horizontal = moveAxis.x;

            if (vertical > 0.5 || horizontal > 0.5 || vertical < -0.5 || horizontal < -0.5)
            {
                cameraMover();
                

                if (!run)
                {
                    transform.Translate(horizontal * speed * Time.deltaTime, 0f, vertical * speed * Time.deltaTime);
                    if (!isToggling)
                    {
                        StartCoroutine(toggleWait(walkMoveCameraTime));
                    }
                }
                else
                {
                    transform.Translate(horizontal * speed * Time.deltaTime, 0f, vertical * runSpeed * Time.deltaTime);
                    if (!isToggling)
                    {
                        StartCoroutine(toggleWait(walkMoveCameraTime));
                    }
                }
            }



        }
        void cameraMover()
        {
            if (ToggleOnOff)
            {
                Camera.Translate(0, Time.deltaTime * movingCameraHeight, 0);

            }
            else
            {
                Camera.Translate(0, Time.deltaTime * movingCameraHeight * -1, 0);
            }
        }


        IEnumerator toggleWait(float t)
        {
            isToggling = true;
            ToggleOnOff = true;
            yield return new WaitForSeconds(t);
            ToggleOnOff = false;
            yield return new WaitForSeconds(t);
            isToggling = false;
        }

        public void CallSendShoot()
        {
            gameObject.GetComponent<NetworkShoot>().SendShoot();
        }

        void Reload()
        {
            if (reloadButton == true)
            {

                
                
                if (totalBullets >= magBullets)
                {
                    int difference = magBullets - curentBullets;
                    curentBullets = magBullets;
                    totalBullets -= difference;
                    currentBulletText.text = curentBullets.ToString();
                    StartCoroutine(reloadTime(1.8f));

                }
                if (totalBullets < magBullets)
                {
                    int difference = magBullets - curentBullets;
                    if (totalBullets >= difference)
                    {
                        curentBullets = magBullets;
                        totalBullets -= difference;
                        currentBulletText.text = curentBullets.ToString();
                        StartCoroutine(reloadTime(1.8f));
                    }
                    else
                    {
                        curentBullets += totalBullets;
                        totalBullets = 0;
                        currentBulletText.text = curentBullets.ToString();
                        StartCoroutine(reloadTime(1.8f));
                    }

                }

                totalBulletText.text = totalBullets.ToString();
            }
        }

        IEnumerator reloadTime(float s)
        {
            canShoot = false;
            yield return new WaitForSeconds(s);
            canShoot = true;
            
        }






    }
}