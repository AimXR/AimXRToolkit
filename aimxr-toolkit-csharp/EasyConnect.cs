// Copyright (C) 2023 Antonin Rousseau
// 
// aimxr-toolkit-csharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// aimxr-toolkit-csharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with aimxr-toolkit-csharp. If not, see <http://www.gnu.org/licenses/>.

using UnityEngine.Events;
using LitJson;

namespace AimXRToolkit;

public class EasyConnect
{
    private readonly CancellationTokenSource _cts;

    /// <summary>
    /// Events triggered when the token is received
    /// </summary>
    public UnityEvent<string> OnTokenReceived;
    /// <summary>
    /// Invoke this event when something goes wrong
    /// </summary>
    public UnityEvent<string> OnError;
    /// <summary>
    /// Invoke this event when the easy connect code changes
    /// </summary>
    public UnityEvent<string> OnCodeChanged;

    private string? _code;

    public EasyConnect()
    {
        _cts = new CancellationTokenSource();
    }
    ~EasyConnect()
    {
        _cts.Cancel();
    }
    /// <summary>
    /// Launch the EasyConnect process
    /// </summary>
    public void Launch()
    {
        Task.Run(async () =>
        {
            string code = await AskForCode();
            WaitForEasyResponse(code);
        });
    }
    /// <summary>
    /// Does an API request to get a EasyConnect code, displays it on screen using the TextMeshPro prefab, and launches the EasyConnect waiting loop
    /// </summary>
    private async Task<string> AskForCode()
    {
        var res = await API.ExecuteAsync(API.ROUTE.EASY_GENERATE, API.Method.Get, API.Type.Json);
        JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
        string code = (string)data["code"];
        OnCodeChanged.Invoke(code);
        return code;
    }
    /// <summary>
    /// Stop the EasyConnect waiting loop
    /// </summary>
    public void Cancel()
    {
        _cts.Cancel();
        OnError.Invoke("EasyConnect cancelled");
    }

    /// <summary>
    /// Waits for a easyconnect response (containing a user token to connect with).
    /// Does not continue waiting if [cancelEasyCode] is called
    /// </summary>
    /// <param name="code">EasyConnect code to wait for</param>
    private async void WaitForEasyResponse(string code)
    {

        for (; ; ) // infinite loop
        {
            // API request to get the easy connect code
            var res = await API.ExecuteAsync(API.ROUTE.EASY_CODE + code, API.Method.Get, API.Type.Json);
            if (res.responseCode == 200)
            {
                // Parse token and log in with this token
                JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
                string token = (string)data["token"];
                OnTokenReceived.Invoke(token);
            }
            await Task.Delay(100, _cts.Token);
        }
    }
}