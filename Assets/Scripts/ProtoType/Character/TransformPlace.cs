using UnityEngine;

public class TransformPlace: MonoBehaviour
{
    public TransformType type;
    public GameObject TransformPlaceEffect;
    private void Update()
    {
        if (PlayerHandler.instance.OnDeformField)
        {
            TransformPlaceEffect.gameObject.SetActive(false);
        }
        else
        {
            TransformPlaceEffect.gameObject.SetActive(true);
        }
    }
    public virtual void transformStart(GameObject other)
    {
       
            other.transform.position = this.transform.position;
            PlayerHandler.instance.LastTransformPlace = this;
            gameObject.SetActive(false);

            other.GetComponent<Player>().FormChange(type);
   
    }

    //private void OnTriggerStay(Collider other)
    //{ 

    //    if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F) || other.CompareTag("Player"))
    //    {
    //        if (PlayerHandler.instance.CurrentPlayer.onTransform)
    //        {
    //            if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.X))
    //            {                   
    //                if (!PlayerHandler.instance.OnDeformField)
    //                {
    //                    transformStart(other);

    //                }
    //            }
    //        }                
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            Debug.Log("트랜스폼 플레이스 체크");
            DownAttackCollider p;
            if (other.TryGetComponent<DownAttackCollider>(out p))
            {
                transformStart(PlayerHandler.instance.CurrentPlayer.gameObject);
            }
                //if (PlayerHandler.instance.CurrentPlayer.onTransform)
                //{
                //    PlayerHandler.instance.CurrentPlayer.onTransform = false;
                //    transformStart(other);
                //}
                //else
                //{
                //    PlayerHandler.instance.CurrentPlayer.onTransform = true;
                //}
            }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        PlayerHandler.instance.CurrentPlayer.onTransform = false;
    //    }
    //}
}
