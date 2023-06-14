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
using UnityEngine;
using LitJson;

namespace AimXRToolkit;

/// <summary>
/// Class for handling the EasyConnect process
/// </summary>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="EasyConnect"/> class.
    /// </summary>
    public EasyConnect()
    {
        _cts = new CancellationTokenSource();
        OnTokenReceived = new UnityEvent<string>();
        OnError = new UnityEvent<string>();
        OnCodeChanged = new UnityEvent<string>();
    }
    /// <summary>
    /// Finalizer for the EasyConnect class. Cancels the CancellationTokenSource.
    /// </summary>
    ~EasyConnect()
    {
        _cts.Cancel();
    }
    /// <summary>
    /// Launch the EasyConnect process
    /// </summary>
    public async Task LaunchAsync()
    {

        var res = await AskForCode();
        WaitForEasyResponse(res.code, res.password);
    }
    /// <summary>
    /// Does an API request to get a EasyConnect code, displays it on screen using the TextMeshPro prefab, and launches the EasyConnect waiting loop
    /// </summary>
    private async Task<EasyResponse> AskForCode()
    {
        var res = await API.GetAsync(API.ROUTE.EASY_GENERATE);
        JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
        string code = (string)data["code"];
        string password = (string)data["password"];
        OnCodeChanged.Invoke(code);
        return new EasyResponse { code = code, password = password };
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
    /// <param name="password"></param>
    private async void WaitForEasyResponse(string code, string password)
    {

        for (; ; ) // infinite loop
        {
            // API request to get the easy connect code
            var res = await API.ExecuteAsync(API.ROUTE.EASY_CODE + code + "?password=" + password, API.Method.Get, API.Type.Json);
            if (res.responseCode == 200)
            {
                // Parse token and log in with this token
                JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
                string token = (string)data["token"];
                OnTokenReceived.Invoke(token);
                return;
            }
            await Task.Delay(100, _cts.Token);
        }
    }

    struct EasyResponse
    {
        public string code;
        public string password;
    }
}