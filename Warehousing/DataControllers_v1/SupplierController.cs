using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Warehousing.DataServices_v1;

namespace Warehousing.DataControllers_v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly SupplierService _supplierService = new SupplierService();
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "suppliers.json");

        [HttpGet]
        public IActionResult GetAllSuppliers()
        {
            try
            {
                var suppliers = _supplierService.ReadsuppliersFromJson();
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error reading suppliers: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetSuppliersById(int id)
        {
            id = id - 1;
            try
            {
                var suppliers = _supplierService.ReadsuppliersFromJson();
                if (id < 0 || id >= suppliers.Count)
                {
                    return StatusCode(200, $"null");
                }
                return Ok(suppliers[id]);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error reading suppliers: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Createsupplier([FromBody] JsonElement supplier)
        {
            try
            {
                var suppliers = _supplierService.ReadsuppliersFromJson();

                var supplierWithId = JsonSerializer.Serialize(supplier);
                suppliers.Add(JsonDocument.Parse(supplierWithId).RootElement);

                _supplierService.WritesuppliersToJson(suppliers);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating suppliers: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Updatesupplier(int id, [FromBody] JsonElement supplier)
        {
            try
            {
                var suppliers = _supplierService.ReadsuppliersFromJson();
                var updatedSuppliers = JsonSerializer.Serialize(supplier);
                suppliers[id - 1] = JsonDocument.Parse(updatedSuppliers).RootElement;

               _supplierService.WritesuppliersToJson(suppliers);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating supplier: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Deletesupplier(int id)
        {          
            System.IO.File.WriteAllText(_filePath, "[]");
            return Ok();
           
            
        }
    }
}