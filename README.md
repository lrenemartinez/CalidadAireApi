# API de Calidad del Aire

API RESTful en .NET 10 con MySQL para gestionar sensores de calidad del aire de una planta industrial: registra lecturas, genera alertas automáticas según umbrales de la OMS y enriquece las lecturas con clima externo (Open-Meteo).

## Requisitos

- .NET SDK 10
- Docker Desktop
- Herramienta de EF Core: `dotnet tool install --global dotnet-ef`

## Despliegue

1. Levantar MySQL:

```bash
docker compose up -d
```

2. Crear las tablas, con una de estas dos opciones:

```bash
dotnet ef database update
```

o ejecutar `Database/script.sql` en MySQL.

3. Ejecutar la API:

```bash
dotnet run
```

4. Abrir Swagger en `http://localhost:5136/swagger`.

## Configuración

Los valores están en `appsettings.json`:

- `ConnectionStrings:Default`: conexión a MySQL.
- `Jwt`: llave, emisor, audiencia y minutos de vida del token.
- `Clima`: URL de Open-Meteo y coordenadas de la planta.

Las credenciales incluidas son de desarrollo. En producción deben sustituirse mediante variables de entorno, por ejemplo `ConnectionStrings__Default` y `Jwt__Key`.

## Uso rápido

1. `POST /api/Auth/registro` para crear un usuario.
2. `POST /api/Auth/login` para obtener el token.
3. En Swagger, botón **Authorize** y pegar el token, sin escribir "Bearer".
4. Crear un sensor con `POST /api/Sensores` y registrar lecturas con `POST /api/Lecturas`.

## Endpoints

| Método | Ruta | Descripción |
|---|---|---|
| POST | /api/Auth/registro | Registra un usuario |
| POST | /api/Auth/login | Devuelve el token JWT |
| GET | /api/Sensores | Lista los sensores |
| GET | /api/Sensores/{id} | Obtiene un sensor |
| POST | /api/Sensores | Crea un sensor |
| PUT | /api/Sensores/{id} | Actualiza un sensor |
| POST | /api/Lecturas | Registra una lectura y genera alerta si aplica |
| GET | /api/Lecturas | Consulta por rango de fechas y contaminante |
| GET | /api/Lecturas/{id} | Obtiene una lectura |
| GET | /api/Lecturas/{id}/enriquecida | Lectura con sensor y clima externo |
| GET | /api/Alertas | Lista alertas, con filtros opcionales |

Todos los endpoints, excepto los de `/api/Auth`, requieren JWT.

## Reglas de alertas

Se genera una sola alerta por lectura, la del nivel más alto que se cumpla:

| Nivel | Condición |
|---|---|
| Extrema | CO2 > 5000 ppm o PM2.5 > 250 |
| Crítica | PM2.5 > 150 o PM10 > 200 |
| Moderada | PM2.5 > 50 o CO2 > 1000 ppm |
| Leve | PM2.5 entre 25 y 50 |

## Validaciones

- PM2.5, PM10 y CO2 no pueden ser negativos.
- El sensor debe existir y estar activo para registrar lecturas.
- La fecha de una lectura no puede estar en el futuro.
- Errores en formato ProblemDetails: 400, 401, 404, 409 y 500.

## Estructura

- `Controllers`: endpoints HTTP.
- `Services`: lógica de negocio, alertas, clima y tokens.
- `Models` y `Data`: entidades y `AppDbContext`.
- `Dto` y `Mapping`: contratos de entrada y salida.
- `Configuration`: opciones tipadas.
- `Middleware` y `Exceptions`: manejo global de errores.
- `Migrations` y `Database`: migraciones y script SQL.