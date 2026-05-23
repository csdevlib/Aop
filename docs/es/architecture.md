# Arquitectura

<p>
  <a href="../en/architecture.md">English</a> | <strong>Español</strong>
</p>

El framework BeyondNet.Aop está diseñado para un alto rendimiento y una clara separación de responsabilidades. Aprovecha el `System.Reflection.DispatchProxy` nativo de `.NET` para construir un pipeline de intercepción dinámico en tiempo de ejecución.

## Componentes Centrales y Responsabilidades

- **`AopProxy<TService, TImplementation>`**: La clase proxy principal. Actúa como un envoltorio transparente alrededor del servicio destino, interceptando cada llamada a un método.
- **`IAspectExecutor`**: Orquesta la cadena de aspectos. Determina qué aspectos aplican al método actual y los ejecuta en el orden correcto.
- **`IJoinPoint`**: El objeto contextual que viaja a través del pipeline. Contiene los argumentos del método, la metadata (`MethodInfo`), la instancia destino, y provee el método `Proceed()` para continuar la ejecución.
- **`AbstractAspect<T>`**: La clase base para todos los aspectos. Lee la configuración del aspecto desde los atributos personalizados que decoran el método interceptado.

## El Pipeline de Intercepción

Cuando se llama a un método en un servicio interceptado, el flujo es el siguiente:

```mermaid
sequenceDiagram
    participant Cliente
    participant AopProxy
    participant AspectExecutor
    participant Aspect (ej. Logger)
    participant Servicio Destino

    Cliente->>AopProxy: Llama Método(args)
    AopProxy->>AspectExecutor: Execute(JoinPoint)
    AspectExecutor->>Aspect: Apply(JoinPoint)
    
    rect rgb(200, 220, 240)
        Note right of Aspect: Lógica pre-ejecución (OnEntry)
    end
    
    Aspect->>AspectExecutor: Proceed()
    AspectExecutor->>Servicio Destino: Invoca Método Original
    Servicio Destino-->>AspectExecutor: Retorna Resultado
    AspectExecutor-->>Aspect: Retorna Control
    
    rect rgb(200, 240, 220)
        Note right of Aspect: Lógica post-ejecución (OnExit / OnException)
    end
    
    Aspect-->>AspectExecutor: Retorna Resultado
    AspectExecutor-->>AopProxy: Retorna Resultado
    AopProxy-->>Cliente: Retorna Resultado
```

## Rendimiento y Caché

Históricamente, la reflexión en .NET es lenta. Para asegurar que BeyondNet.Aop pueda ser usado en entornos de alta concurrencia, implementamos varias capas de caché:

1. **Caché de Resolución de Métodos**: Mapea métodos de interfaz a métodos de implementación en tiempo O(1).
2. **Caché de Búsqueda de Atributos**: `AbstractAspect` cachea fuertemente la búsqueda de atributos personalizados usando `ConcurrentDictionary`.
3. **Caché de Evaluación de Expresiones**: Las expresiones de cadenas dinámicas evaluadas en tiempo de ejecución (ej. extraer un ID anidado de un objeto argumento) son compiladas a instancias de delegados (`Delegate`) muy rápidas y se guardan en caché indefinidamente.
