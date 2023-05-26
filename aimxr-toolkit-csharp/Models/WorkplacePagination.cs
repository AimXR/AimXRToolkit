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

namespace AimXRToolkit.Models;

public class WorkplacePagination
{
    private Page<Workplace>? _currentPage;
    private readonly int _size;

    public WorkplacePagination(int size = 10)
    {
        _size = size;
    }

    public async Task<bool> LoadNextPage()
    {
        if (_currentPage == null)
        {
            _currentPage = await AimXRToolkit.Managers.DataManager.Instance.GetWorkplacesAsync(1, _size);
            return true;
        }
        bool res = _currentPage!.HasNextPage();
        if (res)
            _currentPage = await AimXRToolkit.Managers.DataManager.Instance.GetWorkplacesAsync(_currentPage.GetPage() + 1, _size);
        return res;
    }

    public async Task<bool> LoadPreviousPage()
    {
        if (_currentPage == null)
        {
            _currentPage = await AimXRToolkit.Managers.DataManager.Instance.GetWorkplacesAsync(1, _size);
            return true;
        }
        bool res = _currentPage!.HasPreviousPage();
        if (res)
            _currentPage = await AimXRToolkit.Managers.DataManager.Instance.GetWorkplacesAsync(_currentPage.GetPage() - 1, _size);
        return res;
    }

    public int GetTotal() => _currentPage?.GetTotal() ?? 0;
}