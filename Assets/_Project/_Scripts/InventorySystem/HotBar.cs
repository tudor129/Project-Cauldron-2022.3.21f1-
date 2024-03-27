using System.Collections;
using System.Collections.Generic;

public class HotBar : Storage
{
    public Memento CreateMemento()
    {
        return new Memento(new List<ItemData>(_items));
    }
    
    public void SetMemento(Memento memento)
    {
        _items = memento.GetItems();
        
        for (int i = 0; i < _slots.Count; i++)
        {
            _slots[i].UpdateUI(_items[i]);
        }
    }
    
    public class Memento
    {
        List<ItemData> _items { get; }
        
        public Memento(List<ItemData> items)
        {
            _items = items;
        }
        
        public List<ItemData> GetItems()
        {
            return _items;
        }
    }
}