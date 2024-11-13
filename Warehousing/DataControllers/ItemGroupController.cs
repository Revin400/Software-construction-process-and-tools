using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Controllers
{
    [ApiController]
    [Route("api/itemgroup")]
    public class ItemGroupController : ControllerBase
    {
        private readonly ItemGroupService _service;

        public ItemGroupController()
        {
            _service = new ItemGroupService();
        }

        [HttpGet]
        public ActionResult<List<ItemGroup>> GetItemGroups()
        {
            return _service.GetItemGroups();
        }

        [HttpGet("{id}")]
        public ActionResult<ItemGroup> GetItemGroup(int id)
        {
            var itemGroup = _service.GetItemGroup(id);
            if (itemGroup == null)
            {
                return NotFound();
            }
            return itemGroup;
        }

        [HttpPost]
        public ActionResult AddItemGroup(ItemGroup itemGroup)
        {
            _service.AddItemGroup(itemGroup);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult UpdateItemGroup(int id, ItemGroup itemGroup)
        {
            _service.UpdateItemGroup(id, itemGroup);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult RemoveItemGroup(int id)
        {
            _service.RemoveItemGroup(id);
            return Ok();
        }
    }
}