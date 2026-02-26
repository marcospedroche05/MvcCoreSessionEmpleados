using Microsoft.AspNetCore.Mvc;
using MvcCoreSessionEmpleados.Extensions;
using MvcCoreSessionEmpleados.Models;
using MvcCoreSessionEmpleados.Repositories;
using System.Threading.Tasks;

namespace MvcCoreSessionEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;

        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> SessionSalarios(int? salario)
        {
            if(salario != null)
            {
                //QUEREMOS ALMACENAR LA SUMA TOTAL DE SALARIOS
                //QUE TENGAMOS EN SESSION
                int sumaTotal = 0;
                if(HttpContext.Session.GetString("SUMASALARIAL") != null)
                {
                    sumaTotal = HttpContext.Session.GetObject<int>("SUMASALARIAL");
                }
                //SUMAMOS EL NUEVO SALARIO A LA SUMA TOTAL
                sumaTotal += salario.Value;
                //ALMACENAMOS EL VALOR DENTRO DE SESSION
                HttpContext.Session.SetObject("SUMASALARIAL", sumaTotal);
                ViewData["MENSAJE"] = "Salario almacenado: " + salario;
            }
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public IActionResult SumaSalarial()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SessionEmpleados
            (int? idEmpleado)
        {
            if(idEmpleado != null)
            {
                Empleado empleado = await this.repo.FindEmpleadoAsync(idEmpleado.Value);
                //EN SESSION TENDREMOS ALMACENADOS UN CONJUNTO DE EMPLEADOS
                List<Empleado> empleadosList;
                //DEBEMOS PREGUNTAR SI YA TENEMOS EMPLEADOS EN SESSION
                if (HttpContext.Session.GetObject<List<Empleado>>("EMPLEADOS") != null)
                {
                    empleadosList =
                    HttpContext.Session.GetObject<List<Empleado>>("EMPLEADOS");
                } else {
                    //CREAMOS UNA NUEVA LISTA PARA ALMACENAR LOS EMPLEADOS
                    empleadosList = new List<Empleado>();
                }
                //AGREGAMOS EL EMPLEADO AL LIST
                empleadosList.Add(empleado);
                //ALMACENAMOS LA LISTA EN SESSION
                HttpContext.Session.SetObject("EMPLEADOS", empleadosList);
                ViewData["MENSAJE"] = "Empleado " + empleado.Apellido + " almacenado correctamente";
            }
            List<Empleado> empleados = 
                await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public IActionResult EmpleadosAlmacenados()
        {
            return View();
        }

        public async Task<IActionResult> SessionEmpleadosOk(int? idempleado)
        {
            if(idempleado != null)
            {
                //ALMACENAMOS LO MISMO
                List<int> idsEmpleados;
                if(HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS") != null)
                {
                    //RECUPERAMOS LA COLECCION
                    idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
                } else
                {
                    idsEmpleados = new List<int>();
                }
                idsEmpleados.Add(idempleado.Value);
                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);
                ViewData["MENSAJE"] = "Empleados almacenados";
            }
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public async Task<IActionResult> EmpleadosAlmacenadosOk()
        {
            //RECUPERAMOS LOS DATOS DE SESSION
            List<int> idsEmpleados =
                HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if(idsEmpleados == null)
            {
                ViewData["MENSAJE"] = "No existen empleados en Session";
                return View();
            }else
            {
                List<Empleado> empleados =
                    await this.repo.GetEmpleadosSessionAsync(idsEmpleados);
                return View(empleados);
            }
        }


        public async Task<IActionResult> SessionEmpleadosV4(int? idempleado)
        {
            List<Empleado> empleados = new List<Empleado>();
            if (idempleado != null)
            {
                //ALMACENAMOS LO MISMO
                List<int> idsEmpleados;
                if (HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS") != null)
                {
                    //RECUPERAMOS LA COLECCION
                    idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
                }
                else
                {
                    idsEmpleados = new List<int>();
                }
                idsEmpleados.Add(idempleado.Value);
                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);
                empleados = await this.repo.GetEmpleadosNotInSessionAsync(idsEmpleados);
                ViewData["MENSAJE"] = "Empleados almacenados";
            } else
            {
                List<int> idsEmpleados;
                if (HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS") != null)
                {
                    idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
                    empleados = await this.repo.GetEmpleadosNotInSessionAsync(idsEmpleados);
                } else
                {
                    empleados = await this.repo.GetEmpleadosAsync();
                }
            }
            return View(empleados);
        }

        public async Task<IActionResult> EmpleadosAlmacenadosV4()
        {
            //RECUPERAMOS LOS DATOS DE SESSION
            List<int> idsEmpleados =
                HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if (idsEmpleados == null)
            {
                ViewData["MENSAJE"] = "No existen empleados en Session";
                return View();
            }
            else
            {
                List<Empleado> empleados =
                    await this.repo.GetEmpleadosSessionAsync(idsEmpleados);
                return View(empleados);
            }
        }
    }
}
