using System.Net;
using AutoMapper;
using Core.Dto;
using Core.Especificaciones;
using Core.Models;
using Infraestructura.Data.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController: ControllerBase
    {
        //variables.        
        private ResponseDto _response;
        private ResponsePaginadorDto _responsePaginador;
        private readonly ILogger<EmpleadoController> _logger;
        private readonly IMapper _mapper;        
        private readonly IUnidadDeTrabajo _unidadTrabajo;

        public EmpleadoController(IUnidadDeTrabajo unidadTrabajo, ILogger<EmpleadoController> logger, IMapper mapper)
        {           
            _mapper = mapper;
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;
            _response = new ResponseDto();
            _responsePaginador = new ResponsePaginadorDto();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados([FromQuery] Parametros parametros)
        {
            
            _logger.LogInformation("Listado de Empleados");

            //Query.
            var lista = await _unidadTrabajo.Empleado.ObtenerTodosPaginado(
                                                                          parametros, 
                                                                          incluirPropiedades:"Compania",
                                                                          orderBy: e => e.OrderBy(e=>e.Apellido).ThenBy(e=>e.Nombre)); // retorna la lista de empleados y tambien cluye todos los datos de la compañia.
            
            //mapeo.
            _responsePaginador.TotalPaginas = lista.MetaData.TotalPages;
            _responsePaginador.TotalRegistros = lista.MetaData.TotalCount;
            _responsePaginador.PageSize = lista.MetaData.PageSize;
            _responsePaginador.Resultado = _mapper.Map<IEnumerable<Empleado>, IEnumerable<EmpleadoReadDto>>(lista);         
            _responsePaginador.StatusCode = HttpStatusCode.OK;
            _responsePaginador.Mensaje = "Listado de Empleados.";
            

            //Devuelve una lista de tipo empleado.
            return Ok(_responsePaginador); //El Ok() me garantiza que el status code que devuelve sea igual a 200
        }


        [HttpGet("{id}", Name = "GetEmpleadoPorId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Empleado>> GetEmpleadoPorId(int id){
            
            //Verifica si el id que recibe es igual a 0.
            if(id == 0){
                _logger.LogError("Debe de Enviar el ID ");
                _response.Mensaje = "Debe de Enviar el ID";
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsExitoso = false;                
                return BadRequest(_response);
            }

            //Query.            
            var emp = await _unidadTrabajo.Empleado.Obtenerprimero(e => e.Id == id, incluirPropiedades: "Compania"); // retorna la lista de empleados y tambien cluye todos los datos de la compañia.

            //Verifica si existe ese id en la tabla
            if(emp==null){
                _logger.LogError("No existe ese ID");
                _response.Mensaje ="No existe ese ID";
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }          

            //mapeo. 
            _response.Resultado = _mapper.Map<Empleado, EmpleadoReadDto>(emp);
            //si todo sale bien.
            _response.Mensaje = "Datos del Empleado." + emp.Id;   
            _response.IsExitoso= true;     
            _response.StatusCode = HttpStatusCode.OK;    

            //Devuelve una lista de tipo empleado por id.
            return Ok(_response); //El Ok() me garantiza que el status code que devuelve sea igual a 200
        }

        [HttpGet]
        [Route("EmpleadosPorCompania/{companiaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        public async Task<ActionResult<IEnumerable<EmpleadoReadDto>>> GetEmpleadosPorCompania(int companiaId)
        { 
            _logger.LogInformation("Listado de Empleados Por Compañia");   

            //Query.            
            var empLista = await _unidadTrabajo.Empleado.ObtenerTodos(e=>e.CompaniaId == companiaId, incluirPropiedades: "Compania");// retorna una lista de empleados filtadas por el parametro id.
            
            //mapeo.
            _response.Resultado = _mapper.Map<IEnumerable<Empleado>, IEnumerable<EmpleadoReadDto>>(empLista);
            //si todo sale bien.
            _response.IsExitoso = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Mensaje = "Listado de Empleados por Compañia.";            
            return Ok(_response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public async Task<ActionResult<Empleado>> PostEmpleado([FromBody] EmpleadoUpsertDto empleadoDto){

            //Verifica si lo recibido por input no es nulo.
            if(empleadoDto == null){
                _response.Mensaje = "La informacion ingresada es incorrecta";
                _response.IsExitoso=false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            //Verifica que cada una de las propiedades esten completas.
            //trabaja directamente con el modelo.
            if(!ModelState.IsValid){
                _response.Mensaje = "La informacion ingresada es incorrecta";
                _response.IsExitoso=false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
                
            }
            
            //Query.
            var empleadoExiste = await _unidadTrabajo.Empleado.Obtenerprimero(e => e.Nombre.ToLower() == empleadoDto.Nombre.ToLower() &&
                                                                              e.Apellido.ToLower() == empleadoDto.Apellido.ToLower());

            //Verifica si el nombre ya existe.
            if(empleadoExiste != null){
                _response.IsExitoso = false;
                _response.Mensaje = "El Nombre del Empleado ya Existe.";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            
            //relaciona el objeto que contiene la clase companiaDto y lo adapta para que se pueda grabar en la base de datos correctamente.
            //mapeo.
            Empleado empleado = _mapper.Map<Empleado>(empleadoDto);

            //Agrega un nuevo regristo en la tabla Compania.
            await _unidadTrabajo.Empleado.Agregar(empleado);
            //Graba el nuevo registro en la tabla.
            await _unidadTrabajo.Guardar(); 

            _response.IsExitoso = true;
            _response.Mensaje = "Empleado Actualizado con EXITO!";
            _response.StatusCode = HttpStatusCode.Created;
            _response.Resultado = empleado;
            return CreatedAtRoute("GetEmpleadoPorId", new {id=empleadoDto}, _response); //El CreatedAtRoute() me garantiza que el status code que devuelve sea igual a 201 por ser un metodo de tipo POST.   
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Empleado>> PutEmpleado(int id, [FromBody] EmpleadoUpsertDto empleadoDto){

            //Verifica si el id que recibe del end point es igual al id que tiene compania.
            if(id != empleadoDto.Id){

                _response.IsExitoso = false;
                _response.Mensaje = "El ID ingresado no es correcto.";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            //Verifica si todos los datos del objeto son validos.
            if(!ModelState.IsValid)
            {   
                _response.IsExitoso = false;
                _response.Mensaje = "El Nombre del Empleado ya Existe.";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            
            //Query.
            var empleadoExiste = await _unidadTrabajo.Empleado.Obtenerprimero(e => e.Nombre.ToLower() == empleadoDto.Nombre.ToLower()
                                                                             && e.Apellido.ToLower() == empleadoDto.Apellido.ToLower()
                                                                             && e.Id != empleadoDto.Id);

            //Verifica si el nombre, apellido y id ya existe.
            if(empleadoExiste != null){

                _response.IsExitoso = false;
                _response.Mensaje = "El nombre del Empleado ya existe.";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }            

            //relaciona el objeto que contiene la clase empleadoUpsertDto y lo adapta para que se pueda grabar en la base de datos correctamente.
            //mapeo.
            Empleado empleado = _mapper.Map<Empleado>(empleadoDto);

            //actualiza los datos.
            _unidadTrabajo.Empleado.Actualizar(empleado);

            //graba los datos.
            await _unidadTrabajo.Guardar();
            _response.IsExitoso = true;
            _response.Mensaje = "Empleado Guardado con EXITO";
            _response.StatusCode = HttpStatusCode.NoContent;

            //devuelve una lista de tipo empleado.
            return Ok(_response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteEmpleado(int id){

            //Query.(consulta en la base de datos si existe ese id)
            var empleado = await _unidadTrabajo.Empleado.Obtenerprimero(e=> e.Id == id);
            
            //Si no encontro ningun registro retontar "No Encontrado".
            if(empleado == null){

                _response.IsExitoso = false; 
                _response.Mensaje = "Empleado No Encontrado";
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            //Si encuentra uno lo elimina
           _unidadTrabajo.Empleado.Remover(empleado);

            //Actualiza la base de datos.
            await _unidadTrabajo.Guardar();
            _response.IsExitoso = true;
            _response.Mensaje = "Empleado Eliminado";
            _response.StatusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }
    }
}