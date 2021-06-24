using ChallengerCore.Models;
using Firebase.Auth;
using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChallengerCore.Controllers
{
    public class FileController : Controller
    {
        private static string apikey = "AIzaSyCXE42NcyQaqX1Ld6bXNapQiiNO7BDgKEg";
        private static string bucket = "loginasp-fcb75.appspot.com";
        private static string authemail = "mahmoudqtari0@gmail.com";
        private static string authpass = "mahmoudqtari";

        public FileController()
        {

        }
        // GET: File
        public ActionResult UploadFiles()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadFiles(HttpPostedFileBase file)
        {

            FileStream stream;
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~"), fileName);
                file.SaveAs(path);
                //string path = Path.Combine(Server.MapPath("~/Content/images/"), file.FileName);
                //file.SaveAs(path);
                stream = new FileStream(Path.Combine(path), FileMode.Open, FileAccess.Read, FileShare.Read);
                await Task.Run(() => Upload(stream, file.FileName));
            }
            return View();
        }

        public async void Upload(FileStream stream, string filename)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(apikey));
            var a = await auth.SignInWithEmailAndPasswordAsync(authemail, authpass);
            var cancellation = new CancellationTokenSource();
            var task = new FirebaseStorage(
                bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child("images")
                .Child(filename)
                .PutAsync(stream, cancellation.Token
                );

            try
            {
                string link = await task;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception was thrown :{0}", ex);
            }
        }
    }
}