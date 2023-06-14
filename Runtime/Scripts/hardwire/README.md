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