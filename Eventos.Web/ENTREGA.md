# GLAB S11 Eventos e inscripciones

## Comandos de migración utilizados

```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet ef migrations add AgregarModalidadEvento
dotnet ef database update
dotnet ef migrations add AgregarTelefonoAsistente
dotnet ef database update
```

## Consulta de evidencia

```sql
SELECT MigrationId, ProductVersion FROM __EFMigrationsHistory ORDER BY MigrationId;
```

## Observaciones

- EventosDB se generó mediante migraciones Code First.
- La carpeta Migrations contiene InitialCreate, AgregarModalidadEvento y AgregarTelefonoAsistente.
- Modalidad usa Presencial como valor por defecto en la segunda migración.
- Los datos iniciales se cargan al iniciar si no existen categorías.
- Las operaciones de datos se realizan con LINQ asíncrono desde repositorios.

## Conclusiones

La aplicación implementa la gestión de eventos, asistentes e inscripciones mediante ASP.NET Core MVC y EF Core Code First. Las migraciones permiten versionar los cambios del esquema y la eliminación lógica conserva el historial de información.
