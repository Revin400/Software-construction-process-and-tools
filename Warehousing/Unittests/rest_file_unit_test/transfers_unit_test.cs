// using System;
// using System.Net.Http;
// using System.Net.Http.Json;
// using System.Threading.Tasks;
// using Xunit;

// public class Transfers_Unit_Tests
// {
//     private readonly HttpClient _client;

//     public Transfers_Unit_Tests()
//     {
//         _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/") };
//     }

//     [Fact]
//     public async Task GetAllTransfers_ShouldReturnSuccess()
//     {
//         var response = await _client.GetAsync("transfers");
//         response.EnsureSuccessStatusCode();

//         var transfers = await response.Content.ReadAsStringAsync();
//         Assert.False(string.IsNullOrEmpty(transfers));
//     }

//     [Fact]
//     public async Task GetTransferById_ShouldReturnSuccess()
//     {
//         var response = await _client.GetAsync("transfers/1");
//         response.EnsureSuccessStatusCode();

//         var transfer = await response.Content.ReadAsStringAsync();
//         Assert.False(string.IsNullOrEmpty(transfer));
//     }

//     [Fact]
//     public async Task CreateNewTransfer_ShouldReturnSuccess()
//     {
//         var newTransfer = new
//         {
//             reference = "TRANS001",
//             transferFrom = 1,
//             transferTo = 2,
//             transferStatus = "Processed",
//             items = new[]
//             {
//                 new { itemId = 1, amount = 50 },
//                 new { itemId = 2, amount = 30 }
//             }
//         };

//         var response = await _client.PostAsJsonAsync("transfers", newTransfer);
//         response.EnsureSuccessStatusCode();

//         var createdTransfer = await response.Content.ReadAsStringAsync();
//         Assert.False(string.IsNullOrEmpty(createdTransfer));
//     }

//     [Fact]
//     public async Task UpdateTransfer_ShouldReturnSuccess()
//     {
//         var updatedTransfer = new
//         {
//             id = 1,
//             reference = "TRANS001",
//             transferFrom = 1,
//             transferTo = 2,
//             transferStatus = "Completed",
//             items = new[]
//             {
//                 new { itemId = 1, amount = 50 },
//                 new { itemId = 2, amount = 30 }
//             }
//         };

//         var response = await _client.PutAsJsonAsync("transfers/1", updatedTransfer);
//         response.EnsureSuccessStatusCode();

//         var updatedTransferResponse = await response.Content.ReadAsStringAsync();
//         Assert.False(string.IsNullOrEmpty(updatedTransferResponse));
//     }

//     [Fact]
//     public async Task DeleteTransfer_ShouldReturnSuccess()
//     {
//         var response = await _client.DeleteAsync("transfers/1");
//         response.EnsureSuccessStatusCode();

//         var deleteResponse = await response.Content.ReadAsStringAsync();
//         Assert.False(string.IsNullOrEmpty(deleteResponse));
//     }
// }