# BeyondNet.Aop [![NuGet](https://img.shields.io/nuget/v/BeyondNet.Aop.svg)](https://www.nuget.org/packages/BeyondNet.Aop) 
Just another library to do aspect oriented programming

Derived from `raulnq/Jal.Aop`, licensed under Apache-2.0. The `BeyondNet.Aop`
namespace and package family are maintained in this repository.

## Platform support

The solution targets .NET 10 LTS and runs on Windows and Linux. Install a .NET
10 SDK and build or test from the repository root:

```bash
dotnet restore BeyondNet.Aop.sln
dotnet test BeyondNet.Aop.sln --configuration Release --no-restore
```

Pull requests and changes to `main` are tested on both `ubuntu-latest` and
`windows-latest`.

## How to use?
Create your aspects
```csharp
public class AddAttribute : AbstractAspectAttribute
{
    public object[] Context { get; set; }
}

public class Add10Attribute : AbstractAspectAttribute
{

}

public class Multiple5Attribute : AbstractAspectAttribute
{

}

public class Subtract20Attribute : AbstractAspectAttribute
{

}

public class Add : OnMethodBoundaryAspect<AddAttribute>
{
    protected override void OnExit(IJoinPoint joinPoint)
    {
        var context = CurrentAttribute.Context;

        var add = (int)context[0];

        var value = (int)joinPoint.Return + add;

        joinPoint.Return = value;
    }
}

public class Add10 : OnMethodBoundaryAspect<Add10Attribute>
{
    protected override void OnExit(IJoinPoint joinPoint)
    {
        var value = (int)joinPoint.Return + 10;

        joinPoint.Return = value;
    }
}

public class Multiple5 : OnMethodBoundaryAspect<Multiple5Attribute>
{
    protected override void OnExit(IJoinPoint joinPoint)
    {
        var value = (int)joinPoint.Return * 5;

        joinPoint.Return = value;
    }
}

public class Subtract20 : OnMethodBoundaryAspect<Subtract20Attribute>
{
    protected override void OnExit(IJoinPoint joinPoint)
    {
        var value = (int)joinPoint.Return - 20;

        joinPoint.Return = value;
    }
}
```
Use your aspects
```csharp
public interface INumberProvider
{
    int Get1(int seed);

    int Get2(int seed);

    int Get3(int seed);

    int Get4(int seed);
}

public class NumberProvider : INumberProvider
{
    [LoggerAspect(Type=typeof(SerilogLogger), LogArguments = new string[] { "seed" }, LogReturn =true, LogDuration =true, LogException =true)]
    public int Get4(int seed)
    {
        return seed;
    }

    [AdviceAspect(Type = typeof(AddAdvice))]
    public int Get3(int seed)
    {
        return seed;
    }

    [Add(Context = new object[] { 10 })]
    public int Get1(int seed)
    {
        return seed;
    }

    [Add10(Order = 1)]
    [Multiple5(Order = 2)]
    [Subtract20(Order = 3)]
    public int Get2(int seed)
    {
        return seed;
    }
}
```
## Microsoft.Extensions.DependencyInjection [![NuGet](https://img.shields.io/nuget/v/BeyondNet.Aop.Microsoft.Extensions.DependencyInjection.Aspects.Installer.svg)](https://www.nuget.org/packages/BeyondNet.Aop.Microsoft.Extensions.DependencyInjection.Aspects.Installer)

```csharp
var services = new ServiceCollection();

services.AddAop(c =>
{
    c.AddAspect<Add10>();
    c.AddAspect<Multiple5>();
    c.AddAspect<Subtract20>();
});

// Scoped is the default and aligns with ASP.NET Core request services.
services.AddAopProxy<INumberProvider, NumberProvider>();

using var root = services.BuildServiceProvider();
using var scope = root.CreateScope();
var provider = scope.ServiceProvider.GetRequiredService<INumberProvider>();

var seed = 5;

var value = provider.Get2(seed);
```

Only `Microsoft.Extensions.DependencyInjection` is supported. Intercepted
services are registered explicitly with `AddAopProxy`; the library does not
scan the container or create a service locator. Use scoped or transient proxy
lifetimes; singleton proxies are rejected because aspects may depend on
request-scoped services.
