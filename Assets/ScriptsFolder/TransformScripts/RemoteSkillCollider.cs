using System.Linq;
using UnityEngine;

public class RemoteSkillCollider : MonoBehaviour
{
    public RemoteTransform remocon;

    private void Update()
    {
        transform.localPosition = new Vector3((int)PlayerStat.instance.direction * Mathf.Abs(transform.localPosition.x), transform.localPosition.y, transform.localPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            RemoteObject r;
            if(other.TryGetComponent<RemoteObject>(out r))
            {
                if(r.CanControl)
                    remocon.remoteObj.Add(r);
            }
      
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            RemoteObject r;
            if (other.TryGetComponent<RemoteObject>(out r))
            {
                if (remocon.remoteObj.Contains(r))
                    remocon.remoteObj.Remove(r);
            }
           
        }
    }
}
