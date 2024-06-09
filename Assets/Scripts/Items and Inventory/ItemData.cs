using UnityEngine;
using System.Text;
using UnityEditor;
public enum ItemType
{
    Material,
    Equiment
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Menu/Data")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public string itemId;
    [Range(0, 100)]
    public float chanceDrop;

    protected StringBuilder sb = new StringBuilder();
    public virtual string getDescription()
    {
        return sb.ToString();
    }
    private void OnValidate()
    {
#if UNITY_EDITOR
        string fullPath = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(fullPath);
#endif
    }
}
