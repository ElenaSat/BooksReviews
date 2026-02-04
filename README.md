
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

**Cómo ejecutar (desarrollo local):**

1) API (Backend)

```bash
cd BooksReviews.Api
dotnet restore
dotnet build
dotnet run --project BooksReviews.Api
```

Verifique `appsettings.json` o variables de entorno para `JwtSettings` y la cadena de conexión a la base de datos.

2) Frontend (Angular)

```bash
cd FrontBooksReviews
npm install
npm run dev
```

La API expone OpenAPI/Swagger en entorno de desarrollo (configurado en [BooksReviews.Api/Program.cs](BooksReviews.Api/Program.cs#L1)).

3) Tests

```bash
cd BooksReviews.UnitTests
dotnet test
```

**Notas y recomendaciones rápidas:**
- Revisa `BooksReviews.Infrastructure/Persistence/ApplicationDbContext.cs` para esquemas y migraciones de EF Core.
- Las clases de autenticación están en [BooksReviews.Infrastructure/Authentication](BooksReviews.Infrastructure/Authentication).
- Para producción, ajuste `JwtSettings` y la cadena de conexión (secretos en variables de entorno o un vault).

Si quieres, aplico un commit con este `README.md`, añado instrucciones de Docker o genero un pequeño script de inicio para desarrollo.

**Ejecución con Docker (desarrollo local)**

Se incluyen `Dockerfile` para la API y el frontend, y un `docker-compose.yml` para arrancar la app completa (API + Frontend + SQL Server).

- Construir y levantar todo:

```bash
docker-compose build
docker-compose up
```

- La API quedará accesible en `http://localhost:5000` y el frontend en `http://localhost:4200`.

- Nota de configuración: `docker-compose.yml` contiene un usuario `sa` y contraseña de ejemplo (`Your_strong!Passw0rd`). Cámbialos antes de usar en producción o ponlos en variables de entorno.

**Despliegue a una plataforma de hosting (ejemplo: Azure)**

1) Construir y publicar images (ejemplo en Docker Hub):

```bash
# API
docker build -t your-dockerhub-user/booksreviews-api:latest -f BooksReviews.Api/Dockerfile ./BooksReviews.Api
# Frontend
docker build -t your-dockerhub-user/booksreviews-frontend:latest -f FrontBooksReviews/Dockerfile ./FrontBooksReviews
docker push your-dockerhub-user/booksreviews-api:latest
docker push your-dockerhub-user/booksreviews-frontend:latest
```

2) Desplegar en Azure (App Service Container) — resumen de pasos:

```bash
az login
az group create -n BooksReviewsRG -l westeurope
az acr create -n MyBooksReviewsAcr --sku Basic --admin-enabled true
az acr login --name MyBooksReviewsAcr
docker tag your-dockerhub-user/booksreviews-api:latest MyBooksReviewsAcr.azurecr.io/booksreviews-api:latest
docker push MyBooksReviewsAcr.azurecr.io/booksreviews-api:latest
az appservice plan create -n BooksReviewsPlan -g BooksReviewsRG --is-linux --sku B1
az webapp create -g BooksReviewsRG -p BooksReviewsPlan -n booksreviews-api --deployment-container-image-name MyBooksReviewsAcr.azurecr.io/booksreviews-api:latest
```

3) Frontend estático: puede desplegarse en Azure Static Web Apps, Netlify o Vercel. Si usas la imagen Docker, puedes desplegarla en App Service similar al backend.

**Notas de seguridad y producción**
- Nunca guardes secretos en `appsettings.json` en el repositorio; usa variables de entorno o un vault.
- Cambia la contraseña `sa` y `JwtSettings__Secret` antes de producción.
- Considera usar una base de datos gestionada (Azure SQL, RDS) y un CDN/host estático para el frontend.

Si quieres, hago el commit de los Dockerfiles y el `docker-compose.yml` ahora y configuro un workflow básico de GitHub Actions para construcción y push de imágenes.
