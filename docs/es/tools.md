# Herramientas y Utilidades

<p>
  <a href="../en/tools.md">English</a> | <strong>Español</strong>
</p>

El framework BeyondNet.Aop se integra estrechamente con herramientas reconocidas en el ecosistema .NET para evitar reinventar la rueda.

## Inyección de Dependencias de Microsoft (DI)

En lugar de forzar el uso de un contenedor IoC personalizado, este framework se integra directamente con `Microsoft.Extensions.DependencyInjection` (el contenedor DI incorporado de .NET core).

### `AddProxy<TService, TImplementation>`
Esta extensión instaladora es la utilidad principal para mapear un servicio a su implementación mientras se le envuelve en un `AopProxy`.

**Propósito:** 
Habilitar fácilmente la intercepción AOP en cualquier servicio registrado mediante el ya familiar estilo ASP.NET Core en `Startup.cs` o `Program.cs`.

## Herramientas de Logging

El framework no te obliga a usar un proveedor de logging específico. En su lugar, ofrece una abstracción `ILogger` dentro de los aspectos y paquetes adaptadores para librerías populares:

### `SerilogLogger`
**Propósito:** Provee logging altamente estructurado y de alto rendimiento vía Serilog. Mapea correctamente la metadata del `IJoinPoint` en propiedades de Serilog como `{ClassName}` y `{MethodName}`, lo cual hace que la búsqueda de logs en sistemas como Seq o ElasticSearch sea extremadamente eficiente.

### `CommonLoggingLogger`
**Propósito:** Provee compatibilidad hacia atrás o integración con la fachada más antigua `Common.Logging`. (Nota: Optimizado internamente usando `StringBuilder` para minimizar la presión sobre el Garbage Collector).

## Evaluador de Expresiones

### `Evaluator` (System.Linq.Dynamic.Core)
**Propósito:** En ocasiones, un aspecto necesita leer datos de una propiedad profundamente anidada en un objeto que se pasó como argumento al método interceptado. En lugar de escribir código duro con reflexión manual, la herramienta `Evaluator` te permite pasar una expresión de texto (ej. `"Order.Customer.Id"`) en el atributo del aspecto. El `Evaluator` parsea este texto y lo ejecuta contra los argumentos del `IJoinPoint` en tiempo de ejecución.

**Nota de Rendimiento:** Las expresiones evaluadas son cacheadas fuertemente como instancias de delegados (`Delegate`) compilados, para asegurar que el parseo ocurra solo una vez por firma de método.
