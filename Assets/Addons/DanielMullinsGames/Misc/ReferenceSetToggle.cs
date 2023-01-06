using System.Collections.Generic;

public class ReferenceSetToggle
{
    private HashSet<object> references = new HashSet<object>();

    public bool True    
    {
        get 
        {
            references.RemoveWhere(x => x == null);
            return references.Count > 0;
        }
    }

    public void Add(object obj)
    {
        references.Add(obj);
    }

    public void Remove(object obj)
    {
        references.Remove(obj);
    }
}
