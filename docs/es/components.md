# Resumen de Componentes

<p>
  <a href="../en/components.md">English</a> | <strong>Español</strong>
</p>

El framework BeyondNet.Aop está dividido en paquetes modulares de NuGet. Esto permite a los consumidores seleccionar exactamente lo que necesitan sin incluir dependencias innecesarias.

## 1. `BeyondNet.Aop`
El núcleo absoluto del framework. Contiene las abstracciones e interfaces base necesarias para construir y definir un entorno AOP.

**Tipos clave:**
- `IAspect`: La interfaz fundamental de un aspecto.
- `IJoinPoint`: Objeto de contexto para las intercepciones.
- `AbstractAspect<T>`: La clase base genérica de la cual debes heredar para construir tus aspectos personalizados.
- `IPointCut` & `PointCut`: Evaluador por defecto para hacer coincidir los aspectos con los métodos destino.

## 2. `BeyondNet.Aop.DispatchProxy`
La implementación nativa del proxy de .NET. Depende de `BeyondNet.Aop` y usa `System.Reflection.DispatchProxy` para emitir los proxies en tiempo de ejecución.

**Tipos clave:**
- `AopProxy<TService, TImplementation>`: Emite el objeto proxy dinámico que rutea las llamadas al Ejecutor de Aspectos.
- `AspectExecutor`: Administra la cadena de responsabilidad para ejecutar múltiples aspectos aplicados al mismo método.

## 3. `BeyondNet.Aop.Aspects`
Una librería estándar de lógicas transversales pre-construidas que la mayoría de aplicaciones necesitan. 

**Aspectos clave incluidos:**
- `LoggerAspect`: Envuelve las llamadas a métodos con comportamientos de log `OnEntry`, `OnExit`, y `OnException`.
- `RetryAspect`: Reintenta automáticamente la ejecución de un método si se lanzan ciertas excepciones, basándose en un número máximo de intentos definidos.
- `AdviceAspect`: Un aspecto envoltorio genérico que puede inyectar implementaciones arbitrarias de `IAdvice`.
- `Evaluator`: Un evaluador dinámico de expresiones de texto usando `System.Linq.Dynamic.Core`.

## 4. `BeyondNet.Aop.Aspects.Logger.Serilog`
Un paquete adaptador que implementa la interfaz `ILogger` del framework utilizando la popular librería de logs estructurados **Serilog**.

## 5. `BeyondNet.Aop.Aspects.Logger` (Common.Logging)
Un paquete adaptador que implementa la interfaz `ILogger` del framework utilizando **Common.Logging**.

## 6. `BeyondNet.Aop.Microsoft.Extensions.DependencyInjection.Aspects.Installer`
Provee métodos de extensión de integración para `IServiceCollection`. Te permite registrar tus servicios y hacer que el motor de proxies AOP los envuelva automáticamente durante la fase de resolución nativa de Inyección de Dependencias de .NET.
