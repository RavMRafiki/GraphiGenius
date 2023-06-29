using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GraphiGenius.MVVM.Model
{
    public static class SendToApi
    {
        public static async Task<ScheduleResponse> send_to_api(ScheduleRequest requestData)
        {
            using (HttpClient client = new HttpClient())
            {
                // Set the API endpoint URL
                string apiUrl = "http://127.0.0.1:5000/generate";

                // Prepare the request payload

                string jsonPayload = JsonConvert.SerializeObject(requestData);

                // Send the POST request and receive the response
                HttpResponseMessage response = await client.PostAsync(apiUrl, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Parse the JSON response into a three-dimensional array
                    var scheduleArray = JsonConvert.DeserializeObject<ScheduleResponse>(jsonResponse);
                    for (int x = 0; x < scheduleArray.work_schedule.Count; x++)
                    {
                        for (int y = 0; y < scheduleArray.work_schedule[x].Count; y++)
                        {
                            for (int z = 0; z < scheduleArray.work_schedule[x][y].Count; z++)
                            {
                                Debug.WriteLine(scheduleArray.work_schedule[x][y][z]+" ");
                            }
                        }
                    }
                    return scheduleArray;
                } 
                else
                {
                    // Handle any error that occurred during the request
                    Debug.WriteLine($"HTTP Error: {response.StatusCode}");
                    return null;
                }
            }
        }
    }
}
