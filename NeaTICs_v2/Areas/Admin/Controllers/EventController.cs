using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NeaTICs_v2.Models;
using NeaTICs_v2.DAL;

namespace NeaTICs_v2.Areas.Admin.Controllers
{
    public class EventController : Controller
    {
        //private Context db = new Context();
        private UnitOfWork unitOfWork = new UnitOfWork();

        //
        // GET: /Admin/Event/

        //Listar todas los eventos
        public ActionResult Index()
        {
            //Método para traer todos los eventos de la BD
            var events = unitOfWork.EventsRepository.All();
            foreach (var ob in events)
            {
                //Convierte los arreglos de bytes de la base de datos en string para luego mostrar como imagen en html
                ob.ImageToRead = Convert.ToBase64String(ob.Image);
            }
            return View(events.ToList());
        }

        //
        // GET: /Admin/Event/Details/5

        //Cargar la vista detallada de un evento. Le paso el ID como parametro
        public ActionResult Details(int id = 0)
        {
            //Método para traer un evento de la BD por su ID
            Events @event = unitOfWork.EventsRepository.GetByID(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            @event.ImageToRead = Convert.ToBase64String(@event.Image);
            return View(@event);
        }

        //
        // GET: /Admin/Event/Create

        //Cargado del formulario para crear un nuevo evento
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/Event/Create
        //POST del formulario de creación de nuevo evento. Se crea un nuevo evento con los datos que se pasan del formulario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Events @event)
        {
            //TODO: Revisar tipo de imagen a subir
            //var contentType = @event.ImageToUpload.ContentType;
            //Acá convierto el archivo que suben en un arreglo de bytes para ser guardado en la BD
            var ImageData = new byte[@event.ImageToUpload.ContentLength];
            @event.ImageToUpload.InputStream.Read(ImageData, 0, @event.ImageToUpload.ContentLength);
            @event.Image = ImageData;
            try
            {
                if (ModelState.IsValid)
                {
                    //Inserto nuevo evento en la BD
                    unitOfWork.EventsRepository.Insert(@event);
                    //Guardo
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(@event);
        }

        //
        // GET: /Admin/Event/Edit/5
        //Cargado de la página detallada de eventos pero con los campos para editar. Se le pasa el ID del evento como parametro.
        public ActionResult Edit(int id = 0)
        {
            Events @event = unitOfWork.EventsRepository.GetByID(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            @event.ImageToRead = Convert.ToBase64String(@event.Image);
            return View(@event);
        }

        //
        // POST: /Admin/Event/Edit/5
        //POST del formulario de edición del evento. Recibe por parámetro todos los campos.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Events @event)
        {
            //var ImageData = new byte[@event.ImageToUpload.ContentLength];
            //@event.ImageToUpload.InputStream.Read(ImageData, 0, @event.ImageToUpload.ContentLength);
            //@event.Imagen = ImageData;
            //Recupero el evento a ser editado desde la BD por ID
            Events TempEvent = unitOfWork.EventsRepository.GetByID(@event.ID);
            //Paso todos los atributos a editar al evento que recuperé de la BD
            //Hago esto porque sino vuelve a guardar la imagen pero como nulo
            TempEvent.Title = @event.Title;
            TempEvent.Content = @event.Content;
            TempEvent.Place = @event.Place;
            TempEvent.CreateAt = @event.CreateAt;
            TempEvent.UpdateAt = @event.UpdateAt;
            TempEvent.StartAt = @event.StartAt;
            TempEvent.EndAt = @event.EndAt;
            TempEvent.Url = @event.Url;
            try
            {
                if (ModelState.IsValid)
                {
                    //Actualizo el evento
                    unitOfWork.EventsRepository.Update(TempEvent);
                    //Guardado
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(TempEvent);
        }

        //
        // GET: /Admin/Event/Delete/5
        //Cargado de la vista detallada de un evento para ser borrado. Se le pasa por parámetro el ID
        public ActionResult Delete(int id = 0)
        {
            Events @event = unitOfWork.EventsRepository.GetByID(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            @event.ImageToRead = Convert.ToBase64String(@event.Image);
            return View(@event);
        }

        //
        // POST: /Admin/Event/Delete/5
        //POST para borrar el evento. Recibe el ID como parámetro
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                //Busco el evento a borrar por ID en la BD
                Events @event = unitOfWork.EventsRepository.GetByID(id);
                //Lo borro
                unitOfWork.EventsRepository.Delete(@event);
                //Guardo
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(id);
        }

        protected override void Dispose(bool disposing)
        {
            //Desconectar la base de datos
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}