// using System;
// using System.Net.Http;
// using System.Net.Http.Json;
// using System.Threading.Tasks;
// using Xunit;



// // update en delete functies werken nog niet 100 volgens de unit tests
// public class Inventories_Unit_Tests
// {
//     private readonly HttpClient _client;

//     public Inventories_Unit_Tests()
//     {
//         _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/") };
//     }

//     [Fact]
//     public async Task GetAllInventories_ShouldReturnSuccess()
//     {
//         var response = await _client.GetAsync("inventories");
//         response.EnsureSuccessStatusCode();

//         var inventories = await response.Content.ReadAsStringAsync();
//         Assert.False(string.IsNullOrEmpty(inventories));
//     }

//     [Fact]
//     public async Task GetInventoriesByItemId_ShouldReturnSuccess()
//     {
//         var response = await _client.GetAsync("inventories/item/1");
//         response.EnsureSuccessStatusCode();

//         var inventories = await response.Content.ReadAsStringAsync();
//         Assert.False(string.IsNullOrEmpty(inventories));
//     }

//     [Fact]
//     public async Task CreateNewInventory_ShouldReturnSuccess()
//     {
//         var newInventory = new
//         {
//             Id = 1,
//             ItemId = 2,
//             Description = "Sample Inventory",
//             ItemReference = "ITEM001",
//             LocationId = 1,
//             TotalOnHand = 100,
//             TotalExpected = 100,
//             TotalOrdered = 50,
//             TotalAllocated = 30,
//             TotalAvailable = 20
//         };

//         var response = await _client.PostAsJsonAsync("inventories", newInventory);
//         response.EnsureSuccessStatusCode();

//         var createdInventory = await response.Content.ReadAsStringAsync();
//         Assert.False(string.IsNullOrEmpty(createdInventory));
//     }

//     [Fact]
//     public async Task UpdateInventory_ShouldReturnSuccess()
//     {
//         var updatedInventory = new
//         {
//             Id = 1,
//             ItemId = 1,
//             Description = "Sample Inventory",
//             ItemReference = "ITEM001",
//             LocationId = 1,
//             TotalOnHand = 150,
//             TotalExpected = 150,
//             TotalOrdered = 70,
//             TotalAllocated = 40,
//             TotalAvailable = 40
//         };

//         var response = await _client.PutAsJsonAsync("inventories/1", updatedInventory);
//         response.EnsureSuccessStatusCode();

//         var updatedInventoryResponse = await response.Content.ReadAsStringAsync();
//         Assert.False(string.IsNullOrEmpty(updatedInventoryResponse));
//     }

//     [Fact]
//     public async Task DeleteInventory_ShouldReturnSuccess()
//     {
//         var response = await _client.DeleteAsync("inventories/1");
//         response.EnsureSuccessStatusCode();

//         var deleteResponse = await response.Content.ReadAsStringAsync();
//         Assert.False(string.IsNullOrEmpty(deleteResponse));
//     }
// }