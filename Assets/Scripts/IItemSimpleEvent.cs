/// <summary>
/// Acts like a monoEvent, don't really want to use delegates just for this one.
/// </summary>
public interface IItemSimpleEvent
{    void OnItemSpawn(Item item);    void OnItemPicked(Item item);    void OnItemDie(Item item);    void OnItemDisable(Item item);}