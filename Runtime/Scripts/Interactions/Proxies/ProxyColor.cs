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

using MoonSharp.Interpreter;

namespace AimXRToolkit.Interactions.Proxies
{
    [MoonSharp.Interpreter.MoonSharpUserData]
    public class ProxyColor
    {
        private readonly Interactions.Color _color;
        public ProxyColor(Interactions.Color color)
        {
            _color = color;
        }

        public void SetColor(string hexa)
        {
            _color.SetColor(hexa);
        }
        public string GetColor()
        {
            return _color.GetColor();
        }
    }
}
