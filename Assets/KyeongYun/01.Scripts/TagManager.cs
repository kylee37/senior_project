using UnityEngine;

public enum Tag
{
    defaultTag,
    nature
}
public class TagManager : MonoBehaviour
{
    public Tag currentTag = Tag.defaultTag;

    public void UpdateState(Tag newTag)
    {
        currentTag = newTag;
    }
}