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

using SocketIOClient;
using UnityEngine;
using Newtonsoft.Json;
using SocketIOClient.Newtonsoft.Json;

namespace AimXRToolkit;

class EasyLink
{
    private readonly SocketIO _socket;
    private int _roomId;
    public EasyLink()
    {
        _socket = new SocketIO(API.API_URL, new SocketIOOptions
        {
            Auth = false,
            Reconnection = true,
            ReconnectionDelay = 3000,
            ReconnectionDelayMax = 60 * 1000,
            Path = "/ws/sockets",
        });
        _socket.JsonSerializer = new NewtonsoftJsonSerializer();
        _socket.OnConnected += (sender, e) => Debug.Log("Connected");
        _socket.OnReconnectAttempt += (sender, e) => Debug.Log("ReconnectAttempt");

        _socket.On("roomCreated", (data) =>
        {
            Debug.Log("roomCreated");
            Debug.Log(data);
        });

    }
    public async void Connect()
    {
        await _socket.ConnectAsync();
        await _socket.EmitAsync("createRoom", "test");

    }
}