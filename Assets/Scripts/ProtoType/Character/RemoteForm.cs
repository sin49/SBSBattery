using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteForm : Player
{
    //public float searchRange;

    [Header("��¡ ��ų ����")]
    public float handleMaxTime; // �ִ� ��¡ �ð�
    float handletimer; // ��¡ Ÿ�̸� (�ð��� �����ϴ� ��ŭ ���� ����)
    public float handlediameterrangemax; // ��¡ �ִ� ����
    public float handlediameterrangemin; // ��¡ �ּ� ����
    public SphereCollider handlerange; // ��¡ ���� �ݶ��̴�

    public RectTransform electricCharge;
    public float holdSpeed; // ���� �ӵ�
    public List<GameObject> remoteObj; // Ž�� ������ ����� ��ȣ�ۿ� ������Ʈ ����
    public float timeScale; // ��¡ ���� ���� ���� ����
    public float chargeSpeed; // ��¡ �ӵ� ����

    public bool Charging;

    [Header("�� ���� ����")]
    public GameObject laserPrefab; // �� ��ų ������
    
    private void Awake()
    {
        //handlerange.
        handlerange = transform.Find("Sphere").GetComponent<SphereCollider>();
    }

    /*private void Update()
    {
        SearchRemoteObject();
    }*/

    public override void Skill1()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (!handlerange.gameObject.activeSelf)
            {
                handlerange.gameObject.SetActive(true);
            }

            Charging = true;
            handletimer += Time.deltaTime;
            if (handletimer >= handleMaxTime)
            {
                Debug.Log("������ �ִ�ġ�Դϴ�");
            }
            else
            {
                /*if (handlerange.radius > handlediameterrangemax)
                {
                    handlerange.radius = handlediameterrangemax;
                }
                else
                {
                    handlerange.radius += Time.deltaTime;
                }*/
                if (timeScale > handlediameterrangemax)
                {
                    handlerange.transform.localScale = new Vector3(handlediameterrangemax, handlediameterrangemax, 0);
                }
                else
                {
                    timeScale += Time.deltaTime;
                    handlerange.transform.localScale = new Vector3(chargeSpeed * timeScale, chargeSpeed * timeScale, 0);
                }
            }
        }

        if(!Input.GetKey(KeyCode.S) && Charging)
        {
            /*if (handlerange.radius < handlediameterrangemin)
            {
                handlerange.radius = handlediameterrangemin;
            }*/
            Charging = false;

            if (timeScale < handlediameterrangemin)
            {
                handlerange.transform.localScale = new Vector3(handlediameterrangemin, handlediameterrangemin, 0);
            }
            //handlerange.gameObject.SetActive(true);
            handlerange.enabled = true;
            ActiveRemoteObject();
        }
    }

    public override void Skill2()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Instantiate(laserPrefab, firePoint.position, transform.rotation);
        }
    }

    #region ���������Ǿ� �õ�
    /*public void SearchRemoteObject()
    {
        Collider[] searchColliders = Physics.OverlapSphere(transform.position, searchRange);

        Debug.Log($"�ݶ��̴� Ž���� >> {searchColliders.Length}, {searchColliders[0].gameObject}");

        for (int i = 0; i < searchColliders.Length; i++)
        {
            if (searchColliders[i].CompareTag("GameController"))
            {
                Debug.Log("���������� ��Ʈ�� ������ ������Ʈ Ž����");
                //SaveRemoteObject(searchColliders[i].gameObject);
            }
        }
    }*/

    /*public void SaveRemoteObject(GameObject remote)
    {
        Debug.Log($"������ ������Ʈ�� ����:{remote}");
        for (int i = 0; i < remoteObj.Count; i++)
        {            
            if (remote != remoteObj[i])
            {
                continue;
            }
            else
            {
                remoteObj.Add(remote);
            }
        }
    }*/

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, searchRange);
    }*/
    #endregion

    public void ActiveRemoteObject()
    {
        for (int i = 0; i < remoteObj.Count; i++)
        {
            remoteObj[i].GetComponent<RemoteObject>().Active();
        }

        remoteObj.Clear();        
        handlerange.transform.localScale = new Vector3(0, 0, 0);
        handletimer = 0;
        timeScale = 0;
        handlerange.enabled = false;
    }

    IEnumerator ElectricPower()
    {
        yield return new WaitForSeconds(1f);


    }
}
