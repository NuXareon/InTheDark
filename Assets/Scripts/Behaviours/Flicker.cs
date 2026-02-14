using UnityEngine;
using System.Collections;

public class Flicker : MonoBehaviour, ITriggerOverrideInterface
{
    public bool OnInteraction(GameObject Trigger, GameObject OtherObject)
    {
        return StartFlicker();
    }

    public bool StartFlicker()
    {
        Light LightComponent = GetComponent<Light>();
        if (LightComponent != null)
        {
            StartCoroutine(DoFlicker(LightComponent));
            return true;
        }
        Debug.LogError(transform.name + ": Flicker component present in game object without a light. This interaction will not work.");
        return false;
    }

    IEnumerator DoFlicker(Light light)
    {
        float wait = Random.Range(0.2f, 0.5f);
        light.enabled = !light.enabled;
        yield return new WaitForSeconds(wait);
        light.enabled = !light.enabled;
        wait = Random.Range(0.2f, 0.4f);
        yield return new WaitForSeconds(wait);
        light.enabled = !light.enabled;
        wait = Random.Range(0.1f, 0.3f);
        yield return new WaitForSeconds(wait);
        light.enabled = !light.enabled;
        wait = Random.Range(0.1f, 0.2f);
        yield return new WaitForSeconds(wait);
        light.enabled = !light.enabled;
        wait = Random.Range(0.05f, 0.1f);
        yield return new WaitForSeconds(wait);
        light.enabled = !light.enabled;
        wait = Random.Range(0.02f, 0.05f);
        yield return new WaitForSeconds(wait);
        light.enabled = !light.enabled;
        wait = Random.Range(0.02f, 0.05f);
        yield return new WaitForSeconds(wait);
        light.enabled = !light.enabled;
        wait = Random.Range(0.01f, 0.03f);
        yield return new WaitForSeconds(wait);
        light.enabled = !light.enabled;
    }
}
