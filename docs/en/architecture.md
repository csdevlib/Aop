# Architecture

<p>
  <strong>English</strong> | <a href="../es/architecture.md">Español</a>
</p>

The BeyondNet.Aop framework is designed for high performance and clean separation of concerns. It leverages the native `.NET` `System.Reflection.DispatchProxy` to build a dynamic interception pipeline at runtime.

## Core Components and Responsibilities

- **`AopProxy<TService, TImplementation>`**: The core proxy class. It acts as a transparent wrapper around the target service, intercepting every method call.
- **`IAspectExecutor`**: Orchestrates the chain of aspects. It determines which aspects apply to the current method and executes them in the correct order.
- **`IJoinPoint`**: The contextual object passed through the pipeline. It holds the method arguments, the `MethodInfo`, the target instance, and provides the `Proceed()` method to continue execution.
- **`AbstractAspect<T>`**: The base class for all aspects. It reads the aspect configuration from the custom attributes decorating the intercepted method.

## The Interception Pipeline

When a method is called on an advised service, the flow goes as follows:

```mermaid
sequenceDiagram
    participant Client
    participant AopProxy
    participant AspectExecutor
    participant Aspect (e.g. Logger)
    participant Target Service

    Client->>AopProxy: Calls Method(args)
    AopProxy->>AspectExecutor: Execute(JoinPoint)
    AspectExecutor->>Aspect: Apply(JoinPoint)
    
    rect rgb(200, 220, 240)
        Note right of Aspect: Pre-execution logic (OnEntry)
    end
    
    Aspect->>AspectExecutor: Proceed()
    AspectExecutor->>Target Service: Invoke Original Method
    Target Service-->>AspectExecutor: Returns Result
    AspectExecutor-->>Aspect: Control Returns
    
    rect rgb(200, 240, 220)
        Note right of Aspect: Post-execution logic (OnExit / OnException)
    end
    
    Aspect-->>AspectExecutor: Return Result
    AspectExecutor-->>AopProxy: Return Result
    AopProxy-->>Client: Return Result
```

## Performance & Caching

Reflection in .NET is historically slow. To ensure that BeyondNet.Aop can be used in highly-concurrent and high-throughput environments, we implemented several caching layers:

1. **Method Resolution Cache**: Maps interface methods to concrete implementation methods in O(1) time.
2. **Attribute Lookup Cache**: `AbstractAspect` heavily caches the lookup of custom attributes via `ConcurrentDictionary`.
3. **Expression Evaluation Cache**: Dynamic string expressions evaluated at runtime (e.g., extracting a nested ID from an object argument) are compiled into fast `Delegate` instances and cached indefinitely.
