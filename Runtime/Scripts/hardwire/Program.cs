// See https://aka.ms/new-console-template for more information
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Serialization;
using AimXRToolkit.Interactions;
using AimXRToolkit.Interactions.Proxies;


class Program
{
    static void Main(string[] args)
    {
        UserData.RegisterProxyType<ProxyButton, Button>(r => new ProxyButton(r));
        UserData.RegisterProxyType<ProxyColor, Color>(r => new ProxyColor(r));
        Table dump = UserData.GetDescriptionOfRegisteredTypes(true);
        // dump in a lua file here
        File.WriteAllText("hardwire.lua", dump.Serialize());
    }
}