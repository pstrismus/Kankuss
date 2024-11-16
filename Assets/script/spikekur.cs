using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikekur : MonoBehaviour
{
    [SerializeField] private float hiz = 1f; // H�z de�i�keni
    private Animator anim; // Animator referans�
    [SerializeField] private bool hedefeUlasildi = false; // Hedefe ula��l�p ula��lmad���n� kontrol eden bayrak

    [SerializeField] private GameObject spike;
    [SerializeField] private GameObject toplanankankus;// Hedef pozisyonunu belirleyen GameObject
    Vector3 hedefpoz;

    void Start()
    {
        anim = GetComponent<Animator>(); // Animator bile�enini al
        hedefpoz = kankuscag�r.hedefPozisyonu; // Hedef pozisyonunu ba�lat
    }

    void HedefiTakipEt()
    {
        // Nesne ile hedef pozisyon aras�ndaki mesafeyi hesaplay�n
        float mesafe = Vector3.Distance(transform.position, hedefpoz);

        if (mesafe > 0.5953739f)
        {
            // Hedef pozisyona do�ru hareket ettirin
            transform.position = Vector3.MoveTowards(transform.position, hedefpoz, hiz * Time.deltaTime);
        }
        else
        {
            hedefeUlasildi = true; // Hedefe ula��ld���n� i�aretle
            anim.SetBool("iskosu", false); // "iskosu" animasyon parametresini false yap
            anim.SetTrigger("deneme"); // "deneme" animasyonunu tetikle
        }
    }

   void spikeyerle�tir()
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
