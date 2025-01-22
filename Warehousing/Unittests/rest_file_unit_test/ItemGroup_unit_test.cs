// using System;
// using System.Net.Http;
// using System.Net.Http.Json;
// using System.Threading.Tasks;
// using System.Text.Json;

// using Xunit;

// public class ItemGroups_Unit_Tests
// {
//     private readonly HttpClient _client;
//     private int _lastCreatedItemGroupId = 999999999;

//     public ItemGroups_Unit_Tests()
//     {
//         _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/v1/") };
//         var apiKey = "a1b2c3d4e5";  
//         _client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
//     }

//     [Fact]
//     public async Task GetAllItemGroups_ShouldReturnSuccess()
//     {
//         // Arrange & Act
//         var response = await _client.GetAsync("itemgroups");
        
//         // Assert
//         response.EnsureSuccessStatusCode();

//         var itemGroups = await response.Content.ReadAsStringAsync();
//         Assert.False(string.IsNullOrEmpty(itemGroups));
//     }

//     [Fact]
//     public async Task AddNewItemGroup_ShouldReturnSuccess()
//     {
//         // Arrange
//         ItemGroup newItemGroup = new ItemGroup  
//         {
//             Id = _lastCreatedItemGroupId,
//             Name = "New Item Group test",
//             Description = "This is a new item group test.",
//             CreatedAt = DateTime.Now,
//             UpdatedAt = DateTime.Now
//         };

//             // Act
//             var response = await _client.PostAsJsonAsync("itemgroups", newItemGroup);
//             var createdItemGroup = await response.Content.ReadAsStringAsync();

//             var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

//             // Assert
//             response.EnsureSuccessStatusCode();
//             Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

//     }

//     [Fact]
//     public async Task GetItemGroupById_ShouldReturnSuccess()
//     {
//         // Arrange & Act
//         var response = await _client.GetAsync($"itemgroups/{_lastCreatedItemGroupId}");
        
//         // Assert
//         response.EnsureSuccessStatusCode();

//         var itemGroup = await response.Content.ReadAsStringAsync();
//         Assert.False(string.IsNullOrEmpty(itemGroup));
//     }

   

//     [Fact]
//     public async Task UpdateItemGroup_ShouldReturnSuccess()
//     {
//         // Arrange
//         var getResponse = await _client.GetAsync("itemgroups");
//         getResponse.EnsureSuccessStatusCode();
//         var allItemGroups = await getResponse.Content.ReadAsStringAsync();
//         var itemGroupsList = JsonSerializer.Deserialize<List<ItemGroup>>(allItemGroups);
//         int totalItemGroups = itemGroupsList.Count;
//         var updatedItemGroup = new
//         {
//             Id = _lastCreatedItemGroupId,
//             Name = "Updated Group Name",
//             Description = "Updated group description.",
//             created_at = "2024-12-14 06:58:09",
//             updated_at = "2024-12-14 06:58:09"
//         };

//         // Act
//         var response = await _client.PutAsJsonAsync($"itemgroups/{totalItemGroups}", updatedItemGroup);

//         // Assert
//         response.EnsureSuccessStatusCode();
//     }

//     [Fact]
//     public async Task DeleteItemGroup_ShouldReturnSuccess()
//     {
//         // Act
//         var response = await _client.DeleteAsync($"itemgroups/{_lastCreatedItemGroupId}");

//         // Assert
//         response.EnsureSuccessStatusCode();
//     }
// }
