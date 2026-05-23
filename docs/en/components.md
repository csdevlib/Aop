# Components Overview

<p>
  <strong>English</strong> | <a href="../es/components.md">Español</a>
</p>

The BeyondNet.Aop framework is split into modular NuGet packages. This allows consumers to pick exactly what they need without taking on unnecessary dependencies.

## 1. `BeyondNet.Aop`
The absolute core of the framework. Contains the base abstractions and interfaces necessary to build and define an AOP environment.

**Key types:**
- `IAspect`: The fundamental aspect interface.
- `IJoinPoint`: Context object for interceptions.
- `AbstractAspect<T>`: The generic base class you should inherit from to build your custom aspects.
- `IPointCut` & `PointCut`: Default evaluator to match aspects to target methods.

## 2. `BeyondNet.Aop.DispatchProxy`
The native .NET proxy implementation. Depends on `BeyondNet.Aop` and uses `System.Reflection.DispatchProxy` to emit proxies at runtime.

**Key types:**
- `AopProxy<TService, TImplementation>`: Emits the dynamic proxy object that routes calls to the Aspect Executor.
- `AspectExecutor`: Manages the chain of responsibility for executing multiple aspects applied to the same method.

## 3. `BeyondNet.Aop.Aspects`
A standard library of pre-built, common cross-cutting concerns that most applications need. 

**Key aspects included:**
- `LoggerAspect`: Wraps method calls with `OnEntry`, `OnExit`, and `OnException` logging behaviors.
- `RetryAspect`: Automatically retries method executions if certain exceptions are thrown, based on defined max attempts.
- `AdviceAspect`: A generic wrapper aspect that can inject arbitrary custom `IAdvice` implementations.
- `Evaluator`: A dynamic string expression evaluator using `System.Linq.Dynamic.Core`.

## 4. `BeyondNet.Aop.Aspects.Logger.Serilog`
An adapter package that implements the framework's `ILogger` interface using the popular **Serilog** structured logging library.

## 5. `BeyondNet.Aop.Aspects.Logger` (Common.Logging)
An adapter package that implements the framework's `ILogger` interface using **Common.Logging**.

## 6. `BeyondNet.Aop.Microsoft.Extensions.DependencyInjection.Aspects.Installer`
Provides integration extensions for `IServiceCollection`. It allows you to register your services and have them automatically wrapped by the AOP proxy engine during the native .NET DI resolution phase.
