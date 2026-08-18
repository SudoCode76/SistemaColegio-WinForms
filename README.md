# Sistema de Gestión para Colegio

Este es un sistema de escritorio para la gestión escolar, desarrollado en **C# con Windows Forms** y **SQL Server**. Está diseñado siguiendo una arquitectura de 3 capas y cuenta con una interfaz gráfica moderna utilizando la librería Guna.UI.

> ⚠️ **Aviso:** Este es un proyecto académico desarrollado con fines de demostración y aprendizaje. No está diseñado para ser utilizado en entornos de producción empresariales sin las debidas revisiones de seguridad y optimización.

## 🚀 Módulos Disponibles

El sistema permite gestionar de manera integral las operaciones principales de un colegio:
- **Gestión de Estudiantes y Profesores:** Registro, actualización y consulta.
- **Gestión Académica:** Administración de materias y cursos.
- **Inscripciones y Calificaciones:** Asignación de estudiantes a cursos y registro de notas.
- **Finanzas:** Control de mensualidades y pagos.
- **Reportes:** Generación de reportes generales del sistema.

## 🏗️ Arquitectura
El proyecto está estructurado utilizando el patrón de **Arquitectura en 3 Capas**:
- **Capa de Presentación:** Interfaz de usuario (Formularios de Windows Forms).
- **Capa de Negocio:** Lógica principal y reglas del sistema.
- **Capa de Datos:** Acceso y transacciones con la base de datos SQL Server.

## 🛠️ Tecnologías Utilizadas

- **Lenguaje:** C# (.NET Framework 4.7.2)
- **Base de Datos:** Microsoft SQL Server
- **UI / Diseño:** Guna.UI Framework, FontAwesome.Sharp
- **Reportes:** iTextSharp (PDF)

## ⚙️ Requisitos e Instalación

Para ejecutar este proyecto localmente, necesitas:
- Visual Studio 2019 o superior (con la carga de trabajo de desarrollo de escritorio de .NET).
- SQL Server (Developer o Express edition).

### 1. Base de Datos
1. Abre SQL Server Management Studio (SSMS).
2. Ejecuta el script `base de datos/DBColegioV2.sql` para crear la base de datos `SistemaColegio` y su estructura completa.
3. *(Opcional)* Si deseas datos de prueba, puedes restaurar el archivo `base de datos/DBColegioBackup.sql` en su lugar.

### 2. Configuración de Conexión
El proyecto utiliza Autenticación Integrada de Windows por defecto.
Abre el archivo `ProyectoColegio/App.config` y modifica el valor de `Server` en la etiqueta `<connectionStrings>` para que coincida con el nombre de tu servidor local de SQL Server.

```xml
<connectionStrings>
    <!-- Cambia Server=.\SQLEXPRESS por el nombre de tu servidor si es distinto -->
    <add name="ColegioDB" connectionString="Server=.\SQLEXPRESS;Database=SistemaColegio;Integrated Security=true" providerName="System.Data.SqlClient" />
</connectionStrings>
```

## 🔐 Credenciales de Acceso Demo

Para ingresar al sistema una vez ejecutado, puedes utilizar las siguientes credenciales (incluidas en el script V2):
- **Usuario:** `admin`
- **Contraseña:** `admin`

## 📋 Limitaciones Conocidas
* La seguridad de autenticación es básica, orientada a demostración de funcionalidades.
* Las rutas de reportes y archivos generados podrían requerir permisos de escritura locales.
