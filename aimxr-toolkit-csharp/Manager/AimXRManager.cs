// Copyright (C) 2023 Antonin
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



namespace AimXRToolkit.Managers;
using MoonSharp.Interpreter;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class AimXRManager : MonoBehaviour
{
    private static AimXRManager? _Instance;
    public static AimXRManager Instance => _Instance ?? throw new System.Exception("AimXRManager is not initialized");
    private Models.User? _user;

    [SerializeField]
    public string API_URL = "http://localhost:8000";
    private void Awake()
    {
        API.API_URL = API_URL;
        // prevent creating multiple instances , delete the new one
        if (_Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _Instance = this;
        Hardwire.Initialize();
        // TODO: init Moonsharp
        // UserData.RegisterProxyType<>(p => new Proxy());
        DontDestroyOnLoad(gameObject);
    }
    void OnEnable()
    {
        _Instance = this;
    }
    public void SetUser(Models.User? user)
    {
        _user = user;
    }
    public Models.User? GetUser()
    {
        return _user;
    }
}