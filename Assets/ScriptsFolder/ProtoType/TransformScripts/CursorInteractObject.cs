using UnityEngine;

public class CursorInteractObject : MonoBehaviour
{
    public bool caught, thrown;

    public Collider cursorTargetCollider;
    SphereCollider sphere;
    BoxCollider box;

    private void Awake()
    {
        cursorTargetCollider = GetComponent<Collider>();
        if (cursorTargetCollider is SphereCollider)
        {
            sphere = GetComponent<SphereCollider>();
        }
        else if (cursorTargetCollider is BoxCollider)
        {
            box = GetComponent<BoxCollider>();
        }

    }

    public void CaughtTypeCheck()
    {
        Enemy enemy;
        if (TryGetComponent<Enemy>(out enemy))
        {
            enemy.onStun = true;
            if(enemy.animaor !=null)
            enemy.animaor.SetTrigger("Caught");
            if (enemy.skinRenderer != null)
            {
                Material[] materials = enemy.skinRenderer.materials;
                materials[1] = enemy.hittedMat;
                enemy.skinRenderer.materials = materials;
            }
        }
    }

    public float ColliderEndPoint()
    {
        Vector3 fPoint = Vector3.zero;
        float size=0;
        if (cursorTargetCollider is SphereCollider)
        {
            size = sphere.radius;

            fPoint = transform.forward * size;
        }
        else if (cursorTargetCollider is BoxCollider)
        {
            fPoint = transform.right * (box.size.x / 2);
            size = box.bounds.size.x / 2;
        }
        Debug.Log(fPoint);
        return size;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (caught)
        {
            Enemy enemy;
            if (TryGetComponent<Enemy>(out enemy))
            {
                DamagedByPAttack script;
                if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.TryGetComponent<DamagedByPAttack>(out script))
                {
                    script.Damaged(1);
                    enemy.Dead();
                }
                else if (collision.gameObject.CompareTag("InteractivePlatform") || collision.gameObject.CompareTag("Ground"))
                {
                    gameObject.layer = LayerMask.NameToLayer("Character");
                    caught = false;
                    enemy.Dead();
                    /*enemy.animaor.SetTrigger("Landing");
                    enemy.onStun = false;

                    if (enemy.skinRenderer != null)
                    {
                        Material[] materials = enemy.skinRenderer.materials;
                        materials[1] = enemy.idleMat;
                        enemy.skinRenderer.materials = materials;
                    }*/
                }
            }
        }
    }
}
