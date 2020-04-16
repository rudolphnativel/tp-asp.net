using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TP1Module5.Models;

namespace TP1Module5.Controllers
{
  
    public class ChatController : Controller
    {
        public static List<Chat> listChat = Chat.GetMeuteDeChats();
        // GET: Chat
        public ActionResult Index()
        {
            return View(listChat);
        }

        // GET: Chat/Details/5
        public ActionResult Details(int id)
        {
            Chat chat= (Chat)listChat.Where(C => C.Id == id).FirstOrDefault();
            return View(chat);
        }


     
       

        // GET: Chat/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Chat/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id, FormCollection collection)
        {
            try
            {
               Chat chat = (Chat)listChat.Where(C => C.Id == id).FirstOrDefault();
                listChat.Remove(chat);
                    return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
