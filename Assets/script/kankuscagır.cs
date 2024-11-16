using UnityEngine;
using System.Collections;

public class kankuscagır : MonoBehaviour
{
    RaycastHit hit;
    public LayerMask layerMask;
    GameObject kankus;
    [SerializeField] GameObject kankus_kor;
    [SerializeField] GameObject kankus_spike;
    [SerializeField] LineRenderer _lr;
    [SerializeField] bool islineactive;
    [SerializeField] Camera playerCamera;
    [SerializeField] Transform laserOrigin;
    [SerializeField] float lineborder = 50;
     


    Vector3 hitPoint;

    [SerializeField] float scroolspeed = -0.5f;
    [SerializeField] private float offsetok;
    private Material ok;

    [SerializeField] GameObject spikeon;
    GameObject spikeonclone;

    [SerializeField] float weaponRange = 50.0f; // Sabit mesafe
    [SerializeField] Transform shootPoint;

    
    public static Vector3 hedefPozisyonu;

    void Start()
    {
        _lr = GetComponent<LineRenderer>();
        ok = _lr.materials[0];
    }

    void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            islineactive = !islineactive;
        }

        setArrow();
    }

    void setArrow()
    {
        if (islineactive)
        {
            spike_takip();
            _lr.enabled = true;

            Vector3 laserOriginPosition = laserOrigin.position;
            laserOriginPosition.y = 0.326f;
            _lr.SetPosition(0, laserOriginPosition);

            Vector3 rayOrigin = laserOrigin.position;
            Vector3 LaserOriginforwardDirection = laserOrigin.transform.forward;
            LaserOriginforwardDirection.Normalize();

            hitPoint = rayOrigin + (LaserOriginforwardDirection * lineborder);

            if (Physics.Raycast(rayOrigin, laserOrigin.transform.forward, out hit, lineborder, layerMask))
            {
                hitPoint = hit.point;
            }
            else
            {
                hitPoint = rayOrigin + (LaserOriginforwardDirection * lineborder);
            }

            hitPoint.y = 0.326f;
            _lr.SetPosition(1, hitPoint);

            if (Input.GetMouseButtonDown(0))
            {
                kankus = kankus_kor;
                kankusolustur();
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (spikeonclone != null)
                {
                    kankus = kankus_spike;
                    hedefPozisyonu = spikeonclone.transform.position; // Hedef pozisyonunu ayarla
                    kankusolustur();
                }
            }
            okharaket();
        }
        else
        {
            _lr.enabled = false;
            Destroy(spikeonclone);
        }
    }

    private void kankusolustur()
    {
        GameObject bebekankus = Instantiate(kankus);
        Vector3 offset = gameObject.transform.forward * 1f + gameObject.transform.up / 2;
        bebekankus.transform.position = gameObject.transform.position + offset;

        Vector3 rotation = gameObject.transform.rotation.eulerAngles;
        bebekankus.transform.rotation = Quaternion.Euler(0, rotation.y, 0);
    }

    private void okharaket()
    {
        offsetok -= scroolspeed * Time.deltaTime;
        ok.mainTextureOffset = new Vector2(offsetok, 0);
    }

    private void spike_takip()
    {
        if (spikeonclone != null)
        {
            if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, weaponRange))
            {
                if (hit.collider.CompareTag("spikebolgesi"))
                {
                    spikeonclone.transform.position = hit.point;
                }
                else
                {
                    Destroy(spikeonclone);
                }
            }
        }
        else
        {
            if (islineactive)
            {
                if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, weaponRange))
                {
                    if (hit.collider.CompareTag("spikebolgesi"))
                    {
                        spikeonclone = Instantiate(spikeon, hit.point, Quaternion.identity);
                    }
                }
            }
        }
    }
}
