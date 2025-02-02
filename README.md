# N5 Technical Challenge - Backend

Este proyecto es la API desarrollada para el **N5 Technical Challenge**, construida con **ASP.NET Core 8.0** siguiendo la arquitectura **Clean Architecture** y el patrón **CQRS**. La API permite la gestión de permisos y tipos de permisos, incluyendo autenticación mediante **JWT**, persistencia en **SQL Server** y búsqueda avanzada con **Elasticsearch**.

## Tecnologías Utilizadas

- **ASP.NET Core 8.0** para el desarrollo de la API
- **Entity Framework Core** para el acceso a datos y migraciones
- **SQL Server Developer Edition 2022** como base de datos relacional
- **Elasticsearch** para la indexación y búsqueda avanzada
- **MediatR** para la implementación del patrón **CQRS**
- **AutoMapper** para el mapeo entre entidades y DTOs
- **Serilog** para el registro estructurado de eventos
- **xUnit** y **Moq** para las pruebas unitarias
- **Docker** para la contenerización de dependencias como **Elasticsearch**

## Estructura del Proyecto

```
📁 n5-reto-tecnico-api
├── docker-compose.yml  // Configuración de contenedores para Elasticsearch y SQL Server
├── n5-reto-tecnico-api.sln  // Solución principal
├── 📁 N5.Permissions.Api
│   ├── appsettings.json  // Configuración de la API
│   ├── Program.cs  // Configuración de servicios y middlewares
│   ├── 📁 Controllers
│   │   ├── PermissionController.cs  // Endpoints para permisos
│   │   ├── PermissionTypeController.cs  // Endpoints para tipos de permisos
│   ├── 📁 Middlewares
│   │   └── ExceptionHandlingMiddleware.cs  // Manejo global de excepciones
├── 📁 N5.Permissions.Application
│   ├── 📁 Commands
│   │   ├── PermissionCommand
│   │   │   ├── CreatePermissionCommand.cs
│   │   │   ├── UpdatePermissionCommand.cs
│   │   ├── PermissionTypeCommand
│   │   │   ├── CreatePermissionTypeCommand.cs
│   ├── 📁 Handlers
│   │   ├── PermissionHandler
│   │   │   ├── CreatePermissionHandler.cs
│   │   │   ├── GetPermissionsHandler.cs
│   │   │   ├── SearchPermissionsHandler.cs  // Integración con Elasticsearch
│   │   │   ├── UpdatePermissionHandler.cs
│   ├── 📁 Queries
│   │   ├── PermissionQuerie
│   │   │   ├── GetPermissionQuery.cs
│   │   │   ├── SearchPermissionsQuery.cs
│   ├── 📁 Services
│   │   ├── TokenService.cs  // Generación de JWT
│   │   ├── UserService.cs  // Servicio de autenticación
├── 📁 N5.Permissions.Domain
│   ├── 📁 Entities
│   │   ├── Permission.cs  // Entidad de permisos
│   │   ├── PermissionType.cs  // Entidad de tipos de permisos
│   ├── 📁 Interfaces
│   │   ├── IUnitOfWork.cs
│   │   ├── Repositories
│   │   │   ├── IPermissionRepository.cs
│   │   │   ├── IPermissionTypeRepository.cs
├── 📁 N5.Permissions.Infrastructure
│   ├── 📁 Elasticsearch
│   │   ├── Services
│   │   │   ├── ElasticsearchService.cs  // Servicio para manejar Elasticsearch
│   │   ├── Models
│   │   │   ├── EsPermissionDoc.cs  // Modelo para indexación en Elasticsearch
│   ├── 📁 Persistence
│   │   ├── ApplicationDbContext.cs  // Configuración de EF Core
│   ├── 📁 Repositories
│   │   ├── PermissionRepository.cs
│   │   ├── PermissionTypeRepository.cs
│   │   ├── UnitOfWork.cs
├── 📁 N5.Permissions.Tests
│   ├── 📁 UnitTests
│   │   ├── ApplicationTest
│   │   │   ├── PermissionHandlerTests.cs  // Pruebas unitarias para handlers
│   │   ├── InfrastructureTest
│   ├── 📁 IntegrationTests
│       ├── ApplicationTest
│       ├── InfrastructureTest
```

## Instalación

1. Clona el repositorio:

   ```bash
   git clone https://github.com/tu_usuario/n5-reto-tecnico-api.git
   cd n5-reto-tecnico-api
   ```

2. Configura las variables de entorno en `appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=PermissionsDb;User Id=sa;Password=sadmin;"
     },
     "Elasticsearch": {
       "Uri": "http://localhost:9200",
       "Index": "permissions"
     },
     "Jwt": {
       "Key": "TuClaveSecretaParaJWT",
       "Issuer": "N5Company",
       "Audience": "N5CompanyUsers"
     }
   }
   ```

3. Levanta los servicios de **SQL Server** y **Elasticsearch** usando Docker:

   ```bash
   docker-compose up -d
   ```

4. Ejecuta las migraciones de la base de datos:

   ```bash
   dotnet ef database update --project N5.Permissions.Infrastructure
   ```

5. Corre la API:
   ```bash
   dotnet run --project N5.Permissions.Api
   ```

La API estará disponible en `http://localhost:5215/api`.

## Endpoints Principales

### **Autenticación**

- **POST** `/auth/login`  
  **Body:**
  ```json
  {
    "username": "user",
    "password": "UserPass123"
  }
  ```
  **Respuesta:** Devuelve un token JWT.

### **Permisos**

- **GET** `/permission` - Obtener todos los permisos.
- **POST** `/permission` - Crear un nuevo permiso.
- **PUT** `/permission/{id}` - Modificar un permiso existente.
- **GET** `/permission/search?query={query}` - Buscar permisos usando Elasticsearch.
- **POST** `/permission/reindex` - Reindexar todos los permisos en Elasticsearch.

### **Tipos de Permisos**

- **GET** `/permissiontype` - Obtener todos los tipos de permisos.
- **POST** `/permissiontype` - Crear un nuevo tipo de permiso.

## Pruebas

1. Ejecuta las pruebas unitarias:

   ```bash
   dotnet test
   ```

2. Las pruebas están organizadas en carpetas `UnitTests` e `IntegrationTests` para validar la lógica de negocio y la integración con servicios externos.

## Contribuciones

1. Haz un fork del repositorio.
2. Crea una nueva rama (`git checkout -b feature/nueva-funcionalidad`).
3. Realiza tus cambios y haz commit (`git commit -m 'Agrega nueva funcionalidad'`).
4. Empuja tu rama (`git push origin feature/nueva-funcionalidad`).
5. Abre un Pull Request.

## Licencia

Este proyecto está bajo la licencia MIT.

---

¡Gracias por revisar este proyecto! 🚀 Si tienes alguna duda o sugerencia, no dudes en abrir un issue o contactarme directamente.
