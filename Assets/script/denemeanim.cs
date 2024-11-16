using System.Collections;
using UnityEngine;

public class denemeanim : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] LayerMask mask;
    [SerializeField] Transform ground;
    [SerializeField] float distance = 0f;
    public bool isGrounded;

    // Animasyonun tetiklendiði kontrolü
    Rigidbody rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

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
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}

