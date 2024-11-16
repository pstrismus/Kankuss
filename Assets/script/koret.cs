using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class koret : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] LayerMask mask;
    [SerializeField] Transform ground;
    [SerializeField] float distance = 0f;
    public bool isGrounded;

    [SerializeField] GameObject toplanankankus;

    private string targetTag = "Enemy";  // Hedef nesnenin tag'i
    private float speed = 5f;            // Hareket hýzý
    private float detectionAngle = 45f;  // Algýlama açýsý
    private float detectionRadius = 10f; // Algýlama yarýçapý
    private float rotationSpeed = 5f;    // Rotasyon hýzý
    Rigidbody rb;
    public string animationName="duvarcarp";
    bool duvarcarpbýrak;
    ParticleSystem ps;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<ParticleSystem>();
        StartCoroutine(timer());
    }

    void Update()
    {
        GameObject target = FindClosestTargetWithTag();
        if (target == null)
        {
            haraketet();
        }
        if (target != null)
        {
            hedefegit();
        }


        

    }
    void haraketet()
    {
        

        if (anim.GetBool("iskosu") )
        {
            float xRotation = 0; // Örneðin, x ekseni etrafýnda 45 derece dönme miktarý
            float zRotation = 0f;  // Z ekseni etrafýnda dönme miktarý, þu anda 0

            // Yeni rotasyonu oluþtur
            Quaternion newRotation = Quaternion.Euler(new Vector3(xRotation, transform.rotation.eulerAngles.y, zRotation));
            // Yeni pozisyonu hesapla (sabit yöne doðru)
            Vector3 newPosition = transform.position + transform.forward * speed * Time.deltaTime;

            // Pozisyonu güncelle
            transform.position = newPosition;
            if (duvarcarpbýrak)
            {
                duvarkontrol();
            }
        }
    }
    void hedefegit()
    {
        GameObject target = FindClosestTargetWithTag();
        float mesafe = Vector3.Distance(transform.position, target.transform.position);


        if (anim.GetBool("iskosu") && target != null)
        {
            Vector3 directionToTarget = target.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, directionToTarget);

            float xRotation = 0; // Örneðin, x ekseni etrafýnda 45 derece dönme miktarý
            float zRotation = 0f;  // Z ekseni etrafýnda dönme miktarý, þu anda 0

            // Yeni rotasyonu oluþtur
            Quaternion newRotation = Quaternion.Euler(new Vector3(xRotation, transform.rotation.eulerAngles.y, zRotation));

            // Hedef belirli bir açý içinde ve tag'i "Enemy" ise hareket et
            if (angle < detectionAngle)
            {
                // Yön vektörünü normalize et
                directionToTarget.Normalize();

                // Yön vektöründe y bileþenini sýfýrla, sadece yatay düzlemde döndür
                directionToTarget.y = 0;

                // Hedef yöne doðru yavaþça dön
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Yeni pozisyonu hesapla
                Vector3 newPosition = new Vector3(
                    transform.position.x + directionToTarget.x * speed * Time.deltaTime,
                    transform.position.y,  // Y ekseni sabit kalýyor
                    transform.position.z + directionToTarget.z * speed * Time.deltaTime );
                if (mesafe < 3)
                {
                    anim.SetBool("duvarcarp", false);
                    anim.SetBool("iskosu", false);
                    anim.SetTrigger("kor");
                }

                // Pozisyonu güncelle
                transform.position = newPosition;
            }
        }
    }

    GameObject FindClosestTargetWithTag()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        GameObject closestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (GameObject potentialTarget in targets)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - transform.position;
            float distanceSqr = directionToTarget.sqrMagnitude;

            if (distanceSqr < closestDistanceSqr && distanceSqr < detectionRadius * detectionRadius)
            {
                closestTarget = potentialTarget;
                closestDistanceSqr = distanceSqr;
            }
        }

        return closestTarget;
    }
    void duvar()
    {
        if (!IsAnimationPlaying(animationName))
        {
            anim.SetBool("duvarcarp", true);
            anim.SetBool("iskosu", false);
            rb.isKinematic = true;
            distance = 1;
            float xRotation = 0; // Örneðin, x ekseni etrafýnda 45 derece dönme miktarý
            float zRotation = 0f;  // Z ekseni etrafýnda dönme miktarý, þu anda 0

            // Yeni rotasyonu oluþtur
            Quaternion newRotation = Quaternion.Euler(new Vector3(xRotation, transform.rotation.eulerAngles.y, zRotation));
        }
    }
    void duvarbýrak()
    {
        duvarcarpbýrak = true;
        rb.mass = 30;
        



        // X ve Z ekseni dönme miktarýný ayarla
        float xRotation = 0; // Örneðin, x ekseni etrafýnda 45 derece dönme miktarý
        float zRotation = 0f;  // Z ekseni etrafýnda dönme miktarý, þu anda 0

        // Yeni rotasyonu oluþtur
        Quaternion newRotation = Quaternion.Euler(new Vector3(xRotation, transform.rotation.eulerAngles.y, zRotation));

        // Karakterin rotasyonunu güncelle
        transform.rotation = newRotation;
        rb.isKinematic = false;

    }
    void duvarkontrol()
    {
        if (isGrounded)
        {
            anim.SetBool("duvarcarp", false);
            anim.SetBool("iskosu", true);
            rb.mass = 20;

        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("duvar")&& !IsAnimationPlaying(animationName))
        {
            duvarcarpbýrak = false;
            duvar();
        }
    }
  
    bool IsAnimationPlaying(string animationName)
    {
        // Animator'ýn geçerli oynatma durumunu al
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // Animasyon adýný kontrol et
        return stateInfo.IsName(animationName);
    }
    void yondegis()
    {
        transform.Rotate(Vector3.up, 180.0f);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(ground.position, distance, mask);

        if (isGrounded ) // Bayrak kontrolü ekleyin
        {
            StartCoroutine(bekle());
           
        }
    }

    public IEnumerator bekle()
    {
        yield return new WaitForSeconds(0);
        anim.SetBool("iskosu", true);
        rb.isKinematic = false;
    }
    void koretmek()
    {
        ps.Play();
        Instantiate(toplanankankus, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.2f);
    }
    IEnumerator timer()
    {
        yield return new WaitForSeconds(10);
        Instantiate(toplanankankus, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.1f);
    }
}
