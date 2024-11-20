using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/itemline")]
public class ItemLineController : ControllerBase
{
    private readonly ItemLineService _service;

    public ItemLineController()
    {
        _service = new ItemLineService();
    }

    [HttpGet]
    public ActionResult<List<ItemLine>> GetItemLines()
    {
        return _service.GetItemLines();
    }

    [HttpGet("{id}")]
    public ActionResult<ItemLine> GetItemLine(int id)
    {
        var itemLine = _service.GetItemLine(id);
        if (itemLine == null)
        {
            return NotFound();
        }
        return itemLine;
    }

    [HttpPost]
    public ActionResult AddItemLine(ItemLine itemLine)
    {
        _service.AddItemLine(itemLine);
        return Ok();
    }

    [HttpPut("{id}")]
    public ActionResult UpdateItemLine(int id, ItemLine itemLine)
    {
        _service.UpdateItemLine(id, itemLine);
        return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult RemoveItemLine(int id)
    {
        _service.RemoveItemLine(id);
        return Ok();
    }
}
