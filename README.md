# WebApiInalambria

Esta API proporciona un servicio especializado de conversión de números a su representación textual en español. Está diseñada para transformar números enteros en su pronunciación escrita, permitiendo a los usuarios enviar un valor numérico y recibir como respuesta su representación textual precisa en español. El servicio acepta números en el rango de 0 a 999.999.999.999 y devuelve la pronunciación correcta del número en texto.

## Estructura del proyecto

El proyecto está organizado en varias carpetas y archivos clave para mantener una estructura clara y modular. Esta organización sigue los principios de la arquitectura limpia y el diseño orientado a puertos y adaptadores, implementando una separación de responsabilidades entre los diferentes componentes.

![image](https://github.com/JDuran11/WebApiInalambria/assets/72619994/4c1577b9-af65-44bb-9534-86292f1f19e9)

- [DTOs](#DTOs)
- [DataAdapters](#DataAdapters)
- [Ports](#Ports)
- [Controllers](#Controllers)

### DTOs

**DTOs** (Data Transfer Objects) Es una carpeta que contiene clases que representan estructuras de datos utilizadas específicamente para la transferencia de información entre el cliente y el servidor. Una clase de ejemplo podría ser la que es usada como objeto de salida, que se veria de la siguiente forma:

```csharp
public class NumToTextDTO
{
    public long number { get; set; }
    public string text { get; set; }
}
```

Este DTO  se utiliza para encapsular tanto el número original enviado por el cliente como el texto resultante de la conversión. La propiedad `number` almacena el valor numérico ingresado, mientras que `text` contiene la representación textual generada por la API. Esta estructura simple y clara facilita la serialización de la respuesta en formato JSON y permite al cliente recibir tanto el input original como el resultado procesado en un solo objeto.

### DataAdapters

Esta carpeta contiene clases que actúan como intermediarios entre la lógica de negocio de la aplicación y las fuentes de datos externas o servicios. Los adaptadores de datos son responsables de implementar las interfaces definidas en los puertos, proporcionando la funcionalidad concreta requerida por la aplicación. Un ejemplo de clase en esta carpeta es:

```csharp
public class NumToTextRepositoryAdapter : INumToTextRepositoryPort
{
    public string GetMessage()
    {
        return "hello from data adapter";
    }
}
```

En este ejemplo, `NumToTextRepositoryAdapter` implementa la interfaz `INumToTextRepositoryPort`. Este adaptador proporciona una implementación simple del método `GetMessage()`, que podría ser utilizado para verificar la conectividad o como un placeholder para las diferentes funcionalidades implementadas.

### Ports

Esta carpeta contiene interfaces que definen los contratos para las operaciones del sistema. Los puertos actúan como puntos de entrada y salida para la lógica de negocio, estableciendo un contrato claro entre los diferentes componentes de la aplicación. Un ejemplo de interfaz en esta carpeta es:

```csharp
public interface INumToTextRepositoryPort 
{ 
	string GetMessage(); 
	NumToTextDTO NumberToWords(NumDTO number); 
}
```

En este ejemplo, `INumToTextRepositoryPort` define la estructura de operaciones que deben ser implementadas por los adaptadores correspondientes. Esta interfaz, como otras en la carpeta Ports, establece un contrato claro entre los diferentes componentes de la aplicación, creando una capa de abstracción que desacopla la lógica de negocio de las implementaciones concretas.

### Controllers

Esta carpeta contiene las clases controlador que manejan las solicitudes HTTP de la API. Los controladores actúan como punto de entrada para las peticiones del cliente, gestionando la lógica de routing y la interacción entre la capa de presentación y la lógica de negocio. Un ejemplo de controlador en esta carpeta es la siguiente:

```csharp
namespace WebApiInalambria.Controllers
{
    [ApiController]
    [Route("api/numToText")]
    public class NumToTextController : ControllerBase
    {
        private readonly INumToTextRepositoryPort _repository;

        public NumToTextController(INumToTextRepositoryPort repository)
        {
            _repository = repository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var prueba = _repository.GetMessage();
            return Ok(new { message = prueba });
        }
    }
}
```

Este controlador maneja las operaciones relacionadas con la conversión de números a texto. Utiliza inyección de dependencias para recibir una implementación de `INumToTextRepositoryPort`, demostrando el principio de inversión de dependencias. El atributo `[Authorize]` indica que se requiere autenticación para acceder a los endpoints, mientras que `[ApiController]` y `[Route]` definen el comportamiento y la ruta base del controlador respectivamente.


## ¿Como se usa?

En esta sección, Se proprociona una guía sobre cómo interactuar con el API de conversión de números a texto. 

- [Autenticacion](#Autenticacion)
- [Números a texto](#Convertirnúmerosapalabras) 

### Autenticacion

La autenticación es un paso obligatorio para utilizar el API, sin la cual no será posible acceder a ninguno de los demás endpoints. Es importante destacar que, debido a que este proyecto no está conectado a una base de datos, la autenticación implementada es simbólica. Esto significa que el sistema aceptará cualquier combinación de usuario y contraseña para generar un token. Para obtener un token de autenticación, se debe enviar una solicitud que incluya tanto un nombre de usuario como una contraseña en el siguiente formato:

Endpoint que se consume de tipo POST: _https://localhost:7085/api/Autenticacion_ (El puerto puede variar y de igual forma se debe configurar en appsettings)

```json
{
  "username": "usuario",
  "password": "prueba"
}
```
La respuesta de este endpoint va devuelta en el siguiente formato

```json
{
  "token": "Token_generado"
}
```

Aunque la entrada en los campos de usuario y contraseña pueden ser cualquier valor, ambos campos son obligatorios. Si la solicitud incluye tanto el usuario como la contraseña, el sistema generará un token JWT (JSON Web Token) válido. 

Luego de generado el token, este debe ser incluido en el encabezado de todas las solicitudes subsiguientes a la API para acceder a los demás endpoints. Específicamente, el token debe ser añadido en el encabezado de autorización de la siguiente manera:

- Se Utiliza el esquema de autenticación "Bearer".
- Coloca el token JWT después de la palabra "Bearer", separado por un espacio.

Por ejemplo, el encabezado de autorización debería verse así:

![image](https://github.com/JDuran11/WebApiInalambria/assets/72619994/54d2c4ae-99a5-492d-b4cb-cbfb7833479d)

### Convertir números a palabras

La funcionalidad principal de esta API es la conversión de números a su representación textual en español. Para utilizar este servicio, debes enviar una solicitud al endpoint correspondiente con el siguiente formato:

endpoint que se consume de tipo POST: _https://localhost:7085/api/numToText_ (El puerto puede variar y de igual forma se debe configurar en appsettings)

```json
{
  "number":   1000
}
```
Donde "number" es el valor numérico que se desea convertir a texto. Este número debe estar en el rango de 0 a 999.999.999.999.
La API procesará la solicitud y devolverá un objeto JSON con el siguiente formato:

```json
{
    "number": 1000,
    "text": "mil"
}
```
En este objeto de respuesta, "number" refleja el número original que se envió, y "text" contiene la representación textual de ese número en español.















