# Avaruz.FrameWork.Controls.Win

Avaruz.FrameWork.Controls.Win es una biblioteca de controles personalizados para aplicaciones Windows Forms en .NET 9.0. Proporciona componentes avanzados como asistentes (wizards), cuadros de entrada, controles de DataGridView personalizados y m�s, facilitando el desarrollo de interfaces de usuario ricas y modernas.

## Caracter�sticas

- Controles de asistente (Wizard) con p�ginas personalizables.
- Cuadros de entrada (InputBox) mejorados.
- Columnas y celdas DataGridView con soporte para m�scaras.
- Controles visuales como PaneCaption y botones personalizados.
- Integraci�n con recursos (.resx) y soporte para localizaci�n.

## Requisitos

- .NET 9.0 (Windows)
- Visual Studio 2022 o superior

## Instalaci�n

1. Clona el repositorio:
```
git clone https://github.com/Avaruz/Avaruz.FrameWork
```
2. Abre el archivo `Avaruz.FrameWork.Controls.Win.csproj` en Visual Studio.
3. Restaura los paquetes NuGet y compila el proyecto.

## Uso

Agrega una referencia al proyecto en tu soluci�n y utiliza los controles en tus formularios Windows Forms. Ejemplo de uso de un Wizard:

```csharp
using Avaruz.FrameWork.Controls.Win.Wizard;

// Crear y mostrar un asistente
var wizard = new Wizard();
wizard.Pages.Add(new WizardPage());
wizard.Show();
```

## Dependencias

- [Microsoft.Windows.Compatibility](https://www.nuget.org/packages/Microsoft.Windows.Compatibility)
- [System.Resources.Extensions](https://www.nuget.org/packages/System.Resources.Extensions)

## Licencia

Copyright 2025 Avaruz.
