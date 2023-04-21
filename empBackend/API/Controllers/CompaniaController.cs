using System.Net;
using AutoMapper;
using Core.Dto;
using Core.Models;
using Infraestructura.Data;
using Infraestructura.Data.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniaController: ControllerBase
    {
        //variables.        
        private readonly IUnidadDeTrabajo _unidadTrabajo;
        private ResponseDto _response;
        private ResponsePaginadorDto _responsePaginador;
        private readonly ILogger<CompaniaController> _logger;
        private readonly IMapper _mapper;
        

        public CompaniaController(IUnidadDeTrabajo unidadTrabajo, ILogger<CompaniaController> logger, IMapper mapper)
        {            
            _mapper = mapper;
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;
            _response = new ResponseDto();
            _responsePaginador = new ResponsePaginadorDto();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Compania>>> GetCompanias(){
            
            _logger.LogInformation("Listado de Compañias");

            //Query.
            var lista = await _unidadTrabajo.Compania.ObtenerTodos();

            _response.Resultado = lista;
            _response.Mensaje = "Listado de Compañias.";
            _response.StatusCode = HttpStatusCode.OK;
            

            //Devuelve una lista de tipo compania.
            return Ok(_response); //El Ok() me garantiza que el status code que devuelve sea igual a 200
        }


        [HttpGet("{id}", Name = "GetCompaniaPorId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Compania>> GetCompaniaPorId(int id){
            
            //Verifica si el id que recibe es diferente a 0.
            if(id == 0){
                _logger.LogError("Debe de Enviar el ID ");
                _response.Mensaje = "Debe de Enviar el ID";
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            //Query.
            var comp = await _unidadTrabajo.Compania.Obtenerprimero(c => c.Id == id); 

            //Verifica si existe ese id en la tabla
            if(comp==null){
                _logger.LogError("No existe ese ID");
                _response.Mensaje ="No existe ese ID";
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }           
            _response.Resultado = comp;
            _response.Mensaje = "Datos de la Compañia." + comp.Id;   
            _response.IsExitoso= true;         
            _response.StatusCode = HttpStatusCode.OK;
            //Devuelve una lista de tipo compania por id.
            return Ok(_response); //El Ok() me garantiza que el status code que devuelve sea igual a 200
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public async Task<ActionResult<Compania>> PostCompania([FromBody] CompaniaDto companiaDto){

            //Verifica si lo recibido por input no es nulo.
            if(companiaDto == null){
                _response.Mensaje = "La informacion ingresada es incorrecta";
                _response.IsExitoso=false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            //Verifica que cada una de las propiedades esten completas.
            //trabaja directamente con el modelo.
            if(!ModelState.IsValid){

                return BadRequest(ModelState);
            }
            
            //Query.
            var companiaExiste = await _unidadTrabajo.Compania.Obtenerprimero(c => c.NombreCompania.ToLower() == companiaDto.NombreCompania.ToLower());

            //Verifica si el nombre ya existe.
            if(companiaExiste != null){                
                _response.IsExitoso = false;
                _response.Mensaje = "El Nombre de la Compañia ya Existe.";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            
            //relaciona el objeto que contiene la clase companiaDto y lo adapta para que se pueda grabar en la base de datos correctamente.
            //variable.
            Compania compania = _mapper.Map<Compania>(companiaDto);

            //Agrega un nuevo regristo en la tabla Compania.
            await _unidadTrabajo.Compania.Agregar(compania);
            //Graba el nuevo registro en la tabla.
            await _unidadTrabajo.Guardar();   
            _response.IsExitoso = true;
            _response.Mensaje = "Compañia Guardada con EXITO!";
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtRoute("GetCompaniaPorId", new {id=companiaDto}, _response); //El CreatedAtRoute() me garantiza que el status code que devuelve sea igual a 201 por ser un metodo de tipo POST.   
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Compania>> PutCompania(int id, [FromBody] CompaniaDto companiaDto){

            //Verifica si el id que recibe del end point es igual al id que tiene compania.
            if(id != companiaDto.Id)
            {
                _response.IsExitoso = false;
                _response.Mensaje = "El ID ingresado no es correcto.";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            //Verifica si todos los datos del objeto son validos.
            if(!ModelState.IsValid)
            {   
                return BadRequest(ModelState); 
            }

            //Verifica si el nombre existe.
            //Query.
            var companiaExiste = await _unidadTrabajo.Compania.Obtenerprimero(c => c.NombreCompania.ToLower() == companiaDto.NombreCompania.ToLower()
                                                                              && c.Id != companiaDto.Id);

            if(companiaExiste != null){
                
                _response.IsExitoso = false;
                _response.Mensaje = "El nombre de la compañia ya existe.";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }            

            //relaciona el objeto que contiene la clase companiaDto y lo adapta para que se pueda grabar en la base de datos correctamente.
            //variable.
            Compania compania = _mapper.Map<Compania>(companiaDto);

            //actualiza los datos.
            _unidadTrabajo.Compania.Actualizar(compania);

            //graba los datos.
            await _unidadTrabajo.Guardar();
            _response.IsExitoso = true;
            _response.Mensaje = "Compañia Actualizada";
            _response.StatusCode = HttpStatusCode.OK;

            //devuelve una lista de tipo compania.
            return Ok(_response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCompania(int id){

            //Query.(consulta en la base de datos si existe ese id)
            var compania = await _unidadTrabajo.Compania.Obtenerprimero(c => c.Id == id);
            
            //Si no encontro ningun registro retontar "No Encontrado".
            if(compania == null){

                _response.IsExitoso = false; 
                _response.Mensaje = "Compañia No Encontrada";
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound();
            }

            //Si encuentra uno lo elimina
            _unidadTrabajo.Compania.Remover(compania);

            //Actualiza la base de datos.
            await _unidadTrabajo.Guardar();
            _response.IsExitoso = true;
            _response.Mensaje = "Compañia Eliminada";
            _response.StatusCode = HttpStatusCode.NoContent;

            return Ok(_response);

        }
    }
}