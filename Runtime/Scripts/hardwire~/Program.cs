﻿// See https://aka.ms/new-console-template for more information

using System.IO;
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
        UserData.RegisterProxyType<ProxySound, Sound>(r => new ProxySound(r));
        UserData.RegisterProxyType<ProxySwitch, Switch>(r => new ProxySwitch(r));
        UserData.RegisterProxyType<ProxyGrabbable, Grabbable>(r => new ProxyGrabbable(r));
        UserData.RegisterProxyType<ProxySlider, Slider>(r => new ProxySlider(r));
        UserData.RegisterProxyType<ProxyHinge, Hinge>(r => new ProxyHinge(r));
        Table dump = UserData.GetDescriptionOfRegisteredTypes(true);
        // dump in a lua file here
        File.WriteAllText("hardwire.lua", dump.Serialize());
    }
}