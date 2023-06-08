## MoonSharp HardWiring
A chaque ajout de fonctionnalité via MoonSharp, il faut mettre à jour le fichier HardWire.cs

Procédure:
- Se déplacer dans le dossier hardwire
- ```bash
  cd hardwire
  ```
- Lancer le projet dotnet
- ```bash
  dotnet run
  ```
- Lancer MoonSharp repl
- ```bash
  moonsharp -W hardwire.lua ../HardWire.cs --internals --class:Hardwire --namespace:AimXRToolkit
  ```

## Build
```bash
dotnet build --configuration Release
```

## Build with Helpers inside the dll
```bash
dotnet build --configuration Release /p:Editor=true
```
> You can't reference UnityEditor when building a unity project

## deploy
```bash
Copy-Item -Path ".\bin\Release\net4.7.2\aimxr-toolkit-csharp.dll" -Destination "PATH_TO_PLUGINS_FOLDER_IN_UNITY_PROJECT"
```