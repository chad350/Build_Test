using UnityEngine;
using UnityEngine.UI;
public class UIButtonManager : MonoBehaviour
{
    [SerializeField] Image targetImg; 
    public void Click_RedButton()
    {
        targetImg.color = Color.red;
    }
    
    public void Click_BlueButton()
    {
        targetImg.color = Color.blue;
    }
    
    public void Click_GreenButton()
    {
        targetImg.color = Color.green;
    }
}
