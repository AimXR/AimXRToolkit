-- Copyright (C) 2023 Antonin Rousseau
-- 
-- aimxr-toolkit-csharp is free software: you can redistribute it and/or modify
-- it under the terms of the GNU Lesser General Public License as published by
-- the Free Software Foundation, either version 3 of the License, or
-- (at your option) any later version.
-- 
-- aimxr-toolkit-csharp is distributed in the hope that it will be useful,
-- but WITHOUT ANY WARRANTY; without even the implied warranty of
-- MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
-- GNU Lesser General Public License for more details.
-- 
-- You should have received a copy of the GNU Lesser General Public License
-- along with aimxr-toolkit-csharp. If not, see <http://www.gnu.org/licenses/>.
-- Action function of a hypothetical home button on a machine
function createComponent(name, action)
    _G[name] = {}
    if action ~= nil then
        _G[name].Action = action
    else
        _G[name].Action = function()
            print(name .. " interacted")
        end
    end
end

_G["axisZ"] = {}
_G["axisZ"].Home = function()
    print("Axis Z is homing")
end

createComponent("homeBtn", function()
    print("Home button pressed")
    _G["axisZ"].Home()
end)

createComponent("startBtn")

-- emulate an action
_G["homeBtn"].Action()
_G["startBtn"].Action()

function OnPressed()
    for i = 1, 10 do
        for j = 1, 10 do
            for k = 1, 10 do
                _G['color_smoke_power'].interactable.SetColor('#' .. i .. j .. k)
            end
        end
    end
end


