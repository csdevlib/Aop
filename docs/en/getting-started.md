# Getting Started

<p>
  <strong>English</strong> | <a href="../es/getting-started.md">Español</a>
</p>

This guide will walk you through the process of setting up and using **BeyondNet.Aop** to intercept method calls in your application.

## 1. Installation

Install the core packages via the NuGet Package Manager or .NET CLI:

```bash
dotnet add package BeyondNet.Aop
dotnet add package BeyondNet.Aop.DispatchProxy
dotnet add package BeyondNet.Aop.Microsoft.Extensions.DependencyInjection.Aspects.Installer
```

## 2. Define the Target Service

First, let's create a standard business logic service and its interface. Note that `DispatchProxy` requires the target to be an interface.

```csharp
public interface ICalculatorService
{
    int Add(int a, int b);
}

public class CalculatorService : ICalculatorService
{
    public int Add(int a, int b)
    {
        Console.WriteLine("Executing actual business logic...");
        return a + b;
    }
}
```

## 3. Create a Custom Aspect

To create an aspect, you need two things: an **Attribute** to mark the methods, and an **Aspect implementation** containing the logic.

### Define the Attribute
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

### Define the Aspect Logic
Inherit from `AbstractAspect<TAttribute>`. You can override methods like `OnEntry`, `OnSuccess`, `OnException`, or `OnExit`.

```csharp
using BeyondNet.Aop;
using System;

public class AuditAspect : AbstractAspect<AuditAttribute>
{
    protected override void OnEntry(IJoinPoint joinPoint)
    {
        var attribute = GetAttribute(joinPoint);
        Console.WriteLine($"[Audit] Starting operation: {attribute.OperationName}");
        Console.WriteLine($"[Audit] Target Method: {joinPoint.MethodInfo.Name}");
    }

    protected override void OnSuccess(IJoinPoint joinPoint)
    {
        Console.WriteLine($"[Audit] Operation completed successfully! Result: {joinPoint.Return}");
    }
}
```

## 4. Decorate the Target Interface

Apply the `[Audit]` attribute to the interface method you want to intercept:

```csharp
public interface ICalculatorService
{
    [Audit("Addition Operation")]
    int Add(int a, int b);
}
```

## 5. Register with Dependency Injection

Use the `AddProxy` extension method to register your service into the `IServiceCollection`. This will automatically wrap the service in the `AopProxy` and link the aspects.

```csharp
using Microsoft.Extensions.DependencyInjection;
using BeyondNet.Aop.Microsoft.Extensions.DependencyInjection.Aspects.Installer;

var services = new ServiceCollection();

// Register the Aspect
services.AddTransient<AuditAspect>();

// Register the Service Wrapped in the Proxy
services.AddProxy<ICalculatorService, CalculatorService>();

var provider = services.BuildServiceProvider();
```

## 6. Execute

When you resolve the service and call the method, you will see the interception in action:

```csharp
var calculator = provider.GetRequiredService<ICalculatorService>();

// This call will be intercepted!
var result = calculator.Add(5, 10);
```

**Output:**
```
[Audit] Starting operation: Addition Operation
[Audit] Target Method: Add
Executing actual business logic...
[Audit] Operation completed successfully! Result: 15
```
