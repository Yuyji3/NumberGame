using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    public int number = 0;

    public void SetNumber(int newNumber)
    {
        number = newNumber;
        numberText.text = number.ToString();
        numberText.enabled = true;
    }

    public void Clear()
    {
        number = 0;
        numberText.text = "";
        numberText.enabled = false;
    }

    public bool IsEmpty()
    {
        return number == 0;
    }
}
