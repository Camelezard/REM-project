using UnityEngine;

public class CustomingCategoryButton : MonoBehaviour
{
    [SerializeField] private DressType dressType;

    [SerializeField] private Transform buttonContainer; 
    [SerializeField] private CustomizeButton buttonPrefab;

    public void OnClick()
    {
        Generate();
    }

    private void Generate()
    {
        Clear();

        foreach (DressLabelTypeEnum label in System.Enum.GetValues(typeof(DressLabelTypeEnum)))
        {
            CreateButton(label);
        }
    }

    private void CreateButton(DressLabelTypeEnum label)
    {
        CustomizeButton btn = Instantiate(buttonPrefab, buttonContainer);

        btn.dressLabelType = label;
        btn.dressType = dressType;
    }

    private void Clear()
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
