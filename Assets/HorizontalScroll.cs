using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HorizontalScroll : MonoBehaviour
{
    public RectTransform rectTransform;
    public float _screenWidth = 375f;
    public Button nextButton;
    public float speed = 10f;
    

    private bool isMoving = false;
    Vector2 targetPosition;
    int _pagenumber=0;

    // Start is called before the first frame update
    void Start()
    {
        
        nextButton.onClick.AddListener(Move);

    }

    // Update is called once per frame
    void Update()
    {
       if (isMoving)
        {
           

            /*else //(_pagenumber > 2)
            {
                targetPosition = new Vector2(-_screenWidth - _screenWidth - _screenWidth, rectTransform.anchoredPosition.y);
                nextButton.transform.SetParent(rectTransform);
            }*/
           
           
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * speed);
            //Debug.Log(Vector2.Distance(rectTransform.anchoredPosition, targetPosition));
            if (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) <=0.05f)
            {
                isMoving = false;
               

            }
        }
    }
   
    public void Move()
    {
        //targetPosition = new Vector2(rectTransform.anchoredPosition.x - distance, rectTransform.anchoredPosition.y);
        _pagenumber++;
        if (_pagenumber == 1)
        {
            targetPosition = new Vector2(-_screenWidth, rectTransform.anchoredPosition.y);
            isMoving = true;
        }
        else if (_pagenumber == 2)
        {
            targetPosition = new Vector2(-_screenWidth - _screenWidth, rectTransform.anchoredPosition.y);
            nextButton.onClick.AddListener(() => SceneManager.LoadScene("SignUporSignIn"));

        }
        Debug.Log("Page Number = " + _pagenumber);
        
    }
}
