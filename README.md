# Eventory 

> Sistema web para gestión de eventos, validación de tickets y control de asistencia a nivel académico, pensado para instituciones educativas.

---

## Descripción

**Eventory** es una aplicación web desarrollada en **ASP.NET Core MVC 8** que permite crear publicaciones de eventos por fecha, gestionar inscripciones, validar tickets con claves únicas y controlar la asistencia de los participantes.  

Resuelve el problema de la recolección de datos críticos de alumnos en instituciones, facilitando el registro individual, validación de entradas y control de acceso a eventos específicos.

---

## Objetivos

- Publicar eventos organizados por fechas.
- Registro y control de asistentes.
- Validación de tickets con claves únicas impresas.
- Control de acceso mediante credenciales de institución.
- Visualización de eventos pasados sin posibilidad de inscripción.

---

## Tecnologías utilizadas

| Tecnología         | Descripción                          |
|--------------------|------------------------------------|
| ASP.NET Core MVC 8.0 | Framework web para la lógica MVC.  |
| SQL Server         | Base de datos relacional.           |
| Bootstrap          | Framework CSS para diseño responsivo. |
| Linux (Manjaro, Kali) | Entorno de desarrollo y despliegue. |
| GitHub Actions     | Automatización CI/CD y seguridad.   |

---

## Instalación y despliegue

*En construcción*  

La idea es desplegar Eventory en servidores Linux para aprovechar tecnologías web modernas y facilitar el acceso remoto.  

### Migraciones de base de datos

Comandos en la consola de **Package Manager Console**:

```bash
add-migration MigrationShortDescriptionAdded
update-database
```

*Más detalles sobre migraciones y despliegue se documentarán en la Wiki.*

### Wiki y documentación
Para información detallada, flujos de trabajo, diseño de base de datos, y futuras implementaciones (como control de concurrencia optimista), visita nuestra Wiki:

###Funcionalidades futuras
Implementar control de concurrencia optimista con rowversion para evitar conflictos en actualizaciones simultáneas.
Despliegue en servidores Linux con pipeline automatizado usando GitHub Actions.
Capturas de pantalla y manuales de usuario en la Wiki.

[![Ver Wiki](https://img.shields.io/badge/%20Ir%20a%20la%20Wiki-blue?style=for-the-badge)](https://github.com/Be00wulf/GyRb/wiki/Eventory)

### Contribuciones
¡Contribuciones y sugerencias son bienvenidas! Por favor revisa las pautas en la Wiki antes de enviar un PR.

### Licencia
MIT License

*Nunca pares de aprender :D*  
