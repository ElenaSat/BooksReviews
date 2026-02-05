
# BooksReviews

Aplicación para reseñas de libros que permite a usuarios registrarse, iniciar sesión, navegar y buscar libros, ver detalles, dejar reseñas y leer reseñas de otros usuarios.

**Resumen corto:** Backend en ASP.NET Core (.NET 9) organizado con principios de Clean Architecture y frontend en Angular 21. Autenticación con JWT, almacenamiento con EF Core (SQL Server) y pruebas unitarias incluidas.

**Tecnologías principales:**
- **Backend:** .NET 9, C#, ASP.NET Core Web API, Entity Framework Core 9, Microsoft.Identity/JWT, MediatR, AutoMapper.
- **Infra:** SQL Server (via EF Core), BCrypt para hashing de contraseñas.
- **Frontend:** Angular 21, TypeScript, Tailwind CSS.
- **Dev / Docs:** Swagger / OpenAPI (integrado en [BooksReviews.Api/Program.cs](BooksReviews.Api/Program.cs#L1)).

**Arquitectura y capas:**
- **Domain:** [BooksReviews.Domain](BooksReviews.Domain) — entidades de negocio (`Book`, `User`, `Review`) y lógica pura.
- **Application:** [BooksReviews.Application](BooksReviews.Application) — casos de uso, comandos/queries (CQRS), `MediatR` handlers, `AutoMapper` y validaciones (`FluentValidation`).
- **Infrastructure:** [BooksReviews.Infrastructure](BooksReviews.Infrastructure) — implementación de persistencia (EF Core), servicios de autenticación (`Authentication/JwtTokenGenerator.cs`, `PasswordHasher.cs`) y configuración de DI.
- **API (Presentation):** [BooksReviews.Api](BooksReviews.Api) — controladores REST (`Controllers/BooksController.cs`, `Controllers/ReviewsController.cs`, `Controllers/UsersController.cs`), middleware (`Middleware/ExceptionMiddleware.cs`) y configuración en [BooksReviews.Api/Program.cs](BooksReviews.Api/Program.cs#L1).
- **Frontend:** [FrontBooksReviews](FrontBooksReviews) — UI en Angular para navegación, búsqueda, autenticación y gestión de reseñas.
- **Tests:** [BooksReviews.UnitTests](BooksReviews.UnitTests) — pruebas unitarias de la lógica de aplicación.

**Patrones de diseño relevantes:**
- **Clean Architecture / Onion:** Separación clara de capas con dependencias apuntando hacia el dominio.
- **CQRS:** Comandos y Queries gestionados en la capa de `Application`.
- **Mediator:** `MediatR` para desacoplar controladores de la lógica (Handlers independientes).
- **Dependency Injection (DI):** Registrado en los módulos `DependencyInjection.cs` de cada proyecto; usado por `Program.cs` para componer la aplicación.
- **Repository / Unit of Work (abstracción):** Acceso a datos a través de `IApplicationDbContext` en lugar de acoplar EF Core en la capa de Aplicación.
- **DTOs y Mappings:** `AutoMapper` para mapear entidades a DTOs y viceversa.

**Funcionalidades principales (requeridas por la aplicación):**
- Registro de usuarios y autenticación con JWT (ver `JwtSettings` en [BooksReviews.Api/appsettings.json](BooksReviews.Api/appsettings.json)).
- Navegación por lista de libros, búsqueda y filtrado.
- Visualización de detalles de un libro.
- Creación de reseñas por usuarios autenticados.
- Visualización de reseñas de otros usuarios para cada libro.

**Estructura del proyecto (resumen de carpetas):**
- [BooksReviews.Api](BooksReviews.Api): API, controladores y middleware.
- [BooksReviews.Application](BooksReviews.Application): Casos de uso, comandos/queries, mappings y validaciones.
- [BooksReviews.Domain](BooksReviews.Domain): Entidades de dominio (`Book`, `Review`, `User`).
- [BooksReviews.Infrastructure](BooksReviews.Infrastructure): Persistencia EF Core, hashing de contraseñas y generador de JWT (`Authentication`).
- [FrontBooksReviews](FrontBooksReviews): Frontend Angular 21 (scripts en `package.json`).
- [BooksReviews.UnitTests](BooksReviews.UnitTests): Suite de pruebas unitarias.


## Despliegue local con Docker

Puedes levantar toda la aplicación (API, base de datos y frontend) usando Docker y Docker Compose. Asegúrate de tener [Docker](https://docs.docker.com/get-docker/) y [Docker Compose](https://docs.docker.com/compose/install/) instalados.

### 1. Levantar todos los servicios

Desde la raíz del proyecto, ejecuta:

```bash
docker compose up --build
```

Esto levantará tres servicios:
- **db**: SQL Server (persistencia en volumen local)
- **api**: Backend ASP.NET Core (puerto 5000)
- **frontend**: Angular (puerto 3000)

La primera vez puede tardar varios minutos por la descarga de imágenes y la construcción.

### 2. Acceso a la aplicación

- **Frontend Angular:** [http://localhost:3000](http://localhost:3000)
- **API Swagger/OpenAPI:** [http://localhost:5000/swagger](http://localhost:5000/swagger)
- **Base de datos:** SQL Server expuesto en `localhost:1433` (usuario: `sa`, contraseña: `YourStrong@Passw0rd`)

### 3. Variables y configuración

- La cadena de conexión y los secretos están definidos en `compose.yml` como variables de entorno.
- Puedes modificar la contraseña de SQL Server y otros valores en el archivo `compose.override.yml` para personalizar tu entorno local.
- El frontend se comunica con la API usando la variable `API_URL` definida en el servicio `frontend`.

### 4. Parar y limpiar los contenedores

Para detener los servicios y limpiar los recursos:

```bash
docker compose down -v
```

Esto elimina los contenedores y el volumen de la base de datos.

---

**Notas y recomendaciones rápidas:**
- Revisa `BooksReviews.Infrastructure/Persistence/ApplicationDbContext.cs` para esquemas y migraciones de EF Core.
- Las clases de autenticación están en [BooksReviews.Infrastructure/Authentication](BooksReviews.Infrastructure/Authentication).
- Para producción, ajusta `JwtSettings` y la cadena de conexión (secretos en variables de entorno o un vault).
