using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TaskManager_DAL;

namespace TaskManager.Controllers
{
    public class TaskController : ApiController
    {

        //Get All Tasks
        [HttpGet]
        public HttpResponseMessage Get(string quoteType = "All")
        {
            using (TaskManagerEntities entities = new TaskManagerEntities())
            {
                switch (quoteType.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            entities.Quotes.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            entities.Quotes.Where(e => e.QuoteType.ToLower() == "dyr").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            entities.Quotes.Where(e => e.QuoteType.ToLower() == "bf").ToList());
                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                            "Value for gender must be DYR, BF or All. " + quoteType + " is invalid.");
                }
            }
        }

        //Get All Tasks
        [HttpGet]
        public IEnumerable<Quote> LoadAllTasks()
        {
            using (TaskManagerEntities entities = new TaskManagerEntities())
            {
                return entities.Quotes.ToList();
            }

        }

        //Get Task by ID
        [HttpGet]
        public HttpResponseMessage LoadTaskByID(int id)
        {
            using (TaskManagerEntities entities = new TaskManagerEntities())
            {
                var entity = entities.Quotes.FirstOrDefault(q => q.QuoteID == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Quote with Id " + id.ToString() + " not found");
                }
            }
        }

        //Delete Task by ID
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (TaskManagerEntities entities = new TaskManagerEntities())
                {
                    var entity = entities.Quotes.FirstOrDefault(q => q.QuoteID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Employee with Id = " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Quotes.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //Update takask by ID
        public HttpResponseMessage Put([FromBody] int id, [FromUri] Quote quote)
        {
            try
            {
                using (TaskManagerEntities entities = new TaskManagerEntities())
                {
                    var entity =  entities.Quotes.FirstOrDefault(q => q.QuoteID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Employee with Id " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.QuoteType = quote.QuoteType;
                        entity.Contact = quote.Contact;
                        entity.DueDate = quote.DueDate;
                        entity.Task = quote.Task;
                        entity.TaskType = quote.TaskType;

                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //Create new task
        public HttpResponseMessage Post([FromBody] Quote quote)
        {
            try
            {
                using (TaskManagerEntities entities = new TaskManagerEntities())
                {
                    entities.Quotes.Add(quote);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, quote);
                    message.Headers.Location = new Uri(Request.RequestUri +
                        quote.QuoteID.ToString());

                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

    }

}
