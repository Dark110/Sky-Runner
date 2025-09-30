using UnityEngine;

public class Autoff : MonoBehaviour
{
  public float timetodeactivate = 2f;
    float counter;
    void Start()
    {
        
    }
    private void OnEnable()
    {
        counter = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter > Time.deltaTime)
        {
            gameObject.SetActive(false);
        }
    }
}
