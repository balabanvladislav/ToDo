﻿using System;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ToDo.BL;
using ToDo.Domain;

namespace ToDoList.Controllers
{
    public class ToDoItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ToDoItems
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Intoarce elementele liste doar a anumitui utilizator
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ToDoItem> GetMyItems()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(
                x => x.Id == currentUserId);

            IEnumerable<ToDoItem> myToDoes = db.ToDoItems.ToList().Where(x => x.User == currentUser);

            int completeCount = 0;
            foreach (ToDoItem toDo in myToDoes)
            {
                if (toDo.IsDone)
                {
                    completeCount++;
                }
            }

            ViewBag.Percent = Math.Round(100f * ((float) completeCount / (float) myToDoes.Count()));
            if (myToDoes.Count() == 0)
            {
                ViewBag.Percent = 0;}
            return myToDoes;
        }

        public ActionResult BuildToDoTable()
        {
            return PartialView("_ToDoTable", GetMyItems());
        }

        // GET: ToDoItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ToDoItem toDoItem = db.ToDoItems.Find(id);
            if (toDoItem == null)
            {
                return HttpNotFound();
            }

            return View(toDoItem);
        }

        // GET: ToDoItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToDoItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Text,IsDone")] ToDoItem toDoItem)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(
                    x => x.Id == currentUserId);
                toDoItem.User = currentUser;


                db.ToDoItems.Add(toDoItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.Clear();
            return View(toDoItem);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AJAXCreate([Bind(Include = "Id,Text")] ToDoItem toDoItem)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(
                    x => x.Id == currentUserId);
                toDoItem.User = currentUser;
                toDoItem.IsDone = false;

                db.ToDoItems.Add(toDoItem);
                db.SaveChanges();
            }
            ModelState.Clear();
            return PartialView("_ToDoTable", GetMyItems());
        }

        // GET: ToDoItems/Edit/5
        public ActionResult Edit(int? id)
        {
            
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(
                x => x.Id == currentUserId);
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            
            ToDoItem toDoItem = db.ToDoItems.Find(id);
            
            
            if (toDoItem == null)
            {
                return HttpNotFound();
            }

            if (toDoItem.User != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(toDoItem);
        }

        // POST: ToDoItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Text,IsDone")] ToDoItem toDoItem)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(toDoItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(toDoItem);
        }


        [HttpPost]
        public ActionResult AJAXEdit(int? id, bool value)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ToDoItem toDoItem = db.ToDoItems.Find(id);
            if (toDoItem == null)
            {
                return HttpNotFound();
            }
            else
            {
                toDoItem.IsDone = value;
                db.Entry(toDoItem).State = EntityState.Modified;
                db.SaveChanges();
            }
            return PartialView("_ToDoTable", GetMyItems());
        }

        // GET: ToDoItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ToDoItem toDoItem = db.ToDoItems.Find(id);
            db.ToDoItems.Remove(toDoItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: ToDoItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ToDoItem toDoItem = db.ToDoItems.Find(id);
            db.ToDoItems.Remove(toDoItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}