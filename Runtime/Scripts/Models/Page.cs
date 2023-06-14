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
using UnityEngine;
namespace AimXRToolkit.Models
{
    public class Page<T> where T : IPaginable
    {
        private readonly List<T> _items;
        private readonly int _total;
        private readonly int _page;
        private readonly int _size;
        private readonly int _pages;

        public Page(LitJson.JsonData data)
        {
            _items = new List<T>();
            foreach (LitJson.JsonData item in data["items"])
            {
                _items.Add((T)Activator.CreateInstance(typeof(T), item));
            }
            _total = (int)data["total"];
            _page = (int)data["page"];
            _size = (int)data["size"];
            _pages = (int)data["pages"];
        }

        public bool HasNextPage()
        {
            return _page < _pages;
        }
        public bool HasPreviousPage()
        {
            return _page > 1;
        }
        public List<T> GetItems()
        {
            return _items;
        }
        public int GetTotal()
        {
            return _total;
        }
        public int GetPage()
        {
            return _page;
        }
        public int GetSize()
        {
            return _size;
        }
        public int GetPages()
        {
            return _pages;
        }
    }
}

