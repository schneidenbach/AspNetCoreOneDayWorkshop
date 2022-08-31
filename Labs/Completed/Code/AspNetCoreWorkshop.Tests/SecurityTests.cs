using System.Net.Http.Json;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AspNetCoreWorkshop.Tests
{
    public class SecurityTests : TestBase
    {
        [Test]
        public async Task Valid_token_is_returned()
        {
            using (var server = CreateTestServer())
            {
                var client = server.CreateClient();
                var resp = await client.PostAsJsonAsync("security/generateToken", new
                {
                    role = "project manager"
                });
                resp.EnsureSuccessStatusCode();
            }
        }
    }
}