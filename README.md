# Sistema de Gestión Escolar (WinForms)

Sistema de escritorio desarrollado en C# y Windows Forms para la gestión administrativa y académica de una institución educativa. Implementa una arquitectura en 3 capas y utiliza SQL Server para el almacenamiento de datos.

Este proyecto fue desarrollado como parte de una evaluación académica, con el objetivo de demostrar la aplicación práctica de conceptos de programación orientada a objetos, acceso a datos y diseño de interfaces de usuario en el entorno .NET.

## Módulos Principales

El sistema cubre los flujos de trabajo operativos clave de un colegio:

* **Administración de Personal y Alumnado:** Formularios para el registro y actualización de datos de estudiantes y profesores.
* **Control Académico:** Módulos para la creación de materias, apertura de cursos y asignación de profesores.
* **Procesos Operativos:** Gestión de inscripciones de alumnos a cursos específicos y registro de calificaciones por periodo.
* **Gestión Financiera:** Registro y control de pagos de mensualidades.
* **Generación de Reportes:** Exportación de informes y listados utilizando iTextSharp.

## Arquitectura y Tecnologías

El desarrollo sigue el patrón de diseño de **Arquitectura en 3 Capas** para separar las responsabilidades del código:

1. **Capa de Presentación:** Interfaz gráfica desarrollada con Windows Forms y controles personalizados de Guna.UI.
2. **Capa de Negocio:** Clases que encapsulan las reglas de validación y la lógica principal del sistema.
3. **Capa de Datos:** Clases encargadas de ejecutar consultas, procedimientos almacenados y transacciones mediante ADO.NET (`System.Data.SqlClient`).

**Stack Tecnológico:**
* **Lenguaje:** C# (.NET Framework 4.7.2)
* **Base de Datos:** Microsoft SQL Server
* **Librerías de Terceros:** 
  * Guna.UI (Controles de interfaz gráfica)
  * FontAwesome.Sharp (Iconografía)
  * iTextSharp (Generación de documentos PDF)

## Configuración del Entorno de Desarrollo

Para ejecutar el código fuente en un entorno local, se requiere Visual Studio (2019 o superior) con la carga de trabajo de "Desarrollo de escritorio de .NET" instalada, y una instancia local de SQL Server.

### Preparación de la Base de Datos

En la carpeta `base de datos/` se incluyen los scripts necesarios:
1. Abrir SQL Server Management Studio (SSMS).
2. Ejecutar el script `DBColegioV2.sql`. Este archivo contiene la definición completa de la estructura de tablas (`SistemaColegio`), claves foráneas y datos iniciales requeridos.
3. Existe un archivo `DBColegioBackup.sql` que puede utilizarse en caso de requerir restaurar un conjunto de datos de prueba más extenso.

### Configuración de la Cadena de Conexión

El acceso a la base de datos utiliza Autenticación Integrada de Windows. Es necesario verificar que el nombre del servidor coincida con la instancia local.

Abrir el archivo `ProyectoColegio/App.config` y modificar el atributo `Server` dentro de la etiqueta `<connectionStrings>`:

```xml
<connectionStrings>
    <!-- Actualizar "Server=.\SQLEXPRESS" al nombre de la instancia local correspondiente -->
    <add name="ColegioDB" connectionString="Server=.\SQLEXPRESS;Database=SistemaColegio;Integrated Security=true" providerName="System.Data.SqlClient" />
</connectionStrings>
```

## Acceso al Sistema

El script de la base de datos incluye credenciales administrativas por defecto para acceder al sistema una vez compilado:

* **Usuario:** `admin`
* **Contraseña:** `admin`

## Notas de Desarrollo

* Las dependencias de librerías externas se encuentran en la carpeta `lib/` y están referenciadas mediante rutas relativas en el archivo `.csproj` para facilitar la compilación tras clonar el repositorio.
* La autenticación implementada cumple fines demostrativos. Para entornos de producción, se recomienda implementar un sistema de encriptación (hashing) para las contraseñas en la base de datos.
