using UnityEngine;

public class AutoOff : MonoBehaviour
{
    public float TimeToDeactive = 3;
    float counter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter  > TimeToDeactive)
        {
            gameObject.SetActive(false);
        }
    }
}
