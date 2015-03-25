using Raven.Client;
using Raven.Client.Document;
using System.Web.Mvc;

namespace HelloRavenDB.Controllers
{
    public abstract class RavenController : Controller
    {
        private static IDocumentStore _documentStore;

        public static IDocumentStore DocumentStore
        {
            get
            {
                return _documentStore ?? (_documentStore = new DocumentStore()
                {
                    ConnectionStringName = "ravenDB"
                }.Initialize());
            }
        }

        public IDocumentSession RavenSession { get; protected set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RavenSession = DocumentStore.OpenSession();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;

            using (RavenSession)
            {
                if (filterContext.Exception != null)
                    return;

                if (RavenSession != null)
                    RavenSession.SaveChanges();
            }
        }

        protected HttpStatusCodeResult HttpNotModified()
        {
            return new HttpStatusCodeResult(304);
        }

        protected new JsonResult Json(object data)
        {
            return base.Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}