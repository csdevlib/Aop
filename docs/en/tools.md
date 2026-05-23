# Tools & Utilities

<p>
  <strong>English</strong> | <a href="../es/tools.md">Español</a>
</p>

The BeyondNet.Aop framework integrates tightly with well-known tools in the .NET ecosystem to prevent reinventing the wheel.

## Microsoft Dependency Injection (DI)

Instead of requiring a custom IoC container, this framework integrates directly with `Microsoft.Extensions.DependencyInjection` (the built-in .NET core DI container).

### `AddProxy<TService, TImplementation>`
This installer extension is the main utility to map a service to its implementation while wrapping it in an `AopProxy`.

**Purpose:** 
To easily enable AOP interception on any registered service in a familiar ASP.NET Core-style `Startup.cs` or `Program.cs`.

## Logging Tools

The framework does not force you to use a specific logging provider. Instead, it offers an `ILogger` abstraction within the aspects and adapter packages for popular libraries:

### `SerilogLogger`
**Purpose:** Provides highly structured, high-performance logging via Serilog. It correctly maps the `IJoinPoint` metadata into Serilog properties like `{ClassName}` and `{MethodName}`, which makes querying logs in Seq or ElasticSearch extremely efficient.

### `CommonLoggingLogger`
**Purpose:** Provides backward compatibility or integration with the older `Common.Logging` facade. (Note: Internally optimized using `StringBuilder` to minimize GC pressure).

## Expression Evaluator

### `Evaluator` (System.Linq.Dynamic.Core)
**Purpose:** Sometimes an aspect needs to read data from a deeply nested property in an object passed as an argument to the intercepted method. Instead of hardcoding reflection, the `Evaluator` tool allows you to pass a string expression (e.g. `"Order.Customer.Id"`) in the aspect attribute. The `Evaluator` parses this string and executes it against the runtime `IJoinPoint` arguments.

**Performance Note:** Evaluated expressions are strongly cached as compiled `Delegate` instances to ensure parsing only happens once per method signature.
