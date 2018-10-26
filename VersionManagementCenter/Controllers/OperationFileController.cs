using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VersionManagementCenter.Controllers
{
    public class OperationFileController : Controller
    {
        public ActionResult SyncUploadFile()
        {
            return View();
        }
    }
}
