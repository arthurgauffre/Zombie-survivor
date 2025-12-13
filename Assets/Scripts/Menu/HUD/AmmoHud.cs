using UnityEngine;
using TMPro;

public class AmmoHud : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    
    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        if (ammoText != null)
        {
            ammoText.text = $"{currentAmmo} / {maxAmmo}";
        }
    }

    public void UpdateAmmo(int currentAmmo)
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo.ToString();
        }
    }
}
