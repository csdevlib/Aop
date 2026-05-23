# Glosario

Este glosario define los términos comunes utilizados en el framework BeyondNet.Aop y en el ecosistema general de la Programación Orientada a Aspectos (AOP).

<p>
  <a href="../en/glossary.md">English</a> | <strong>Español</strong>
</p>

## Términos AOP

- **Aspecto (Aspect)**: Una modularización de una preocupación transversal que corta a través de múltiples clases. En BeyondNet.Aop, los aspectos son clases que heredan de `AbstractAspect<T>`. Ejemplos incluyen logging, reintentos y manejo de errores.
- **Punto de Unión (Join Point)**: Un punto durante la ejecución de un programa, como la ejecución de un método. En este framework, está representado por la interfaz `IJoinPoint`, dando acceso a los argumentos del método, valores de retorno y metadatos.
- **Consejo (Advice)**: La acción tomada por un aspecto en un *Join Point* particular. Define *qué* código se ejecuta cuando se dispara un aspecto.
- **Corte de Punto (Pointcut)**: Un predicado que coincide con los *Join Points*. Los *Pointcuts* determinan *dónde* debe aplicarse un consejo. En nuestro framework, esto se maneja mediante atributos y la clase `PointCut` evaluando `CanApply`.
- **Objeto Destino (Target Object)**: El objeto que está siendo asesorado por uno o más aspectos. Esta es la clase original de lógica de negocio.
- **Proxy AOP**: Un objeto creado por el framework AOP para implementar los contratos del aspecto. BeyondNet.Aop usa `System.Reflection.DispatchProxy` para generar dinámicamente estos proxies en tiempo de ejecución.
- **Preocupación Transversal (Cross-cutting Concern)**: Funcionalidad que afecta a múltiples partes de una aplicación. Abarca múltiples capas o módulos.

## Términos Específicos del Framework

- **Evaluador (Evaluator)**: Un componente (`IEvaluator`) responsable de parsear y ejecutar expresiones lambda dinámicas (usando `System.Linq.Dynamic.Core`), utilizado principalmente para extraer claves dinámicas o datos en tiempo de ejecución sin usar reflexión pesada manual.
- **Instalador (Installer)**: Un componente de inyección de dependencias (extensión de `IServiceCollection`) usado para registrar tus objetos destino, implementaciones de proxy y aspectos en el contenedor nativo de .NET.
