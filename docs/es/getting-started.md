# Guía de Inicio

<p>
  <a href="../en/getting-started.md">English</a> | <strong>Español</strong>
</p>

Esta guía te llevará paso a paso por el proceso de configuración y uso de **BeyondNet.Aop** para interceptar llamadas a métodos en tu aplicación.

## 1. Instalación

Instala los paquetes principales a través del Gestor de Paquetes de NuGet o el CLI de .NET:

```bash
dotnet add package BeyondNet.Aop
dotnet add package BeyondNet.Aop.DispatchProxy
dotnet add package BeyondNet.Aop.Microsoft.Extensions.DependencyInjection.Aspects.Installer
```

## 2. Definir el Servicio Destino

Primero, vamos a crear un servicio de lógica de negocio estándar y su interfaz. Ten en cuenta que `DispatchProxy` requiere que el destino sea una interfaz.

```csharp
public interface ICalculatorService
{
    int Add(int a, int b);
}

public class CalculatorService : ICalculatorService
{
    public int Add(int a, int b)
    {
        Console.WriteLine("Ejecutando la lógica de negocio real...");
        return a + b;
    }
}
```

## 3. Crear un Aspecto Personalizado

Para crear un aspecto, necesitas dos cosas: un **Atributo** para marcar los métodos, y una **Implementación de Aspecto** que contenga la lógica.

### Definir el Atributo
```csharp
using BeyondNet.Aop;

public class AuditAttribute : AbstractAspectAttribute
{
    public string OperationName { get; set; }

    public AuditAttribute(string operationName)
    {
        OperationName = operationName;
    }
}
```

### Definir la Lógica del Aspecto
Hereda de `AbstractAspect<TAttribute>`. Puedes sobrescribir métodos como `OnEntry`, `OnSuccess`, `OnException` u `OnExit`.

```csharp
using BeyondNet.Aop;
using System;

public class AuditAspect : AbstractAspect<AuditAttribute>
{
    protected override void OnEntry(IJoinPoint joinPoint)
    {
        var attribute = GetAttribute(joinPoint);
        Console.WriteLine($"[Audit] Iniciando operación: {attribute.OperationName}");
        Console.WriteLine($"[Audit] Método Destino: {joinPoint.MethodInfo.Name}");
    }

    protected override void OnSuccess(IJoinPoint joinPoint)
    {
        Console.WriteLine($"[Audit] ¡Operación completada con éxito! Resultado: {joinPoint.Return}");
    }
}
```

## 4. Decorar la Interfaz Destino

Aplica el atributo `[Audit]` al método de la interfaz que deseas interceptar:

```csharp
public interface ICalculatorService
{
    [Audit("Operación de Suma")]
    int Add(int a, int b);
}
```

## 5. Registrar con Inyección de Dependencias

Usa el método de extensión `AddProxy` para registrar tu servicio en la `IServiceCollection`. Esto envolverá automáticamente el servicio en el `AopProxy` y enlazará los aspectos.

```csharp
using Microsoft.Extensions.DependencyInjection;
using BeyondNet.Aop.Microsoft.Extensions.DependencyInjection.Aspects.Installer;

var services = new ServiceCollection();

// Registrar el Aspecto
services.AddTransient<AuditAspect>();

// Registrar el Servicio Envuelto en el Proxy
services.AddProxy<ICalculatorService, CalculatorService>();

var provider = services.BuildServiceProvider();
```

## 6. Ejecutar

Cuando resuelvas el servicio y llames al método, verás la intercepción en acción:

```csharp
var calculator = provider.GetRequiredService<ICalculatorService>();

// ¡Esta llamada será interceptada!
var result = calculator.Add(5, 10);
```

**Salida:**
```
[Audit] Iniciando operación: Operación de Suma
[Audit] Método Destino: Add
Ejecutando la lógica de negocio real...
[Audit] ¡Operación completada con éxito! Resultado: 15
```
