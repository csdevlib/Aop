# Glossary

This glossary defines the common terms used throughout the BeyondNet.Aop framework and the general Aspect-Oriented Programming (AOP) ecosystem.

<p>
  <strong>English</strong> | <a href="../es/glossary.md">Español</a>
</p>

## AOP Terms

- **Aspect**: A modularization of a concern that cuts across multiple classes. In BeyondNet.Aop, aspects are classes that inherit from `AbstractAspect<T>`. Examples include logging, retries, and error handling.
- **Join Point**: A point during the execution of a program, such as the execution of a method. In this framework, it is represented by the `IJoinPoint` interface, giving access to method arguments, return values, and metadata.
- **Advice**: The action taken by an aspect at a particular join point. It defines *what* code executes when an aspect is triggered.
- **Pointcut**: A predicate that matches join points. Pointcuts determine *where* an advice should be applied. In our framework, this is handled by attributes and the `PointCut` class evaluating `CanApply`.
- **Target Object**: The object being advised by one or more aspects. This is the original business logic class.
- **AOP Proxy**: An object created by the AOP framework in order to implement the aspect contracts. BeyondNet.Aop uses `System.Reflection.DispatchProxy` to dynamically generate these proxies at runtime.
- **Cross-cutting Concern**: Functionality that affects multiple parts of an application. It spans across multiple layers or modules.

## Framework Specific Terms

- **Evaluator**: A component (`IEvaluator`) responsible for parsing and executing dynamic lambda expressions (using `System.Linq.Dynamic.Core`), primarily used for extracting dynamic keys or data at runtime without hardcoded reflection.
- **Installer**: A dependency injection component (`IServiceCollection` extension) used to register your target objects, proxy implementations, and aspects into the .NET host container.
