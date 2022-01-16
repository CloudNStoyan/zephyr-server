using System.Diagnostics;
using System.Management;
using System.Net.Http.Headers;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Zephyr.DAL;

namespace Zephyr.Areas.LoL.LCU
{
    [SupportedOSPlatform("windows")]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LcuService
    {
        private static LcuInfo? GetPortAndToken()
        {
            var processes = Process.GetProcessesByName("LeagueClientUx");

            if (processes.Length == 0)
            {
                return null;
            }

            var process = processes[0];

            using var searcher = new ManagementObjectSearcher(
                $"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {process.Id}");
            using var objects = searcher.Get();
            string? cmdLine = objects.Cast<ManagementBaseObject>().SingleOrDefault()?["CommandLine"]?.ToString();

            if (cmdLine == null)
            {
                return null;
            }

            string port = Regex.Match(cmdLine, @"(?<=--app-port=)\d+").Value;
            string token = Regex.Match(cmdLine, "(?<=--remoting-auth-token=).*?(?=\")").Value;

            return new LcuInfo
            {
                Port = port,
                Token = token
            };
        }

        private HttpClient? CreateClient()
        {
            var lcuInfo = GetPortAndToken();

            if (lcuInfo == null)
            {
                return null;
            }

            // LCU server doesn't have proper SSL
            // so we just bypass it all, thus making a security risk
            var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;

            var client = new HttpClient(clientHandler);

            client.BaseAddress = new Uri($"https://127.0.0.1:{lcuInfo.Port}/");

            string base64EncodedAuthString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"riot:{lcuInfo.Token}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthString);

            return client;
        }

        public async Task<RunePageDto[]?> GetRunePages()
        {
            using var client = this.CreateClient();

            if (client == null)
            {
                return null;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, "lol-perks/v1/pages/");

            var response = await client.SendAsync(request);
            string json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<RunePageDto[]>(json);
        }

        public async Task DeleteRunePage(int pageId)
        {
            using var client = this.CreateClient();

            if (client == null)
            {
                return;
            }

            string resource = $"lol-perks/v1/pages/{pageId}";

            var request = new HttpRequestMessage(HttpMethod.Delete, resource);

            await client.SendAsync(request);
        }

        public async Task<RunePageDto?> CreateRunePage(RunePageDto runePageDto)
        {
            using var client = this.CreateClient();

            if (client == null)
            {
                return null;
            }

            const string resource = "lol-perks/v1/pages/";

            var request = new HttpRequestMessage(HttpMethod.Post, resource);

            string json = JsonConvert.SerializeObject(runePageDto, Formatting.Indented);

            request.Content = new StringContent(json, Encoding.ASCII, "application/json");

            var response = await client.SendAsync(request);

            return JsonConvert.DeserializeObject<RunePageDto>(await response.Content.ReadAsStringAsync());
        }

        public async Task PutCurrentRunePage(int pageId)
        {
            using var client = this.CreateClient();

            if (client == null)
            {
                return;
            }

            const string resource = "lol-perks/v1/currentpage";

            var request = new HttpRequestMessage(HttpMethod.Put, resource);

            string json = JsonConvert.SerializeObject(pageId, Formatting.Indented);

            request.Content = new StringContent(json, Encoding.ASCII, "application/json");

            await client.SendAsync(request);
        }

        public async Task ImportPage(RunePagePoco runePagePoco)
        {
            var currentRunePages = await this.GetRunePages();

            if (currentRunePages == null || currentRunePages.Length == 0)
            {
                return;
            }

            var currentRunePage = currentRunePages[0];

            using var client = this.CreateClient();

            if (client == null)
            {
                return;
            }

            await this.DeleteRunePage(currentRunePage.Id);

            var runePageDto = new RunePageDto
            {
                Id = currentRunePage.Id,
                Name = $"{runePagePoco.Name} - Zephyr",
                PrimaryStyleId = runePagePoco.PrimaryStyleId,
                SelectedPerkIds = runePagePoco.PerkIds.Cast<int>().ToArray(),
                SubStyleId = runePagePoco.SubStyleId
            };

            var createdRunePage = await this.CreateRunePage(runePageDto);

            if (createdRunePage == null)
            {
                return;
            }

            await this.PutCurrentRunePage(createdRunePage.Id);
        }
    }

    public class LcuInfo
    {
        public string Port { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
