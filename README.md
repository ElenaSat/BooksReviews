# SmartCard Project

Sistema de administración de tarjetas inteligentes desarrollado con una arquitectura moderna y escalable.

## 🛠 Tecnologías Utilizadas

### Backend (.NET)
*   **Framework:** .NET 9
*   **Lenguaje:** C#
*   **API:** ASP.NET Core Web API
*   **ORM:** Entity Framework Core 9 (SQL Server)
*   **Mediación:** MediatR
*   **Mapeo:** AutoMapper
*   **Documentación:** Swagger / OpenAPI

### Frontend (Angular)
*   **Framework:** Angular 21
*   **Lenguaje:** TypeScript
*   **Estilos:**
    *   Bootstrap 5
    *   Tailwind CSS 4
    *   SCSS
*   **Componentes UI:** NgBootstrap, NgSelect
*   **Gráficos:** ApexCharts
*   **Iconos:** Ant Design Icons

## 🏗 Arquitectura

El proyecto sigue los principios de **Clean Architecture** (Arquitectura Limpia), organizando el código en capas para separar responsabilidades y facilitar el mantenimiento y las pruebas.

### Capas del Proyecto:
1.  **Domain (`SmartCard.Domain`)**: El núcleo central. Contiene las Entidades y la lógica de negocio pura. No tiene dependencias externas.
2.  **Application (`SmartCard.Application`)**: Contiene la lógica de la aplicación y los casos de uso.
    *   Implementa **CQRS** (Command Query Responsibility Segregation).
    *   Define interfaces para la abstracción de datos (`IApplicationDbContext`).
    *   Gestiona DTOs y Mappings.
3.  **Infrastructure (`SmartCard.Infrastructure`)**: Implementa las interfaces definidas en Application.
    *   Acceso a datos (Entity Framework Core).
    *   Inyección de dependencias de servicios externos.
4.  **Presentation (`WebApiSmartCard`)**: La entrada al sistema (API REST).
    *   Controladores que envían comandos/queries a través de MediatR.

## 🧩 Patrones de Diseño

El sistema implementa varios patrones de diseño clave para garantizar desacoplamiento y escalabilidad:

*   **CQRS (Command Query Responsibility Segregation):** Separación estricta entre operaciones de lectura (`Queries`) y escritura (`Commands`). Implementado en la capa de Aplicación.
*   **Mediator Pattern:** Utilizado vía **MediatR** para desacoplar los controladores de la lógica de negocio. Los controladores envían mensajes que son manejados por Handlers específicos.
*   **Dependency Injection (DI):** Utilizado extensivamente en todo el proyecto para invertir el control y gestionar el ciclo de vida de los servicios.
*   **Repository / Unit of Work Pattern:** Abstraído a través de `IApplicationDbContext`, permitiendo que la capa de Aplicación interactúe con la base de datos sin conocer la implementación concreta de EF Core.
*   **DTO (Data Transfer Object):** Uso de objetos planos para transferir datos entre capas y hacia el cliente, evitando exponer las entidades de dominio directamente.
