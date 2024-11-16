using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikekur : MonoBehaviour
{
    [SerializeField] private float hiz = 1f; // Hýz deðiþkeni
    private Animator anim; // Animator referansý
    [SerializeField] private bool hedefeUlasildi = false; // Hedefe ulaþýlýp ulaþýlmadýðýný kontrol eden bayrak

    [SerializeField] private GameObject spike;
    [SerializeField] private GameObject toplanankankus;// Hedef pozisyonunu belirleyen GameObject
    Vector3 hedefpoz;

    void Start()
    {
        anim = GetComponent<Animator>(); // Animator bileþenini al
        hedefpoz = kankuscagýr.hedefPozisyonu; // Hedef pozisyonunu baþlat
    }

    void HedefiTakipEt()
    {
        // Nesne ile hedef pozisyon arasýndaki mesafeyi hesaplayýn
        float mesafe = Vector3.Distance(transform.position, hedefpoz);

        if (mesafe > 0.5953739f)
        {
            // Hedef pozisyona doðru hareket ettirin
            transform.position = Vector3.MoveTowards(transform.position, hedefpoz, hiz * Time.deltaTime);
        }
        else
        {
            hedefeUlasildi = true; // Hedefe ulaþýldýðýný iþaretle
            anim.SetBool("iskosu", false); // "iskosu" animasyon parametresini false yap
            anim.SetTrigger("deneme"); // "deneme" animasyonunu tetikle
        }
    }

   void spikeyerleþtir()
    {
        Instantiate(spike, hedefpoz, Quaternion.identity);
        Instantiate(toplanankankus, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void Update()
    {
        if (anim.GetBool("iskosu") )
        {
            HedefiTakipEt();
        }
    }
}
