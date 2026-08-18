using ProyectoColegio.capaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace ProyectoColegio
{
    public class Usuario
    {
        public string NombreUsuario { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string TipoUsuario { get; set; }
    }

    public class BaseDeDatos
    {
        private SqlConnection conexion;

        public BaseDeDatos()
        {
            //pc de mesa
            conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["ColegioDB"].ConnectionString);
            //laptop
            //conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["ColegioDB"].ConnectionString);

        }

        public void AbrirConexion()
        {
            conexion.Open();
        }

        public void CerrarConexion()
        {
            if (conexion.State == System.Data.ConnectionState.Open)
            {
                conexion.Close();
            }
        }

        public Usuario ValidarUsuario(string usuario, string contrasena)
        {
            string consulta = "SELECT nombreUsuario, nombre, apellidoPaterno, tipoUsuario FROM Usuarios WHERE nombreUsuario=@usuario AND contrasena=@contrasena";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@usuario", usuario);
            comando.Parameters.AddWithValue("@contrasena", contrasena);

            SqlDataReader lector = comando.ExecuteReader();
            if (lector.Read())
            {
                Usuario usuarioEncontrado = new Usuario()
                {
                    NombreUsuario = lector["nombreUsuario"].ToString(),
                    Nombre = lector["nombre"].ToString(),
                    ApellidoPaterno = lector["apellidoPaterno"].ToString(),
                    TipoUsuario = lector["tipoUsuario"].ToString()
                };
                lector.Close();
                return usuarioEncontrado;
            }
            lector.Close();
            return null;
        }

        public void RegistrarEstudiante(Estudiante estudiante)
        {
            string consulta = @"INSERT INTO Estudiantes (CI, nombreCompleto, genero, fechaNacimiento, nacionalidad, direccion, tutor) 
                        VALUES (@CI, @nombreCompleto, @genero, @fechaNacimiento, @nacionalidad, @direccion, @tutor)";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@CI", estudiante.CI);
            comando.Parameters.AddWithValue("@nombreCompleto", estudiante.NombreCompleto);
            comando.Parameters.AddWithValue("@genero", estudiante.Genero);
            comando.Parameters.AddWithValue("@fechaNacimiento", estudiante.FechaNacimiento);
            comando.Parameters.AddWithValue("@nacionalidad", estudiante.Nacionalidad);
            comando.Parameters.AddWithValue("@direccion", estudiante.Direccion);
            comando.Parameters.AddWithValue("@tutor", estudiante.Tutor);

            AbrirConexion();
            comando.ExecuteNonQuery();
            CerrarConexion();
        }

        public List<Estudiante> ObtenerEstudiantes()
        {
            List<Estudiante> listaEstudiantes = new List<Estudiante>();
            string consulta = "SELECT * FROM Estudiantes";
            SqlCommand comando = new SqlCommand(consulta, conexion);

            AbrirConexion();
            SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                listaEstudiantes.Add(new Estudiante()
                {
                    IdEstudiante = Convert.ToInt32(lector["idEstudiante"]),
                    CI = lector["CI"].ToString(),
                    NombreCompleto = lector["nombreCompleto"].ToString(),
                    Genero = lector["genero"].ToString(),
                    FechaNacimiento = Convert.ToDateTime(lector["fechaNacimiento"]),
                    Nacionalidad = lector["nacionalidad"].ToString(),
                    Direccion = lector["direccion"].ToString(),
                    Tutor = lector["tutor"].ToString()
                });
            }
            lector.Close();
            CerrarConexion();

            return listaEstudiantes;
        }
        public List<gestionCurso> ObtenerCursos()
        {
            List<gestionCurso> cursos = new List<gestionCurso>();
            string consulta = "SELECT IdCurso, Nombre FROM Cursos";
            SqlCommand comando = new SqlCommand(consulta, conexion);

            AbrirConexion();
            SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                cursos.Add(new gestionCurso()
                {
                    IdCurso = lector.GetInt32(0),
                    Nombre = lector.GetString(1)
                });
            }
            lector.Close();
            CerrarConexion();

            return cursos;
        }
        public List<Materia> ObtenerMateria()
        {
            List<Materia> materias = new List<Materia>();
            string consulta = "SELECT IdMateria, nombreMateria FROM Materia";
            SqlCommand comando = new SqlCommand(consulta, conexion);

            AbrirConexion();
            SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                materias.Add(new Materia()
                {
                    IdMateria = lector.GetInt32(0),
                    Nombre = lector.GetString(1)
                });
            }
            lector.Close();
            CerrarConexion();

            return materias;
        }



        public void InscribirEstudianteACurso(int idEstudiante, int idCurso)
        {
            string consulta = "INSERT INTO Inscripcion (idEstudiante, idCurso, fechaInscripcion) VALUES (@idEstudiante, @idCurso, GETDATE())";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@idEstudiante", idEstudiante);
            comando.Parameters.AddWithValue("@idCurso", idCurso);

            AbrirConexion();
            comando.ExecuteNonQuery();
            CerrarConexion();
        }

        

        public bool InsertarCalificacion(int idEstudiante, int idMateria, decimal primerTrimestre, decimal segundoTrimestre, decimal tercerTrimestre)
        {
            string consulta = @"INSERT INTO Notas (idEstudiante, idMateria, primerTrimestre, segundoTrimestre, tercerTrimestre)VALUES (@IdEstudiante, @IdMateria, @PrimerTrimestre, @SegundoTrimestre, @TercerTrimestre)";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@IdEstudiante", idEstudiante);
            comando.Parameters.AddWithValue("@IdMateria", idMateria);
            comando.Parameters.AddWithValue("@PrimerTrimestre", primerTrimestre);
            comando.Parameters.AddWithValue("@SegundoTrimestre", segundoTrimestre);
            comando.Parameters.AddWithValue("@TercerTrimestre", tercerTrimestre);

            AbrirConexion();
            int filasAfectadas = comando.ExecuteNonQuery();
            CerrarConexion();

            return filasAfectadas > 0;
        }

        public DataTable ObtenerCalificaciones(int idEstudiante)
        {
            string consulta = @"
        SELECT M.nombreMateria, N.primerTrimestre, N.segundoTrimestre, N.tercerTrimestre
        FROM Notas N
        INNER JOIN Materia M ON N.idMateria = M.idMateria
        WHERE N.idEstudiante = @idEstudiante";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@idEstudiante", idEstudiante);

            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            DataTable resultado = new DataTable();
            AbrirConexion();
            adaptador.Fill(resultado);
            CerrarConexion();

            return resultado;
        }




        public List<CursoInscrito> ObtenerCursosInscritos(int idEstudiante)
        {
            List<CursoInscrito> cursosInscritos = new List<CursoInscrito>();
            string consulta = "SELECT c.IdCurso, c.Nombre, i.fechaInscripcion FROM Inscripcion i INNER JOIN Cursos c ON i.idCurso = c.IdCurso WHERE i.idEstudiante = @idEstudiante";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@idEstudiante", idEstudiante);

            AbrirConexion();
            SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                cursosInscritos.Add(new CursoInscrito()
                {
                    IdCurso = lector.GetInt32(0),
                    Nombre = lector.GetString(1),
                    FechaInscripcion = lector.GetDateTime(2)
                });
            }
            lector.Close();
            CerrarConexion();

            return cursosInscritos;
        }

        public List<PagoEstudiante> ObtenerPagosEstudiante(int idEstudiante)
        {
            List<PagoEstudiante> pagosEstudiante = new List<PagoEstudiante>();
            string consulta = "SELECT monto, fechaPago, observacion FROM Mensualidades WHERE idEstudiante = @idEstudiante";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@idEstudiante", idEstudiante);

            AbrirConexion();
            SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                pagosEstudiante.Add(new PagoEstudiante()
                {
                    Monto = lector.GetDecimal(0),
                    FechaPago = lector.GetDateTime(1),
                    Observacion = lector.GetString(2)
                });
            }
            lector.Close();
            CerrarConexion();

            return pagosEstudiante;
        }

        public void RegistrarAdministrador(string nombre, string apellido, string cargo)
        {
            string consulta = "INSERT INTO Administradores (nombre, apellido, cargo) VALUES (@nombre, @apellido, @cargo)";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@nombre", nombre);
            comando.Parameters.AddWithValue("@apellido", apellido);
            comando.Parameters.AddWithValue("@cargo", cargo);

            AbrirConexion();
            comando.ExecuteNonQuery();
            CerrarConexion();
        }


        public DataTable ObtenerAdministradores()
        {
            DataTable dtAdministradores = new DataTable();
            string consulta = "SELECT idAdministrador, nombre,apellido, cargo FROM Administradores";
            SqlCommand comando = new SqlCommand(consulta, conexion);

            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            AbrirConexion();
            adaptador.Fill(dtAdministradores);
            CerrarConexion();

            return dtAdministradores;
        }

        public bool ActualizarAdministrador(int idAdministrador, string nombre, string apellido, string cargo)
        {
            string consulta = "UPDATE Administradores SET nombre = @nombre, apellido = @apellido, cargo = @cargo WHERE idAdministrador = @idAdministrador";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@idAdministrador", idAdministrador);
            comando.Parameters.AddWithValue("@nombre", nombre);
            comando.Parameters.AddWithValue("@apellido", apellido);
            comando.Parameters.AddWithValue("@cargo", cargo);

            AbrirConexion();
            int filasAfectadas = comando.ExecuteNonQuery();
            CerrarConexion();

            return filasAfectadas > 0;
        }

        public DataTable ObtenerCargos()
        {
            DataTable dtCargos = new DataTable();
            string consulta = "SELECT nombreCargo FROM Cargos";
            SqlCommand comando = new SqlCommand(consulta, conexion);

            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            AbrirConexion();
            adaptador.Fill(dtCargos);
            CerrarConexion();

            return dtCargos;
        }



        public bool AgregarMateria(string nombreMateria, string gestion)
        {
            string consulta = "INSERT INTO Materia (nombreMateria, gestion) VALUES (@nombreMateria, @gestion)";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@nombreMateria", nombreMateria);
            comando.Parameters.AddWithValue("@gestion", gestion);

            AbrirConexion();
            int filasAfectadas = comando.ExecuteNonQuery();
            CerrarConexion();

            return filasAfectadas > 0;
        }

        public bool ActualizarMateria(int idMateria, string nombreMateria, string gestion)
        {
            string consulta = "UPDATE Materia SET nombreMateria = @nombreMateria, gestion = @gestion WHERE idMateria = @idMateria";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@idMateria", idMateria);
            comando.Parameters.AddWithValue("@nombreMateria", nombreMateria);
            comando.Parameters.AddWithValue("@gestion", gestion);

            AbrirConexion();
            int filasAfectadas = comando.ExecuteNonQuery();
            CerrarConexion();

            return filasAfectadas > 0;
        }

        public bool EliminarMateria(int idMateria)
        {
            string consulta = "DELETE FROM Materia WHERE idMateria = @idMateria";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@idMateria", idMateria);

            AbrirConexion();
            int filasAfectadas = comando.ExecuteNonQuery();
            CerrarConexion();

            return filasAfectadas > 0;
        }

        public DataTable ObtenerMaterias()
        {
            DataTable dtMaterias = new DataTable();
            string consulta = "SELECT idMateria, nombreMateria, gestion FROM Materia";
            SqlCommand comando = new SqlCommand(consulta, conexion);

            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            AbrirConexion();
            adaptador.Fill(dtMaterias);
            CerrarConexion();

            return dtMaterias;
        }

        public bool MateriaExiste(string nombreMateria)
        {
            string consulta = "SELECT COUNT(*) FROM Materia WHERE nombreMateria = @nombreMateria";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@nombreMateria", nombreMateria);

            AbrirConexion();
            int count = (int)comando.ExecuteScalar();
            CerrarConexion();

            return count > 0;
        }

        public DataTable ObtenerEstudiantesPorEstadoYFecha(bool pagado, int mes, int anio)
        {
            string consulta = pagado ? @"
        SELECT E.idEstudiante, E.nombreCompleto, M.monto, M.fechaPago, M.observacion
        FROM Estudiantes E
        INNER JOIN Mensualidades M ON E.idEstudiante = M.idEstudiante
        WHERE M.estado = @estado AND MONTH(M.fechaPago) = @mes AND YEAR(M.fechaPago) = @anio"
    : @"
        SELECT E.idEstudiante, E.nombreCompleto, ISNULL(M.monto, 0) as monto, M.fechaPago, M.observacion
        FROM Estudiantes E
        LEFT JOIN Mensualidades M ON E.idEstudiante = M.idEstudiante AND MONTH(M.fechaPago) = @mes AND YEAR(M.fechaPago) = @anio
        WHERE M.estado IS NULL OR M.estado = @estado";

            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@estado", pagado ? 1 : 0);
            comando.Parameters.AddWithValue("@mes", mes);
            comando.Parameters.AddWithValue("@anio", anio);

            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            DataTable resultado = new DataTable();
            AbrirConexion();
            adaptador.Fill(resultado);
            CerrarConexion();

            return resultado;
        }

        public void RegistrarPago(int idEstudiante, decimal monto, string observacion)
        {
            string consulta = "INSERT INTO Mensualidades (idEstudiante, monto, observacion, fechaPago, estado) VALUES (@idEstudiante, @monto, @observacion, GETDATE(), 1)";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@idEstudiante", idEstudiante);
            comando.Parameters.AddWithValue("@monto", monto);
            comando.Parameters.AddWithValue("@observacion", observacion);

            AbrirConexion();
            comando.ExecuteNonQuery();
            CerrarConexion();
        }

        public bool AgregarCurso(string nombre, string nivel, string estado)
        {
            string consulta = "INSERT INTO Cursos (nombre, nivel, estado) VALUES (@nombre, @nivel, @estado)";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@nombre", nombre);
            comando.Parameters.AddWithValue("@nivel", nivel);
            comando.Parameters.AddWithValue("@estado", estado);

            AbrirConexion();
            int filasAfectadas = comando.ExecuteNonQuery();
            CerrarConexion();

            return filasAfectadas > 0;
        }

        public bool ActualizarCurso(int idCurso, string nombre, string nivel, string estado)
        {
            string consulta = "UPDATE Cursos SET nombre = @nombre, nivel = @nivel, estado = @estado WHERE idCurso = @idCurso";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@idCurso", idCurso);
            comando.Parameters.AddWithValue("@nombre", nombre);
            comando.Parameters.AddWithValue("@nivel", nivel);
            comando.Parameters.AddWithValue("@estado", estado);

            AbrirConexion();
            int filasAfectadas = comando.ExecuteNonQuery();
            CerrarConexion();

            return filasAfectadas > 0;
        }

        public bool EliminarCurso(int idCurso)
        {
            string consulta = "DELETE FROM Cursos WHERE idCurso = @idCurso";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@idCurso", idCurso);

            AbrirConexion();
            int filasAfectadas = comando.ExecuteNonQuery();
            CerrarConexion();

            return filasAfectadas > 0;
        }

        public DataTable ObtenerCursos1()
        {
            DataTable dtCursos = new DataTable();
            string consulta = "SELECT idCurso, nombre, nivel, estado FROM Cursos";
            SqlCommand comando = new SqlCommand(consulta, conexion);

            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            AbrirConexion();
            adaptador.Fill(dtCursos);
            CerrarConexion();

            return dtCursos;
        }

        public DataTable ObtenerInscripcionesPorFecha(int mes, int anio)
        {
            string consulta = @"
        SELECT E.nombreCompleto, C.nombre AS curso, I.observacion, I.fechaInscripcion
        FROM Inscripcion I
        INNER JOIN Estudiantes E ON I.idEstudiante = E.idEstudiante
        INNER JOIN Cursos C ON I.idCurso = C.idCurso
        WHERE MONTH(I.fechaInscripcion) = @mes AND YEAR(I.fechaInscripcion) = @anio";

            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@mes", mes);
            comando.Parameters.AddWithValue("@anio", anio);

            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            DataTable resultado = new DataTable();
            AbrirConexion();
            adaptador.Fill(resultado);
            CerrarConexion();

            return resultado;
        }

        public DataTable ObtenerPagosPorFecha(int mes, int anio, bool pagado)
        {
            string consulta = @"
        SELECT E.nombreCompleto, ISNULL(M.monto, 0) AS monto, M.fechaPago, ISNULL(M.observacion, '') AS observacion
        FROM Estudiantes E
        LEFT JOIN Mensualidades M ON E.idEstudiante = M.idEstudiante AND MONTH(M.fechaPago) = @mes AND YEAR(M.fechaPago) = @anio
        WHERE (@estado = 1 AND M.estado = 1) OR (@estado = 0 AND (M.estado = 0 OR M.estado IS NULL))";

            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@mes", mes);
            comando.Parameters.AddWithValue("@anio", anio);
            comando.Parameters.AddWithValue("@estado", pagado ? 1 : 0);

            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            DataTable resultado = new DataTable();
            AbrirConexion();
            adaptador.Fill(resultado);
            CerrarConexion();

            return resultado;
        }

        public DataTable ObtenerPagosPorAnio(int anio, bool pagado)
        {
            string consulta = @"
        SELECT E.nombreCompleto, ISNULL(M.monto, 0) AS monto, M.fechaPago, ISNULL(M.observacion, '') AS observacion
        FROM Estudiantes E
        LEFT JOIN Mensualidades M ON E.idEstudiante = M.idEstudiante AND YEAR(M.fechaPago) = @anio
        WHERE (@estado = 1 AND M.estado = 1) OR (@estado = 0 AND (M.estado = 0 OR M.estado IS NULL))";

            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@anio", anio);
            comando.Parameters.AddWithValue("@estado", pagado ? 1 : 0);

            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            DataTable resultado = new DataTable();
            AbrirConexion();
            adaptador.Fill(resultado);
            CerrarConexion();

            return resultado;
        }


        public bool AgregarProfesor(string nombre, string apellido, string direccion, string telefono, string email, string estado)
        {
            string consulta = "INSERT INTO Profesores (nombre, apellido, direccion, telefono, email, estado) VALUES (@nombre, @apellido, @direccion, @telefono, @email, @estado)";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@nombre", nombre);
            comando.Parameters.AddWithValue("@apellido", apellido);
            comando.Parameters.AddWithValue("@direccion", direccion);
            comando.Parameters.AddWithValue("@telefono", telefono);
            comando.Parameters.AddWithValue("@email", email);
            comando.Parameters.AddWithValue("@estado", estado);

            AbrirConexion();
            int filasAfectadas = comando.ExecuteNonQuery();
            CerrarConexion();

            return filasAfectadas > 0;
        }

        public DataTable ObtenerProfesoresSinMateria()
        {
            string consulta = "SELECT idProfesor, nombre + ' ' + apellido AS nombreCompleto FROM Profesores WHERE estado = 'Sin Asignar'";
            SqlCommand comando = new SqlCommand(consulta, conexion);

            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            DataTable resultado = new DataTable();
            AbrirConexion();
            adaptador.Fill(resultado);
            CerrarConexion();

            return resultado;
        }

        public bool AsignarMateriaAProfesor(int idProfesor, int idMateria)
        {
            string consulta = "UPDATE Profesores SET idMateria = @idMateria, estado = 'Asignado' WHERE idProfesor = @idProfesor";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@idMateria", idMateria);
            comando.Parameters.AddWithValue("@idProfesor", idProfesor);

            AbrirConexion();
            int filasAfectadas = comando.ExecuteNonQuery();
            CerrarConexion();

            return filasAfectadas > 0;
        }

        public DataTable ObtenerTodosLosProfesores()
        {
            string consulta = @"
        SELECT P.idProfesor, P.nombre, P.apellido, P.direccion, P.telefono, P.email, P.estado, P.idMateria, M.nombreMateria
        FROM Profesores P
        LEFT JOIN Materia M ON P.idMateria = M.idMateria";

            SqlCommand comando = new SqlCommand(consulta, conexion);

            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            DataTable resultado = new DataTable();
            AbrirConexion();
            adaptador.Fill(resultado);
            CerrarConexion();

            return resultado;
        }

        public DataTable ObtenerMateriasProfesores()
        {
            string consulta = "SELECT idMateria, nombreMateria FROM Materia";
            SqlCommand comando = new SqlCommand(consulta, conexion);

            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            DataTable resultado = new DataTable();
            AbrirConexion();
            adaptador.Fill(resultado);
            CerrarConexion();

            return resultado;
        }

        public bool EliminarProfesor(int idProfesor)
        {
            string consulta = "DELETE FROM Profesores WHERE idProfesor = @idProfesor";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@idProfesor", idProfesor);

            AbrirConexion();
            int filasAfectadas = comando.ExecuteNonQuery();
            CerrarConexion();

            return filasAfectadas > 0;
        }

        public bool ActualizarProfesor(int idProfesor, string nombre, string apellido, string direccion, string telefono, string email, string estado)
        {
            string consulta = "UPDATE Profesores SET nombre = @nombre, apellido = @apellido, direccion = @direccion, telefono = @telefono, email = @email, estado = @estado WHERE idProfesor = @idProfesor";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@idProfesor", idProfesor);
            comando.Parameters.AddWithValue("@nombre", nombre);
            comando.Parameters.AddWithValue("@apellido", apellido);
            comando.Parameters.AddWithValue("@direccion", direccion);
            comando.Parameters.AddWithValue("@telefono", telefono);
            comando.Parameters.AddWithValue("@email", email);
            comando.Parameters.AddWithValue("@estado", estado);

            AbrirConexion();
            int filasAfectadas = comando.ExecuteNonQuery();
            CerrarConexion();

            return filasAfectadas > 0;
        }

        public bool ActualizarEstudiante(Estudiante estudiante)
        {
            string consulta = @"
        UPDATE Estudiantes
        SET CI = @CI, nombreCompleto = @nombreCompleto, genero = @genero, fechaNacimiento = @fechaNacimiento,
            nacionalidad = @nacionalidad, direccion = @direccion, tutor = @tutor
        WHERE idEstudiante = @idEstudiante";

            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@idEstudiante", estudiante.IdEstudiante);
            comando.Parameters.AddWithValue("@CI", estudiante.CI);
            comando.Parameters.AddWithValue("@nombreCompleto", estudiante.NombreCompleto);
            comando.Parameters.AddWithValue("@genero", estudiante.Genero);
            comando.Parameters.AddWithValue("@fechaNacimiento", estudiante.FechaNacimiento);
            comando.Parameters.AddWithValue("@nacionalidad", estudiante.Nacionalidad);
            comando.Parameters.AddWithValue("@direccion", estudiante.Direccion);
            comando.Parameters.AddWithValue("@tutor", estudiante.Tutor);

            AbrirConexion();
            int filasAfectadas = comando.ExecuteNonQuery();
            CerrarConexion();

            return filasAfectadas > 0;
        }

        public bool EliminarEstudiante(int idEstudiante)
        {
            string consulta = "DELETE FROM Estudiantes WHERE idEstudiante = @idEstudiante";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@idEstudiante", idEstudiante);

            AbrirConexion();
            int filasAfectadas = comando.ExecuteNonQuery();
            CerrarConexion();

            return filasAfectadas > 0;
        }

        public bool AgregarUsuario(string nombreUsuario, string contrasena, string nombre, string apellidoPaterno, string apellidoMaterno, string tipoUsuario)
        {
            string consulta = "INSERT INTO Usuarios (nombreUsuario, contrasena, nombre, apellidoPaterno, apellidoMaterno, tipoUsuario) VALUES (@nombreUsuario, @contrasena, @nombre, @apellidoPaterno, @apellidoMaterno, @tipoUsuario)";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
            comando.Parameters.AddWithValue("@contrasena", contrasena);
            comando.Parameters.AddWithValue("@nombre", nombre);
            comando.Parameters.AddWithValue("@apellidoPaterno", apellidoPaterno);
            comando.Parameters.AddWithValue("@apellidoMaterno", apellidoMaterno);
            comando.Parameters.AddWithValue("@tipoUsuario", tipoUsuario);

            AbrirConexion();
            int filasAfectadas = comando.ExecuteNonQuery();
            CerrarConexion();

            return filasAfectadas > 0;
        }


        public DataTable ObtenerUsuarios()
        {
            string consulta = "SELECT * FROM Usuarios";
            SqlCommand comando = new SqlCommand(consulta, conexion);

            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            DataTable resultado = new DataTable();
            AbrirConexion();
            adaptador.Fill(resultado);
            CerrarConexion();

            return resultado;
        }

        public bool ActualizarUsuario(string nombreUsuario, string contrasena, string nombre, string apellidoPaterno, string apellidoMaterno, string tipoUsuario)
        {
            string consulta = @"
        UPDATE Usuarios
        SET contrasena = @contrasena, nombre = @nombre, apellidoPaterno = @apellidoPaterno, apellidoMaterno = @apellidoMaterno, tipoUsuario = @tipoUsuario
        WHERE nombreUsuario = @nombreUsuario";

            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
            comando.Parameters.AddWithValue("@contrasena", contrasena);
            comando.Parameters.AddWithValue("@nombre", nombre);
            comando.Parameters.AddWithValue("@apellidoPaterno", apellidoPaterno);
            comando.Parameters.AddWithValue("@apellidoMaterno", apellidoMaterno);
            comando.Parameters.AddWithValue("@tipoUsuario", tipoUsuario);

            AbrirConexion();
            int filasAfectadas = comando.ExecuteNonQuery();
            CerrarConexion();

            return filasAfectadas > 0;
        }

        public bool EliminarUsuario(string nombreUsuario)
        {
            string consulta = "DELETE FROM Usuarios WHERE nombreUsuario = @nombreUsuario";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);

            AbrirConexion();
            int filasAfectadas = comando.ExecuteNonQuery();
            CerrarConexion();

            return filasAfectadas > 0;
        }


    }
}
